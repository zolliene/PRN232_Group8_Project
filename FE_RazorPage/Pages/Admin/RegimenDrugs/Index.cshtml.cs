using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE_RazorPage.Pages.Admin.RegimenDrugs
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApi");
        }

        public List<GetRegimenDrugModel> RegimenDrugs { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int RegimenId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<GetRegimenDrugModel>>($"api/adminTreatmentRegimen/{RegimenId}/drugs");
            RegimenDrugs = result ?? new();
            return Page();
        }
    }

}
