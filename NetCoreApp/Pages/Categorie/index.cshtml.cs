using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetCoreApp.Pages.Categorie
{
    public class indexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public List<Core.Entities.Categorie> Categories { get; set; } = new List<Core.Entities.Categorie>();

        public indexModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task OnGet()
        {
            throw new Exception();
            var request = new HttpRequestMessage(HttpMethod.Get,
            "http://localhost:64781/api/categorie");
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
    }
}