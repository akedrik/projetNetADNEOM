using Microsoft.AspNetCore.Components;
using NetCoreApp.Core.Interfaces.Services.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreApp.Components.Shop.Categorie
{
    public class ListCategorieBase : ComponentBase
    {
        public List<NetCoreApp.Core.Entities.Categorie> Categories { get; set; }
        [Inject] ICategoriePageService _categoriePageService { get; set; }
        [Inject] NavigationManager _navigationManager { get; set; }

        public async Task GetCategoriesAsync()
        {
            Categories = await _categoriePageService.GetCategoriesInArticles();
        }

        protected override async Task OnInitializedAsync()
        {
            await GetCategoriesAsync();
        }

        protected void SelectArticlesByCategorie(int id)
        {
            _navigationManager.NavigateTo("/Shop/" + id.ToString());
        }
    }
}
