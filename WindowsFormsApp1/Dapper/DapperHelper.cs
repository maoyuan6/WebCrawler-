using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

/// <summary>
/// Dapper 数据库操作帮助类，提供基本的增删改查功能。
/// </summary>
public class DapperHelper
{
    private readonly string _connectionString;

    /// <summary>
    /// 初始化 DapperHelper 实例。
    /// </summary>
    /// <param name="DBName">数据库名称</param>
    public DapperHelper(string DBName)
    {
        _connectionString = $@"Database={DBName};Server=localhost\MSSQLSERVER03;user id=sa;password=Aa123456;";
    }

    /// <summary>
    /// 获取数据库连接。
    /// </summary>
    /// <returns>SQL 数据库连接对象</returns>
    private IDbConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
    /// <summary>
    /// 执行查询并返回 DataTable
    /// </summary>
    /// <param name="sql">SQL 查询语句</param>
    /// <param name="parameters">查询参数</param>
    /// <returns>查询结果 DataTable</returns>
    public DataTable QueryToDataTable(string sql, object parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var reader = connection.ExecuteReader(sql, parameters);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }
    }
    /// <summary>
    /// 查询数据列表。
    /// </summary>
    /// <typeparam name="T">返回对象类型</typeparam>
    /// <param name="sql">SQL 查询语句</param>
    /// <param name="parameters">查询参数</param>
    /// <returns>查询结果列表</returns>
    public IEnumerable<T> Query<T>(string sql, object parameters = null)
    {
        using (var connection = GetConnection())
        {
            return connection.Query<T>(sql, parameters).ToList();
        }
    }

    /// <summary>
    /// 查询单条数据。
    /// </summary>
    /// <typeparam name="T">返回对象类型</typeparam>
    /// <param name="sql">SQL 查询语句</param>
    /// <param name="parameters">查询参数</param>
    /// <returns>查询结果对象</returns>
    public T QuerySingle<T>(string sql, object parameters = null)
    {
        using (var connection = GetConnection())
        {
            return connection.QuerySingleOrDefault<T>(sql, parameters);
        }
    }

    /// <summary>
    /// 执行增删改操作。
    /// </summary>
    /// <param name="sql">SQL 语句</param>
    /// <param name="parameters">SQL 参数</param>
    /// <returns>受影响的行数</returns>
    public int Execute(string sql, object parameters = null)
    {
        using (var connection = GetConnection())
        {
            return connection.Execute(sql, parameters);
        }
    }

    /// <summary>
    /// 插入数据并返回主键ID。
    /// </summary>
    /// <typeparam name="T">返回主键类型</typeparam>
    /// <param name="sql">SQL 插入语句</param>
    /// <param name="parameters">SQL 参数</param>
    /// <returns>新插入数据的主键</returns>
    public T Insert<T>(string sql, object parameters = null)
    {
        using (var connection = GetConnection())
        {
            return connection.ExecuteScalar<T>(sql, parameters);
        }
    }

    /// <summary>
    /// 获取最大 ID 值
    /// </summary>
    /// <returns>当前表中的最大 ID，如果表为空，则返回 0</returns>
    public int GetMaxId(string IDFiled,string tablename)
    {
        using (var connection = GetConnection())
        {
            string sql = $"SELECT ISNULL(MAX({IDFiled}), 0) FROM {tablename}";
            return connection.ExecuteScalar<int>(sql);
        }
    }
}
