using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE_RazorPage.Pages.Admin.RegimenDrugs
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApi");
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int RegimenId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DrugName { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string Dose { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string Frequency { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.DeleteAsync($"api/adminTreatmentRegimen/drugs/{Id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "🗑️ Đã xoá thuốc khỏi phác đồ!";
                return RedirectToPage("Index", new { RegimenId });
            }

            ModelState.AddModelError(string.Empty, "❌ Xoá thất bại!");
            return Page();
        }
    }


}
