using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Repositories;
using NetCoreApp.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApp.Core.Services
{
    public class CategorieService : ICategorieService
    {
        private IAsyncRepository<Categorie> _categorieRepository;

        public CategorieService(IAsyncRepository<Categorie> categorieRepository)
        {
            _categorieRepository = categorieRepository;
        }
        public async Task<bool> AddCategorie(Categorie categorie)
        {
            var categories = await GetAllCategories();
            var categorieAvecLeLibelle = categories.Where(c => c.Libelle == categorie.Libelle).FirstOrDefault();
            if (categorieAvecLeLibelle != null)
                return false;

            var maxId = 1;
            if (categories.Any())
            {
                maxId = categories.Max(x => x.Id);
                maxId += 1;
            }
            categorie.setId(maxId);
            categorie.DateSaisie = DateTime.Now;
            categorie.DateModification = DateTime.Now;
            try
            {
                await _categorieRepository.AddAsync(categorie);
                return true;
            }
            catch { return false;}
        }

        public async Task<IEnumerable<Categorie>> GetAllCategories()
        {
            return await _categorieRepository.ListAllAsync();
        }

        public async Task<Categorie> GetCategorieById(int id)
        {
            return await _categorieRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateCategorie(int id,string libelle)
        {
            Categorie categorie = await GetCategorieById(id);
            if (categorie == null)
                return false;

            categorie.Libelle = libelle;
            categorie.DateModification = DateTime.Now;
            try
            {
                await _categorieRepository.UpdateAsync(categorie);
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteCategorie(int id)
        {
            Categorie categorie = await GetCategorieById(id);
            if (categorie == null)
                return false;
            try
            {
                await _categorieRepository.DeleteAsync(categorie);
                return true;
            }
            catch { return false; }
           
        }

    }
}
