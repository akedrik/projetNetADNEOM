using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Services.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreApp.Services
{
    public class ArticlePageService : IArticlePageService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;

        public ArticlePageService(IHttpClientFactory clientFactory, IConfiguration configuration,
            IWebHostEnvironment environment, IHttpContextAccessor contextAccessor)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;

            _environment = environment;
            _contextAccessor = contextAccessor;

        }
        public async Task DeleteArticle(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete,
           _configuration["ApiBaseUrl"] + "article/" + id);

            var client = _clientFactory.CreateClient();
            await client.SendAsync(request);
        }

        public async Task<Article> GetArticleById(int id)
        {
            Article article = new Article();
            var request = new HttpRequestMessage(HttpMethod.Get,
          _configuration["ApiBaseUrl"] + "article/" + id);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                article = JsonConvert.DeserializeObject<Core.Entities.Article>(stringResponse);
            }
            return article;
        }

        public async Task<List<Article>> GetArticleContainsLibelle(string libelle)
        {
            List<Article> articles = new List<Article>();
            var request = new HttpRequestMessage(HttpMethod.Get,
           _configuration["ApiBaseUrl"] + "article/byName/" + libelle);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                articles = JsonConvert.DeserializeObject<List<Core.Entities.Article>>(stringResponse);
            }
            return articles;
        }

        public async Task<List<Article>> GetArticles()
        {
            List<Article> articles = new List<Article>();
            var request = new HttpRequestMessage(HttpMethod.Get,
           _configuration["ApiBaseUrl"] + "article");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                articles = JsonConvert.DeserializeObject<List<Core.Entities.Article>>(stringResponse);
            }
            return articles;
        }

       

        public async Task<string> SaveArticle(int id, IFormFile FormFile, Article article)
        {
            var message = "";
            await SaveFile(FormFile, article);

            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = new HttpResponseMessage();
            if (id == 0)
            {
                var content = JsonConvert.SerializeObject(article);
                response = await client.PostAsync(_configuration["ApiBaseUrl"] + "article",
                new StringContent(content, Encoding.UTF8, "application/json"));
            }
            else
            {
                article.SetId(id);
                var content = JsonConvert.SerializeObject(article);
                response = await client.PutAsync(_configuration["ApiBaseUrl"] + "article/" + id.ToString(),
                new StringContent(content, Encoding.UTF8, "application/json"));
            }

            if (!response.IsSuccessStatusCode)
            {
                message = await response.Content.ReadAsStringAsync();
            }
            return message;
        }

        public async Task SaveFile(IFormFile FormFile, Article article)
        {
            string extension = Path.GetExtension(FormFile.FileName);
            string filename = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMdd_hhmmss"), extension);
            var file = Path.Combine(_environment.WebRootPath, "uploads", "articles", filename);

            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await FormFile.CopyToAsync(fileStream);
            }
            var url = _contextAccessor.HttpContext.Request;
            article.Image = string.Format("{0}://{1}{2}{3}", url.Scheme, url.Host,
                "/uploads/articles/", filename);
        }
    }
}
