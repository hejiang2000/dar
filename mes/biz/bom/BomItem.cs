using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MES.Biz.Bom
{
    public class BomItem : MES.Intf.Entity
    {
        /// <summary>
        /// 父元件编号
        /// </summary>
        public string ParentPN;

        /// <summary>
        /// 元件类型
        /// </summary>
        public string Type;

        /// <summary>
        /// 元件编号
        /// </summary>
        public string PartNo;

        /// <summary>
        /// 元件的替换组名，空或空字符串表示不可替换
        /// </summary>
        public string AltGroup;

        /// <summary>
        /// 元件数量
        /// </summary>
        public int Quantity;

        public string Description;

        /// <summary>
        /// 元件属性
        /// </summary>
        public BomProperty[] Properties;

        /// <summary>
        /// 子元件列表
        /// </summary>
        public BomItem[] SubItems;
    }

    public class BomProperty : MES.Intf.Entity
    {
        public string PartNo;
        public string Type;
        public string Value;
        public string Description;
    }
}
