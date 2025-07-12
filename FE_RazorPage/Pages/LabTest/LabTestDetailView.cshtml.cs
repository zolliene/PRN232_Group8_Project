using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Dto.response;
using System.Text.Json;
using System.Text;
using Services.Dto.request;

namespace FE_RazorPage.Pages.LabTest
{
    public class LabTestDetailViewModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LabTestDetailViewModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<GetLabTestResqV1> LabTests { get; set; } = new();
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Session { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var res = await client.GetAsync($"https://localhost:7022/api/LabTestV1/getById?id={id}");
            if (!res.IsSuccessStatusCode) return NotFound();

            var json = await res.Content.ReadAsStringAsync();
            var wrapper = JsonSerializer.Deserialize<ApiResponseWrapper<GetLabTestResqV1>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var selectedLab = wrapper?.Data;
            if (selectedLab == null) return NotFound();

            var listRes = await client.GetAsync($"https://localhost:7022/api/LabTestV1/getByAppointmentId?appointmentId={selectedLab.AppointmentId}");
            var listJson = await listRes.Content.ReadAsStringAsync();
            var listWrapper = JsonSerializer.Deserialize<ApiResponseWrapper<List<GetLabTestResqV1>>>(listJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            LabTests = listWrapper?.Data ?? new List<GetLabTestResqV1>();

            FullName = selectedLab.FullName;
            Phone = selectedLab.Phone;
            DateOfBirth = selectedLab.DateOfBirth.ToDateTime(TimeOnly.MinValue);
            Session = selectedLab.session;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient();

            if (int.TryParse(Request.Form["updateId"], out int labTestId))
            {
                var dto = new UpdateLabTestDto
                {
                    ResultValue = Request.Form[$"ResultValue_{labTestId}"],
                    ResultStatus = Request.Form[$"ResultStatus_{labTestId}"],
                    Comments = Request.Form[$"Comments_{labTestId}"]
                };

                var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"https://localhost:7022/api/LabTestV1/update?id={labTestId}", content);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Cập nhật không thành công.");
                }

                return RedirectToPage(new { id = labTestId });
            }

            return Page(); // fallback
        }


        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!int.TryParse(Request.Form["AppointmentId"], out int appointmentId))
            {
                return BadRequest();
            }

            if (!int.TryParse(Request.Form["deleteId"], out int deleteId))
            {
                return BadRequest();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7022/api/LabTestV1/delete?id={deleteId}");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Xóa không thành công.");
                return await OnGetAsync(deleteId);
            }

            // Load lại danh sách lab test sau khi xóa
            var listRes = await client.GetAsync($"https://localhost:7022/api/LabTestV1/getByAppointmentId?appointmentId={appointmentId}");
            if (!listRes.IsSuccessStatusCode)
            {
                return RedirectToPage("/LabTest/LabTestList");
            }

            var listJson = await listRes.Content.ReadAsStringAsync();
            var listWrapper = JsonSerializer.Deserialize<ApiResponseWrapper<List<GetLabTestResqV1>>>(listJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var labTests = listWrapper?.Data;
            if (labTests == null || labTests.Count == 0)
            {
                return RedirectToPage("/LabTest/LabTestList");
            }

            return RedirectToPage(new { id = labTests.First().Id });
        }

        public class ApiResponseWrapper<T>
        {
            public T Data { get; set; }
            public int Status { get; set; }
            public string Message { get; set; }
        }
    }
}
