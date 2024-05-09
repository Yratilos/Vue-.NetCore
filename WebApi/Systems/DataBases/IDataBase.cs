using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace WebApi.Systems.DataBases
{
    public interface IDataBase
    {
        public object ExecuteScalar(string sql, params IDataParameter[] param);
        public DataSet Execute(string sql, params IDataParameter[] param);
        public bool ExecuteTrans(Hashtable hashtable);
        public bool ExecuteTrans(HashSet<string> hash);
    }
}
