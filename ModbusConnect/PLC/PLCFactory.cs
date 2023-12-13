using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCConnect
{
    enum PLCType
    {
        Beckhoff,
        Inovance
    }
    public class PLCFactory
    {
        DataTable _variableDt;
        public PLCFactory(DataTable variableDt)
        {
            _variableDt = variableDt;
        }
        public PLC ConnectBeckhoffPLC(string AMSNetIP, int port)
        {
            return null;
        }
        public PLC ConnectInovance(string ip ,int port = 502,byte address = 1)
        {
            var plc = new ModbusTCPHelper(ip, port,address);
            plc.InitialDataDic(_variableDt);
            //plc.MonitorPLCHeart();
            //plc.SendPCHeart();
            return plc;
        }
    }
}
