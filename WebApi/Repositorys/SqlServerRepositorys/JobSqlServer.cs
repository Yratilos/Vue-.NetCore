using Microsoft.Data.SqlClient;
using System;
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
        public List<Job> GetAll()
        {
            var ds = db.Execute("select * from [dbo].[Job];select * from [dbo].[Job]");
            return ds.ToEnumerable<Job>().ToList();
        }
        public Job Add(Job j)
        {
            var job = GetById(j);
            if (job.ID == Guid.Empty && j.ID != Guid.Empty)
            {
                if (db.Add(j))
                {
                    return j;
                }
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
