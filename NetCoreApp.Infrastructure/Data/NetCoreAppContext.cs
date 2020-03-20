using Microsoft.EntityFrameworkCore;
using NetCoreApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreApp.Infrastructure.Data
{
    public class NetCoreAppContext : DbContext
    {
        public NetCoreAppContext(DbContextOptions<NetCoreAppContext> options)
            : base (options)
        {

        }

        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
       
    }
}
