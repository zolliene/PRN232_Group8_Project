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

public class LabExamination : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<LabExamination> _logger;
    private readonly JsonSerializerOptions _jsonOptions;


    public LabExamination(IHttpClientFactory httpClientFactory, ILogger<LabExamination> logger,
        JsonSerializerOptions jsonOptions)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _jsonOptions = jsonOptions;
    }


    [BindProperty] public int PatientId { get; set; }

    [BindProperty] public int AppointmentId { get; set; }

    [BindProperty] public int SelectedTestTypeId { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Vui lòng nhập kết quả khám lâm sàng")]
    public string ClinicalData { get; set; }

    public GetPatientDetail PatientDetail { get; set; }
    public IList<GetTestTypeRes> TestTypes { get; set; }

    public async Task<IActionResult> OnGetAsync(int appointmentId)
    {
        try
        {
            var http = _httpClientFactory.CreateClient("BackendApi");
            var token = HttpContext.Session.GetString("jwtToken");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var responseAppointment = await http.GetAsync($"Appointment/{appointmentId}");
            var responseTestType = await http.GetAsync($"TestType");

            var jsonAppointment = await responseAppointment.Content.ReadAsStringAsync();
            var apiResultAppointment =
                JsonSerializer.Deserialize<ApiResponse<GetPatientDetail>>(jsonAppointment, _jsonOptions);

            var jsonTestType = await responseTestType.Content.ReadAsStringAsync();
            var apiResultTestType =
                JsonSerializer.Deserialize<ApiResponse<IList<GetTestTypeRes>>>(jsonTestType, _jsonOptions);

            if (responseAppointment.StatusCode != HttpStatusCode.OK ||
                apiResultAppointment.Status != (int)HttpStatusCode.OK)
            {
                throw new ApplicationException(
                    $"Unable to get patient details for appointment {appointmentId} cause by {apiResultAppointment?.Message}");
            }

            if (responseTestType.StatusCode != HttpStatusCode.OK ||
                apiResultAppointment.Status != (int)HttpStatusCode.OK)
            {
                throw new ApplicationException($"Unable to get list test type cause by {apiResultTestType?.Message}");
            }

            PatientDetail = apiResultAppointment.Data;
            TestTypes = apiResultTestType.Data;
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get patient details async cause by {}", e.Message);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostCreateLabTestAsync()
    {
        try
        {
            var http = _httpClientFactory.CreateClient("BackendApi");
            var token = HttpContext.Session.GetString("jwtToken");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var createExaminationReq = new CreateExaminationReq
            {
                AppointmentId = AppointmentId,
                ClinicalData = ClinicalData
            };

            var createLabTestReq = new CreateLabTestReq
            {
                AppointmentId = AppointmentId,
                TestTypeId = SelectedTestTypeId,
                OrderTime = DateTime.Now
            };
            
            var responseCreateExamination = await http.PostAsJsonAsync("Examination", createExaminationReq);
            var jsonCreateExamination = await responseCreateExamination.Content.ReadAsStringAsync();
            
            var responseCreateLab = await http.PostAsJsonAsync("Lab", createLabTestReq);
            var jsonCreateLab = await responseCreateLab.Content.ReadAsStringAsync();
            
            
            var apiCreateExamination =
                JsonSerializer.Deserialize<ApiResponse<string>>(jsonCreateExamination, _jsonOptions);
            if (responseCreateExamination.StatusCode != HttpStatusCode.OK ||
                apiCreateExamination?.Status != (int)HttpStatusCode.Created)
            {
                if (apiCreateExamination?.Status == (int)HttpStatusCode.BadRequest)
                {
                    TempData["ErrorMessage"] = "Đã tồn tại examination với appointment";
                    throw new ApplicationException(
                        $"Unable to create examination cause by {apiCreateExamination?.Message}");
                }
                
                TempData["ErrorMessage"] = "Không thể tạo examination";
                throw new ApplicationException(
                    $"Unable to create examination cause by {apiCreateExamination?.Message}");
            }
            
            var apiCreateLab =
                JsonSerializer.Deserialize<ApiResponse<string>>(jsonCreateLab, _jsonOptions);
            if (responseCreateExamination.StatusCode != HttpStatusCode.OK ||
                apiCreateLab?.Status != (int)HttpStatusCode.Created)
            {
                TempData["ErrorMessage"] = "Không thể tạo examination";
                throw new ApplicationException(
                    $"Unable to create examination cause by {apiCreateExamination?.Message}");
            }

            TempData["successMessage"] = "Tạo thành công examination";
        }
        catch (Exception e)
        {
            _logger.LogError("Error at create lab test cause by {}", e.Message);
        }

        return RedirectToPage( new { appointmentId = AppointmentId });
    }
}