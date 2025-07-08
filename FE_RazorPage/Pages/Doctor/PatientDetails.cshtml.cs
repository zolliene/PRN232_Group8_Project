using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Dto.response;
using WebAPI.Models;

namespace FE_RazorPage.Pages.Doctor;

public class PatientDetails : PageModel
{

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<PatientDetails> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public PatientDetails(IHttpClientFactory httpClientFactory, ILogger<PatientDetails> logger, JsonSerializerOptions jsonOptions)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _jsonOptions = jsonOptions;
    }

    public GetPatientDetail PatientDetail { get; set; }
    
    public async Task<IActionResult> OnGetAsync(int appointmentId)
    {
        try
        {
            var http = _httpClientFactory.CreateClient("BackendApi");
            var token = HttpContext.Session.GetString("jwtToken");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var baseUrl = $"Appointment/{appointmentId}";
            var response = await http.GetAsync(baseUrl);
            var json = await response.Content.ReadAsStringAsync();
            var apiResult = JsonSerializer.Deserialize<ApiResponse<GetPatientDetail>>(json, _jsonOptions);
            if (response.StatusCode != HttpStatusCode.OK || response.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException($"Unable to get patient details for appointment {appointmentId} cause by {apiResult.Message}");
            }

            PatientDetail = apiResult.Data;
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get patient details async cause by {}", e.Message);
        }

        return Page();
    }
}