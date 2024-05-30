using Kdbndp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        public List<Job> GetAll()
        {
            var ds = db.Execute("select * from \"public\".\"Job\";select * from \"public\".\"Job\";");
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
            if (db.Delete(j))
            {
                return j;
            }
            return new Job();
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
