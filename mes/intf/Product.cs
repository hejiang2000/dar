using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MES.Intf
{
    public class ProductProperty : Entity
    {
        /// <summary>
        /// 对于组装属性，PropType就是零件的PartNo
        /// </summary>
        public string PropType;

        /// <summary>
        /// 对于组装属性，PropValue就是零件的SerialNo
        /// </summary>
        public string PropValue;

        /// <summary>
        /// 对于组装属性，PropDesc就是零件的Properties
        /// </summary>
        public string PropDesc;
    }

    public class ProductLog : Entity
    {
        public string LogType;
        public string LogValue;
        public string LogDesc;
    }

    /// <summary>
    /// 产品类型
    /// 
    /// 零件组装步骤:
    /// 1. 识别零件 Part MaterialManager.Recognize(string scanCode, Product product)
    /// 2. 确认需要 bool Product.CanBind(Part part)
    /// 3. 开始组装 bool Part.BeforeBindTo(Product product)
    /// 4. 零件组装 void Product.Bind(Part part)
    /// 5. 完成组装 void Part.AfterBindTo(Product product)
    /// </summary>
    public abstract class Product : Part
    {
        /// <summary>
        /// 确认零件可组装进当前产品
        /// </summary>
        /// <param name="part">零件实例</param>
        /// <returns>零件是否可用组装进当前产品</returns>
        public virtual bool CanBind(Part part)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将零件组装进当前产品
        /// </summary>
        /// <param name="part">零件实例</param>
        public virtual void Bind(Part part)
        {
            throw new NotImplementedException();
        }

        public virtual ProductProperty[] GetProperties(string propType)
        {
            throw new NotImplementedException();
        }

        public virtual ProductProperty AddProperty(string propType, string propValue, string propDesc)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateProperty(ProductProperty prop)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteProperty(ProductProperty prop)
        {
            throw new NotImplementedException();
        }

        public virtual ProductLog[] GetLogs(string logType)
        {
            throw new NotImplementedException();
        }

        public virtual ProductLog AddLog(string logType, string logValue, string logDesc)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteLog(ProductLog log)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class ProductManager<T> where T : Product
    {
        /// <summary>
        /// 通过scanCode取得Product实例。
        /// scanCode是可以识别Product的信息，根据具体产品而定。
        /// </summary>
        /// <param name="scanCode">识别Product的信息</param>
        /// <param name="parentProduct">父产品，帮助识别Product</param>
        /// <returns></returns>
        public virtual T GetProduct(string scanCode, Product parentProduct)
        {
            throw new NotImplementedException();
        }

        public virtual T AddProduct(string model, string serialNo, string desc)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateProduct(T product)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteProduct(T product)
        {
            throw new NotImplementedException();
        }
    }
}
