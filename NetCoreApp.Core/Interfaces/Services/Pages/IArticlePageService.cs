using Microsoft.AspNetCore.Http;
using NetCoreApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreApp.Core.Interfaces.Services.Pages
{
    public interface IArticlePageService
    {
        Task<List<Article>> GetArticles();
        Task<List<Article>> GetArticleContainsLibelle(string libelle);
        Task DeleteArticle(int id);
        Task<Article> GetArticleById(int id);
        Task SaveFile(IFormFile FormFile, Article article);
        Task<string> SaveArticle(int id, IFormFile FormFile, Article article);

       
    }
}
