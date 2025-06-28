using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE_RazorPage.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Xo� t?t c? session li�n quan ??n user
            HttpContext.Session.Clear();

            // Chuy?n v? trang login
            return RedirectToPage("/Auth/Login");
        }
    }
}
