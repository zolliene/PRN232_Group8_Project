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
    public List<GetLabResultRes> LabResult { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var http = _httpClientFactory.CreateClient("BackendApi");
            var token = HttpContext.Session.GetString("jwtToken");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await http.GetAsync("Lab/results");
            var json = await response.Content.ReadAsStringAsync();
            var apiResult = JsonSerializer.Deserialize<ApiResponse<List<GetLabResultRes>>>(json, _jsonOptions);
            
            if (response.StatusCode != HttpStatusCode.OK || apiResult?.Status != (int)HttpStatusCode.OK)
            {
                ErrorMessage = apiResult?.Message ?? "Error occurred while fetching lab results";
                _logger.LogError("Error getting lab results: {}", ErrorMessage);
            }
            else
            {
                LabResult = apiResult.Data ?? new List<GetLabResultRes>();
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get lab results async cause by {}", e.Message);
            ErrorMessage = "Không thể kết nối đến server";
        }
        
        return Page();
    }
}