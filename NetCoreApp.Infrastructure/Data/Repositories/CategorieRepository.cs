using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Repositories;
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

        public int MaxId()
        {
            return _dbContext.Categories.Max(c => c.Id);
        }
    }
}
