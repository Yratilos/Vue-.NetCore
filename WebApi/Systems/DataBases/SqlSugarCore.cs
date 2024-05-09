using SqlSugar;

namespace WebApi.Systems.DataBases
{
    public class SqlSugarCore
    {
        string conn;
        string dbType;
        private static SqlSugarCore instance = null;
        private static readonly object padlock = new object();
        private SqlSugarCore(string conn, string dbType)
        {
            this.conn = conn;
            this.dbType = dbType;
        }
        public static SqlSugarCore GetInstance(string conn, string dbType)
        {
            if (instance is null)
            {
                lock (padlock)
                {
                    if (instance is null)
                    {
                        instance = new SqlSugarCore(conn, dbType);
                    }
                }
            }
            return instance;
        }
        public DbType DbType { get { if (dbType == "KingBase") { return DbType.Kdbndp; } else { return DbType.SqlServer; } } }
        /// <summary>
        /// 创建实体类
        /// </summary>
        public void CreateModel()
        {
            // 创建数据库对象
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = conn,
                DbType = DbType,
                IsAutoCloseConnection = true
            },
            db =>
            {
                //5.1.3.24统一了语法和SqlSugarScope一样，老版本AOP可以写外面

                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    //Console.WriteLine(sql);//输出sql,查看执行sql 性能无影响

                    //获取原生SQL推荐 5.1.4.63  性能OK
                    //UtilMethods.GetNativeSql(sql,pars)

                    //获取无参数化SQL 对性能有影响，特别大的SQL参数多的，调试使用
                    //UtilMethods.GetSqlString(DbType.SqlServer,sql,pars)
                };

                //注意多租户 有几个设置几个
                //db.GetConnection(i).Aop
            });
            db.DbFirst.IsCreateDefaultValue().CreateClassFile("E:\\TestProject\\WebApi\\Models\\TableModels", "WebApi.Models.TableModels");
        }
    }
}
