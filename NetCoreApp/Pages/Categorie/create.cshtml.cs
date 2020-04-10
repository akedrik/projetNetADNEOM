using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using NetCoreApp.Core.Interfaces.Services.Pages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreApp.Pages.Categorie
{
    [Authorize]
    public class createModel : PageModel
    {
        private readonly ICategoriePageService _categoriePageService;

        [BindProperty]
        public NetCoreApp.Core.Entities.Categorie Categorie { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public string Message { get; set; }

        public createModel(ICategoriePageService categoriePageService)
        {
            _categoriePageService = categoriePageService;
        }
        public async Task OnGet()
        {
            Categorie = await _categoriePageService.GetCategorieById(Id);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "Vous avez des erreus de formes";
                return Page();
            }

            var statut = await _categoriePageService.SaveCategorie(Id, Categorie);
            if (!string.IsNullOrEmpty(statut))
            {
                Message = statut;
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}