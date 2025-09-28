using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVMedia.Adm.Models;
using MVMedia.Adm.Services.Interfaces;

namespace MVMedia.Adm.Controllers;

public class MediasController : Controller
{
    private readonly IMediaService _mediaSerivce;
    private readonly IClientService _clientService;

    public MediasController(IMediaService mediaSerivce, IClientService clientService)
    {
        _mediaSerivce = mediaSerivce;
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MediaViewModel>>> Index()
    {
        var result = await _mediaSerivce.GetAllMedia();
        if (result == null)
            return View("Error", new string[] { "Something went wrong while processing your request" });
        return View(result);
    }

    [HttpGet]
    public async Task<ActionResult> AddMediaClient(int clientId)
    {
        var client = await _clientService.GetClientById(clientId);
        var mediaVM = new MediaViewModel { ClientId = client.Id, ClientName = client.Name };
        return View ("AddMediaClient", mediaVM);
    }

    [HttpPost]
    public async Task<ActionResult> AddMediaClient(int clientId, MediaViewModel mediaVM)
    {
        if (ModelState.IsValid)
        {
            mediaVM.ClientId = clientId;
            var result = await _mediaSerivce.AddMedia(mediaVM);
            if(result != null)
                return RedirectToAction("ClientDetail", "Clients", new { id = clientId });

        }else
        {
            ViewBag.ClientId = new SelectList(await _clientService.GetAllClients(), "Id", "Name", clientId);
            mediaVM = new MediaViewModel { ClientId = clientId };
        }
        return View("AddMediaClient", mediaVM);
    }

    [HttpGet]
    public async Task<ActionResult<MediaViewModel>> GetMediaByClienteId(int clientId)
    {
        var result = await _mediaSerivce.GetMediaByClientId(clientId);
        // Corrigido: Verifica se result é nulo ou se a lista de mídias está vazia
        if (result == null || result.Medias == null || !result.Medias.Any())
            return View("Error", new string[] { "Something went wrong while processing your request" });
        return View("Index", result.Medias);
    }

    [HttpGet]
    public async Task<ActionResult> UpdateMedia(int id)
    {
        var result = await _mediaSerivce.GetMediaById(id);
        if (result is null)
            return View("Error", new string[] { "Something went wrong while processing your request" });
        return View(result);
    }

    [HttpPost]
    public async Task<ActionResult> UpdateMedia(MediaViewModel mediaVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _mediaSerivce.UpdateMedia(mediaVM);
            if (result is not null)
                // Corrigido: Use RedirectToAction com o nome da action e o parâmetro
                return RedirectToAction("ClientDetail", "Clients", new { id = mediaVM.ClientId });
            else
                return View("Error", new string[] { "Something whete wrong while processing your request" });
        }
        return View(mediaVM);
    }

    [HttpGet]
    public async Task<ActionResult> DeleteMedia(int id)
    {
        var result = await _mediaSerivce.GetMediaById(id);
        if (result is null)
            return View("Error", new string[] { "Something went wrong while processing your request" });
        return View(result);
    }

    [HttpPost, ActionName("DeleteMedia")]
    public async Task<ActionResult> DeleteMediaConfirmed(int id)
    {
        var clientId = (await _mediaSerivce.GetMediaById(id))?.ClientId;
        var result = await _mediaSerivce.DeleteMedia(id);
        if (!result)
            return View("Error", new string[] { "Something went wrong while processing your request" });

        return RedirectToAction("ClienteDetail","Clients", new { id = clientId });
    }
}

