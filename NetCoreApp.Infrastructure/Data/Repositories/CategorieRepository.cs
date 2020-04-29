using Microsoft.EntityFrameworkCore;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreApp.Infrastructure.Data.Repositories
{
    public class CategorieRepository : EfRepository<Categorie>, ICategorieRepository
    {
        public CategorieRepository(NetCoreAppContext dbContext)
            : base(dbContext)
        {

        }

        public Categorie GetByLibelleAsync(string libelle)
        {
            return _dbContext.Categories.
                            Where(c => c.Libelle == libelle).FirstOrDefault();
        }

        public Categorie GetByLibelleWithNoIdAsync(int id, string libelle)
        {
            return _dbContext.Categories.
                Where(c => c.Libelle == libelle &&
                        c.Id != id).FirstOrDefault();
        }

        public IEnumerable<Categorie> GetCategoriesInArticles()
        {
            return _dbContext.Categories.Include(c => c.Articles).Where(c=>c.Articles.Count!=0);
        }

        public int MaxId()
        {
            return _dbContext.Categories.Max(c => c.Id);
        }
    }
}
