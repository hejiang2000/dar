using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MES.Intf
{
    /// <summary>
    /// 产品零件类型
    /// </summary>
    public abstract class Part : Entity
    {
        public string PartNo;
        public string PartSN;
        public string PartDesc;

        /// <summary>
        /// 确认该零件可用并锁定零件
        /// </summary>
        /// <param name="product">将要绑定的产品实例</param>
        /// <returns>该零件是否可用</returns>
        public virtual bool BeforeBindTo(Product product)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 标记该零件已被使用
        /// </summary>
        /// <param name="product">已使用该零件的产品实例</param>
        public virtual void AfterBindTo(Product product)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class PartManager<T> where T : Part
    {
        /// <summary>
        /// 通过scanCode取得Part实例。
        /// scanCode是可以识别Part的信息，根据具体产品而定。
        /// </summary>
        /// <param name="scanCode">识别Part的信息</param>
        /// <param name="parentProduct">父产品，帮助识别Product</param>
        /// <returns></returns>
        public virtual T GetPart(string scanCode, Product parentProduct)
        {
            throw new NotImplementedException();
        }

        public virtual T AddPart(string partNo, string partSN, string partDesc)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdatePart(T part)
        {
            throw new NotImplementedException();
        }

        public virtual void DeletePart(T part)
        {
            throw new NotImplementedException();
        }
    }
}
