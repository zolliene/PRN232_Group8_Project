using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FE_RazorPage.Pages.Admin.TreatmentRegimens
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApi");
        }

        public List<GetTreatmentRegimenModel> Regimens { get; set; } = new();

        public async Task OnGetAsync()
        {
            Regimens = await _httpClient.GetFromJsonAsync<List<GetTreatmentRegimenModel>>("api/admintreatmentregimen") ?? new();
        }
    }

}