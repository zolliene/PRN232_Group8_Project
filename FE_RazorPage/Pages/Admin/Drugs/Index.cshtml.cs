using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FE_RazorPage.Pages.Admin.Drugs
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApi");
        }

        public List<GetArvDrugModel> Drugs { get; set; } = new();
        public List<GetArvDrugGroupModel> Groups { get; set; } = new();

        public async Task OnGetAsync()
        {
            Drugs = await _httpClient.GetFromJsonAsync<List<GetArvDrugModel>>("api/arvdrugs") ?? new();
            Groups = await _httpClient.GetFromJsonAsync<List<GetArvDrugGroupModel>>("api/arvdrugs/groups") ?? new();
        }

        public string GetGroupName(int? groupId)
        {
            return Groups.FirstOrDefault(g => g.Id == groupId)?.Name ?? "";
        }
    }
}