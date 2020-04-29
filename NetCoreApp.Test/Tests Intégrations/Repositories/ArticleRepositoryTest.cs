using Microsoft.EntityFrameworkCore;
using NetCoreApp.Core.Interfaces.Repositories;
using NetCoreApp.Infrastructure.Data;
using NetCoreApp.Infrastructure.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NetCoreApp.Test.Tests_Intégrations.Repositories
{
    public class ArticleRepositoryTest
    {
        private readonly NetCoreAppContext _dbContext;
        private readonly IArticleRepository _articleRepository;
        private readonly ICategorieRepository _categorieRepository;

        public ArticleRepositoryTest()
        {
            var dbOptions = new DbContextOptionsBuilder<NetCoreAppContext>()
                .UseInMemoryDatabase(databaseName: "TestNetCoreAppArticle")
                .Options;
            _dbContext = new NetCoreAppContext(dbOptions);
            _articleRepository = new ArticleRepository(_dbContext);
            _categorieRepository = new CategorieRepository(_dbContext);
        }

        [Fact]
        public async Task Test_GetAllArticlesIncludeCategorie_Retourne_Aussi_Categorie()
        {
            await _categorieRepository.AddAsync(new Core.Entities.Categorie(1, "Polo"));
            await _categorieRepository.AddAsync(new Core.Entities.Categorie(2, "TV"));
            await _categorieRepository.AddAsync(new Core.Entities.Categorie(3, "Maillot"));

            await _articleRepository.AddAsync(new Core.Entities.Article{ Id=1, Libelle="Article 1", 
                Prix=10, Stock=4, CategorieId=2, Image=""});
            await _articleRepository.AddAsync(new Core.Entities.Article
            {
                Id = 2,
                Libelle = "Article 2",
                Prix = 10,
                Stock = 4,
                CategorieId = 2,
                Image = ""
            });
            await _articleRepository.AddAsync(new Core.Entities.Article
            {
                Id = 3,
                Libelle = "Article 3",
                Prix = 10,
                Stock = 4,
                CategorieId = 1,
                Image = ""
            });

            var articles = await _articleRepository.ListAllAsync();
            List< Core.Entities.Article> listArticles = articles.ToList();

            Assert.Equal(3, listArticles.Count);
            Assert.Equal("TV", listArticles[1].Categorie.Libelle);
            Assert.Equal("Polo", listArticles[2].Categorie.Libelle);

            List<Core.Entities.Categorie> categories = _categorieRepository.GetCategoriesInArticles().ToList();
            Assert.Equal(2, categories.Count);
        }
    }
}
