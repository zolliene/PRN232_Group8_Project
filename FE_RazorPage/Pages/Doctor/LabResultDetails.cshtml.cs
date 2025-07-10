using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Dto.response;
using WebAPI.Models;

namespace FE_RazorPage.Pages.Doctor;

public class LabResultDetails : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<LabResultDetails> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public LabResultDetails(IHttpClientFactory httpClientFactory, ILogger<LabResultDetails> logger, JsonSerializerOptions jsonOptions)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _jsonOptions = jsonOptions;
    }

    public GetLabResultRes? LabResult { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var http = _httpClientFactory.CreateClient("BackendApi");
            var token = HttpContext.Session.GetString("jwtToken");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await http.GetAsync($"Lab/results/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var apiResult = JsonSerializer.Deserialize<ApiResponse<GetLabResultRes>>(json, _jsonOptions);

            if (response.StatusCode != HttpStatusCode.OK || apiResult?.Status != (int)HttpStatusCode.OK)
            {
                TempData["ErrorMessage"] = apiResult?.Message ?? "Không thể tải thông tin kết quả xét nghiệm";
                _logger.LogError("Error getting lab result details for id {}: {}", id, apiResult?.Message);
            }
            else
            {
                LabResult = apiResult.Data;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get lab result details async for id {} cause by {}", id, e.Message);
            TempData["ErrorMessage"] = "Không thể kết nối đến server";
        }

        return Page();
    }
}