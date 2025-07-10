using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE_RazorPage.Pages.Admin.TreatmentRegimens
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApi");
        }

        [BindProperty]
        public UpdateTreatmentRegimenModel Regimen { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<GetTreatmentRegimenModel>($"api/TreatmentRegimen/{id}");
            if (result == null) return NotFound();

            Regimen = new UpdateTreatmentRegimenModel
            {
                Id = result.Id,
                RegimenName = result.RegimenName,
                Drugs = result.Drugs,
                Description = result.Description
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var response = await _httpClient.PutAsJsonAsync($"api/TreatmentRegimen/{Regimen.Id}", Regimen);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "✅ Cập nhật phác đồ thành công!";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "❌ Cập nhật thất bại!");
            return Page();
        }
    }


}
