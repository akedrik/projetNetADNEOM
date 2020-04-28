using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreApp.Core.Interfaces.Services.Pages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace NetCoreApp.Pages.Article
{
    [Authorize]
    public class createModel : PageModel
    {
        private readonly IArticlePageService _articlePageService;
        private readonly ICategoriePageService _categoriePageService;

        [BindProperty]
        public Core.Entities.Article Article { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public string Message { get; set; }

        public List<Core.Entities.Categorie> Categories { get; set; }

        public SelectList TagOptions { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Image")]
        public IFormFile FormFile { get; set; }

        public createModel(IArticlePageService articlePageService, ICategoriePageService categoriePageService)
        {
            _articlePageService = articlePageService;
            _categoriePageService = categoriePageService;
        }
        public async Task OnGet()
        {
            Categories = await _categoriePageService.GetCategories();

            Article = await _articlePageService.GetArticleById(Id);

            TagOptions = new SelectList(Categories, nameof(Core.Entities.Categorie.Id),
                nameof(Core.Entities.Categorie.Libelle));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "Vous avez des erreus de formes";
                return Page();
            }

            var statut = await _articlePageService.SaveArticle(Id, FormFile, Article);
            if (!string.IsNullOrEmpty(statut))
            {
                Message = statut;
                return Page();
            }
            return RedirectToPage("./Search");
        }
    }
}