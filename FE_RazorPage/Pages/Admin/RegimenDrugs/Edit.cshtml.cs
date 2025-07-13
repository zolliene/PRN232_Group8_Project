using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE_RazorPage.Pages.Admin.RegimenDrugs
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApi");
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int RegimenId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Dose { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string Frequency { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string DrugName { get; set; } = string.Empty;

        [BindProperty]
        public UpdateRegimenDrugModel Drug { get; set; } = new();

        public void OnGet()
        {
            // Gán dữ liệu sang model để binding
            Drug.Id = Id;
            Drug.Dose = Dose;
            Drug.Frequency = Frequency;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.PutAsJsonAsync($"api/adminTreatmentRegimen/drugs/{Drug.Id}", Drug);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "✅ Cập nhật thuốc thành công!";
                return RedirectToPage("Index", new { RegimenId });
            }

            ModelState.AddModelError(string.Empty, "❌ Cập nhật thất bại!");
            return Page();
        }
    }


}
