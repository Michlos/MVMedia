using Microsoft.AspNetCore.Mvc;
using MVMedia.Web.Models;
using MVMedia.Web.Services;

namespace MVMedia.Web.Controllers;

public class AccountController : Controller
{
    private readonly ApiAuthService _apiAuthService;

    public AccountController(ApiAuthService apiAuthService)
    {
        _apiAuthService = apiAuthService;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(UserViewModel model)
    {
        if(!ModelState.IsValid) return View(model);

        var token = await _apiAuthService.LoginAsync(model.Login, model.Password);
        if (token == null)
        {
            ModelState.AddModelError("", "Usuário ou senha inválidos.");
            return View(model);
        }

        // Armazena o token na sessão em um cookie seguro
        Response.Cookies.Append("AuthToken", token, new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        //redireciona para a página inicial
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        // Remove o token da sessão
        Response.Cookies.Delete("AuthToken");
        // Redireciona para a página de login
        return RedirectToAction("Index", "Home");
    }
}
