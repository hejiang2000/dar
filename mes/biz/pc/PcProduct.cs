using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MES.Intf;
using MES.Biz.Bom;

namespace MES.Biz.Pc
{
    public static class PcStatus
    {
        public static readonly string MoveIn = "MoveIn";
        public static readonly string InProc = "InProc";
        public static readonly string MoveOut = "MoveOut";
    }

    public class PcProduct: Product
    {
        public string ModelNo;
        public string SerialNo;
        public string WC;
        public string Status;

        public override bool CanBind(Part part)
        {
            // 取得产品BOM
            //BomItem bom = BomManager.Inst.GetBOM( ModelNo);

            // 取得已收集该类型的料品
            string attrType = part.PartNo;
            //PcAttr[] binds = PcManager.Inst.GetProperties(this, attrType);

            // 检查是否还需要绑定当前part
            // todo
            throw new NotImplementedException();
        }

        public override void Bind(Part part)
        {
            string attrType = part.PartNo;
            string attrValue = part.PartSN;
            string attrDesc = part.PartDesc;

            //PcManager.Inst.AddAttr(this, attrType, attrValue, attrDesc);
        }

    }
}
