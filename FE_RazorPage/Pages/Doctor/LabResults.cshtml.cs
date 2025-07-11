using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Dto.response;
using WebAPI.Models;

namespace FE_RazorPage.Pages.Doctor;

public class LabResults : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<LabResults> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public LabResults(IHttpClientFactory httpClientFactory, ILogger<LabResults> logger, JsonSerializerOptions jsonOptions)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _jsonOptions = jsonOptions;
    }

    [BindProperty]
    public List<GetAppointmentWithLabRes> AppointmentsWithLabResults { get; set; } = new();
    
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var http = _httpClientFactory.CreateClient("BackendApi");
            var token = HttpContext.Session.GetString("jwtToken");
            
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }
            
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            _logger.LogInformation("Calling API: Lab/appointments-with-results");
            var response = await http.GetAsync("Lab/appointments-with-results");
            var json = await response.Content.ReadAsStringAsync();
            
            _logger.LogInformation("API Response Status: {StatusCode}", response.StatusCode);
            _logger.LogInformation("API Response Body: {Json}", json);

            var apiResult = JsonSerializer.Deserialize<ApiResponse<List<GetAppointmentWithLabRes>>>(json, _jsonOptions);
            
            if (response.StatusCode == HttpStatusCode.OK && apiResult?.Status == (int)HttpStatusCode.OK)
            {
                AppointmentsWithLabResults = apiResult.Data ?? new List<GetAppointmentWithLabRes>();
                _logger.LogInformation("Successfully loaded {Count} appointments with lab results", AppointmentsWithLabResults.Count);
            }
            else
            {
                ErrorMessage = apiResult?.Message ?? "Error occurred while fetching appointments with lab results";
                _logger.LogError("Error getting appointments with lab results: {ErrorMessage}", ErrorMessage);
                AppointmentsWithLabResults = new List<GetAppointmentWithLabRes>();
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Exception at get appointments with lab results async: {Message}", e.Message);
            ErrorMessage = "Không thể kết nối đến server: " + e.Message;
            AppointmentsWithLabResults = new List<GetAppointmentWithLabRes>();
        }
        
        return Page();
    }
}