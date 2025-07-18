﻿using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FE_RazorPage.Pages.Admin.Doctors
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("MyApi");
        }

        [BindProperty]
        public GetDoctorModel Doctor { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/DoctorAccount/{id}");


                if (response == null) return NotFound();
                

                var json = await response.Content.ReadAsStringAsync();


                Doctor = JsonSerializer.Deserialize<GetDoctorModel>(json)!;


                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPTION] {ex}");
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.DeleteAsync($"api/DoctorAccount/{Doctor.Id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Xóa doctor thành công!";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Xóa thất bại.");
            return Page();
        }
    }
}