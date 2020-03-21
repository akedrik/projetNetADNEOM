using Microsoft.EntityFrameworkCore;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Repositories;
using NetCoreApp.Infrastructure.Data;
using NetCoreApp.Infrastructure.Data.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace NetCoreApp.Test.Tests_Intégrations.Repositories
{
    public class NetCoreAppContextSeedTest
    {
        private readonly NetCoreAppContext _dbContext;
        private readonly IAsyncRepository<Categorie> _categorieRepository;
        private readonly IAsyncRepository<Article> _articleRepository;

        public NetCoreAppContextSeedTest()
        {
            var dbOptions = new DbContextOptionsBuilder<NetCoreAppContext>()
                .UseInMemoryDatabase(databaseName: "TestNetCoreAppSeed")
                .Options;
            _dbContext = new NetCoreAppContext(dbOptions);
            _categorieRepository = new EfRepository<Categorie>(_dbContext);
            _articleRepository = new EfRepository<Article>(_dbContext);
        }

        [Fact]
        public async Task Test_Methode_SeedAsync()
        {
            await NetCoreAppContextSeed.SeedAsync(_dbContext);

            int count_categorie = await _categorieRepository.CountAsync();
            Assert.Equal(10, count_categorie);

            int count_Articles = await _articleRepository.CountAsync();
            Assert.Equal(3, count_Articles);

            var categorie = await _categorieRepository.GetByIdAsync(5);
            Assert.Equal("Chaussette", categorie.Libelle);

            var article = await _articleRepository.GetByIdAsync(2);
            Assert.Equal("Polo Tommy HilFiger", article.Libelle);
            Assert.Equal(1, article.CategorieId);
            Assert.Equal(20, article.Prix);
            Assert.Equal(75, article.Stock);
        }
    }
}
