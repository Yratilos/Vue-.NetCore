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
    public class UserKingBase : BasePage, IUserRepository
    {
        IDataBase db;
        public UserKingBase(IDataBase db)
        {
            this.db = db;
        }

        public User Add(User u)
        {
            var user = GetById(u);
            if (user.ID == Guid.Empty && u.ID != Guid.Empty)
            {
                IDataParameter[] parameters = new KdbndpParameter[]{
                    new KdbndpParameter("CreateTime",u.CreateTime),
                    new KdbndpParameter("UpdateTime",u.UpdateTime),
                    new KdbndpParameter("ID",u.ID),
                    new KdbndpParameter("Name",u.Name),
                    new KdbndpParameter("Account",u.Account),
                    new KdbndpParameter("Password",u.Password)
                };
                Hashtable hashtable = new Hashtable()
                {
                    {"insert into \"public\".\"User\" (CreateTime,UpdateTime,ID,Name,Account,Password)values(:CreateTime,:UpdateTime,:ID,:Name,:Account,:Password)", parameters}
                };
                db.ExecuteTrans(hashtable);
                return u;
            }
            return new User();
        }

        public User Delete(Guid id, out bool result)
        {
            var user = GetById(id);
            if (user.ID == Guid.Empty)
            {
                result = false;
                return user;
            }
            IDataParameter[] parameters = new KdbndpParameter[]{
                new KdbndpParameter("id",id)
            };
            Hashtable hashtable = new Hashtable()
            {
                {"delete from \"public\".\"User\" where ID=:id",parameters}
            };
            result = db.ExecuteTrans(hashtable);
            return user;
        }

        public List<User> GetAll()
        {
            var ds = db.Execute("select * from \"public\".\"User\";select * from \"public\".\"User\";");
            return ds.ToEnumerable<User>().ToList();
        }

        public User GetById(Guid id)
        {
            IDataParameter[] parameters = new KdbndpParameter[]{
                    new KdbndpParameter("id",id)
                };
            var ds = db.Execute("select * from \"public\".\"User\" where ID=:id", parameters);
            var data = ds.ToModel<User>();
            return data;
        }

        User GetById(User user)
        {
            return GetById(user.ID);
        }

        public string GetName(Guid id)
        {
            IDataParameter[] parameters = new KdbndpParameter[]{
                new KdbndpParameter("id",id)
            };
            var data = db.ExecuteScalar("select Name from \"public\".\"User\" where ID=:id", parameters);
            return (data ?? "").ToString();
        }
    }
}
