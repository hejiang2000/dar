using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MES.Intf
{
    public static class ServiceHelper
    {
        public static IService GetService(Type type)
        {
            Type t = typeof(SvcWrapper<>).MakeGenericType(type);
            object o = Activator.CreateInstance(t);

            return (IService)o;
        }
    }

    class SvcWrapper<T> : IService where T : class, new()
    {
        private T _t = new T();

        public object Execute(string command, params object[] args)
        {
            MethodInfo mi = typeof(T).GetMethod(command);
            if (mi != null)
                return mi.Invoke(_t, args);

            throw new InvalidOperationException();
        }
    }
}
