using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Repositories;

namespace NetCoreApp.Infrastructure.Data.Repositories
{
    public class CategorieRepository : EfRepository<Categorie>, ICategorieRepository
    {
        public CategorieRepository(NetCoreAppContext dbContext)
            : base(dbContext)
        {

        }
    }
}
