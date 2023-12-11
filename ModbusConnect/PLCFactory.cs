using System;
using System.Collections.Generic;
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
        public static PLC ConnectBeckhoffPLC(string AMSNetIP, int port)
        {
            return null;
        }
        public static PLC ConnectInovance(string ip ,int port = 502,byte address = 1)
        {
            return new ModbusTCPHelper(ip, port,address);
        }
    }
}
