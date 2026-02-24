using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace MVMedia.Web.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IConfiguration _configuration;

    public LoginModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var apiBaseUrl = _configuration["ApiBaseUrl"];
        using var client = new HttpClient();
        client.BaseAddress = new Uri(apiBaseUrl);
        var loginData = new { Username = Input.Username, Password = Input.Password };
        var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("api/User/login", content);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
            return Page();
        }

        var result = await response.Content.ReadAsStringAsync();
        var tokenObj = JsonSerializer.Deserialize<UserToken>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (tokenObj?.Token == null)
        {
            ModelState.AddModelError(string.Empty, "Erro ao autenticar.");
            return Page();
        }

        Response.Cookies.Append("AuthToken", tokenObj.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        return RedirectToPage("/Index");
    }

    public class InputModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class UserToken
    {
        public string? Token { get; set; }
    }
}
