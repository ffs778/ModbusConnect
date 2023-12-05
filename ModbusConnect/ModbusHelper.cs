using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModbusConnect
{
    public class ModbusHelper
    {
        bool last = false;
        bool trigIn = false;
        bool trigQ = false;

        private string ip;
        private string port;


        //private List<ActualData> actualDatas = new List<ActualData>();

        //public List<ActualData> ActualDatas
        //{
        //    get { return actualDatas; }
        //    set { actualDatas = value; }
        //}

        /**
         * modubs从站ID
         */
        private string slaveNo;

        private bool isConnected = false;

        private bool isFirstConn = true;

        private int errorTimes = 0;

        CancellationTokenSource cts = new CancellationTokenSource();

        /**
         * 起始地址
         */

        private string address;

        //变量个数
        private string varNum;

        private TcpClient tcpClient = null;

        private ModbusIpMaster master;

        //配置文件地址
        private string jsonfile = Application.StartupPath + "\\Config\\modbuscfg1.json";

        private List<string> trendList = new List<string>();

        public List<string> TrendList
        {
            get { return trendList; }
            set { trendList = value; }
        }

        private Dictionary<string, string> currentPLCNote = new Dictionary<string, string>();

        public Dictionary<string, string> CurrentPLCNote
        {
            get { return currentPLCNote; }
            set { currentPLCNote = value; }
        }

        private Dictionary<string, string> currentPLCValue = new Dictionary<string, string>();

        public Dictionary<string, string> CurrentPLCValue
        {
            get { return currentPLCValue; }
            set { currentPLCValue = value; }
        }




        public string Jsonfile
        {
            get { return jsonfile; }
        }

        //读取变量
        private string str;
        //private CfgJson cfgJson;
        //private ModelRepository modelRepository;
        //private List<ReportData> reportDatas = new List<ReportData>();

        public ModbusHelper(ModelRepository modelRepository)
        {
           // this.modelRepository = modelRepository;
            //str = File.ReadAllText(jsonfile);
            //cfgJson = JsonConvert.DeserializeObject<CfgJson>(str);

            //this.ip = cfgJson.Ip;
            //this.port = cfgJson.Port;
            //this.slaveNo = cfgJson.SlaveNo;
            //this.address = cfgJson.Address;
            //this.varNum = cfgJson.VarNum;
            //this.actualDatas = cfgJson.ActualDatas;

            foreach (var item in actualDatas)
            {
                trendList.Add(item.Name);
                if (!currentPLCNote.ContainsKey(item.Name))
                {
                    currentPLCNote.Add(item.Name, item.Description);
                }
                else
                {
                    currentPLCNote[item.Name] = item.Description;
                }
            }



            //this.Conn();
            //this.readHoldingRegisters();
        }

        public string Conn()
        {
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(IPAddress.Parse(ip), int.Parse(port));
                master = ModbusIpMaster.CreateIp(tcpClient);
                isConnected = true;
                master.Transport.ReadTimeout = 1000;//读取数据超时时间为设定值

                master.Transport.WriteTimeout = 1000;//写入数据超时时间为设定值

                master.Transport.Retries = 5;

                master.Transport.WaitToRetryMilliseconds = 2500;
                Console.WriteLine("OK");
                return "OK";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        public string DisConn()
        {
            if (master != null)
            {
                master.Dispose();
                tcpClient.Dispose();
                tcpClient.Close();
                return "OK";
            }
            else
            {
                return "Failed";
            }
        }

        public void readHoldingRegisters()
        {
            Task.Run(async () =>
            {
                while (!cts.IsCancellationRequested)
                {
                    if (isConnected == true)
                    {
                        ushort[] des = null;
                        await Task.Delay(500);
                        try
                        {
                            des = master.ReadHoldingRegisters(byte.Parse(slaveNo), ushort.Parse(address), ushort.Parse(varNum));
                        }
                        catch (Exception ex)
                        {

                        }

                        //读出的数据不为空，则解析数据，存储
                        if (des != null)
                        {
                            errorTimes = 0;
                            for (int i = 0; i < actualDatas.Count; i++)
                            {
                                actualDatas[i].Value = ((float)des[i] * float.Parse(actualDatas[i].Scale)).ToString();
                                actualDatas[i].CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            //foreach (var item in actualDatas)
                            //{
                            //    Console.WriteLine(item.Value);
                            //}

                            foreach (var item in actualDatas)
                            {
                                if (!currentPLCValue.ContainsKey(item.Name))
                                {
                                    currentPLCValue.Add(item.Name, item.Value);
                                }
                                else
                                {
                                    currentPLCValue[item.Name] = item.Value;
                                }
                            }


                            ReportDataGenerate();
                            //按事件插入数据库
                            Insert();
                        }

                        else
                        {
                            errorTimes++;
                            await Task.Delay(500);
                            if (errorTimes >= 3)
                            {
                                isConnected = false;
                            }
                        }
                    }
                    else
                    {
                        if (!isFirstConn)
                        {
                            await Task.Delay(1000);

                            tcpClient?.Close();
                            //已经测试，不行
                            //tcpClient.Dispose();
                            //master?.Transport.Dispose();
                            //master?.Dispose();
                        }

                        try
                        {
                            //首次连接，或多次连接
                            tcpClient = new TcpClient();
                            tcpClient.Connect(IPAddress.Parse(ip), int.Parse(port));
                            //先将master置为空，再断线重连
                            master = null;
                            master = ModbusIpMaster.CreateIp(tcpClient);
                            master.Transport.ReadTimeout = 1000;//读取数据超时时间为设定值

                            master.Transport.WriteTimeout = 1000;//写入数据超时时间为设定值

                            master.Transport.Retries = 5;

                            master.Transport.WaitToRetryMilliseconds = 2500;
                            isConnected = true;
                            isFirstConn = false;

                        }
                        catch (Exception ex)
                        {

                        }


                    }

                }
            }, cts.Token);
        }

        public string writeHoldingRegisters(string varAddress, string value)
        {
            master.WriteSingleRegister(byte.Parse(this.slaveNo), ushort.Parse(varAddress), ushort.Parse(value));
            return "OK";
        }

        public string WriteMultipleRegisters(ushort starAddress, ushort[] data)
        {
            master.WriteMultipleRegisters(byte.Parse(this.slaveNo), starAddress, data);
            return "OK";
        }

        public void Insert()
        {
            trigIn = this.actualDatas[9].Value.Equals("10");
            trigQ = (trigIn & !last);
            last = trigIn;
            if (trigQ)
            {
                try
                {
                    InsertDataBase();
                }
                catch (Exception ex)
                {

                }
            }
        }

        //插入数据工具类
        public int InsertDataBase()
        {
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            foreach (var item in this.actualDatas)
            {
                item.InsertTime = now;
            }
            int myCount = modelRepository.InsertReportData(reportDatas);
            int count = modelRepository.InsertActualData(this.actualDatas);

            return count + myCount;
        }

        //生成数据
        public void ReportDataGenerate()
        {
            if (reportDatas.Count < 500)
            {
                reportDatas.Add(new ReportData()
                {
                    InsertTime = DateTime.Now,
                    Var1 = actualDatas[0].Value,
                    Var2 = actualDatas[1].Value,
                    Var3 = actualDatas[2].Value,
                    Var4 = actualDatas[3].Value,
                    Var5 = actualDatas[4].Value,
                    Var6 = actualDatas[5].Value,
                    Var7 = actualDatas[6].Value,
                    Var8 = actualDatas[7].Value,
                    Var9 = actualDatas[8].Value,
                    Var10 = actualDatas[9].Value
                });
            }
            else
            {
                reportDatas.RemoveAt(0);
            }
        }


    }
}

