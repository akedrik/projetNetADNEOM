using Microsoft.EntityFrameworkCore;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApp.Infrastructure.Data.Repositories
{
    public class ArticleRepository : EfRepository<Article>, IArticleRepository
    {
        public ArticleRepository(NetCoreAppContext dbContext)
            : base(dbContext)
        {

        }
        public override  async Task<IEnumerable<Article>> ListAllAsync()
        {
            await Task.CompletedTask;
            return _dbContext.Articles.Include(a => a.Categorie).ToList();
        }

        public override async Task<Article> GetByIdAsync(int id)
        {
            await Task.CompletedTask;
            return _dbContext.Articles.Include(a => a.Categorie)
                     .Where(a => a.Id == id).FirstOrDefault();
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
