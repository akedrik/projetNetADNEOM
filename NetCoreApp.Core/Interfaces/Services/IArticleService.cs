using NetCoreApp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreApp.Core.Interfaces.Services
{
    public interface IArticleService
    {
        Task AddArticle(Article article);
        Task UpdateArticle(Article article);
        Task DeleteArticle(int id);
        Task<IEnumerable<Article>> GetAllArticles();
        Task<Article> GetArticleById(int id);
    }
}
