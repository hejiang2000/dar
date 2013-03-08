// Author: He Jiang (mailto:hejiang@tju.edu.cn)

using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using DAR.Internal;

namespace DAR.Catalog
{
    public class AggregateCatalog<T> : IEnumerable<T>, ICatalog where T : ICatalog
    {
        private readonly Lock _thisLock = new Lock();
        private List<T> _catalogs = new List<T>();

        public void Add(T t)
        {
            using (new WriteLock(_thisLock))
            {
                _catalogs.Add(t);
            }
        }

        public bool Remove(T t)
        {
            using (new WriteLock(_thisLock))
            {
                return _catalogs.Remove(t);
            }
        }
        
        public AssemblyCatalog ResolveAssembly(string assemblyName)
        {
            List<AssemblyCatalog> l = new List<AssemblyCatalog>();
            foreach (T cat in this)
            {
                AssemblyCatalog a = cat.ResolveAssembly(assemblyName);
                if (a != null)
                    l.Add(a);
            }

            if (l.Count > 0)
            {
                AssemblyCatalog ca = l[0];
                string cv = ca.Assembly.FullName.Split(',')[1].Split('=')[1];
                if (l.Count > 1)
                {
                    l.ForEach(ia =>
                    {
                        string iv = ia.Assembly.FullName.Split(',')[1].Split('=')[1];
                        if (iv.CompareTo(cv) > 0)
                            ca = ia;
                    });
                }
                return ca;
            }
            return null;
        }
        
        public AssemblyCatalog ResolveType(string typeName)
        {
            List<AssemblyCatalog> l = new List<AssemblyCatalog>();
            foreach (T cat in this)
            {
                AssemblyCatalog a = cat.ResolveType(typeName);
                if (a != null)
                    l.Add(a);
            }

            if (l.Count > 0)
            {
                AssemblyCatalog ca = l[0];
                string cv = ca.Assembly.FullName.Split(',')[1].Split('=')[1];
                if (l.Count > 1)
                {
                    l.ForEach(ia =>
                    {
                        string iv = ia.Assembly.FullName.Split(',')[1].Split('=')[1];
                        if (iv.CompareTo(cv) > 0)
                            ca = ia;
                    });
                }
                return ca;
            }
            return null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(_catalogs.GetEnumerator(), _thisLock);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class Enumerator : IEnumerator<T>
        {
            IEnumerator<T> _enum;
            private readonly Lock _lock = new Lock();
            private int _isDisposed;

            internal Enumerator(IEnumerator<T> @enum, Lock @lock)
            {
                _enum = @enum;
                _lock = @lock;
                this._lock.EnterReadLock();
            }

            public T Current
            {
                get { return _enum.Current; }
            }

            public void Dispose()
            {
                if (Interlocked.CompareExchange(ref this._isDisposed, 1, 0) == 0)
                {
                    this._lock.ExitReadLock();
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get { return _enum.Current; }
            }

            public bool MoveNext()
            {
                return _enum.MoveNext();
            }

            public void Reset()
            {
                _enum.Reset();
            }
        }
    }
}
