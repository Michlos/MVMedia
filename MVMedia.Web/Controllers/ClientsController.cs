using Microsoft.AspNetCore.Mvc;
using MVMedia.Web.Models;
using MVMedia.Web.Services.Interfaces;

namespace MVMedia.Web.Controllers;

public class ClientsController : Controller
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientViewModel>>> Index()
    {
        var result = await _clientService.GetAllClients();

        if (result == null)
            return View("Error", new string[] { "Something went wrong while processing your request" });

        return View(result);
    }
    [HttpGet]
    public async Task<ActionResult> AddClient()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> AddClient(ClientViewModel clientVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _clientService.AddClient(clientVM);
            if (result != null)
                return RedirectToAction(nameof(Index));
            else
                return View("Error", new string[] { "Something went wrong while processing your request" });
        }
        return View(clientVM);
    }

    
}
