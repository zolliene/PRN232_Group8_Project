using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE_RazorPage.Pages.Admin.TreatmentRegimens
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApi");
        }

        [BindProperty]
        public CreateTreatmentRegimenModel Regimen { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var response = await _httpClient.PostAsJsonAsync("api/adminTreatmentRegimen", Regimen);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "✅ Thêm phác đồ thành công!";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "❌ Thêm phác đồ thất bại!");
            return Page();
        }
    }


}
