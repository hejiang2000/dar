using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MES.Biz.Bom;
using System.Configuration;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MES.DB"].ConnectionString;
            string providerName = ConfigurationManager.ConnectionStrings["MES.DB"].ProviderName;

            BomManager.Initialize(connStr, providerName);

            BomItem[] items = BomManager.Inst.GetBomByPartNo("PCAS02ADN00Y");

        }
    }
}
