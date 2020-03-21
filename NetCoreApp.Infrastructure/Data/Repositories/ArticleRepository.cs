using NetCoreApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreApp.Infrastructure.Data.Repositories
{
    public class ArticleRepository : EfRepository<Article>
    {
        public ArticleRepository(NetCoreAppContext dbContext)
            : base(dbContext)
        {
                
        }
    }
}
