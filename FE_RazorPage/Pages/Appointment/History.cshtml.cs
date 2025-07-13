using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Dto.response;
using System.Net.Http.Headers;
using System.Text.Json;

public class HistoryModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HistoryModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public List<GetPatientExaminationHistoryRes> Histories { get; set; } = new();
    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    public int TotalPages { get; set; } = 1;
    public int PageSize { get; set; } = 8; // Mỗi trang 5 dòng (có thể sửa tuỳ ý)

    public async Task OnGetAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var token = HttpContext.Session.GetString("jwtToken");
        var userId = HttpContext.Session.GetInt32("userId");

        if (string.IsNullOrEmpty(token) || userId == null)
        {
            Response.Redirect("/Login");
            return;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync($"https://localhost:7022/api/Booking/history/{userId}");
        if (!response.IsSuccessStatusCode) return;

        var json = await response.Content.ReadAsStringAsync();
        var root = JsonDocument.Parse(json).RootElement;
        var all = JsonSerializer.Deserialize<List<GetPatientExaminationHistoryRes>>(root.GetProperty("Data").GetRawText())
            ?? new List<GetPatientExaminationHistoryRes>();

        // ✅ Phân trang theo CurrentPage
        TotalPages = (int)Math.Ceiling(all.Count / (double)PageSize);
        Histories = all
            .OrderByDescending(h => h.Date)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }



}
