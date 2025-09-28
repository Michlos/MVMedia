using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Mvc;
using MVMedia.Adm.DTOs;
using MVMedia.Adm.Models;
using MVMedia.Adm.Services.Interfaces;

namespace MVMedia.Adm.Controllers;

public class ClientsController : Controller
{
    private readonly IClientService _clientService;
    private readonly IMediaService _mediaService;

    public ClientsController(IClientService clientService, IMediaService mediaService)
    {
        _clientService = clientService;
        _mediaService = mediaService;
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

    
    [HttpGet]
    public async Task<ActionResult<ClientWithMediaDTO>> ClientDetail(int id)
    {
        var clientWithMedia = await _mediaService.GetMediaByClientId(id);
        if (clientWithMedia == null)
        {
            return NotFound("Client not found.");
        }
        return View(clientWithMedia);
    }


}
