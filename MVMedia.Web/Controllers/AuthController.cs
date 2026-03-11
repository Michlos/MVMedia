using Microsoft.AspNetCore.Mvc;
using MVMedia.Web.Models;
using MVMedia.Web.Service;

namespace MVMedia.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _apiService;

        public AuthController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = await _apiService.AuthenticateAsync(model.Login, model.Password);
            if (!string.IsNullOrEmpty(token))
            {
                _apiService.SetAuthorizationHeader(token);
                // Salve o token em cookie/session se necessário
                return RedirectToAction("Index", "Client");
            }

            ModelState.AddModelError(string.Empty, "Login inválido.");
            return View(model);
        }
    }
}
