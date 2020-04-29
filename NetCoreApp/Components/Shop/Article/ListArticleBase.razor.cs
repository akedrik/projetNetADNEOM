using Microsoft.AspNetCore.Components;
using NetCoreApp.Core.Interfaces.Services.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApp.Components.Shop.Article
{
    public class ListArticleBase : ComponentBase
    {
        public List<NetCoreApp.Core.Entities.Article> Articles { get; set; }
        [Inject] IArticlePageService _articlePageService { get; set; }
        [Parameter]
        public string Categorie { get; set; }
        public async Task GetArticlesAsync()
        {
            
            Articles = await _articlePageService.GetArticles();
            if (Categorie != "0")
                Articles = Articles.Where(c => c.CategorieId == int.Parse(Categorie)).ToList();
        }

        protected override async  Task OnParametersSetAsync()
        {
            Categorie = Categorie ?? "0";
            await GetArticlesAsync();
        }

    }
}
