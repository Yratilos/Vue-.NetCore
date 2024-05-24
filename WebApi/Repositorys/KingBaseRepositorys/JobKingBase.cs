using Kdbndp;
using System;
using System.Collections;
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
                var dic = j.ToDictionary();
                List<IDataParameter> parametersList = new List<IDataParameter>();
                foreach (var v in dic)
                {
                    parametersList.Add(new KdbndpParameter(v.Key, v.Value));
                }
                IDataParameter[] parameters = parametersList.ToArray();
                var sql = $"insert into \"public\".\"Job\" ({string.Join(",", dic.Keys)})values({string.Join(",", dic.Keys.Select(s => $":{s}"))})";
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
