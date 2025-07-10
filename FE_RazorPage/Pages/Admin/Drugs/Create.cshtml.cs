using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE_RazorPage.Pages.Admin.Drugs
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApi");
        }

        [BindProperty]
        public CreateArvDrugModel Drug { get; set; } = new();

        public List<GetArvDrugGroupModel> Groups { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
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

            var response = await _httpClient.PostAsJsonAsync("api/ArvDrugs", Drug);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Thêm thuốc thành công!";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Thêm thuốc thất bại!");
            Groups = await _httpClient.GetFromJsonAsync<List<GetArvDrugGroupModel>>("api/ArvDrugs/groups") ?? new();
            return Page();
        }
    }
}
