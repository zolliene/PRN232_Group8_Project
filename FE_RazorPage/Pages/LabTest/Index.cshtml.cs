using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Dto.response;
using System.Text.Json;

namespace FE_RazorPage.Pages.LabTest
{
    public class LabTestDetailModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LabTestDetailModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<GetLabTestResqV1> LabTests { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FilterDate { get; set; }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            string url;

            if (FilterDate.HasValue)
            {
                // Nếu có ngày được chọn → gọi API lọc theo ngày
                string formattedDate = FilterDate.Value.ToString("yyyy-MM-dd");
                url = $"https://localhost:7022/api/LabTestV1/getAllInADay?date={formattedDate}";
            }
            else
            {
                // Không có ngày → gọi API lấy tất cả
                url = "https://localhost:7022/api/LabTestV1/All";
            }

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var wrapper = JsonSerializer.Deserialize<ApiResponseWrapper<List<GetLabTestResqV1>>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                LabTests = wrapper?.Data ?? new List<GetLabTestResqV1>();
            }
            else
            {
                LabTests = new();
            }
        }

        public class ApiResponseWrapper<T>
        {
            public T Data { get; set; }
            public int Status { get; set; }
            public string Message { get; set; }
        }
    }
}
