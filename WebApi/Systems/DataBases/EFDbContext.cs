using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;

namespace WebApi.Systems.DataBases
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(DbContextOptions<EFDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 使用反射获取所有继承自Entity的实体类型和命名空间为WebApi.Models的所有类
            var entityTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => (t.IsSubclassOf(typeof(Entity)) || t.Namespace == "WebApi.Models") && !t.IsAbstract && t.Name != "Entity");

            foreach (var entityType in entityTypes)
            {
                modelBuilder.Entity(entityType).ToTable(entityType.Name);
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DbSet<T> GetDbSet<T>() where T : class
        {
            return Set<T>();
        }
    }
}
