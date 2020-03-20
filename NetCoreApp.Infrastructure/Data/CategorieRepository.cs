using NetCoreApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreApp.Infrastructure.Data
{
    public class CategorieRepository: EfRepository<Categorie>
    {
        public CategorieRepository(NetCoreAppContext dbContext)
            :base (dbContext)
        {

        }
    }
}
