using Modbus.Device;
using Modbus.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModbusConnect
{
    public partial class Form1 : Form
    {
        ModbusTCP modbusTCP;
        public Form1()
        {
            InitializeComponent();
            dataType_cbx.SelectedIndex = 0;
        }
        #region 连接

        private void Connect_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (modbusTCP != null) modbusTCP.DisConnect();
                modbusTCP = new ModbusTCP(ip_tbx.Text, int.Parse(port_tbx.Text));
                string resultMsg = modbusTCP.Connected ? "连接成功！" : "连接失败";
                MessageBox.Show(resultMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"连接错误：{ex.Message}");
            }
        }
        #endregion
        private void GetData_tbx_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (modbusTCP != null && modbusTCP.Connected && autoRead_cbx.Checked)
            {
                RefreshData();
            }
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="data"></param>
        private bool RefreshData()
        {
            try
            {
                var data = GetRegisterData();
                // data 是ushort类型的数组，需要转byte数组，再转成相应的类型。
                // 类型转换后显示
                switch (dataType_cbx.SelectedItem.ToString())
                {
                    case "bool": boolValue_tbx.Text = string.Join(" ", MODBUS.GetBool(data)); break;
                    case "short": shortValue_tbx.Text = MODBUS.GetShort(data, 0).ToString(); break;
                    case "float": floatValue_tbx.Text = MODBUS.GetReal(data, 0).ToString(); break;
                    case "string": stringValue_tbx.Text = MODBUS.GetWString(data); break;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取数据错误：" + ex.Message);
                return false;
            }
        }
        private ushort[] GetRegisterData()
        {
            // 获得原始数据
            byte slaveAddress = byte.Parse(slaveAddress_tbx.Text);
            ushort startAddress = ushort.Parse(startAddress_tbx.Text);
            ushort num = ushort.Parse(num_tbx.Text);
            var data = modbusTCP.ReadHoldingRegisters(slaveAddress, startAddress, num);  // 站号， 数据起始地址,寄存器数量
            return data;
        }
        /// <summary>
        /// 向寄存器中写入数据
        /// </summary>
        /// <param name="data"></param>
        private void WriteRegisterData(ushort[] data)
        {
            byte slaveAddress = byte.Parse(slaveAddress_tbx.Text);
            ushort startAddress = ushort.Parse(startAddress_tbx.Text);
            modbusTCP.WriteMultipleRegisters(slaveAddress, startAddress, data);
        }
        private void WriteData_tbx_Click(object sender, EventArgs e)
        {
            switch (dataType_cbx.SelectedItem.ToString())
            {
                case "string":
                    ushort num = ushort.Parse(num_tbx.Text);
                    WriteRegisterData(MODBUS.SetWString(num, stringValue_tbx.Text));break;
                case "float":
                    WriteRegisterData(MODBUS.SetReal(Convert.ToSingle(floatValue_tbx.Text)));break;
                case "short":
                    WriteRegisterData(MODBUS.SetShort(Convert.ToInt16(shortValue_tbx.Text)));break;
                case "bool":
                    WriteRegisterData(MODBUS.SetBool(Convert.ToBoolean(boolValue_tbx.Text)));break;
            }

        }
    }
}
