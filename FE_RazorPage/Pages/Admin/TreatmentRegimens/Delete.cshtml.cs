using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE_RazorPage.Pages.Admin.TreatmentRegimens
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApi");
        }

        [BindProperty]
        public GetTreatmentRegimenModel Regimen { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<GetTreatmentRegimenModel>($"api/TreatmentRegimen/{id}");
            if (result == null) return NotFound();

            Regimen = result;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.DeleteAsync($"api/TreatmentRegimen/{Regimen.Id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "🗑️ Đã xóa phác đồ thành công!";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "❌ Xóa thất bại!");
            return Page();
        }
    }



}
