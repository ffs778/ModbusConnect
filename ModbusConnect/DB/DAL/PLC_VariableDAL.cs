using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCConnect
{
    public class PLC_VariableDAL
    {
        public static DataTable GetData()
        {
            string commandText = $"select * from PLC_Variable;";
            return DBHelper.Instance.CheckSQL(commandText);
        }
    }
}
