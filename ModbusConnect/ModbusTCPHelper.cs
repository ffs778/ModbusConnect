using NModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ModbusConnect
{
    public class ModbusTCPHelper
    {
        #region 私有字段
        private ModbusFactory _modbusFactory;
        private IModbusMaster _master;
        private TcpClient _tcpClient;
        #endregion

        #region 公开属性
        public string IP { get; set; }
        public int Port { get; set; }
        /// <summary>
        /// 从站号
        /// </summary>
        public byte SlaveAddresss { get; set; }
        public bool IsConnected => _tcpClient.Connected;
        #endregion

        public ModbusTCPHelper(string ip, int port = 502, byte slaveAddress = 1)
        {
            IP = ip;
            Port = port;
            SlaveAddresss = slaveAddress;

            _modbusFactory = new ModbusFactory();
            _tcpClient = new TcpClient(IP, Port);
            _master = _modbusFactory.CreateMaster(_tcpClient);
            _master.Transport.ReadTimeout = 1000;    // 读数据超时时间
            _master.Transport.WriteTimeout = 1000;    // 写数据超时时间
            _master.Transport.Retries = 10;
            _master.Transport.WaitToRetryMilliseconds = 2500;
        }
        /// <summary>
        /// 获得变量在PLC中的值
        /// </summary>
        /// <param name="varDic"></param>
        public void GetData(ref Dictionary<string, VariableModel> varDic)
        {
            VariableModel[] variableModels = varDic.Values.ToArray();
            foreach (var item in variableModels)
            {
                ushort startAddress = Convert.ToUInt16(item.StartAddress);
                ushort length = Convert.ToUInt16(item.Length);
                item.Value = ReadHoldingRegister(startAddress, item.DataType, length);
            }
        }
        /// <summary>
        /// 修改变量在PLC中的值
        /// </summary>
        /// <param name="var"></param>
        /// <param name="value"></param>
        public void WriteData(VariableModel varModel, object value)
        {
            ushort[] buff = null;
            switch (varModel.DataType)
            {
                case "bool": buff = MODBUS.SetBool(Convert.ToBoolean(value)); break;
                case "short": buff = MODBUS.SetShort(Convert.ToInt16(value)); break;
                case "float": buff = MODBUS.SetReal(Convert.ToSingle(value)); break;
                case "string":
                    ushort stringLength = Convert.ToUInt16(varModel.Length);
                    buff = MODBUS.SetString(stringLength, Convert.ToString(value)); break;
                case "wstring":
                    ushort wstringLength = Convert.ToUInt16(varModel.Length);
                    buff = MODBUS.SetWString(wstringLength, Convert.ToString(value)); break;
            }
            ushort startAddress = Convert.ToUInt16(varModel.StartAddress);
            _master.WriteMultipleRegisters(SlaveAddresss, startAddress, buff);  // 最多写123个寄存器，即256字节
        }
        /// <summary>
        /// 读取保持寄存器中的数据
        /// 保持寄存器是字寄存器（16位）
        /// </summary>
        private string ReadHoldingRegister(ushort startAddresss, string dataType, ushort num)
        {
            ushort[] buff = _master.ReadHoldingRegisters(SlaveAddresss, startAddresss, num);
            switch (dataType)
            {
                case "bool": return MODBUS.GetBool(buff).ToString();
                case "short": return MODBUS.GetShort(buff).ToString();
                case "float": return MODBUS.GetReal(buff).ToString();
                case "string": return MODBUS.GetString(buff); // string类型的长度
                case "wstring": return MODBUS.GetWString(buff);// wstring 类型的长度
                default: return null;
            }
        }

        public void DisConnect()
        {
            _tcpClient?.Close();
        }
    }
}
