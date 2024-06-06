using Kdbndp;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace WebApi.Systems.DataBases
{
    public class KingBase : IDataBase
    {
        private static IDataBase instance = null;
        private static object padlock = new object();
        private string conn = "Server={0};Port={4};UID={1};PWD={2};database={3};";

        /// <summary>
        /// 设置数据库连接
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private KingBase(string conn)
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
        private KingBase(string server, string uid, string pwd, string database, string port)
        {
            conn = string.Format(conn, server, uid, pwd, database, port);
        }

        public static IDataBase GetInstance(string conn)
        {
            if (instance is null)
            {
                lock (padlock)
                {
                    if (instance is null)
                    {
                        instance = new KingBase(conn);
                    }
                }
            }
            return instance;
        }

        public static IDataBase GetInstance(string server, string uid, string pwd, string database, string port)
        {
            if (instance is null)
            {
                lock (padlock)
                {
                    if (instance is null)
                    {
                        instance = new KingBase(server, uid, pwd, database, port);
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
        /// KdbndpParameter[] parameters = new KdbndpParameter[]{
        ///     new KdbndpParameter("@field1",value1),
        ///     new KdbndpParameter("@field2",value2),
        /// }
        /// </param>
        /// <returns>第一行第一列</returns>
        public object ExecuteScalar(string sql, params IDataParameter[] param)
        {
            using (KdbndpConnection conn = new KdbndpConnection(this.conn))
            {
                conn.Open();
                using (KdbndpCommand cmd = conn.CreateCommand())
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
        /// KdbndpParameter[] parameters = new KdbndpParameter[]{
        ///     new KdbndpParameter("@field1",value1),
        ///     new KdbndpParameter("@field2",value2),
        /// }
        /// </param>
        /// <returns></returns>
        public DataSet Execute(string sql, params IDataParameter[] param)
        {
            using (KdbndpConnection conn = new KdbndpConnection(this.conn))
            {
                conn.Open();
                using (KdbndpCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(param);
                    using (KdbndpDataAdapter da = new KdbndpDataAdapter(cmd))
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
        /// KdbndpParameter[] parameters = new KdbndpParameter[]{
        ///     new KdbndpParameter("@field1",value1),
        ///     new KdbndpParameter("@field2",value2),
        /// }
        /// </param>
        /// <returns>是否成功</returns>
        public bool ExecuteTrans(Hashtable hashtable)
        {
            using (KdbndpConnection conn = new KdbndpConnection(this.conn))
            {
                conn.Open();
                using (KdbndpTransaction trans = conn.BeginTransaction())
                {
                    using (KdbndpCommand cmd = conn.CreateCommand())
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
                hashtable.Add(item, new KdbndpParameter[] { });
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
                parametersList.Add(new KdbndpParameter(v.Key, v.Value));
            }
            IDataParameter[] parameters = parametersList.ToArray();
            var sql = $"insert into \"public\".\"{typeof(T).Name}\" ({string.Join(",", dic.Keys)})values({string.Join(",", dic.Keys.Select(s => $":{s}"))})";
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
                parametersList.Add(new KdbndpParameter(v.Key, v.Value));
            }
            IDataParameter[] parameters = parametersList.ToArray();
            var sql = $"delete from \"public\".\"{typeof(T).Name}\" where {string.Join(" and ", dic.Keys.Select(s => $"{s}=:{s}"))}";
            Hashtable hashtable = new Hashtable()
            {
                {sql, parameters}
            };
            var t = ExecuteTrans(hashtable);
            if (!t)
            {
                sql = $"delete from \"public\".\"{typeof(T).Name}\" where {string.Join(" and ", dic.Select(s => $"{s.Key}='{s.Value}'"))}";
                hashtable.Clear();
                hashtable.Add(sql, new IDataParameter[] { });
                t = ExecuteTrans(hashtable);
            }
            return t;
        }
    }
}
