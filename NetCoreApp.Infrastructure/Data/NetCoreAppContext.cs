using Microsoft.EntityFrameworkCore;
using NetCoreApp.Core.Entities;
using System.Reflection;

namespace NetCoreApp.Infrastructure.Data
{
    public class NetCoreAppContext : DbContext
    {
        public NetCoreAppContext(DbContextOptions<NetCoreAppContext> options)
            : base(options)
        {

        }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
