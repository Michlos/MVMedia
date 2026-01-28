using Microsoft.AspNetCore.Mvc;
using MVMedia.Adm.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using MVMedia.Adm.Models;

namespace MVMedia.Adm.Controllers;

public class MediaListController : Controller
{
    private readonly IMediaListService _mediaListService;
    private readonly IMediaFileService _mediaFileService;

    public MediaListController(IMediaListService mediaListService, IMediaFileService mediaFileService)
    {
        _mediaListService = mediaListService;
        _mediaFileService = mediaFileService;
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveMediaList()
    {
        var result = await _mediaListService.GetMediaList();
        if (result == null)
            return View("Error", new ErrorViewModel { Message = "Something went wrong while processing your request" });
        return View(result);
    }

    //[HttpPost]
    //public async Task<IActionResult> DeactivateMediaList(int id)
    //{
    //    var result = await _mediaListService.
    //    if (!result)
    //        return View("Error", new string[] { "Something went wrong while processing your request" });
    //    return RedirectToAction(nameof(GetActiveMediaList));
    //}

    [HttpGet]
    public async Task<IActionResult> AddMediaList()
    {
        // Obtenha os arquivos de mídia diretamente do serviço
        var mediaFiles = await _mediaFileService.GetAllMediaFiles();
        if (mediaFiles == null)
            return View("Error", new ErrorViewModel { Message = "Something went wrong while processing your request" });

        // Aqui você precisa converter mediaFiles para o tipo esperado por AddMediaList

        // Supondo que MediaListViewModel tenha propriedades compatíveis:
        var mediaListViewModels = mediaFiles.Select(f => new MediaListViewModel
        {
            Name = "Lista de Mídias", // Ou outro nome padrão
            CreateDate = DateTime.Now,
            IsActive = true
        }).ToList();

        var result = await _mediaListService.AddMediaList(mediaListViewModels);
        if (result == null)
            return View("Error", new ErrorViewModel { Message = "Something went wrong while processing your request" });

        TempData["SuccessMessage"] = "Lista de media atualizada com sucesso!";
        return RedirectToAction("Index", "Clients");
    }


    public async Task<ActionResult> GetAllMedia()
    {
        var result = await _mediaFileService.GetAllMediaFiles();
        if (result == null)
            return View("Error", new ErrorViewModel { Message = "Something went wrong while processing your request" });
        return View(result);
    }

    public IActionResult Index()
    {
        return View();
    }
}
