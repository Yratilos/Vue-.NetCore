using Kdbndp;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebApi.IRepositorys;
using WebApi.Models;
using WebApi.Systems.DataBases;
using WebApi.Systems.Extensions;

namespace WebApi.Repositorys.SqlServerRepositorys
{
    public class JobSqlServer : IJobRepository
    {
        IDataBase db;
        public JobSqlServer(IDataBase db)
        {
            this.db = db;
        }
        public Job Add(Job j)
        {
            var job = GetById(j);
            if (job.ID == Guid.Empty && j.ID != Guid.Empty)
            {
                var dic = j.ToDictionary();
                List<IDataParameter> parametersList = new List<IDataParameter>();
                foreach (var v in dic)
                {
                    parametersList.Add(new SqlParameter("@"+v.Key, v.Value));
                }
                IDataParameter[] parameters = parametersList.ToArray();
                var sql = $"insert into [dbo].[Job] ({string.Join(",", dic.Keys)})values({string.Join(",", dic.Keys.Select(s => $"@{s}"))})";
                Hashtable hashtable = new Hashtable()
                {
                    {sql, parameters}
                };
                db.ExecuteTrans(hashtable);
                return j;
            }
            return new Job();
        }

        public Job Delete(Job job)
        {
            var j = GetById(job);
            IDataParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",job.ID)
            };
            var ds = db.Execute("delete from [dbo].[Job] where ID=@id", parameters);
            return j;
        }

        Job GetById(Guid id)
        {
            IDataParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",id)
            };
            var ds = db.Execute("select * from [dbo].[Job] where ID=@id", parameters);
            var j = ds.ToModel<Job>();
            return j;
        }

        Job GetById(Job u)
        {
            return GetById(u.ID);
        }
    }
}
