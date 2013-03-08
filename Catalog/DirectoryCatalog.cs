// Author: He Jiang (mailto:hejiang@tju.edu.cn)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DAR.Internal;
using log4net;

namespace DAR.Catalog
{
    public class DirectoryCatalog : ICatalog
    {
        private static readonly ILog _logger = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Lock _thisLock = new Lock();
        protected FileSystemWatcher _watcher;

        protected string _path, _fullPath, _searchPattern;

        protected AggregateCatalog<AssemblyCatalog> _loadedCatalogs;
        private IList<Tuple<string, DateTime>> _loadedFiles;

        public DirectoryCatalog(string path, bool watch = true)
            : this(path, "*.dll", watch)
        {
        }

        public DirectoryCatalog(string path, string searchPattern, bool watch = true)
        {
            this.Initialize(path, searchPattern);

            if (watch)
            {
                _watcher = new FileSystemWatcher(_fullPath, searchPattern);
                _watcher.Changed += new FileSystemEventHandler((obj, args) => { this.Refresh(); });
                _watcher.Created += new FileSystemEventHandler((obj, args) => { this.Refresh(); });
                _watcher.Deleted += new FileSystemEventHandler((obj, args) => { this.Refresh(); });
                _watcher.Renamed += new RenamedEventHandler((obj, args) => { this.Refresh(); });

                _watcher.EnableRaisingEvents = true;
                _logger.DebugFormat("Start to watch change of files with pattern ({0}) in directory ({1})",
                    searchPattern, _fullPath);
            }
        }

        private void Initialize(string path, string searchPattern)
        {
            this._path = path;
            this._fullPath = GetFullPath(path);
            this._searchPattern = searchPattern;

            _loadedFiles = new List<Tuple<string, DateTime>>();
            _loadedCatalogs = new AggregateCatalog<AssemblyCatalog>();
            Refresh();
        }

        public void Refresh()
        {
            try
            {
                IList<Tuple<string, DateTime>> afterFiles = GetFiles();
                IList<Tuple<string, DateTime>> beforeFiles;
                using (new ReadLock(this._thisLock))
                {
                    beforeFiles = _loadedFiles;
                }

                IEnumerable<Tuple<string, DateTime>> filesToRemove =
                        beforeFiles.Except(afterFiles, new FileEqualityComparer());
                foreach (var file in filesToRemove)
                {
                    var codeBase = file.Item1;
                    AssemblyCatalog cat = _loadedCatalogs.FirstOrDefault(
                        c => c.FilePath.Equals(codeBase, StringComparison.InvariantCultureIgnoreCase));
                    if (cat != null)
                    {
                        if (cat.Assembly != null)
                        {
                            cat.FilePath = "* " + cat.FilePath;
                        }
                        else
                        {
                            _loadedCatalogs.Remove(cat);
                        }

                        _logger.InfoFormat("File ({0}) removed", codeBase);
                    }
                }

                IEnumerable<Tuple<string, DateTime>> filesToAdd =
                    afterFiles.Except(beforeFiles, new FileEqualityComparer());

                foreach (var file in filesToAdd)
                {
                    string codeBase = file.Item1;
                    _loadedCatalogs.Add(new AssemblyCatalog(codeBase));
                    _logger.InfoFormat("File ({0}) added", codeBase);
                }

                using (new WriteLock(this._thisLock))
                {
                    _loadedFiles = afterFiles;
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Fail to refresh directory ({0}) : {1}", _fullPath, ex.Message);
            }
        }

        internal class FileEqualityComparer : IEqualityComparer<Tuple<string, DateTime>>
        {
            public bool Equals(Tuple<string, DateTime> x, Tuple<string, DateTime> y)
            {
                return x.Item1.Equals(y.Item1, StringComparison.InvariantCultureIgnoreCase) &&
                    (x.Item2 == y.Item2);
            }

            public int GetHashCode(Tuple<string, DateTime> obj)
            {
                return (obj.Item1.GetHashCode() + obj.Item2.GetHashCode());
            }
        }

        private IList<Tuple<string, DateTime>> GetFiles()
        {
            return Directory.GetFiles(this._fullPath, this._searchPattern)
                .Select(f => new Tuple<string, DateTime>(f, File.GetLastWriteTimeUtc(f)))
                .ToList();
        }

        private static string GetFullPath(string path)
        {
            if (!Path.IsPathRooted(path) && AppDomain.CurrentDomain.BaseDirectory != null)
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }

            return Path.GetFullPath(path);
        }

        public AssemblyCatalog ResolveAssembly(string assemblyName)
        {
            using (new ReadLock(this._thisLock))
            {
                return _loadedCatalogs.ResolveAssembly(assemblyName);
            }
        }

        public AssemblyCatalog ResolveType(string typeName)
        {
            using (new ReadLock(this._thisLock))
            {
                return _loadedCatalogs.ResolveType(typeName);
            }
        }

        internal class Tuple<T, S>
        {
            public T Item1;
            public S Item2;

            public Tuple(T t, S s)
            {
                Item1 = t;
                Item2 = s;
            }
        }
    }
}
