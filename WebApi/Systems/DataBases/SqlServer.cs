using Microsoft.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace WebApi.Systems.DataBases
{
    public class SqlServer : IDataBase
    {
        private static IDataBase instance = null;
        private static object padlock = new object();
        private string conn = "server={0};uid={1};pwd={2};database={3}";

        /// <summary>
        /// 设置数据库连接
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private SqlServer(string conn)
        {
            this.conn = conn;
        }

        /// <summary>
        /// 设置数据库连接
        /// </summary>
        /// <param name="server">服务器名称</param>
        /// <param name="uid">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="database">数据库</param>
        /// <returns></returns>
        private SqlServer(string server, string uid, string pwd, string database)
        {
            conn = string.Format(conn, server, uid, pwd, database);
        }

        public static IDataBase GetInstance(string conn)
        {
            if (instance is null)
            {
                lock (padlock)
                {
                    if (instance is null)
                    {
                        instance = new SqlServer(conn);
                    }
                }
            }
            return instance;
        }

        public static IDataBase GetInstance(string server, string uid, string pwd, string database)
        {
            if (instance is null)
            {
                lock (padlock)
                {
                    if (instance is null)
                    {
                        instance = new SqlServer(server, uid, pwd, database);
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 获取数据库中的值
        /// </summary>
        /// <param name="sql">... @value1=field1 and @value2=field2</param>
        /// <param name="param">
        /// IDataParameter[] parameters = new SqlParameter[]{
        ///     new SqlParameter("@field1",value1),
        ///     new SqlParameter("@field2",value2),
        /// }
        /// </param>
        /// <returns>第一行第一列</returns>
        public object ExecuteScalar(string sql, params IDataParameter[] param)
        {
            using (SqlConnection conn = new SqlConnection(this.conn))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(param);
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sql">... @value1=field1 and @value2=field2</param>
        /// <param name="param">
        /// IDataParameter[] parameters = new SqlParameter[]{
        ///     new SqlParameter("@field1",value1),
        ///     new SqlParameter("@field2",value2),
        /// }
        /// </param>
        /// <returns></returns>
        public DataSet Execute(string sql, params IDataParameter[] param)
        {
            using (SqlConnection conn = new SqlConnection(this.conn))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(param);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        var ds = new DataSet();
                        da.Fill(ds);
                        return ds;
                    }
                }
            }
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="hashtable">
        /// Key:... @value1=field1 and @value2=field2
        /// Value:
        /// IDataParameter[] parameters = new SqlParameter[]{
        ///     new SqlParameter("@field1",value1),
        ///     new SqlParameter("@field2",value2),
        /// }
        /// </param>
        /// <returns>是否成功</returns>
        public bool ExecuteTrans(Hashtable hashtable)
        {
            using (SqlConnection conn = new SqlConnection(this.conn))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        foreach (DictionaryEntry item in hashtable)
                        {
                            cmd.CommandText = item.Key.ToString();
                            if (item.Value is IDataParameter[])
                            {
                                cmd.Parameters.AddRange(item.Value as IDataParameter[]);
                            }
                            cmd.Transaction = trans;
                            var c = cmd.ExecuteNonQuery();
                            if (c <= 0)
                            {
                                trans.Rollback();
                                return false;
                            }
                        }
                        trans.Commit();
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="hash">数据库操作语句</param>
        /// <returns>是否成功</returns>
        public bool ExecuteTrans(HashSet<string> hash)
        {
            var hashtable = new Hashtable();
            foreach (var item in hash)
            {
                hashtable.Add(item, new SqlParameter[] { });
            }
            return ExecuteTrans(hashtable);
        }

        public bool Add<T>(T model) where T : class, new()
        {
            var dic = new Dictionary<string, object>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                object value = prop.GetValue(model);
                dic.Add(prop.Name, value);
            }
            List<IDataParameter> parametersList = new List<IDataParameter>();
            foreach (var v in dic)
            {
                parametersList.Add(new SqlParameter(v.Key, v.Value));
            }
            IDataParameter[] parameters = parametersList.ToArray();
            var sql = $"insert into [dbo].[{typeof(T).Name}] ({string.Join(",", dic.Keys)})values({string.Join(",", dic.Keys.Select(s => $"@{s}"))})";
            Hashtable hashtable = new Hashtable()
            {
                {sql, parameters}
            };
            return ExecuteTrans(hashtable);
        }

        public bool Delete<T>(T model) where T : class, new()
        {
            var dic = new Dictionary<string, object>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                object value = prop.GetValue(model);
                dic.Add(prop.Name, value);
            }
            List<IDataParameter> parametersList = new List<IDataParameter>();
            foreach (var v in dic)
            {
                parametersList.Add(new SqlParameter(v.Key, v.Value));
            }
            IDataParameter[] parameters = parametersList.ToArray();
            var sql = $"delete from [dbo].[{typeof(T).Name}] where {string.Join(" and ", $"{dic.Keys}=@{dic.Keys}")}";
            Hashtable hashtable = new Hashtable()
            {
                {sql, parameters}
            };
            return ExecuteTrans(hashtable);
        }
    }
}
