// Author: He Jiang (mailto:hejiang@tju.edu.cn)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MES.Intf
{
    public interface IService
    {
        object Execute(string command, params object[] args);
    }
}
