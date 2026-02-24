using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace MVMedia.Web.Pages.Account;

public class LogoutModel : PageModel
{
    public IActionResult OnGet()
    {
        Response.Cookies.Delete("AuthToken");
        return RedirectToPage("/Account/Login");
    }
}
