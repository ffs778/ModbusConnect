using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCConnect
{
    public abstract class PLC
    {
        #region 事件
        // 外部
        public abstract event EventHandler OnAdsNotificationMsg; // 消息通知给外部 
        #endregion

        #region 公开属性
        public string IP { get; set; }
        public int Port { get; set; }
        public bool IsConnected { get; set; }
        /// <summary>
        /// 是否屏蔽PLC
        /// </summary>
        public bool IsShieldPLCHeart { get; set; }
        public Dictionary<string, dynamic> DataDic { get; set; } = new();
        #endregion
        /// <summary>
        /// PLC连接
        /// </summary>
        public abstract void Connect();
        /// <summary>
        /// 断开连接
        /// </summary>
        public abstract void DisConnect();
        /// <summary>
        /// 写变量
        /// </summary>
        /// <param name="model"></param>
        /// <param name="value"></param>
        public abstract void Write(string variableName, dynamic value);
        /// <summary>
        /// 读变量
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns></returns>
        public abstract dynamic Read(string variableName);
        /// <summary>
        /// 监控PLC心跳
        /// </summary>
        public abstract void MonitorPLCHeart();
        /// <summary>
        /// 发送PC心跳
        /// </summary>
        public abstract void SendPCHeart();
        /// <summary>
        /// 初始化变量字典
        /// 获得所有变量的基本信息
        /// </summary>
        /// <param name="dbDataTable"></param>
        public abstract void InitialDataDic(DataTable dbDataTable);
    }
    interface IEvent
    {
        public static event EventHandler OnAdsNotificationMsg; // 消息通知给外部 
    }
}
