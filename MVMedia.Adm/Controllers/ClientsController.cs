using Microsoft.AspNetCore.Mvc;
using MVMedia.Adm.Models;
using MVMedia.Adm.Services.Interfaces;

namespace MVMedia.Adm.Controllers;

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

    [HttpGet]
    public async Task<ActionResult> UpdateClient(int id)
    {
        var result = await _clientService.GetClientById(id);
        if (result is null)
            return View("Error", new string[] { "Something went wrong while processing your request" });
        return View(result);
    }

    [HttpPost]
    public async Task<ActionResult> UpdateClient(ClientViewModel clientVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _clientService.UpdateClient(clientVM);
            if (result is not null)
                return RedirectToAction(nameof(Index));
            else
                return View("Error", new string[] { "Something went wrong while processing your request" });
        }
        return View(clientVM);
    }


}
