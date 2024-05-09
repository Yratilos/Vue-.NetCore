using Microsoft.Extensions.Configuration;
using WebApi.Systems.DataBases;

namespace WebApiTests
{
    internal class Common
    {
        public static IConfigurationRoot GetConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var Configuration = configBuilder.Build();
            return Configuration;
        }

        public static IDataBase GetDataBase()
        {
            var Configuration = GetConfiguration();
            var dataBase = Configuration["DataBase"];
            IDataBase db = DataBaseFactory.CreateDataBase(dataBase, Configuration["ConnectionStrings:" + dataBase]);
            return db;
        }
    }
}
