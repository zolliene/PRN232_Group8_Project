using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE_RazorPage.Pages.Admin.Drugs
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApi");
        }

        [BindProperty]
        public GetArvDrugModel Drug { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var drug = await _httpClient.GetFromJsonAsync<GetArvDrugModel>($"api/ArvDrugs/{id}");
            if (drug == null) return NotFound();

            Drug = drug;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.DeleteAsync($"api/ArvDrugs/{Drug.Id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Xóa thuốc thành công!";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Xóa thất bại.");
            return Page();
        }
    }
}
