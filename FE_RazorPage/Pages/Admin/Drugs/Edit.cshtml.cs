using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE_RazorPage.Pages.Admin.Drugs
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApi");
        }

        [BindProperty]
        public UpdateArvDrugModel Drug { get; set; } = new();

        public List<GetArvDrugGroupModel> Groups { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Lấy thông tin thuốc
            var result = await _httpClient.GetFromJsonAsync<GetArvDrugModel>($"api/ArvDrugs/{id}");
            if (result == null)
                return NotFound();

            Drug = new UpdateArvDrugModel
            {
                Id = result.Id,
                Name = result.Name,
                GroupId = result.GroupId,
                ActiveIngredient = result.ActiveIngredient,
                Description = result.Description
            };

            // Lấy nhóm thuốc
            Groups = await _httpClient.GetFromJsonAsync<List<GetArvDrugGroupModel>>("api/ArvDrugs/groups") ?? new();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Groups = await _httpClient.GetFromJsonAsync<List<GetArvDrugGroupModel>>("api/ArvDrugs/groups") ?? new();
                return Page();
            }

            var response = await _httpClient.PutAsJsonAsync($"api/ArvDrugs/{Drug.Id}", Drug);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "✅ Cập nhật thuốc thành công!";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "❌ Cập nhật thất bại!");
            Groups = await _httpClient.GetFromJsonAsync<List<GetArvDrugGroupModel>>("api/ArvDrugs/groups") ?? new();
            return Page();
        }
    }




}
