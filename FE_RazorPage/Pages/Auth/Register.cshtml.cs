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
        if (!Input.Email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
        {
            ErrorMessage = "Email phải có kết thúc bằng @gmail.com";
            return Page(); // Trả về trang hiện tại và hiển thị lỗi
        }
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
                SuccessMessage = "Đăng ký thành công!";
                return Page();
            }
            else
            {
                using var doc = JsonDocument.Parse(body);
                var msg = doc.RootElement.GetProperty("message").GetString();
                ErrorMessage = msg ?? "Đăng ký thất bại";
                return Page();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Lỗi kết nối server " + ex.Message;
            return Page();
        }
    }
}
