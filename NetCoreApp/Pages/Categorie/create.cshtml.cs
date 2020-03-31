using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreApp.Pages.Categorie
{
    public class createModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public NetCoreApp.Core.Entities.Categorie Categorie { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public string Message { get; set; }

        public createModel(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        public async Task OnGet()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
           _configuration["ApiBaseUrl"] + "categorie/" + Id);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                Categorie = JsonConvert.DeserializeObject<Core.Entities.Categorie>(stringResponse);
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "Vous avez des erreus de formes";
                return Page();
            }

            var content = JsonConvert.SerializeObject(Categorie);
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = new HttpResponseMessage();
            if (Id == 0)
            {
                response = await client.PostAsync(_configuration["ApiBaseUrl"] + "categorie",
                new StringContent(content, Encoding.UTF8, "application/json"));
            }
            else
            {
                response = await client.PutAsync(_configuration["ApiBaseUrl"] + "categorie/" + Id.ToString(),
                new StringContent(content, Encoding.UTF8, "application/json"));
            }

            if (!response.IsSuccessStatusCode)
            {
                Message = await response.Content.ReadAsStringAsync();
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}