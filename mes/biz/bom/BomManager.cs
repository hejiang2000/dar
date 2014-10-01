using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MES.Biz.Db;
using System.Data.Common;
using MES.Intf;

namespace MES.Biz.Bom
{
    /// <summary>
    /// BomManager负责获取BOM
    /// 并在可能情况下提供对BomItem的Cache功能
    /// </summary>
    public class BomManager
    {
        private static BomManager _inst;
        public static BomManager Inst { get { return _inst; } }
        public static void Initialize(string connStr, string providerName)
        {
            _inst = new BomManager(connStr, providerName);
        }

        protected readonly string _sql_get_bom_by_partno = "SELECT * FROM [MES_BOM] WHERE [PartNo] = @PartNo";
        protected readonly string _sql_get_bom_by_parentpn = "SELECT * FROM [MES_BOM] WHERE [ParentPN] = @ParentPN";
        protected readonly string _sql_get_bom_property_by_partno = "SELECT * FROM [MES_BOM_PROPERTY] WHERE [PartNo] = @PartNo";

        protected DbHelper _db_helper;

        protected BomManager(string connStr, string providerName)
        {
            _db_helper = new DbHelper(connStr, providerName);
        }

        /// <summary>
        /// 通过PartNo获取BOM
        /// </summary>
        /// <param name="partNo">BOM的PartNo</param>
        /// <returns>完整的BOM资料信息</returns>
        public BomItem[] GetBomByPartNo(string partNo)
        {
            BomItem[] items = null;
            using (DbCommand cmd = _db_helper.CreateCommand(_sql_get_bom_by_partno, System.Data.CommandType.Text))
            {
                _db_helper.AddDbCommandParameter(cmd, "@PartNo", System.Data.DbType.String,
                    System.Data.ParameterDirection.Input, partNo);
                items = _db_helper.ExecuteReader<BomItem>(cmd);
            }

            foreach (BomItem i in items)
            {
                i.Properties = GetBomProperty(i.PartNo);
                i.SubItems = GetBomByParentPN(i.PartNo);
            }

            return items;
        }

        /// <summary>
        /// 通过ParentPN获取BOM
        /// </summary>
        /// <param name="parentPN">BOM的ParentPN</param>
        /// <returns>完整的BOM资料信息</returns>
        public BomItem[] GetBomByParentPN(string parentPN)
        {
            BomItem[] items;

            using (DbCommand cmd = _db_helper.CreateCommand(_sql_get_bom_by_parentpn, System.Data.CommandType.Text))
            {
                _db_helper.AddDbCommandParameter(cmd, "@ParentPN",System.Data.DbType.String,
                    System.Data.ParameterDirection.Input, parentPN);
                items = _db_helper.ExecuteReader<BomItem>(cmd);
            }

            foreach (BomItem i in items)
            {
                i.Properties = GetBomProperty(i.PartNo);
                i.SubItems = GetBomByParentPN(i.PartNo);
            }

            return items;
        }

        public BomProperty[] GetBomProperty(string partNo)
        {
            using (DbCommand cmd = _db_helper.CreateCommand(_sql_get_bom_property_by_partno, System.Data.CommandType.Text))
            {
                _db_helper.AddDbCommandParameter(cmd, "@PartNo", System.Data.DbType.String,
                    System.Data.ParameterDirection.Input, partNo);
                BomProperty[] props = _db_helper.ExecuteReader<BomProperty>(cmd);

                return props;
            }
        }
    }
}
