using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MES.Intf;

namespace MES.Biz.Pc
{
    /// <summary>
    /// 管理PC相关的数据表:
    /// PC_Status
    /// PC_Property
    /// PC_Log
    /// 
    /// 所有的数据操作都通过Manager最后实际完成：
    /// 1. 创建/读取/更新/删除 PC产品(状态表)
    /// 2. 创建/读取/更新/删除 PC属性(属性表)
    /// 3. 创建/读取/更新/删除 PC日志(日志表)
    /// </summary>
    public class PcManager : ProductManager<PcProduct>
    {
        private static PcManager _inst;
        public static PcManager Inst
        {
            get
            {
                return _inst;
            }
        }

        /// <summary>
        /// 这里可以传入参数，初始化实例
        /// </summary>
        public static void Initialize()
        {
            _inst = new PcManager();
        }

        /// <summary>
        /// 识别PC产品: 读取 PC产品(状态表)
        /// </summary>
        /// <param name="scanCode"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public override PcProduct GetProduct(string scanCode, Product parentProduct)
        {
            throw new NotImplementedException();
        }

        public override PcProduct AddProduct(string model, string serialNo, string desc)
        {
            throw new NotImplementedException();
        }

        public override void UpdateProduct(PcProduct product)
        {
            throw new NotImplementedException();
        }

        public override void DeleteProduct(PcProduct product)
        {
            throw new NotImplementedException();
        }

    }
}
