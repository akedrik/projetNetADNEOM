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

namespace NetCoreApp.Test.Tests_Intégrations.Pages
{
    public class CategoriePageTest
    {
        [Fact]
        public async Task Page_Categorie_OnGet_Retourne_5_Categories()
        {
            // Arrange  
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var categories = new List<Categorie>()
            {
                new Categorie(1,"Teeshirt"),
                new Categorie(2,"Pantalon"),
                new Categorie(3,"Maillot"),
                new Categorie(4,"Chaussure"),
                new Categorie(5,"Chaussette")
            };

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(categories)),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
           // client.BaseAddress = fixture.Create<Uri>();
            httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            // Act  
            var controller = new indexModel(httpClientFactory.Object);
            await controller.OnGet();

            //Assert
            httpClientFactory.Verify(f => f.CreateClient(It.IsAny<String>()), Times.Once);
            Assert.Equal(5, controller.Categories.Count);
        }
    }
}
