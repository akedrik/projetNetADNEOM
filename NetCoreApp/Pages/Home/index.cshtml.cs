using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace NetCoreApp.Pages.Home
{
    public class indexModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPostSetLanguageAsync(string culture, string returnUrl)
        {
            //Response.Cookies.Append(
            //    CookieRequestCultureProvider.DefaultCookieName,
            //    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            //    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            //);

            Response.Cookies.Append(
                       CookieRequestCultureProvider.DefaultCookieName,
                       CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                       new CookieOptions
                       {
                           Expires = DateTimeOffset.UtcNow.AddYears(1),
                           IsEssential = true,  //critical settings to apply new culture
                           Path = "/",
                           HttpOnly = false,
                       }
                );

            return LocalRedirect(returnUrl);
        }
    }
}