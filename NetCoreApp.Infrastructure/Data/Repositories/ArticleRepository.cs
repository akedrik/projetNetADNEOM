using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Repositories;
using System;
using System.Linq;

namespace NetCoreApp.Infrastructure.Data.Repositories
{
    public class ArticleRepository : EfRepository<Article>, IArticleRepository
    {
        public ArticleRepository(NetCoreAppContext dbContext)
            : base(dbContext)
        {

        }

        public Article GetByLibelleAsync(string libelle)
        {
            return _dbContext.Articles.
                           Where(c => c.Libelle == libelle).FirstOrDefault();
        }

        public Article GetByLibelleWithNoIdAsync(int id, string libelle)
        {
            return _dbContext.Articles.
                Where(c => c.Libelle == libelle &&
                        c.Id != id).FirstOrDefault();
        }

        public int MaxId()
        {
            return _dbContext.Articles.Max(c => c.Id);
        }
    }
}
