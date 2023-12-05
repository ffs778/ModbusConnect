
namespace ModbusConnect
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ip_tbx = new System.Windows.Forms.TextBox();
            this.port_tbx = new System.Windows.Forms.TextBox();
            this.slaveAddress_tbx = new System.Windows.Forms.TextBox();
            this.boolValue_tbx = new System.Windows.Forms.TextBox();
            this.num_tbx = new System.Windows.Forms.TextBox();
            this.startAddress_tbx = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.shortValue_tbx = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.floatValue_tbx = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.stringValue_tbx = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.GetData_tbx = new System.Windows.Forms.Button();
            this.WriteData_tbx = new System.Windows.Forms.Button();
            this.Connect_btn = new System.Windows.Forms.Button();
            this.dataType_cbx = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.autoRead_cbx = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.stringLen_tbx = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(135, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "从站号";
            // 
            // ip_tbx
            // 
            this.ip_tbx.Location = new System.Drawing.Point(209, 101);
            this.ip_tbx.Name = "ip_tbx";
            this.ip_tbx.Size = new System.Drawing.Size(100, 23);
            this.ip_tbx.TabIndex = 3;
            this.ip_tbx.Text = "127.0.0.1";
            // 
            // port_tbx
            // 
            this.port_tbx.Location = new System.Drawing.Point(209, 137);
            this.port_tbx.Name = "port_tbx";
            this.port_tbx.Size = new System.Drawing.Size(100, 23);
            this.port_tbx.TabIndex = 4;
            this.port_tbx.Text = "502";
            // 
            // slaveAddress_tbx
            // 
            this.slaveAddress_tbx.Location = new System.Drawing.Point(209, 179);
            this.slaveAddress_tbx.Name = "slaveAddress_tbx";
            this.slaveAddress_tbx.Size = new System.Drawing.Size(100, 23);
            this.slaveAddress_tbx.TabIndex = 5;
            this.slaveAddress_tbx.Text = "1";
            // 
            // boolValue_tbx
            // 
            this.boolValue_tbx.Location = new System.Drawing.Point(425, 100);
            this.boolValue_tbx.Name = "boolValue_tbx";
            this.boolValue_tbx.Size = new System.Drawing.Size(100, 23);
            this.boolValue_tbx.TabIndex = 11;
            // 
            // length_tbx
            // 
            this.num_tbx.Location = new System.Drawing.Point(208, 260);
            this.num_tbx.Name = "length_tbx";
            this.num_tbx.Size = new System.Drawing.Size(100, 23);
            this.num_tbx.TabIndex = 10;
            this.num_tbx.Text = "10";
            // 
            // startAddress_tbx
            // 
            this.startAddress_tbx.Location = new System.Drawing.Point(208, 224);
            this.startAddress_tbx.Name = "startAddress_tbx";
            this.startAddress_tbx.Size = new System.Drawing.Size(100, 23);
            this.startAddress_tbx.TabIndex = 9;
            this.startAddress_tbx.Text = "100";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(352, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "bool值";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(134, 263);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "数据长度";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(135, 227);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 17);
            this.label6.TabIndex = 6;
            this.label6.Text = "起始地址";
            // 
            // shortValue_tbx
            // 
            this.shortValue_tbx.Location = new System.Drawing.Point(424, 137);
            this.shortValue_tbx.Name = "shortValue_tbx";
            this.shortValue_tbx.Size = new System.Drawing.Size(100, 23);
            this.shortValue_tbx.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(351, 140);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 17);
            this.label7.TabIndex = 12;
            this.label7.Text = "short值";
            // 
            // floatValue_tbx
            // 
            this.floatValue_tbx.Location = new System.Drawing.Point(424, 173);
            this.floatValue_tbx.Name = "floatValue_tbx";
            this.floatValue_tbx.Size = new System.Drawing.Size(100, 23);
            this.floatValue_tbx.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(351, 176);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "float值";
            // 
            // stringValue_tbx
            // 
            this.stringValue_tbx.Location = new System.Drawing.Point(424, 209);
            this.stringValue_tbx.Name = "stringValue_tbx";
            this.stringValue_tbx.Size = new System.Drawing.Size(100, 23);
            this.stringValue_tbx.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(351, 212);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 17);
            this.label9.TabIndex = 16;
            this.label9.Text = "string值";
            // 
            // GetData_tbx
            // 
            this.GetData_tbx.Location = new System.Drawing.Point(351, 303);
            this.GetData_tbx.Name = "GetData_tbx";
            this.GetData_tbx.Size = new System.Drawing.Size(72, 23);
            this.GetData_tbx.TabIndex = 18;
            this.GetData_tbx.Text = "数据读取";
            this.GetData_tbx.UseVisualStyleBackColor = true;
            this.GetData_tbx.Click += new System.EventHandler(this.GetData_tbx_Click);
            // 
            // WriteData_tbx
            // 
            this.WriteData_tbx.Location = new System.Drawing.Point(449, 303);
            this.WriteData_tbx.Name = "WriteData_tbx";
            this.WriteData_tbx.Size = new System.Drawing.Size(75, 23);
            this.WriteData_tbx.TabIndex = 19;
            this.WriteData_tbx.Text = "数据写入";
            this.WriteData_tbx.UseVisualStyleBackColor = true;
            this.WriteData_tbx.Click += new System.EventHandler(this.WriteData_tbx_Click);
            // 
            // Connect_btn
            // 
            this.Connect_btn.Location = new System.Drawing.Point(209, 303);
            this.Connect_btn.Name = "Connect_btn";
            this.Connect_btn.Size = new System.Drawing.Size(75, 23);
            this.Connect_btn.TabIndex = 20;
            this.Connect_btn.Text = "连接";
            this.Connect_btn.UseVisualStyleBackColor = true;
            this.Connect_btn.Click += new System.EventHandler(this.Connect_btn_Click);
            // 
            // dataType_cbx
            // 
            this.dataType_cbx.FormattingEnabled = true;
            this.dataType_cbx.Items.AddRange(new object[] {
            "bool",
            "short",
            "float",
            "string"});
            this.dataType_cbx.Location = new System.Drawing.Point(426, 258);
            this.dataType_cbx.Name = "dataType_cbx";
            this.dataType_cbx.Size = new System.Drawing.Size(99, 25);
            this.dataType_cbx.TabIndex = 21;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(352, 263);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 17);
            this.label10.TabIndex = 22;
            this.label10.Text = "数据类型";
            // 
            // autoRead_cbx
            // 
            this.autoRead_cbx.AutoSize = true;
            this.autoRead_cbx.Checked = true;
            this.autoRead_cbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoRead_cbx.Location = new System.Drawing.Point(352, 333);
            this.autoRead_cbx.Name = "autoRead_cbx";
            this.autoRead_cbx.Size = new System.Drawing.Size(75, 21);
            this.autoRead_cbx.TabIndex = 23;
            this.autoRead_cbx.Text = "持续读取";
            this.autoRead_cbx.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // stringLen_tbx
            // 
            this.stringLen_tbx.Location = new System.Drawing.Point(568, 209);
            this.stringLen_tbx.Name = "stringLen_tbx";
            this.stringLen_tbx.Size = new System.Drawing.Size(42, 23);
            this.stringLen_tbx.TabIndex = 25;
            this.stringLen_tbx.Text = "10";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(530, 212);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 17);
            this.label11.TabIndex = 24;
            this.label11.Text = "长度";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 459);
            this.Controls.Add(this.stringLen_tbx);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.autoRead_cbx);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.dataType_cbx);
            this.Controls.Add(this.Connect_btn);
            this.Controls.Add(this.WriteData_tbx);
            this.Controls.Add(this.GetData_tbx);
            this.Controls.Add(this.stringValue_tbx);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.floatValue_tbx);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.shortValue_tbx);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.boolValue_tbx);
            this.Controls.Add(this.num_tbx);
            this.Controls.Add(this.startAddress_tbx);
            this.Controls.Add(this.slaveAddress_tbx);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.port_tbx);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ip_tbx);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "ModbusTCP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ip_tbx;
        private System.Windows.Forms.TextBox port_tbx;
        private System.Windows.Forms.TextBox slaveAddress_tbx;
        private System.Windows.Forms.TextBox boolValue_tbx;
        private System.Windows.Forms.TextBox num_tbx;
        private System.Windows.Forms.TextBox startAddress_tbx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox shortValue_tbx;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox floatValue_tbx;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox stringValue_tbx;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button GetData_tbx;
        private System.Windows.Forms.Button WriteData_tbx;
        private System.Windows.Forms.Button Connect_btn;
        private System.Windows.Forms.ComboBox dataType_cbx;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox autoRead_cbx;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox stringLen_tbx;
        private System.Windows.Forms.Label label11;
    }
}

