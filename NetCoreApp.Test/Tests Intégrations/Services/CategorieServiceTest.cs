using Moq;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Repositories;
using NetCoreApp.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NetCoreApp.Test.Tests_Intégrations.Services
{
    public class CategorieServiceTest
    {
        private Task<IEnumerable<Categorie>> _categories;
        private Mock<IAsyncRepository<Categorie>> _mockCategorieRepository;
        private CategorieService _categorieService;
        private List<Categorie> _listeCategories;
        public CategorieServiceTest()
        {
            _listeCategories = new List<Categorie>();
            _categories = GetCategories();
            _mockCategorieRepository = new Mock<IAsyncRepository<Categorie>>();
            _mockCategorieRepository.Setup(m => m.ListAllAsync()).Returns(_categories);
            _categorieService = new CategorieService(_mockCategorieRepository.Object);
        }

        [Fact]
        public async Task Test_GetAllCategories_Retourne_5_Categories()
        {
            var categories = await _categorieService.GetAllCategories();
            Assert.Equal(5, categories.ToList().Count);
        }

        [Theory]
        [InlineData("Châpeau")]
        [InlineData("Maillot")]
        [InlineData("Pantalon")]
        [InlineData("TV")]
        public async Task Test_AddCategorie_Verifie_Si_La_Methode_AddAsync_Est_Appelee(string libelle)
        {
            Categorie categorie = new Categorie(6, libelle);
            var categorieAvecLibelle = _listeCategories.Where(c => c.Libelle == libelle).FirstOrDefault();
            var resultat = await _categorieService.AddCategorie(categorie);
            int nombreAppel = (categorieAvecLibelle != null ? 0 : 1);

            _mockCategorieRepository.Verify(x => x.AddAsync(It.IsAny<Categorie>()), Times.Exactly(nombreAppel));
            if (nombreAppel == 0)
                Assert.False(resultat);
            else
                Assert.True(resultat);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Test_AddCategorie_Verifie_Valeur_Incremente_De_MaxId(bool initialiseListe)
        {
            if (initialiseListe)
            {
                _categories = GetCategoriesVides();
                _mockCategorieRepository.Setup(m => m.ListAllAsync()).Returns(_categories);
            }

            Categorie categorie = new Categorie(0, "Châpeau");
            Categorie categorieRetour = null;
            _mockCategorieRepository.Setup(x => x.AddAsync(It.IsAny<Categorie>()))
                .Callback<Categorie>(
                    c =>
                    {
                        categorieRetour = c;
                    }
                );
            var resultat = await _categorieService.AddCategorie(categorie);

            Assert.True(resultat);
            Assert.NotNull(categorieRetour);
            Assert.Equal((initialiseListe ? 1 : 6), categorieRetour.Id);
            Assert.Equal(categorie.Libelle, categorieRetour.Libelle);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(7)]
        public async Task Test_UpdateCategorie_Verifie_Si_La_Methode_UpdateAsync_Est_Appelee(int id)
        {
            _mockCategorieRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>()))
                .Returns(GetCategorieById(id));

            var resultat = await _categorieService.UpdateCategorie(id, "Coronavirus");
            var categorie = _listeCategories.Where(c => c.Id == id).FirstOrDefault();
            int nombreAppel = (categorie == null ? 0 : 1);

            if (categorie == null)
                Assert.False(resultat);
            else
                Assert.True(resultat);

            _mockCategorieRepository.Verify(x => x.UpdateAsync(It.IsAny<Categorie>()), Times.Exactly(nombreAppel));

        }

        [Theory]
        [InlineData(1)]
        [InlineData(7)]
        public async Task Test_DeleteCategorie_Verifie_Si_La_Methode_DeleteAsync_Est_Appelee(int id)
        {
            _mockCategorieRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>()))
                .Returns(GetCategorieById(id));

            var resultat = await _categorieService.DeleteCategorie(id);
            var categorie = _listeCategories.Where(c => c.Id == id).FirstOrDefault();
            int nombreAppel = (categorie == null ? 0 : 1);

            if (categorie == null)
                Assert.False(resultat);
            else
                Assert.True(resultat);

            _mockCategorieRepository.Verify(x => x.DeleteAsync(It.IsAny<Categorie>()), Times.Exactly(nombreAppel));

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

        private async Task<Categorie>GetCategorieById(int id)
        {
            return  _listeCategories.Where(c => c.Id == id).FirstOrDefault();
        }

        private async Task<IEnumerable<Categorie>> GetCategoriesVides()
        {
            _listeCategories = new List<Categorie>();
            return _listeCategories;
        }
    }
}
