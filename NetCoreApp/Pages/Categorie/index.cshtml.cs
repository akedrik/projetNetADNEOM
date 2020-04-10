using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetCoreApp.Core.Interfaces.Services.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreApp.Pages.Categorie
{
    [Authorize]
    public class indexModel : PageModel
    {
        private readonly ICategoriePageService _categoriePageService;

        public List<Core.Entities.Categorie> Categories { get; set; } = new List<Core.Entities.Categorie>();

        public indexModel(ICategoriePageService categoriePageService)
        {
            _categoriePageService = categoriePageService;
        }
        public async Task OnGetAsync()
        {
            Categories = await _categoriePageService.GetCategories();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _categoriePageService.DeleteCategorie(id);
            return RedirectToPage();
        }
    }
}