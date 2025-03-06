using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace HIS.Clinic.DoctorWorkstation
{
    public class OracleHelpher
    {
        public static string connectString = @"User ID=jk;Password=jk;Data Source=(DESCRIPTION =(ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.0.2)(PORT = 1521)))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = yygl)))";
        #region 数据库连接
        public static OracleConnection DbConn(ref string message, ref Boolean re)
        {
            //数据库的连接的方式
            OracleConnection conn;
            re = false;
            message = "";
            conn = new OracleConnection(connectString);
            try
            {
                conn.Open();
                re = true;
            }
            catch (Exception ex)
            {
                message = "错误：" + ex.Message.ToString();
                re = false;
                return null;
            }
            finally
            {
                conn.Close();
            }

            return conn;
        }
        #endregion

        #region 增
        public static Boolean AddSql(string sql, ref string message)
        {
            bool re = false;
            try
            {
                OracleConnection conn = DbConn(ref message, ref re);
                if (conn == null)
                {
                    re = false;
                    message = "数据库连接对象为空";
                }
                else
                {
                    try
                    {
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                    }
                    catch (Exception eee)
                    {
                        re = false;
                        message = eee.Message.ToString();
                    }
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        re = true;
                    }
                    else
                    {
                        re = false;
                        message = "数据插入失败";
                    }
                    cmd.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ee)
            {
                re = false;
                message = "add数据出错,原因:" + ee.Message.ToString();
            }
            return re;
        }
        #endregion

        #region 删
        public static Boolean DeleteSql(string sql, ref string message)
        {
            bool re = false;
            try
            {
                OracleConnection conn = DbConn(ref message, ref re);
                if (conn == null)
                {
                    re = false;
                    message = "数据库连接对象为空";
                }
                else
                {
                    try
                    {
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                    }
                    catch (Exception eee)
                    {
                        re = false;
                        message = eee.Message.ToString();
                    }
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        re = true;
                    }
                    else
                    {
                        re = false;
                        message = "数据删除失败";
                    }
                    cmd.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ee)
            {
                re = false;
                message = "add数据出错,原因:" + ee.Message.ToString();
            }
            return re;
        }
        #endregion

        #region 改每次调用都会调用代码
        public static Boolean UpdateSql(string sql, ref string message)
        {
            bool re = false;
            try
            {
                OracleConnection conn = DbConn(ref message, ref re);
                if (conn == null)
                {
                    re = false;
                    message = "数据库连接对象为空";
                }
                else
                {
                    try
                    {
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                    }
                    catch (Exception eee)
                    {
                        re = false;
                        message = eee.Message.ToString();
                    }
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        re = true;
                    }
                    else
                    {
                        re = false;
                        message = "数据更新失败";
                    }
                    cmd.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ee)
            {
                re = false;
                message = "add数据出错,原因:" + ee.Message.ToString();
            }
            return re;
        }
        #endregion

        #region 查
        public static DataTable SelectSql(string sql, ref string message)
        {

            DataTable dt = null;
            OracleConnection conn;
            message = string.Empty;
            string err = "";
            bool re = false;
            try
            {
                conn = DbConn(ref err, ref re);
                if (conn == null)
                {
                    message = "数据库连接对象为空";
                    return null;
                }
                if (conn.State == ConnectionState.Closed)
                {
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                        return null;
                    }
                }
            }
            catch (Exception ee)
            {
                message = "数据库连接失败,原因:" + ee.Message.ToString();
                return null;
            }
            try
            {
                OracleDataAdapter adapter = new OracleDataAdapter(sql, conn);
                DataSet set = new DataSet();
                adapter.Fill(set);
                dt = set.Tables[0];
                conn.Close();
                conn.Dispose();
                adapter.Dispose();
                set.Dispose();
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString(); ;
            }
            return dt;
        }
        #endregion

    }
}
