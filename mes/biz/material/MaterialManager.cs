using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MES.Intf;

namespace MES.Biz.Material
{
    /// <summary>
    /// 材料管理器
    /// 负责识别各种材料，构建并返回正确的材料类型。
    /// </summary>
    public class MaterialManager : PartManager<Part>
    {
        /// <summary>
        /// 识别刷入的编码
        /// </summary>
        /// <param name="scanCode">刷入的编码</param>
        /// <param name="parentProduct">产品上下文。当识别刷入的产品编号时，产品上下文为null。</param>
        /// <returns></returns>
        public override Part GetPart(string scanCode, Product parentProduct)
        {
            throw new NotImplementedException();
        }
    }
}
