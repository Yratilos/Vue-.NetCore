using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace WebApi.Systems.Extensions
{
    public class Redis
    {
        private IDatabase redis;
        IConfiguration Configuration;

        public Redis(IConfiguration configuration)
        {
            Configuration = configuration;
            if (Configuration["Redis:Enabled"].ToBoolean())
            {
                redis = new Lazy<ConnectionMultiplexer>(() =>
                {
                    return ConnectionMultiplexer.Connect(Configuration["Redis:Connection"]);
                }).Value.GetDatabase();
            }
        }

        public object this[string key]
        {
            get
            {
                if (Configuration["Redis:Enabled"].ToBoolean())
                {
                    return redis.StringGet(key);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Configuration["Redis:Enabled"].ToBoolean())
                {
                    redis.StringSet(key, JsonConvert.SerializeObject(value));
                }
            }
        }
    }
}
