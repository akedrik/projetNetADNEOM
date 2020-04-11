using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Services.Pages;

namespace NetCoreApp.Components.Article
{
    public class ArticleListBase : ComponentBase
    {
        public List<NetCoreApp.Core.Entities.Article> Articles { get; set; }
        public string Search { get; set; }
        [Inject] IArticlePageService _articlePageService { get; set; }

        public async Task GetArticlesAsync()
        {
            Articles = new List<NetCoreApp.Core.Entities.Article>();
            Articles = await _articlePageService.GetArticles();
        }

        protected override async Task OnInitializedAsync()
        {
            await GetArticlesAsync();
        }

        public async Task GetArticleByLibelle()
        {
            Articles = new List<NetCoreApp.Core.Entities.Article>();
            NetCoreApp.Core.Entities.Article article = new NetCoreApp.Core.Entities.Article();

            article = await _articlePageService.GetArticleById(int.Parse(Search));
            if (!string.IsNullOrEmpty(article.Libelle)) Articles.Add(article);
        }
    }
}
