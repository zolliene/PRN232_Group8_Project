using System.Text;
using System.Text.Json;
using FE_RazorPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE_RazorPage.Pages.Auth;

public class RegisterModel : PageModel
{
    [BindProperty]
    public RegisterPatientModel Input { get; set; } = new RegisterPatientModel();

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var json = JsonSerializer.Serialize(Input);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var http = new HttpClient();
        http.BaseAddress = new Uri("https://localhost:7022");

        try
        {
            var response = await http.PostAsync("/api/Auth/register", content);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = "??ng ký thành công!";
                return Page();
            }
            else
            {
                using var doc = JsonDocument.Parse(body);
                var msg = doc.RootElement.GetProperty("message").GetString();
                ErrorMessage = msg ?? "??ng ký th?t b?i!";
                return Page();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "L?i k?t n?i ??n server: " + ex.Message;
            return Page();
        }
    }
}
