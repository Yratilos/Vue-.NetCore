using Kdbndp;
using System;
using System.Collections;
using System.Data;
using WebApi.IRepositorys;
using WebApi.Models;
using WebApi.Systems.DataBases;
using WebApi.Systems.Extensions;

namespace WebApi.Repositorys.KingBaseRepositorys
{
    public class JobKingBase : IJobRepository
    {
        IDataBase db;
        public JobKingBase(IDataBase db)
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
                    {"insert into \"public\".\"Job\" (CreateTime,UpdateTime,ID,LogType,Content,Model)values(:CreateTime,:UpdateTime,:ID,:LogType,:Content,:Model)", parameters}
                };
                db.ExecuteTrans(hashtable);
                return j;
            }
            return new Job();
        }

        public Job Delete(Job job)
        {
            var j = GetById(job);
            IDataParameter[] parameters = new KdbndpParameter[]{
                    new KdbndpParameter("id",job.ID)
                };
            var ds = db.Execute("delete from \"public\".\"Job\" where ID=:id", parameters);
            return j;
        }

        Job GetById(Guid id)
        {
            IDataParameter[] parameters = new KdbndpParameter[]{
                    new KdbndpParameter("id",id)
                };
            var ds = db.Execute("select * from \"public\".\"Job\" where ID=:id", parameters);
            var data = ds.ToModel<Job>();
            return data;
        }

        Job GetById(Job user)
        {
            return GetById(user.ID);
        }
    }
}
