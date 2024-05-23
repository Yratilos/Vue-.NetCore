﻿using WebApi.IRepositorys;
using WebApi.IServices;
using WebApi.Models;
using WebApiTests;

namespace WebApi.Services.Tests
{
    [TestClass()]
    public class UserServiceTests
    {
        static IUserService UserService
        {
            get
            {
                var db = Common.GetDataBase();
                var Configuration = Common.GetConfiguration();
                IUserRepository userRepository;
                switch (Configuration["DataBase"])
                {
                    case "KingBase":
                        userRepository = new Repositorys.KingBaseRepositorys.UserKingBase(db);
                        break;
                    default:
                        userRepository = new Repositorys.SqlServerRepositorys.UserSqlServer(db);
                        break;
                }
                return new UserService(userRepository);
            }
        }

        [DataRow("2056ee13-aa02-ef11-9c2c-5a44875600c1")]
        [DataRow("ef21f658-83fc-ee11-9c29-5a44875800c2")]
        [TestMethod()]
        public void DeleteTest(string id)
        {
            var data = UserService.Delete(new Guid(id), out _);
            Assert.IsNotNull(data);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var data = UserService.GetAll();
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Count >= 0);
        }

        [DataRow("2056ee13-aa02-ef11-9c2c-5a44875600c1")]
        [DataRow("ef21f658-83fc-ee11-9c29-5a44875800c2")]
        [TestMethod()]
        public void GetByIdTest(string id)
        {
            var data = UserService.GetById(new Guid(id));
            Assert.IsNotNull(data);
        }

        [DataRow("2056ee13-aa02-ef11-9c2c-5a44875600c1")]
        [DataRow("ef21f658-83fc-ee11-9c29-5a44875800c2")]
        [TestMethod()]
        public void GetNameTest(string id)
        {
            var data = UserService.GetName(new Guid(id));
            Assert.IsNotNull(data);
        }

        [DataRow("2056ee13-aa02-ef11-9c2c-5a44875600c1")]
        [TestMethod()]
        public void AddTest(string _id)
        {
            var id = new Guid(_id);
            var u = UserService.Add(new User() { ID = id });
            var d = UserService.Delete(id, out bool b);
            Assert.AreEqual(u.ID, id);
            Assert.AreEqual(d.ID, id);
        }
    }
}