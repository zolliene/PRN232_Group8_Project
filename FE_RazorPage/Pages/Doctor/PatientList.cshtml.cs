using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Dto.response;
using WebAPI.Models;

namespace FE_RazorPage.Pages.Doctor;

public class PatientList : PageModel
{
   private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<PatientList> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public PatientList(IHttpClientFactory httpClientFactory, ILogger<PatientList> logger, JsonSerializerOptions jsonOptions)
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
            
            var response = await http.GetAsync("Appointment");
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
    
}