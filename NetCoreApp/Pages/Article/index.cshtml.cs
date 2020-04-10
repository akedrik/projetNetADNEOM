using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetCoreApp.Core.Interfaces.Services.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreApp.Pages.Article
{
    [Authorize]
    public class indexModel : PageModel
    {
        private readonly IArticlePageService _articlePageService;

        public List<Core.Entities.Article> Articles { get; set; } = new List<Core.Entities.Article>();

        public indexModel(IArticlePageService articlePageService)
        {
            _articlePageService = articlePageService;
        }

        public async Task OnGetAsync()
        {
            Articles = await _articlePageService.GetArticles();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _articlePageService.DeleteArticle(id);
            return RedirectToPage();
        }
    }
}