// Author: He Jiang (mailto:hejiang@tju.edu.cn)

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;

namespace DAR.Catalog
{
    public class AssemblyCatalog : ICatalog
    {
        private static readonly ILog _logger = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);
        protected Assembly _assembly;
        protected string _filePath;

        internal string FilePath { get { return _filePath; } set { _filePath = value; } }
        internal Assembly Assembly { get { return _assembly; } }

        public AssemblyCatalog(string filePath)
        {
            _assembly = LoadAssembly(filePath);
            _filePath = filePath;
        }

        public AssemblyCatalog ResolveAssembly(string assemblyName)
        {
            if (_assembly == null)
                _assembly = LoadAssembly(_filePath);

            if (_assembly != null && IsAssemblyNameMatched(_assembly.FullName, assemblyName))
                return this;

            return null;
        }

        public AssemblyCatalog ResolveType(string typeName)
        {
            if (_assembly == null)
                _assembly = LoadAssembly(_filePath);

            if (_assembly != null && _assembly.GetType(typeName) != null)
                return this;

            return null;
        }

        public static Assembly LoadAssembly(string filePath)
        {
            Assembly assembly = null;
            try
            {
                byte[] rawAssembly = File.ReadAllBytes(filePath);
                if (rawAssembly != null && rawAssembly.Length > 0)
                {
                    assembly = Assembly.Load(rawAssembly);
                    _logger.InfoFormat("Assembly ({0}) loaded from file ({1})",
                        assembly.FullName, filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Fail to load assembly from file ({0}): {1}", filePath, ex.Message);
            }

            return assembly;
        }
        
        protected static bool IsAssemblyNameMatched(string fullAssemblyName, string resolvingAssemblyName)
        {
            string[] resolvingNameParts = resolvingAssemblyName.Split(',').Select(s => s.Trim()).ToArray();
            string[] fullNameParts = fullAssemblyName.Split(',').Select(s => s.Trim()).ToArray();

            if (resolvingNameParts.Length > fullNameParts.Length)
                return false;

            for (int i = 0; i < resolvingNameParts.Length; ++i)
            {
                if (!resolvingNameParts[i].Equals(fullNameParts[i],
                    StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }

            return true;
        }
        
    }
}
