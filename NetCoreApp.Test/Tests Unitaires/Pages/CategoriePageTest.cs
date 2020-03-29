using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using NetCoreApp.Core.Entities;
using NetCoreApp.Pages.Categorie;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NetCoreApp.Test.Tests_Unitaires.Pages
{
    public class CategoriePageTest
    {
        private readonly IConfiguration _configuration;
        private readonly List<Categorie> _categories;
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public CategoriePageTest()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"ApiBaseUrl", "http://localhost:64781/api/"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            _categories = new List<Categorie>()
            {
                new Categorie(1,"Teeshirt"),
                new Categorie(2,"Pantalon"),
                new Categorie(3,"Maillot"),
                new Categorie(4,"Chaussure"),
                new Categorie(5,"Chaussette")
            };

            _httpClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        }

        [Fact]
        public async Task Page_Categorie_OnGet_Retourne_5_Categories()
        {
            // Arrange  
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(_categories)),
                });

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            // Act  
            var controller = new indexModel(_httpClientFactory.Object, _configuration);
            await controller.OnGetAsync();

            //Assert
            _httpClientFactory.Verify(f => f.CreateClient(It.IsAny<String>()), Times.Once);
            Assert.Equal(5, controller.Categories.Count);
        }
    }
}
