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
    public class UserSqlServer : BasePage, IUserRepository
    {
        IDataBase db;
        public UserSqlServer(IDataBase db)
        {
            this.db = db;
        }

        public User Add(User u)
        {
            var user = GetById(u);
            if (user.ID == Guid.Empty && u.ID != Guid.Empty)
            {
                IDataParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@CreateTime",u.CreateTime),
                    new SqlParameter("@UpdateTime",u.UpdateTime),
                    new SqlParameter("@ID",u.ID),
                    new SqlParameter("@Name",u.Name),
                    new SqlParameter("@Account",u.Account),
                    new SqlParameter("@Password",u.Password)
                };
                Hashtable hashtable = new Hashtable()
                {
                    {"insert into [dbo].[User] (CreateTime,UpdateTime,ID,Name,Account,Password)values(@CreateTime,@UpdateTime,@ID,@Name,@Account,@Password)", parameters}
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
            IDataParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",id)
            };
            Hashtable hashtable = new Hashtable()
            {
                {"delete from [dbo].[User] where ID=@id",parameters}
            };
            result = db.ExecuteTrans(hashtable);
            return user;
        }

        public List<User> GetAll()
        {
            var ds = db.Execute("select * from [dbo].[User];select * from [dbo].[User]");
            return ds.ToEnumerable<User>().ToList();
        }

        public User GetById(Guid id)
        {
            IDataParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",id)
            };
            var ds = db.Execute("select * from [dbo].[User] where ID=@id", parameters);
            var user = ds.ToModel<User>();
            return user;
        }

        User GetById(User user)
        {
            return GetById(user.ID);
        }

        public string GetName(Guid id)
        {
            IDataParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",id)
            };
            var data = db.ExecuteScalar("select Name from [dbo].[User] where ID=@id", parameters);
            return (data ?? "").ToString();
        }
    }
}
