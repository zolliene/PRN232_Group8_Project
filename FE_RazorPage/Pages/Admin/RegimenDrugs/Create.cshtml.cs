using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE_RazorPage.Pages.Admin.RegimenDrugs
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApi");
        }

        [BindProperty(SupportsGet = true)]
        public int RegimenId { get; set; }

        [BindProperty]
        public CreateRegimenDrugModel NewDrug { get; set; } = new();

        public List<GetArvDrugModel> AvailableDrugs { get; set; } = new();

        public async Task OnGetAsync()
        {
            AvailableDrugs = await _httpClient.GetFromJsonAsync<List<GetArvDrugModel>>("api/ArvDrugs") ?? new();
            NewDrug.RegimenId = RegimenId;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                AvailableDrugs = await _httpClient.GetFromJsonAsync<List<GetArvDrugModel>>("api/ArvDrugs") ?? new();
                return Page();
            }

            var response = await _httpClient.PostAsJsonAsync($"api/adminTreatmentRegimen/{RegimenId}/drugs", NewDrug);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "✅ Thêm thuốc vào phác đồ thành công!";
                return RedirectToPage("Index", new { RegimenId = RegimenId });
            }

            ModelState.AddModelError(string.Empty, "❌ Thêm thuốc thất bại!");
            return Page();
        }
    }

}
