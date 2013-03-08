// Author: He Jiang (mailto:hejiang@tju.edu.cn)

using System.Reflection;
using DAR.Catalog;

namespace DAR
{
    public interface ICatalog
    {
        ///
        /// ResolveAssembly causes the assembly to be tracked by
        /// system, then it would be failed to change
        ///
        AssemblyCatalog ResolveAssembly(string assemblyName);
        AssemblyCatalog ResolveType(string typeName);
    }
}
