using Microsoft.AspNetCore.Mvc;
using MVMedia.Adm.Models;
using MVMedia.Adm.Services;

namespace MVMedia.Adm.Controllers;

public class AccountController : Controller
{
    //for autentication
    private readonly ApiAuthService _apiAuthService;


    public AccountController(ApiAuthService apiAuthService)
    {
        _apiAuthService = apiAuthService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserViewModel model, UserToken? token)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Chama o serviço que faz a requisição para a API
        var userData = await _apiAuthService.LoginAsync(model.Username, model.Password);

        if (string.IsNullOrEmpty(userData.Token))
        {
            ModelState.AddModelError("", "Usuário ou senha inválidos.");
            return View(model);
        }

        // Salva o token em cookie seguro
        Response.Cookies.Append("AuthToken", userData.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });
        Response.Cookies.Append("IsAdmin", userData.IsAdmin.ToString().ToLower());
        Response.Cookies.Append("Username", model.Username);

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
