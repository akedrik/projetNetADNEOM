using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using NetCoreApp.Core.Interfaces.Services.Pages;
using Newtonsoft.Json;

namespace NetCoreApp.Pages.Article
{
    public class indexModel : PageModel
    {
        private readonly IArticlePageService _articlePageService;

        public List<Core.Entities.Article> Articles { get; set; } = new List<Core.Entities.Article>();

        public indexModel( IArticlePageService articlePageService)
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