using NetCoreApp.Core.Entities;

namespace NetCoreApp.Infrastructure.Data.Repositories
{
    public class CategorieRepository : EfRepository<Categorie>
    {
        public CategorieRepository(NetCoreAppContext dbContext)
            : base(dbContext)
        {

        }
    }
}
