// Author: He Jiang (mailto:hejiang@tju.edu.cn)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MES.Intf
{
    /// <summary>
    /// Entity describes a general object
    /// </summary>
    public class Entity
    {
        protected Dictionary<string, object> _attrs = new Dictionary<string, object>();
        public int ID;

        public object this[string name]
        {
            get
            {
                FieldInfo fi = this.GetType().GetField(name);
                if (fi != null)
                    return fi.GetValue(this);

                if (_attrs.ContainsKey(name))
                    return _attrs[name];

                throw new Exception(string.Format("Invalid attribute name ({0})", name));
            }
            set
            {
                FieldInfo fi = this.GetType().GetField(name);
                if (fi != null)
                {
                    fi.SetValue(this, value);
                    return;
                }

                _attrs[name] = value;
            }
        }

        public T ConvertTo<T>() where T : Entity, new()
        {
            T t;

            if (typeof(T).IsAssignableFrom(this.GetType()))
            {
                t = (T)this;
                return t;
            }

            t = new T();

            AssignTo(t);

            return t;
        }

        public void AssignTo<T>(T t) where T : Entity
        {
            foreach (var f in this.GetType().GetFields())
            {
                AssignAttr<T>(t, f.Name, f.GetValue(this));
            }

            foreach (var a in this._attrs)
            {
                AssignAttr<T>(t, a.Key, a.Value);
            }
        }

        private void AssignAttr<T>(T t, string attrName, object srcValue) where T : Entity
        {
            if (srcValue == null || srcValue == DBNull.Value)
            {
                t[attrName] = null;
                return;
            }

            Type srcType = srcValue.GetType();
            FieldInfo dstField = typeof(T).GetField(attrName);
            if (dstField == null)
            {
                t[attrName] = srcValue;
                return;
            }

            Type dstType = dstField.FieldType;
            if (dstType.IsAssignableFrom(srcType))
            {
                t[attrName] = srcValue;
                return;
            }

            if (typeof(Entity).IsAssignableFrom(srcType) && typeof(Entity).IsAssignableFrom(dstType))
            {
                MethodInfo miConvertTo = typeof(Entity).GetMethod("ConvertTo").MakeGenericMethod(dstType);
                t[attrName] = miConvertTo.Invoke(srcValue, null);
                return;
            }

            if (typeof(Entity[]).IsAssignableFrom(srcType) && typeof(Entity[]).IsAssignableFrom(dstType))
            {
                Type dstElementType = dstType.GetElementType();

                Array srcArray = (Array)srcValue;
                Array dstArray = Array.CreateInstance(dstElementType, srcArray.Length);

                MethodInfo miConvertTo = typeof(Entity).GetMethod("ConvertTo").MakeGenericMethod(dstElementType);
                for (int i = 0; i < srcArray.Length; ++i)
                {
                    object dstElementValue = miConvertTo.Invoke(srcArray.GetValue(i), null);
                    dstArray.SetValue(dstElementValue, i);
                }

                t[attrName] = dstArray;
                return;
            }

            t[attrName] = Convert.ChangeType(srcValue, dstType);
        }
    }
}
