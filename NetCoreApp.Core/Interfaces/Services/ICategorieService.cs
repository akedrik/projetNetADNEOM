using NetCoreApp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreApp.Core.Interfaces.Services
{
    public interface ICategorieService
    {
        Task AddCategorie(Categorie categorie);
        Task UpdateCategorie(int id, string libelle);
        Task DeleteCategorie(int id);
        Task<IEnumerable<Categorie>> GetAllCategories();
        Task<Categorie> GetCategorieById(int id);
        Task<IEnumerable<Categorie>> GetCategoriesInArticles();
    }
}
