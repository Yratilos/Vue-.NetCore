using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace WebApi.Systems.Extensions
{
    public class CustomCache
    {
        private IMemoryCache memoryCache;
        IConfiguration Configuration;
        public CustomCache(IMemoryCache memoryCache, IConfiguration configuration)
        {
            this.memoryCache = memoryCache;
            Configuration = configuration;
        }

        /// <summary>
        /// 要保证实时性的话在修改数据后清空对应缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                if (Configuration["Cache:Enabled"].ToBoolean())
                {
                    var res = memoryCache.TryGetValue(key, out object cachedData);
                    return res ? cachedData : null;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Configuration["Cache:Enabled"].ToBoolean())
                {
                    memoryCache.Set(key, value);
                }
            }
        }

    }
}
