using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVMedia.Adm.DTOs;
using MVMedia.Adm.Models;
using MVMedia.Adm.Services.Interfaces;

namespace MVMedia.Adm.Controllers;

public class MediaFileController : Controller
{
    private readonly IMediaFileService _mediaFileService;
    private readonly IClientService _clientService;

    public MediaFileController(IMediaFileService mediaFileService, IClientService clientService)
    {
        _mediaFileService = mediaFileService;
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MediaFileViewModel>>> Index()
    {
        var result = await _mediaFileService.GetAllMediaFiles();
        if (result == null)
            return View("Error", new string[] { "Something went wrong while processing your request" });
        return View(result);
    }

    [HttpGet]
    public async Task<ActionResult> AddMediaFileClient(int clientId)
    {
        var client = await _clientService.GetClientById(clientId);
        var mediaFileVM = new MediaFileViewModel { ClientId = client.Id, ClientName = client.Name };
        return View("AddMediaFileClient", mediaFileVM);
    }

    [HttpPost]
    public async Task<ActionResult> AddMediaFileClient(int clientId, MediaFileViewModel mediaFileVM)
    {
        if (ModelState.IsValid)
        {
            mediaFileVM.ClientId = clientId;

            // Preenche o FileName e o FileSize usando o arquivo enviado
            if (mediaFileVM.File != null)
            {
                mediaFileVM.FileName = mediaFileVM.File.FileName;
                mediaFileVM.FileSize = mediaFileVM.File.Length;
            }

            var result = await _mediaFileService.AddMediaFile(mediaFileVM);
            if (result != null)
                return RedirectToAction("ClientDetail", "Clients", new { id = clientId });
        }
        else
        {
            ViewBag.ClientId = new SelectList(await _clientService.GetAllClients(), "Id", "Name", clientId);
            mediaFileVM = new MediaFileViewModel { ClientId = clientId };
        }
        return View("AddMediaFileClient", mediaFileVM);
    }

    //[HttpGet]
    //public async Task<ActionResult<MediaFileViewModel>> UpdateMediaFile([FromBody] MediaFileViewModel mediaFile)
    //{
    //    var result = await _mediaFileService.UpdateMediaFile(mediaFile, mediaFile.FileName);
    //    if(result is null)
    //        return View("Error", new string[] { "Something went wrong while processing your request" });
    //    return View(result);
    //}

    //[HttpPost]
    //public async Task<ActionResult> UpdateMediaFile(MediaFileViewModel mediaFileVM)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var result = await _mediaFileService.UpdateMediaFile(mediaFileVM, mediaFileVM.FileName);
    //        if(result is not null)
    //            return RedirectToAction(nameof(Index));
    //        else
    //            return View("Error", new string[] { "Something went wrong while processing your request" });

    //    }
    //    return View(mediaFileVM);
    //}


    [HttpGet]
    public async Task<ActionResult<MediaFileViewModel>> GetMediaByClientId(int clientId)
    {
        var result = await _mediaFileService.GetMediaByClientId(clientId);
        // Corrigido: Verifica se result é nulo ou se a lista de mídias está vazia
        if (result == null || result.MediaFiles == null || !result.MediaFiles.Any())
            return View("Error", new string[] { "Something went wrong while processing your request" });
        return View("Index", result.MediaFiles);
    }

    [HttpGet]
    public async Task<ActionResult<MediaFileViewModel>> DeleteMediaFile(Guid id)
    {
        var mediaFileId = await _mediaFileService.GetMediaFileById(id);
        var result = await _mediaFileService.DeleteMediaFile(id);
        if (!result)
            return View("Error", new string[] { "Something went wrong while processing your request" });
        return RedirectToAction("ClientDetail", "Clients", new {id = mediaFileId});
    }


}
