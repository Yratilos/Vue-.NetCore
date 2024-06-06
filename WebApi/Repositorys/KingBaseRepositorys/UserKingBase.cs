﻿using Kdbndp;
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
                if (db.Add(u))
                {
                    return u;
                }
            }
            return new User();
        }

        public User Delete(Guid id, out bool result)
        {
            var user = GetById(id);
            result = db.Delete(user);
            if (result)
            {
                return user;
            }
            return new User();
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
