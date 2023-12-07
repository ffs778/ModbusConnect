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

namespace ModbusConnect
{
    public partial class MainForm : Form
    {
        ModbusTCPHelper _modbusTCP;
        Dictionary<string, VariableModel> _varDic = new();
        public MainForm()
        {
            InitializeComponent();
            CreateDataTable();
        }
        #region 初始化数据表

        /// <summary>
        /// 模拟变量表
        /// </summary>
        private void CreateDataTable()
        {
            string[] headers = { "变量名", "数据类型", "起始地址", "数据长度", "当前值" };
            string[] varName = { "boolVar", "shortVar", "floatVar", "stringVar", "wstringVar" };
            string[] varDataType = { "bool", "short", "float", "string", "wstring" };
            string[] startAddress = { "0", "0", "0", "0", "0" };
            string[] length = { "1", "1", "2", "10", "10" };

            DataTable dt = new();
            for (int i = 0; i < headers.Length; i++)
            {
                DataColumn dc = new DataColumn();
                dc.ColumnName = headers[i];
                dt.Columns.Add(dc);
            }
            for (int i = 0; i < varName.Length; i++)
            {
                VariableModel model = new VariableModel()
                {
                    VariableName = varName[i],
                    DataType = varDataType[i],
                    StartAddress = startAddress[i],
                    Length = length[i]
                };

                DataRow dr = dt.NewRow();
                dr.ItemArray = new object[] { model.VariableName, model.DataType, model.StartAddress, model.Length };
                dt.Rows.Add(dr);
            }
            dataGridView1.DataSource = dt;
        } 
        #endregion

        #region 连接

        private void Connect_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (_modbusTCP != null) _modbusTCP.DisConnect();
                _modbusTCP = new ModbusTCPHelper(ip_tbx.Text, int.Parse(port_tbx.Text));
                if (_modbusTCP.IsConnected)
                {
                    connectState_lab.Text = "已连接";
                    connectState_lab.ForeColor = Color.Green;
                }
                else
                {
                    connectState_lab.Text = "未连接";
                    connectState_lab.ForeColor = Color.DarkGray;
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
            if (_modbusTCP == null || !_modbusTCP.IsConnected) return;
            // 跟据每一行变量的地址信息，从PLC中读取后写入到value列
            DataTable dt = dataGridView1.DataSource as DataTable;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                VariableModel model = new VariableModel()
                {
                    VariableName = dt.Rows[i][0].ToString(),
                    DataType = dt.Rows[i][1].ToString(),
                    StartAddress = dt.Rows[i][2].ToString(),
                    Length = dt.Rows[i][3].ToString(),
                };
                _varDic[model.VariableName] = model;
            }
            _modbusTCP.GetData(ref _varDic);
            var datas = _varDic.Values.ToArray();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][4] = datas[i].Value;
            }
            dataGridView1.DataSource = dt;
        }
        #endregion

        #region 写

        private void Wirte_btn_Click(object sender, EventArgs e)
        {
            if (_modbusTCP == null || !_modbusTCP.IsConnected) return;
            // 根据每个变量的信息，将value列的值写入到PLC中。
            DataTable dt = dataGridView1.DataSource as DataTable;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                VariableModel model = new VariableModel()
                {
                    VariableName = dt.Rows[i][0].ToString(),
                    DataType = dt.Rows[i][1].ToString(),
                    StartAddress = dt.Rows[i][2].ToString(),
                    Length = dt.Rows[i][3].ToString(),
                    Value = dt.Rows[i][4].ToString()
                };
                _modbusTCP.WriteData(model, GetDataByType(model.DataType, model.Value)) ;
            }
        } 
        private dynamic GetDataByType(string dataType, string value)
        {
            switch (dataType)
            {
                case "bool": return Convert.ToBoolean(value);
                case "short": return Convert.ToInt16(value);
                case "float": return Convert.ToSingle(value);
                case "string": return value;
                case "wstring": return value;
                default: return null;
            }
        }
        #endregion
    }
}