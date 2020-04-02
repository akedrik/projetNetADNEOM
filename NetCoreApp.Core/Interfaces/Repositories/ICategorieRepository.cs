using NetCoreApp.Core.Entities;

namespace NetCoreApp.Core.Interfaces.Repositories
{
    public interface ICategorieRepository : IAsyncRepository<Categorie>
    {
        Categorie GetByLibelleAsync(string libelle);
        Categorie GetByLibelleWithNoIdAsync(int id, string libelle);
        int MaxId();
    }
}
