using Microsoft.AspNetCore.Components;
using NetCoreApp.Core.Interfaces.Services.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreApp.Components.Article
{
    public class ArticleListBase : ComponentBase
    {
        public List<NetCoreApp.Core.Entities.Article> Articles { get; set; }
        public string Search { get; set; }
        [Inject] IArticlePageService _articlePageService { get; set; }
        [Inject] NavigationManager _navigationManager { get; set; }

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
            if (string.IsNullOrEmpty(Search))
                Articles = await _articlePageService.GetArticles();
            else
                Articles = await _articlePageService.GetArticleContainsLibelle(Search);
        }

        public void SelectArticle(int id)
        {
            _navigationManager.NavigateTo("/Article/Create/" + id, true);
        }
    }
}
