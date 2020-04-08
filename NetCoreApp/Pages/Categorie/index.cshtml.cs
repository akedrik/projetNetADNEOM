using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetCoreApp.Pages.Categorie
{
   [Authorize]
    public class indexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public List<Core.Entities.Categorie> Categories { get; set; } = new List<Core.Entities.Categorie>();

        public indexModel(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        public async Task OnGetAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
           _configuration["ApiBaseUrl"] + "categorie");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                Categories = JsonConvert.DeserializeObject<List<Core.Entities.Categorie>>(stringResponse);
            }
            else
            {
                Categories = new List<Core.Entities.Categorie>();
            }

            
        }


        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete,
          _configuration["ApiBaseUrl"] + "categorie/" + id);

            var client = _clientFactory.CreateClient();
            await client.SendAsync(request);
            return RedirectToPage();
        }
    }
}