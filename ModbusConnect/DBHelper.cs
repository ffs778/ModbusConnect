using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace PLCConnect
{
    public class DBHelper
    {
        private static readonly DBHelper _instance = new();
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["connStr"].ToString();
        readonly SqlConnection _sqlConnection;
        readonly object objLock = new(); // 因为是单例连接，所以连接对象只能存在一个，需要加锁，并且不同的方法用同一把锁，不允许同时执行。
        readonly bool _isConnSuccess;    // 当前数据库是否连接成功。
        /// <summary>
        /// 数据库实例
        /// </summary>
        public static DBHelper Instance => _instance;
        /// <summary>
        /// 当前连接是否正常
        /// </summary>
        public bool IsConnSuccess => _isConnSuccess;
        private DBHelper()
        {
            try
            {
                _sqlConnection = new SqlConnection(_connStr);
                _sqlConnection.Open();  // 通过打开数据库连接，是否抛出异常判断当前连接是否正常
                _isConnSuccess = true;  // 连接成功
            }
            catch (Exception ex)
            {
                _isConnSuccess = false; // 连接失败
               // Logger.WriteErrorLog(ex);
            }
        }

        #region 执行无返回值的SQL命令
        /// <summary>
        /// 执行无返回值的SQL命令
        /// </summary>
        /// <param name="commandText"></param>
        public void ExecuteSQL(string commandText)
        {
            lock (objLock)
            {
                CheckConnection();
                using SqlCommand sqlCommand = new(commandText, _sqlConnection);
                sqlCommand.ExecuteNonQuery();//执行数据库的语句
            }
        }
        #endregion

        #region 执行有返回值的SQL命令
        /// <summary>
        /// 执行有返回值的SQL命令
        /// </summary>
        /// <param name="commandText"></param>
        public DataTable CheckSQL(string commandText)
        {
            lock (objLock)
            {
                CheckConnection();
                using SqlCommand sc = new(commandText, _sqlConnection);
                DataTable dtData = new();
                using SqlDataAdapter sda = new(sc);
                sda.Fill(dtData);
                return dtData;
            }
        }
        #endregion

        #region 执行无返回值的存储过程
        /// <summary>
        /// 执行无返回值数据库存储过程
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        public void ExecuteProcedure(string procedureName, params object[] parameters)
        {
            lock (objLock)
            {
                CheckConnection();
                using SqlCommand sc = new(procedureName, _sqlConnection);
                sc.CommandType = CommandType.StoredProcedure;
                if (parameters != null) sc.Parameters.AddRange(parameters);
                sc.ExecuteNonQuery();
            }
        }
        #endregion

        #region 执行有返回值的存储过程
        /// <summary>
        /// 执行有返回值的存储过程
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable CheckProcedure(string procedureName, params object[] parameters)
        {
            lock (objLock)
            {
                CheckConnection();
                using SqlCommand sc = new(procedureName, _sqlConnection);    // using语句简化版，省去了大括号
                sc.CommandType = CommandType.StoredProcedure;
                if (parameters != null) sc.Parameters.AddRange(parameters);
                DataTable dtData = new();
                using SqlDataAdapter sda = new(sc);
                sda.Fill(dtData);
                return dtData;
            }
        }
        #endregion


        #region 连接检测
        /// <summary>
        /// 检测当前连接
        /// </summary>
        public void CheckConnection()
        {
            if (_sqlConnection.State != ConnectionState.Open)
            {
                _sqlConnection.Close();
                _sqlConnection.Open();
            }
        }
        #endregion
    }
}
