using Microsoft.EntityFrameworkCore;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces;
using NetCoreApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NetCoreApp.Test.Tests_Intégrations.Repositories
{
    public class EFRepositoryTest
    {
        private readonly NetCoreAppContext _dbContext;
        private readonly IAsyncRepository<Categorie> _categorieRepository;
        private Categorie _categorie;
        private List<Categorie> _categories;
        private const int NOMBRE_INSERTION = 5;

        public EFRepositoryTest()
        {
            var dbOptions = new DbContextOptionsBuilder<NetCoreAppContext>()
                .UseInMemoryDatabase(databaseName: "TestNetCoreApp")
                .Options;
            _dbContext = new NetCoreAppContext(dbOptions);
            _categorieRepository = new EfRepository<Categorie>(_dbContext);
            _categories = new List<Categorie>();

            for (int i = 1; i <= NOMBRE_INSERTION; i++)
            {
                _categorie = new Categorie(i, "Teeshirt " + i);
                _categories.Add(_categorie);
            }
        }

        [Fact]
        public async Task Test_CRUD_EFRepository()
        {
            int count_valeur = await _categorieRepository.CountAsync();
            Assert.Equal(0, count_valeur);

            foreach (var elt in _categories)
                await _categorieRepository.AddAsync(elt);

            count_valeur = await _categorieRepository.CountAsync();
            Assert.Equal(_categories.Count, count_valeur);

            Categorie categorie = await _categorieRepository.GetByIdAsync(2);
            Assert.Equal("Teeshirt 2", categorie.Libelle);

            categorie = await _categorieRepository.GetByIdAsync(3);
            categorie.Libelle = "Coronavirus";
            categorie.UpdateDateModification();
            await _categorieRepository.UpdateAsync(categorie);
            
            categorie = await _categorieRepository.GetByIdAsync(3);
            Assert.Equal("Coronavirus", categorie.Libelle);
            Assert.NotEqual(categorie.DateSaisie, categorie.DateModification);
            Assert.True(categorie.DateModification > categorie.DateSaisie);

            categorie = await _categorieRepository.GetByIdAsync(3);
            await _categorieRepository.DeleteAsync(categorie);
            _categories.Remove(categorie);
            count_valeur = await _categorieRepository.CountAsync();
            Assert.Equal(_categories.Count, count_valeur);
        }

        
    

    }
}
