using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FE_RazorPage.Pages.Admin.Doctors
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("MyApi");
        }

        public List<GetDoctorModel> Doctors { get; set; } = new();

        public async Task OnGetAsync()
        {
            var response = await _httpClient.GetAsync("api/DoctorAccount");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Doctors = JsonSerializer.Deserialize<List<GetDoctorModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
            }
        }
    }
}
