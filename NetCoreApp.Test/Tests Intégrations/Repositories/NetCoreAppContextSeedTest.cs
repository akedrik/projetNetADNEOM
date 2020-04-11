using Microsoft.EntityFrameworkCore;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Repositories;
using NetCoreApp.Infrastructure.Data;
using NetCoreApp.Infrastructure.Data.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NetCoreApp.Test.Tests_Intégrations.Repositories
{
    public class NetCoreAppContextSeedTest
    {
        private readonly NetCoreAppContext _dbContext;
        private readonly ICategorieRepository _categorieRepository;
        private readonly IArticleRepository _articleRepository;

        public NetCoreAppContextSeedTest()
        {
            var dbOptions = new DbContextOptionsBuilder<NetCoreAppContext>()
                .UseInMemoryDatabase(databaseName: "TestNetCoreAppSeed")
                .Options;
            _dbContext = new NetCoreAppContext(dbOptions);
            _articleRepository = new ArticleRepository(_dbContext);
            _categorieRepository = new CategorieRepository(_dbContext);
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

        [Fact]
        public void Test_GetCategoriesFromJsonFile_Retourne_Liste_Des_Categories()
        {
            var categories = NetCoreAppContextSeed.GetCategoriesFromJsonFile();
            Assert.Equal(10, categories.ToList().Count);
        }
    }
}
