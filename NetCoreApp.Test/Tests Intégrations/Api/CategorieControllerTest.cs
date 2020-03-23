using Microsoft.AspNetCore.Mvc;
using Moq;
using NetCoreApp.Controllers.Api;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NetCoreApp.Test.Tests_Intégrations.Api
{
    public class CategorieControllerTest
    {
        private List<Categorie> _listeCategories;
        private Categorie _categorie;
        private Task<IEnumerable<Categorie>> _categories;
        private Mock<ICategorieService> _mockCategorieServiceRepository;
        private CategorieController _CategorieController;
        public CategorieControllerTest()
        {
            _categorie = new Categorie();
            _listeCategories = new List<Categorie>();
            _categories = GetCategories();
            _mockCategorieServiceRepository = new Mock<ICategorieService>();
            _mockCategorieServiceRepository.Setup(m => m.GetAllCategories()).Returns(_categories);
            _CategorieController = new CategorieController(_mockCategorieServiceRepository.Object);
        }

        [Fact]
        public async Task Test_Get_Retourne_5_Categories()
        {
            var actionResult = await _CategorieController.Get();
            var okResult = actionResult as OkObjectResult;
            var categories = okResult.Value as List<Categorie>;

            Assert.NotNull(categories);
            Assert.Equal(5, categories.Count);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(7)]
        [InlineData(5)]
        [InlineData(20)]
        public async Task Test_Get_Id_Retourne_Id_Ou_NotFound(int id)
        {
            Task<Categorie> categorieId = GetCategorieById(id);
            _mockCategorieServiceRepository.Setup(m => m.GetCategorieById(It.IsAny<int>()))
                    .Returns(categorieId);

            var actionResult = await _CategorieController.Get(id);
            if (_categorie == null)
            {
                Assert.IsType<NotFoundResult>(actionResult);
            }
            else
            {
                Assert.IsType<OkObjectResult>(actionResult);
                var okResult = actionResult as OkObjectResult;
                var categorie = okResult.Value as Categorie;

                Assert.NotNull(categorie);
                Assert.Equal(_categorie.Libelle, categorie.Libelle);
            }
        }

        private async Task<IEnumerable<Categorie>> GetCategories()
        {
            _listeCategories = new List<Categorie>()
            {
                new Categorie(1,"Teeshirt"),
                new Categorie(2,"Pantalon"),
                new Categorie(3,"Maillot"),
                new Categorie(4,"Chaussure"),
                new Categorie(5,"Chaussette")
            };
            return _listeCategories;
        }

        private async Task<Categorie> GetCategorieById(int id)
        {
            _categorie = _listeCategories.Where(c => c.Id == id).FirstOrDefault();
            return _categorie;
        }
    }
}
