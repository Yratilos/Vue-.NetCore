using Kdbndp;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Data;
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
                IDataParameter[] parameters = new KdbndpParameter[]{
                    new KdbndpParameter("CreateTime",j.CreateTime),
                    new KdbndpParameter("UpdateTime",j.UpdateTime),
                    new KdbndpParameter("ID",j.ID),
                    new KdbndpParameter("LogType",j.LogType),
                    new KdbndpParameter("Content",j.Content),
                    new KdbndpParameter("Model",j.Model),
                };
                Hashtable hashtable = new Hashtable()
                {
                    {"insert into [dbo].[User] (CreateTime,UpdateTime,ID,LogType,Content,Model)values(@CreateTime,@UpdateTime,@ID,@LogType,@Content,@Model)", parameters}
                };
                db.ExecuteTrans(hashtable);
                return j;
            }
            return new Job();
        }

        public Job Delete(Job job)
        {
            var j= GetById(job);
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
