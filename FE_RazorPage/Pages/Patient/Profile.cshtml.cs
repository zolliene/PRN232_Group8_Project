
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class PatientProfileModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PatientProfileModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public PatientProfileViewModel Profile { get; set; } = new();

    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var token = HttpContext.Session.GetString("jwtToken");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Auth/Login");
        }

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("https://localhost:7022/api/Patient/profile");
        if (!response.IsSuccessStatusCode) return RedirectToPage("/Error");

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonDocument.Parse(json).RootElement.GetProperty("Data");

        Profile = JsonSerializer.Deserialize<PatientProfileViewModel>(data.GetRawText())!;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var token = HttpContext.Session.GetString("jwtToken");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Auth/Login");
        }

        var updateRequest = new
        {
            FirstName = Profile.FirstName,
            LastName = Profile.LastName,
            Dob = Profile.Dob,
            Gender = Profile.Gender,
            Phone = Profile.Phone,
            Address = Profile.Address,
            InsuranceNumber = Profile.InsuranceNumber,
            Email = Profile.Email,
            Username = Profile.Username
        };

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var jsonContent = new StringContent(JsonSerializer.Serialize(updateRequest), Encoding.UTF8, "application/json");
        var response = await client.PutAsync("https://localhost:7022/api/Patient/profile", jsonContent);

        if (response.IsSuccessStatusCode)
        {
            SuccessMessage = "Cập nhật thành công!";
            return await OnGetAsync(); // Load lại data mới
        }

        ModelState.AddModelError(string.Empty, "Lỗi khi cập nhật thông tin.");
        return Page();
    }

    public class PatientProfileViewModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly Dob { get; set; }
        public string Gender { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string InsuranceNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
    }
}
