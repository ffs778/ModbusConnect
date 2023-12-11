using NModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PLCConnect
{
    public class ModbusTCPHelper: PLC
    {
        #region 私有字段
        private ModbusFactory _modbusFactory;
        private IModbusMaster _master;
        private TcpClient _tcpClient;
        #endregion

        #region 事件
        private event EventHandler OnDisConnecting;   // 监控此Ads对象是否处于未连接状态执行重连逻辑。
        #endregion

        #region 公开属性
   
        public byte SlaveAddress { get; set; }
        #endregion

        public ModbusTCPHelper(string ip, int port = 502, byte slaveAddress = 1)
        {
            IP = ip;
            Port = port;
            SlaveAddress = slaveAddress;
            Connect();
            OnDisConnecting += ModbusTCPHelper_OnDisConnecting;
            Task.Factory.StartNew(() => { MonitorPLCHeart(); }, TaskCreationOptions.LongRunning);
            Task.Factory.StartNew(() => { SendPCHeart(); }, TaskCreationOptions.LongRunning);
        }
        public override void Connect()
        {
            try
            {
                _modbusFactory = new ModbusFactory();
                _tcpClient = new TcpClient(IP, Port);
                _master = _modbusFactory.CreateMaster(_tcpClient);
                _master.Transport.ReadTimeout = 1000;    // 读数据超时时间
                _master.Transport.WriteTimeout = 1000;    // 写数据超时时间
                _master.Transport.Retries = 10;
                _master.Transport.WaitToRetryMilliseconds = 2500;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        public override void DisConnect()
        {
            _tcpClient?.Close();
        }

        private void ModbusTCPHelper_OnDisConnecting(object sender, EventArgs e)
        {
            DisConnect();
            Connect();
        }

        /// <summary>
        /// 监控PLC心跳
        /// </summary>
        protected void MonitorPLCHeart()
        {
            List<bool> heartLst = new();
            while (true)
            {
                if (!IsShieldPLCHeart) // 心跳屏蔽状态
                {
                    // 监控PLC心跳，如果脉冲长时间不变化，则说明断线
                    if (heartLst.Count <= 5)
                    {
                        heartLst.Add(DataDic["PLCHeart"].Value);
                    }
                    else
                    {
                        if (heartLst.Distinct().Count() == 1)
                        {
                            // list中只有一种bool类型，即心跳断开
                            IsConnected = false;
                            OnDisConnecting.Invoke(true, EventArgs.Empty);   // 表示捕捉到ads通讯未连接，执行事件
                        }
                        else
                        {
                            IsConnected = true;
                        }
                        heartLst.Clear();
                    }
                }
                else
                {
                    IsConnected = true;    // 屏蔽心跳，强制为已连接状态。
                }
                Thread.Sleep(300);
            }
        }
        /// <summary>
        /// 发送PC心跳
        /// </summary>
        protected void SendPCHeart()
        {
            bool heart = false;
            while (true)
            {
                Write(DataDic["PCHeart"], !heart);
                Thread.Sleep(1000);
            }
        }

        #region 读写变量

        public override void Write(VariableModel model, object value)
        {
            var varModel = model as SiteTypeVarModel;
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
            _master.WriteMultipleRegisters(SlaveAddress, startAddress, buff);  // 最多写123个寄存器，即256字节
        }

        public override dynamic Read(VariableModel model)
        {
            var varModel = model as SiteTypeVarModel;
            ushort startAddress = Convert.ToUInt16(varModel.StartAddress);
            ushort length = Convert.ToUInt16(varModel.Length);
            return ReadHoldingRegister(startAddress, varModel.DataType, length);
        }
        /// <summary>
        /// 读取保持寄存器中的数据
        /// 保持寄存器是字寄存器（16位）
        /// </summary>
        private string ReadHoldingRegister(ushort startAddresss, string dataType, ushort num)
        {
            ushort[] buff = _master.ReadHoldingRegisters(SlaveAddress, startAddresss, num);
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
        #endregion
    }
}
