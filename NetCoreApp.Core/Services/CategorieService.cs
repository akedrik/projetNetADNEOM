using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Exceptions;
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
        private ICategorieRepository _categorieRepository;

        public CategorieService(ICategorieRepository categorieRepository)
        {
            _categorieRepository = categorieRepository;
        }
        
        public async Task AddCategorie(Categorie categorie)
        {
            var rowsCount = await _categorieRepository.CountAsync();
            var categorieAvecLeLibelle = _categorieRepository.GetByLibelleAsync(categorie.Libelle);
            if (categorieAvecLeLibelle != null)
                throw new RecordAlreadyExistException();

            var maxId = 1;
            if (rowsCount > 0)
            {
                maxId = _categorieRepository.MaxId();
                maxId += 1;
            }
            categorie.SetId(maxId);
            categorie.DateSaisie = DateTime.Now;
            categorie.DateModification = DateTime.Now;
            try
            {
                await _categorieRepository.AddAsync(categorie);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Categorie>> GetAllCategories()
        {
            return await _categorieRepository.ListAllAsync();
        }

        public async Task<Categorie> GetCategorieById(int id)
        {
            return await _categorieRepository.GetByIdAsync(id);
        }

        public async Task UpdateCategorie(int id,string libelle)
        {
            var categorieAvecLeLibelle = _categorieRepository.GetByLibelleWithNoIdAsync(id, libelle);
            if (categorieAvecLeLibelle != null)
                throw new RecordAlreadyExistException();

            Categorie categorie = await GetCategorieById(id);
            if (categorie == null)
                throw new RecordNotFoundException();

            categorie.Libelle = libelle;
            categorie.DateModification = DateTime.Now;
            try
            {
                await _categorieRepository.UpdateAsync(categorie);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeleteCategorie(int id)
        {
            Categorie categorie = await GetCategorieById(id);
            if (categorie == null)
                throw new RecordNotFoundException();
            try
            {
                await _categorieRepository.DeleteAsync(categorie);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
