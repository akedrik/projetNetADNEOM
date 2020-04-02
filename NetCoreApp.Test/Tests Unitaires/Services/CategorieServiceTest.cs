using Moq;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Exceptions;
using NetCoreApp.Core.Interfaces.Repositories;
using NetCoreApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NetCoreApp.Test.Tests_Unitaires.Services
{
    public class CategorieServiceTest
    {
        private Task<IEnumerable<Categorie>> _categories;
        private Mock<ICategorieRepository> _mockCategorieRepository;
        private CategorieService _categorieService;
        private List<Categorie> _listeCategories;
        private int _rowsCount = 0;
        public CategorieServiceTest()
        {
            _listeCategories = new List<Categorie>();
            _categories = GetCategories();
            _mockCategorieRepository = new Mock<ICategorieRepository>();
            _mockCategorieRepository.Setup(m => m.ListAllAsync()).Returns(_categories);
            _mockCategorieRepository.Setup(m => m.CountAsync()).Returns(GetRowsCount());            
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
            _mockCategorieRepository.Setup(m => m.GetByLibelleAsync(It.IsAny<string>()))
                .Returns(getByLibelle(categorie.Libelle));
            var categorieAvecLibelle = _listeCategories.Where(c => c.Libelle == libelle).FirstOrDefault();
            int nombreAppel = (categorieAvecLibelle != null ? 0 : 1);
            if (nombreAppel == 0)
            {
                await Assert.ThrowsAsync<RecordAlreadyExistException>(() => _categorieService.AddCategorie(categorie));
            }
            else
            {
                await _categorieService.AddCategorie(categorie);
            }
            _mockCategorieRepository.Verify(x => x.AddAsync(It.IsAny<Categorie>()), Times.Exactly(nombreAppel));

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
                _mockCategorieRepository.Setup(m => m.CountAsync()).Returns(GetRowsCount());
            }

            Categorie categorie = new Categorie(0, "Châpeau");
            _mockCategorieRepository.Setup(m => m.GetByLibelleAsync(It.IsAny<string>()))
                .Returns(getByLibelle(categorie.Libelle));
            var maxId = _listeCategories.Any() ? _listeCategories.Max(c => c.Id):0;
           
            _mockCategorieRepository.Setup(m => m.MaxId()).Returns(maxId);

            Categorie categorieRetour = null;
            _mockCategorieRepository.Setup(x => x.AddAsync(It.IsAny<Categorie>()))
                .Callback<Categorie>(
                    c =>
                    {
                        categorieRetour = c;
                    }
                );
            await _categorieService.AddCategorie(categorie);

            Assert.NotNull(categorieRetour);
            Assert.Equal((initialiseListe ? 1 : 6), categorieRetour.Id);
            Assert.Equal(categorie.Libelle, categorieRetour.Libelle);
        }

        [Fact]
        public async Task Test_AddCategorie_Leve_Exception_De_Type_RecordAlreadyExistException()
        {
            Categorie categorie = new Categorie(6, "Maillot");
            _mockCategorieRepository.Setup(m => m.GetByLibelleAsync(It.IsAny<string>()))
                .Returns(getByLibelle(categorie.Libelle));
            var ex = await Assert.ThrowsAsync<RecordAlreadyExistException>(() => _categorieService.AddCategorie(categorie));
            Assert.Equal("Cet enregistrement existe déjà.", ex.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(7)]
        public async Task Test_UpdateCategorie_Verifie_Si_La_Methode_UpdateAsync_Est_Appelee(int id)
        {
            _mockCategorieRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>()))
                .Returns(GetCategorieById(id));

            var categorie = _listeCategories.Where(c => c.Id == id).SingleOrDefault();
            int nombreAppel = (categorie == null ? 0 : 1);

            if (nombreAppel == 0)
            {
                await Assert.ThrowsAsync<RecordNotFoundException>(() => _categorieService.UpdateCategorie(id, "Coronavirus"));
            }
            else
            {
                await _categorieService.UpdateCategorie(id, "Coronavirus");
            }

            _mockCategorieRepository.Verify(x => x.UpdateAsync(It.IsAny<Categorie>()), Times.Exactly(nombreAppel));
        }

        [Theory]
        [InlineData("RecordNotFoundException")]
        [InlineData("RecordAlreadyExistException")]
        public async Task Test_UpdateCategorie_Leve_Exacte_Exeception(string exception)
        {
            _mockCategorieRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>()))
              .Returns(GetCategorieById(exception == "RecordNotFoundException" ? 20 : 5));

            if (exception == "RecordNotFoundException")
            {
                _mockCategorieRepository.Setup(m => m.GetByLibelleWithNoIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(getByLibelleWithNoId(20, "Coronavirus"));
                await Assert.ThrowsAsync<RecordNotFoundException>(() => _categorieService.UpdateCategorie(20, "Coronavirus"));

            }
            else
            {
                _mockCategorieRepository.Setup(m => m.GetByLibelleWithNoIdAsync(It.IsAny<int>(), It.IsAny<string>()))
               .Returns(getByLibelleWithNoId(5, "Maillot"));
                await Assert.ThrowsAsync<RecordAlreadyExistException>(() => _categorieService.UpdateCategorie(5, "Maillot"));

            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(7)]
        public async Task Test_DeleteCategorie_Verifie_Si_La_Methode_DeleteAsync_Est_Appelee(int id)
        {
            _mockCategorieRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>()))
                .Returns(GetCategorieById(id));
            var categorie = _listeCategories.Where(c => c.Id == id).FirstOrDefault();
            int nombreAppel = (categorie == null ? 0 : 1);

            if (nombreAppel == 0)
            {
                await Assert.ThrowsAsync<RecordNotFoundException>(() => _categorieService.DeleteCategorie(id));
            }
            else
            {
                await _categorieService.DeleteCategorie(id);
            }

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

        private async Task<Categorie> GetCategorieById(int id)
        {
            return _listeCategories.Where(c => c.Id == id).FirstOrDefault();
        }

        private async Task<IEnumerable<Categorie>> GetCategoriesVides()
        {
            _listeCategories = new List<Categorie>();
            return _listeCategories;
        }

        private async Task<int> GetRowsCount()
        {
            _rowsCount = _listeCategories.Count();
            return _rowsCount;
        }
        private Categorie getByLibelle(string libelle)
        {
            return _listeCategories.Where(c => c.Libelle == libelle).FirstOrDefault();
        }

        private Categorie getByLibelleWithNoId(int id, string libelle)
        {
            return _listeCategories.Where(c => c.Libelle == libelle
                      && c.Id != id).FirstOrDefault();
        }
    }
}
