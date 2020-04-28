using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NetCoreApp.Pages.Article
{
    [Authorize]
    public class searchModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}