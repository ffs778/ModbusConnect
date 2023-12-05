using NModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ModbusConnect
{
    public class ModbusTCP
    {
        public string IPAdress { get; set; }
        public int Port { get; set; }

        public bool Connected
        {
            get => tcpClient.Connected;
        }
        private ModbusFactory modbusFactory;
        private IModbusMaster master;
        private TcpClient tcpClient;
        public ModbusTCP(string ip, int port)
        {
            IPAdress = ip;
            Port = port;

            modbusFactory = new ModbusFactory();
            tcpClient = new TcpClient(IPAdress, Port);
            master = modbusFactory.CreateMaster(tcpClient);
            master.Transport.ReadTimeout = 1000;    // 读数据超时时间
            master.Transport.WriteTimeout = 1000;    // 写数据超时时间
            master.Transport.Retries = 10;
            master.Transport.WaitToRetryMilliseconds = 2500;
        }
        public void DisConnect()
        {
            tcpClient?.Close();
        }

        #region 读写保持寄存器
        /// <summary>
        /// 读保存寄存器
        /// </summary>
        /// <param name="slaveAddress"></param>
        /// <param name="startAddress"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public ushort[] ReadHoldingRegisters(byte slaveAddress, ushort startAddress, ushort num)
        {
            return master.ReadHoldingRegisters(slaveAddress, startAddress, num);
        }
        /// <summary>
        /// 写单个寄存器
        /// </summary>
        /// <param name="slaveAddress"></param>
        /// <param name="startAddress"></param>
        /// <param name="value"></param>
        public void WriteSingleRegister(byte slaveAddress, ushort startAddress, ushort value)
        {
            master.WriteSingleRegister(slaveAddress, startAddress, value);
        }
        /// <summary>
        /// 写多个寄存器
        /// </summary>
        /// <param name="slaveAddress"></param>
        /// <param name="startAddress"></param>
        /// <param name="value"></param>
        public void WriteMultipleRegisters(byte slaveAddress, ushort startAddress, ushort[] value)
        {
            master.WriteMultipleRegisters(slaveAddress, startAddress, value);
        }
        #endregion

        #region 读写线圈
        /// <summary>
        /// 读线圈
        /// </summary>
        /// <param name="slaveAddress"></param>
        /// <param name="startAddress"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool[] ReadCoils(byte slaveAddress, ushort startAddress, ushort num)
        {
            return master.ReadCoils(slaveAddress, startAddress, num);
        }
        /// <summary>
        /// 写单个线圈
        /// </summary>
        /// <param name="slaveAddress"></param>
        /// <param name="startAddress"></param>
        /// <param name="value"></param>
        public void WriteSingleCoil(byte slaveAddress, ushort startAddress, bool value)
        {
            master.WriteSingleCoil(slaveAddress, startAddress, value);
        }

        /// <summary>
        /// 写多个线圈
        /// </summary>
        /// <param name="slaveAddress"></param>
        /// <param name="startAddress"></param>
        /// <param name="value"></param>
        public void WriteMultipleCoils(byte slaveAddress, ushort startAddress, bool[] value)
        {
            master.WriteMultipleCoils(slaveAddress, startAddress, value);
        }
        #endregion

        #region 读输入点
        /// <summary>
        /// 读输入点
        /// </summary>
        /// <param name="slaveAddress"></param>
        /// <param name="startAddress"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool[] ReadInputs(byte slaveAddress, ushort startAddress, ushort num)
        {
            return master.ReadInputs(slaveAddress, startAddress, num);
        }

        /// <summary>
        /// 读输入寄存器
        /// </summary>
        /// <param name="slaveAddress"></param>
        /// <param name="startAddress"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public ushort[] ReadInputRegisters(byte slaveAddress, ushort startAddress, ushort num)
        {
            return master.ReadInputRegisters(slaveAddress, startAddress, num);
        } 
        #endregion
    }
}
