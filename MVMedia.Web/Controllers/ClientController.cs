using Microsoft.AspNetCore.Mvc;
using MVMedia.Web.Service;

namespace MVMedia.Web.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApiService _apiService;

        public ClientController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            // Exemplo: buscar clientes autenticados
            // var clients = await _apiService.GetClientsAsync();
            return View();
        }
    }
}
