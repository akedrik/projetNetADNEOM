using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Exceptions;
using NetCoreApp.Core.Interfaces.Repositories;
using NetCoreApp.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreApp.Core.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }
        public async Task AddArticle(Article article)
        {
            var rowsCount = await _articleRepository.CountAsync();
            var articleAvecLeLibelle = _articleRepository.GetByLibelleAsync(article.Libelle);
           
            if (articleAvecLeLibelle != null)
                throw new RecordAlreadyExistException();

            var maxId = 1;
            if (rowsCount > 0)
            {
                maxId = _articleRepository.MaxId();
                maxId += 1;
            }
            article.SetId(maxId);
            article.DateSaisie = DateTime.Now;
            article.DateModification = DateTime.Now;
            try
            {
                await _articleRepository.AddAsync(article);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeleteArticle(int id)
        {
            Article article = await GetArticleById(id);
            if (article == null)
                throw new RecordNotFoundException();
            try
            {
                await _articleRepository.DeleteAsync(article);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Article>> GetAllArticles()
        {
            return await _articleRepository.ListAllAsync();
        }

        public async Task<Article> GetArticleById(int id)
        {
            return await _articleRepository.GetByIdAsync(id);
        }

        public async Task UpdateArticle(Article article)
        {
            var articleAvecLeLibelle = _articleRepository.GetByLibelleWithNoIdAsync(article.Id, article.Libelle);
            if (articleAvecLeLibelle != null)
                throw new RecordAlreadyExistException();

            Article article1 = await GetArticleById(article.Id);
            if (article1 == null)
                throw new RecordNotFoundException();

            article1.Libelle = article.Libelle;
            article1.Prix = article.Prix;
            article1.Stock = article.Stock;
            article1.Image = article.Image;
            article1.DateModification = DateTime.Now;
            article1.CategorieId = article.CategorieId;
            try
            {
                await _articleRepository.UpdateAsync(article1);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
