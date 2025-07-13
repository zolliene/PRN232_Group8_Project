using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Services.Dto.response;
using WebAPI.Models;

namespace FE_RazorPage.Pages.Receptionist;

public class CheckInList : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CheckInList> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public CheckInList(IHttpClientFactory httpClientFactory, ILogger<CheckInList> logger, JsonSerializerOptions jsonOptions)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _jsonOptions = jsonOptions;
    }

    
    [BindProperty]
    public List<GetAppointmentRes> Appointments { get; set; } = new ();
    public string? ErrorMessage { get; set; }
    
    public async Task<IActionResult> OnGetAsync(DateOnly? date)
    {
        try
        {
            var http = _httpClientFactory.CreateClient("BackendApi");
            var token = HttpContext.Session.GetString("jwtToken");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var baseUrl = "Appointment";
            var queryParams = new Dictionary<string, string>();

            if (date.HasValue)
            {
                queryParams["date"] = date.Value.ToString("yyyy-MM-dd");
            }

            var finalUrl = QueryHelpers.AddQueryString(baseUrl, queryParams);
            
            var response = await http.GetAsync(finalUrl);
            var json = await response.Content.ReadAsStringAsync();
            var apiResult = JsonSerializer.Deserialize<ApiResponse<List<GetAppointmentRes>>>(json, _jsonOptions);
            if (response.StatusCode != HttpStatusCode.OK || response.StatusCode != HttpStatusCode.OK)
            {
                ErrorMessage = apiResult.Message;
            }
            Appointments = apiResult.Data;
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get get list appointment async cause by {}", e.Message);
        }
        return Page();
    }

    public async Task<IActionResult> OnPostCheckInAsync(int appointmentId)
    {
        try
        {
            var http = _httpClientFactory.CreateClient("BackendApi");
            var token = HttpContext.Session.GetString("jwtToken");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await http.PutAsync($"Appointment/{appointmentId}", null);
            var json = await response.Content.ReadAsStringAsync();
            
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                var apiResult = JsonSerializer.Deserialize<ApiResponse<string>>(json, _jsonOptions);
                ErrorMessage = apiResult.Message;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get get list appointment async cause by {}", e.Message);
        }
        return RedirectToPage();
    }
}