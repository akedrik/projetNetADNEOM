using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreApp.Infrastructure.Data.Repositories
{
    public class ArticleRepository : EfRepository<Article>, IArticleRepository
    {
        public ArticleRepository(NetCoreAppContext dbContext)
            : base(dbContext)
        {
                
        }
    }
}
