using NetCoreApp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreApp.Core.Interfaces.Services
{
    public interface ICategorieService
    {
        Task<bool> AddCategorie(Categorie categorie);
        Task UpdateCategorie(Categorie categorie);
        Task<IEnumerable<Categorie>> GetAllCategories();
        Task<Categorie> GetCategorieById(int id);
    }
}
