using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusConnect
{
    enum FunctionCode
    {


    }
    class 汇川客户Class2
    {
        /// <summary>
        /// 读取命令
        /// </summary>
        /// <param name="address"></param>
        /// <param name="stationNumber"></param>
        /// <param name="functionCode"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] GetReadCommand(ushort address, byte stationNumber, FunctionCode functionCode, ushort length)
        {
            byte[] buffer = new byte[12];
            buffer[0] = 0x19;
            buffer[1] = 0xB2;       // 19 B2 两个字节，表示是Client发出的检验信息
            buffer[2] = 0x00;
            buffer[3] = 0x00;       // 00 00 两个字节，表示是tcp/ip协议的modbus通讯。
            buffer[4] = 0x00;
            buffer[5] = 0x06;       // 00 06 表示header handle后面还有6个字节。 前六个字节就代表header handler。
            buffer[6] = stationNumber;  // 一个字节表示 从站号
            buffer[7] = (byte)functionCode;     // 一个字节表示 功能码
            Array.Copy(BitConverter.GetBytes(address).Reverse().ToArray(), 0, buffer, 8, 2);    // 两个字节表示Client Request 寄存器地址
            Array.Copy(BitConverter.GetBytes(length).Reverse().ToArray(), 0, buffer, 10, 2);    // 两个字节表示Client Request 寄存器个数
            return buffer;      // 注意BitConvert.GetBytes 转换后的值 [0]代表最低位 ushort 返回包含2个元素的byte数组，即16位无符号整数值。
        }
        /// <summary>
        /// 写入命令
        /// </summary>
        /// <param name="address"></param>
        /// <param name="stationNumber"></param>
        /// <param name="functionCode"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public byte[] GetWriteCommand(ushort address, byte stationNumber, FunctionCode functionCode, byte[] values)
        {
            byte[] buffer = new byte[13 + values.Length];
            buffer[0] = 0x19;
            buffer[1] = 0xB2;
            buffer[2] = 0x00;
            buffer[3] = 0x00;        // 同上
            buffer[4] = BitConverter.GetBytes(7 + values.Length)[1];
            buffer[5] = BitConverter.GetBytes(7 + values.Length)[0];
            buffer[6] = stationNumber;
            buffer[7] = (byte)functionCode;
            buffer[8] = BitConverter.GetBytes(address)[1];
            buffer[9] = BitConverter.GetBytes(address)[0];
            buffer[10] = (byte)(values.Length / 2 / 256);
            buffer[11] = (byte)(values.Length / 2 % 256);
            buffer[12] = (byte)(values.Length);
            values.CopyTo(buffer, 13);
            return buffer;
        }
    }
}
