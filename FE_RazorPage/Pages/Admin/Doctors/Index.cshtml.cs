using FE_RazorPage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Net.Http.Json;

namespace FE_RazorPage.Pages.Admin.Doctors
{

    public class DoctorIndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DoctorIndexModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("MyApi");
        }

        public List<GetDoctorModel> Doctors { get; set; } = new();

        public async Task OnGetAsync()
        {
            var response = await _httpClient.GetAsync("api/doctoraccount");
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
