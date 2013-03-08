// Author: He Jiang (mailto:hejiang@tju.edu.cn)

using System;
using System.Reflection;
using System.Threading;
using DAR.Catalog;
using log4net;
using System.Linq;
using System.Collections.Generic;
using DAR.Internal;

namespace DAR
{
    public class Resolver
    {
        private static readonly ILog _logger = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);
        static AggregateCatalog<ICatalog> _root = new AggregateCatalog<ICatalog>();
        private static readonly Lock _thisLock = new Lock();
        private static List<AppDomain> _domains = new List<AppDomain>();

        public static void Start(AppDomain domain = null)
        {
            Start(null, null, domain);
        }

        public static void Start(string path, string searchPattern = null, AppDomain domain = null)
        {
            if (domain == null)
                domain = AppDomain.CurrentDomain;

            using (new WriteLock(_thisLock))
            {
                if (_domains.Contains(domain))
                {
                    string message = string.Format("Dynamic Assembly Resolver already started for domain ({0})", domain.FriendlyName);
                    _logger.Warn(message);
                    throw new Exception(message);
                }

                _domains.Add(domain);
            }

            if (path != null)
            {
                if (searchPattern != null)
                    _root.Add(new DirectoryCatalog(path, searchPattern, true));
                else
                    _root.Add(new DirectoryCatalog(path, true));
            }

            domain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
            domain.TypeResolve += new ResolveEventHandler(TypeResolveEventHandler);

            _logger.InfoFormat("Dynamic Assembly Resolver started for domain ({0})",
                domain.FriendlyName);
        }
        public static Type GetType(string typeName, bool throwOnError = true)
        {
            Type t = null;
            try
            {
                string[] s = typeName.Split(new char[] { ',' }, 2);
                AssemblyCatalog a = null;
                if (s.Length == 2)
                {
                    a = _root.ResolveAssembly(s[1]);
                }

                if (a != null)
                {
                    t = a.Assembly.GetType(s[0], true);
                    _logger.DebugFormat("Type resolved: type ({0}) in assembly ({1}) from file ({2})",
                        typeName, a.Assembly.FullName, a.FilePath);
                }
                else
                {
                    t = Type.GetType(typeName, true);
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Fail to resolve type ({0}): {1}",
                    typeName, ex.Message);

                if (throwOnError)
                    throw ex;
            }

            return t;
        }

        public static void AddCatalog(ICatalog cat)
        {
            _root.Add(cat);
        }
        
        protected static Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            AssemblyCatalog a = _root.ResolveAssembly(args.Name);
            if (a != null)
            {
                _logger.DebugFormat("Assembly resolved: ({0}) => assembly ({1}) from file ({2})",
                    args.Name, a.Assembly.FullName, a.FilePath);
                return a.Assembly;
            }
            else
            {
                _logger.WarnFormat("Can't resolve assembly ({0})", args.Name);
            }

            return null;
        }
        
        protected static Assembly TypeResolveEventHandler(object sender, ResolveEventArgs args)
        {
            AssemblyCatalog a = _root.ResolveType(args.Name);
            if (a != null)
            {
                _logger.DebugFormat("Type resolved: type ({0}) in assembly ({1}) from file ({2})",
                    args.Name, a.Assembly.FullName, a.FilePath);
                return a.Assembly;
            }
            else
            {
                _logger.WarnFormat("Can't resolve type ({0})", args.Name);
            }

            return null;
        }
    }
}
