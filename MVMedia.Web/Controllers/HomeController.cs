using Microsoft.AspNetCore.Mvc;
using MVMedia.Web.Models;
using MVMedia.Web.Service;
using System.Diagnostics;

namespace MVMedia.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly MediaService _mediaService;
        public HomeController(MediaService mediaService)
        {
            _mediaService = mediaService;
        }
        public async Task<IActionResult> Index()
        {
            var media = await _mediaService.GetAllMedia();
            return View(media);
        }
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
