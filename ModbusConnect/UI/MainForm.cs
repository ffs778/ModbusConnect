﻿using Modbus.Device;
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

namespace PLCConnect
{
    public partial class MainForm : Form
    {
        DataTable dt;
        PLC _plc;
        Dictionary<string, VariableModel> _varDic = new();
        public MainForm()
        {
            InitializeComponent();
            plcType_cbx.SelectedIndex = 1;
            ip_cbx.SelectedIndex = 0;

            dt = PLC_VariableDAL.GetData();
            dataGridView1.DataSource = dt;
            dataGridView2.DataSource = dt;
        }
        #region 连接

        private void Connect_btn_Click(object sender, EventArgs e)
        {
            try
            {
                PLCFactory pLCFactory = new PLCFactory(dt);
                switch (plcType_cbx.Text)
                {
                    case "Beckhoff":
                        _plc = pLCFactory.ConnectBeckhoffPLC(ip_cbx.Text, int.Parse(port_tbx.Text)); break;
                    case "Inovance":
                        _plc = pLCFactory.ConnectInovance(ip_cbx.Text, int.Parse(port_tbx.Text), byte.Parse(slaveAddress_tbx.Text));
                        break;

                    default: throw new Exception("plc类型出现错误！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"连接错误：{ex.Message}");
            }
        }
        #endregion

        #region 读
        private void GetData_tbx_Click(object sender, EventArgs e)
        {
            if (_plc == null || !_plc.IsConnected) return;
            dataGridView1.DataSource = GetRefreshData();
            dataGridView2.DataSource = GetRefreshData();
        }
        public DataTable GetRefreshData()
        {
            // 跟据每一行变量的地址信息，从PLC中读取后写入到value列
            DataTable dtSrc = dataGridView1.DataSource as DataTable;
            DataTable dt = dtSrc.Copy();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SiteTypeVarModel model = new SiteTypeVarModel()
                {
                    VariableName = dt.Rows[i]["VariableName"].ToString(),
                    DataType = dt.Rows[i]["DataType"].ToString(),
                    StartAddress = dt.Rows[i]["StartAddress"].ToString(),
                    Length = dt.Rows[i]["Length"].ToString(),
                };
                _varDic[model.VariableName] = model;
            }
            var keys = _varDic.Keys.ToArray();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var temp = _plc.Read(keys[i]);
                if (_varDic[keys[i]].DataType.Contains("[]"))
                {
                    dt.Rows[i]["Value"] = string.Join(',', temp);
                }
                else
                {
                    dt.Rows[i]["Value"] = temp;
                }
            }
            return dt;
        }
        #endregion

        #region 写

        private void Wirte_btn_Click(object sender, EventArgs e)
        {
            if (_plc == null || !_plc.IsConnected) return;
            // 根据每个变量的信息，将value列的值写入到PLC中。
            DataTable dt = dataGridView1.DataSource as DataTable;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string dataType = dt.Rows[i]["DataType"].ToString();
                string value = dt.Rows[i]["Value"].ToString();
                _plc.Write(dt.Rows[i]["VariableName"].ToString(), GetDataByType(dataType, value));
            }
        }
        private dynamic GetDataByType(string dataType, string value)
        {
            switch (dataType)
            {
                case "bool": return Convert.ToBoolean(value);
                case "bool[]": return value.Split(',').Select(x => Convert.ToBoolean(x)).ToArray();
                case "short": return Convert.ToInt16(value);
                case "short[]": return value.Split(',').Select(x => Convert.ToInt16(x)).ToArray();
                case "float": return Convert.ToSingle(value);
                case "float[]": return value.Split(',').Select(x => Convert.ToSingle(x)).ToArray();
                case "string": return value;
                case "wstring": return value;
                default: return null;
            }
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 始终判断PLC连接状态
            if (_plc == null) return;
            if (_plc.IsConnected)
            {
                connectState_lab.Text = "已连接";
                connectState_lab.ForeColor = Color.Green;
            }
            else
            {
                connectState_lab.Text = "未连接";
                connectState_lab.ForeColor = Color.DarkGray;
            }

            // 始终读取
            dataGridView2.DataSource = GetRefreshData();
        }
        private void plcHeartShield_btn_Click(object sender, EventArgs e)
        {
            if (_plc == null) return;
            if (plcHeartShield_btn.Text == "屏蔽PLC心跳")
            {
                _plc.IsShieldPLCHeart = true;
                plcHeartShield_btn.Text = "取消PLC心跳屏蔽";
            }
            else
            {
                _plc.IsShieldPLCHeart = false;
                plcHeartShield_btn.Text = "屏蔽PLC心跳";
            }

        }
    }
}
