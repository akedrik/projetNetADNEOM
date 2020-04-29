using Microsoft.Extensions.Configuration;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Services.Pages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreApp.Services
{
    public class CategoriePageService : ICategoriePageService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public CategoriePageService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        public async Task DeleteCategorie(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete,
           _configuration["ApiBaseUrl"] + "categorie/" + id);

            var client = _clientFactory.CreateClient();
            await client.SendAsync(request);
        }

        public async Task<Categorie> GetCategorieById(int id)
        {
            Categorie categorie = new Categorie();
            var request = new HttpRequestMessage(HttpMethod.Get,
           _configuration["ApiBaseUrl"] + "categorie/" + id);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                categorie = JsonConvert.DeserializeObject<Core.Entities.Categorie>(stringResponse);
            }
            return categorie;
        }

        public async Task<List<Categorie>> GetCategories()
        {
            List<Categorie> categories = new List<Categorie>();
            var request = new HttpRequestMessage(HttpMethod.Get,
           _configuration["ApiBaseUrl"] + "categorie");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<Core.Entities.Categorie>>(stringResponse);
            }
            return categories;
        }

        public async Task<List<Categorie>> GetCategoriesInArticles()
        {
            List<Categorie> categories = new List<Categorie>();
            var request = new HttpRequestMessage(HttpMethod.Get,
           _configuration["ApiBaseUrl"] + "categorie/inArticle");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<Core.Entities.Categorie>>(stringResponse);
            }
            return categories;
        }

        public async Task<string> SaveCategorie(int id, Categorie categorie)
        {
            var message = "";
            var content = JsonConvert.SerializeObject(categorie);
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = new HttpResponseMessage();
            if (id == 0)
            {
                response = await client.PostAsync(_configuration["ApiBaseUrl"] + "categorie",
                new StringContent(content, Encoding.UTF8, "application/json"));
            }
            else
            {
                response = await client.PutAsync(_configuration["ApiBaseUrl"] + "categorie/" + id.ToString(),
                new StringContent(content, Encoding.UTF8, "application/json"));
            }

            if (!response.IsSuccessStatusCode)
            {
                message = await response.Content.ReadAsStringAsync();
            }
            return message;
        }
    }
}
