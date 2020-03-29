using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace NetCoreApp.Pages.Categorie
{
    public class createModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public NetCoreApp.Core.Entities.Categorie Categorie { get; set; }
        public createModel(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var content = JsonConvert.SerializeObject(Categorie);
            var client = _clientFactory.CreateClient();
            var response = await  client.PostAsync(_configuration["ApiBaseUrl"] + "categorie",
                new StringContent(content, Encoding.UTF8, "application/json") );

            if (!response.IsSuccessStatusCode)
            {
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}