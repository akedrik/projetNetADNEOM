using NetCoreApp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreApp.Core.Interfaces.Services.Pages
{
    public interface ICategoriePageService
    {
        Task<List<Categorie>> GetCategories();
        Task DeleteCategorie(int id);
        Task<Categorie> GetCategorieById(int id);
        Task<string> SaveCategorie(int id,Categorie categorie);
    }
}
