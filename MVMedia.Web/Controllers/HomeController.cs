using Microsoft.AspNetCore.Mvc;
using MVMedia.Web.Models;
using MVMedia.Web.Service;
using System.Diagnostics;
using System.Text.Json; // Adicionado para JsonSerializer

namespace MVMedia.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly MediaService _mediaService;
        private readonly IConfiguration _configuration; // Injete a configuração

        // Cache da instância de JsonSerializerOptions para evitar CA1869
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public HomeController(MediaService mediaService, IConfiguration configuration)
        {
            _mediaService = mediaService;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            // Pegue a URL base da API do appsettings.json
            var apiBaseUrl = _configuration["ServiceUri:MVMediaAPI"];
            var apiUrl = $"{apiBaseUrl}/api/MediaList/GetActiveMediaList";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
                return View(null);

            var json = await response.Content.ReadAsStringAsync();
            var mediaList = JsonSerializer.Deserialize<MediaViewModel>(json, _jsonOptions);

            // Passe o objeto para a view
            return View(mediaList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
