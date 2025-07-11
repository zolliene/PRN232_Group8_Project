﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Dto.response;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class CreateAppointmentModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    [BindProperty]
    public bool Success { get; set; } = false;

    public CreateAppointmentModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public CreateAppointmentRequest Appointment { get; set; } = new();

    public List<SelectListItem> DoctorOptions { get; set; } = new();
    public List<GetDoctorRes> DoctorDisplay { get; set; } = new();


    public async Task<IActionResult> OnGetAsync()
    {
        var token = HttpContext.Session.GetString("jwtToken");
        if (string.IsNullOrEmpty(token))
        {
            TempData["Warning"] = "Bạn phải đăng nhập để đặt lịch khám.";
            
        }
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("https://localhost:7022/api/Doctorlist");
       
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();

            var result = JsonDocument.Parse(json);
            var doctorList = result.RootElement.GetProperty("Data").EnumerateArray();

          
            DoctorOptions = doctorList.Select(d => new SelectListItem
            {
                Value = d.GetProperty("Id").ToString(),
                Text = d.GetProperty("FullName").ToString()
            }).ToList();

            DoctorDisplay = doctorList.Select(d => new GetDoctorRes
            {
                Id = int.Parse(d.GetProperty("Id").ToString()),
                FullName = d.GetProperty("FullName").ToString(),
                Qualification = d.GetProperty("Qualification").ToString()
            }).ToList();
        }
        return Page();

    }

  
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var client = _httpClientFactory.CreateClient();

        var token = HttpContext.Session.GetString("jwtToken");
        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }else
        {
            return RedirectToPage("/Auth/Login");
        }

            var content = new StringContent(JsonSerializer.Serialize(Appointment), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:7022/api/Booking", content);

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Đặt lịch thành công!";
            Success = true;
            return Page();
        }

        TempData["Error"] = "Lỗi khi đặt lịch.";
        return Page();
    }


    public class CreateAppointmentRequest
    {
       
        public int DoctorId { get; set; }
        public DateOnly Date { get; set; }
        public string Session { get; set; } = "morning";
        public string? Note { get; set; }
    }
}
