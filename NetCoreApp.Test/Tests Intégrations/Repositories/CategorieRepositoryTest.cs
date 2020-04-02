using Microsoft.EntityFrameworkCore;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Repositories;
using NetCoreApp.Infrastructure.Data;
using NetCoreApp.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NetCoreApp.Test.Tests_Intégrations.Repositories
{
   public class CategorieRepositoryTest
    {
        private readonly NetCoreAppContext _dbContext;
        private readonly ICategorieRepository _categorieRepository;

        public CategorieRepositoryTest()
        {
            var dbOptions = new DbContextOptionsBuilder<NetCoreAppContext>()
                .UseInMemoryDatabase(databaseName: "TestNetCoreAppCategorie")
                .Options;
            _dbContext = new NetCoreAppContext(dbOptions);
            _categorieRepository = new CategorieRepository(_dbContext);
        }

        [Fact]
        public async Task Test_MaxId_et_CountAsync_Retourne_Le_Maximum()
        {
            await NetCoreAppContextSeed.SeedAsync(_dbContext);

            int maxId = _categorieRepository.MaxId();
            Assert.Equal(10, maxId);
            
            Categorie categorie = new Categorie(11, "Eau");
            await _categorieRepository.AddAsync(categorie);
            maxId = _categorieRepository.MaxId();
            Assert.Equal(11, maxId);

            var rowsCount = await _categorieRepository.CountAsync();
            Assert.Equal(11, rowsCount);
        }
    }
}
