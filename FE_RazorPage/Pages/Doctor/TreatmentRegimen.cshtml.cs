using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Dto.request;
using Services.Dto.response;
using WebAPI.Models;

namespace FE_RazorPage.Pages.Doctor;

public class TreatmentRegimen : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<TreatmentRegimen> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public TreatmentRegimen(IHttpClientFactory httpClientFactory, ILogger<TreatmentRegimen> logger, JsonSerializerOptions jsonOptions)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _jsonOptions = jsonOptions;
    }

    [BindProperty]
    public int AppointmentId { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Vui lòng chọn một phác đồ điều trị")]
    public int SelectedRegimenId { get; set; }

    [BindProperty]
    public string? CustomNotes { get; set; }

    public GetPatientDetail? PatientDetail { get; set; }
    public List<GetTreatmentRegimenRes> TreatmentRegimens { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int appointmentId)
    {
        AppointmentId = appointmentId;
        
        try
        {
            var http = _httpClientFactory.CreateClient("BackendApi");
            var token = HttpContext.Session.GetString("jwtToken");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get patient details
            var patientResponse = await http.GetAsync($"TreatmentRegimen/patient/{appointmentId}");
            var patientJson = await patientResponse.Content.ReadAsStringAsync();
            var patientApiResult = JsonSerializer.Deserialize<ApiResponse<GetPatientDetail>>(patientJson, _jsonOptions);

            if (patientResponse.StatusCode == HttpStatusCode.OK && patientApiResult?.Status == (int)HttpStatusCode.OK)
            {
                PatientDetail = patientApiResult.Data;
            }
            else
            {
                TempData["ErrorMessage"] = patientApiResult?.Message ?? "Không thể tải thông tin bệnh nhân";
                _logger.LogError("Error getting patient details: {}", patientApiResult?.Message);
            }

            // Get treatment regimens
            var regimensResponse = await http.GetAsync("TreatmentRegimen");
            var regimensJson = await regimensResponse.Content.ReadAsStringAsync();
            var regimensApiResult = JsonSerializer.Deserialize<ApiResponse<List<GetTreatmentRegimenRes>>>(regimensJson, _jsonOptions);

            if (regimensResponse.StatusCode == HttpStatusCode.OK && regimensApiResult?.Status == (int)HttpStatusCode.OK)
            {
                TreatmentRegimens = regimensApiResult.Data ?? new List<GetTreatmentRegimenRes>();
            }
            else
            {
                TempData["ErrorMessage"] = regimensApiResult?.Message ?? "Không thể tải danh sách phác đồ điều trị";
                _logger.LogError("Error getting treatment regimens: {}", regimensApiResult?.Message);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get treatment regimen page async cause by {}", e.Message);
            TempData["ErrorMessage"] = "Không thể kết nối đến server";
        }

        return Page();
    }

    public async Task<IActionResult> OnPostCreatePatientRegimenAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(AppointmentId); // Reload data
                return Page();
            }

            var http = _httpClientFactory.CreateClient("BackendApi");
            var token = HttpContext.Session.GetString("jwtToken");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new CreatePatientRegimenReq
            {
                AppointmentId = AppointmentId,
                RegimenMasterId = SelectedRegimenId,
                CustomNotes = CustomNotes
            };

            var response = await http.PostAsJsonAsync("TreatmentRegimen/patient-regimen", request);
            var json = await response.Content.ReadAsStringAsync();
            var apiResult = JsonSerializer.Deserialize<ApiResponse<string>>(json, _jsonOptions);

            if (response.StatusCode == HttpStatusCode.OK && apiResult?.Status == (int)HttpStatusCode.Created)
            {
                TempData["SuccessMessage"] = "Đã chọn phác đồ điều trị thành công! Cuộc hẹn đã được hoàn thành.";
                return RedirectToPage("/Doctor/LabResults");
            }
            else
            {
                TempData["ErrorMessage"] = apiResult?.Message ?? "Không thể tạo phác đồ điều trị";
                _logger.LogError("Error creating patient regimen: {}", apiResult?.Message);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error at create patient regimen async cause by {}", e.Message);
            TempData["ErrorMessage"] = "Không thể kết nối đến server";
        }

        await OnGetAsync(AppointmentId); // Reload data
        return Page();
    }
}