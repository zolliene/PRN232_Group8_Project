using FE_RazorPage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Text;
using System.Text.Json;

namespace FE_RazorPage.Pages.Login
{
    public class LoginModelPage : PageModel
    {
        [BindProperty]
        public LoginModel Input { get; set; } = new LoginModel();

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var loginRequest = new
            {
                email = Input.Email,
                password = Input.Password
            };

            var json = JsonSerializer.Serialize(loginRequest);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using var http = new HttpClient();
            http.BaseAddress = new Uri("https://localhost:7022");

            try
            {
                var response = await http.PostAsync("/api/Auth/login", data);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    // Giả sử API trả về: { "data": { "id":..., "token":... }, "status": ..., "message": ... }
                    using var doc = JsonDocument.Parse(body);
                    var root = doc.RootElement;

                    var dataElement = root.GetProperty("Data");
                    var token = dataElement.GetProperty("token").GetString();
                    var id = dataElement.GetProperty("id").GetInt32();
                    var role = dataElement.GetProperty("role").GetString();
                    Console.WriteLine("ROLE TRẢ VỀ: " + role);
                    // (Tuỳ ý) Lưu token vào Session/Cookie
                    HttpContext.Session.SetString("jwtToken", token ?? "");
                    HttpContext.Session.SetInt32("userId", id);
                    HttpContext.Session.SetString("role", role ?? "guest");
                    

                    // Chuyển về trang Home hoặc Dashboard
                    return RedirectToPage("/Index");
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    string? message = null;
                    try
                    {
                        using var doc = JsonDocument.Parse(body);
                        message = doc.RootElement.GetProperty("message").GetString();
                    }
                    catch(Exception e) {
                        Console.WriteLine("Lỗi khi parse JSON từ login API: " + e.Message);
                        message = "Email hoặc mật khẩu không đúng.";
                    }
                    ErrorMessage = message ?? "Đăng nhập thất bại!";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Không thể kết nối API: " + ex.Message;
                return Page();
            }
        }
    }
}
