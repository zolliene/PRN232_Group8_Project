using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Dto.response;
using WebAPI.Models;

namespace FE_RazorPage.Pages.Doctor;

public class PatientLabResults : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<PatientLabResults> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public PatientLabResults(IHttpClientFactory httpClientFactory, ILogger<PatientLabResults> logger, JsonSerializerOptions jsonOptions)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _jsonOptions = jsonOptions;
    }

    public GetAppointmentLabResultsRes? AppointmentLabResults { get; set; }

    public async Task<IActionResult> OnGetAsync(int appointmentId)
    {
        try
        {
            var http = _httpClientFactory.CreateClient("BackendApi");
            var token = HttpContext.Session.GetString("jwtToken");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await http.GetAsync($"Lab/appointment/{appointmentId}");
            var json = await response.Content.ReadAsStringAsync();
            var apiResult = JsonSerializer.Deserialize<ApiResponse<GetAppointmentLabResultsRes>>(json, _jsonOptions);

            if (response.StatusCode == HttpStatusCode.OK && apiResult?.Status == (int)HttpStatusCode.OK)
            {
                AppointmentLabResults = apiResult.Data;
            }
            else
            {
                TempData["ErrorMessage"] = apiResult?.Message ?? "Không thể tải thông tin kết quả xét nghiệm";
                _logger.LogError("Error getting appointment lab results for appointment {}: {}", appointmentId, apiResult?.Message);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get appointment lab results async for appointment {} cause by {}", appointmentId, e.Message);
            TempData["ErrorMessage"] = "Không thể kết nối đến server";
        }

        return Page();
    }
}