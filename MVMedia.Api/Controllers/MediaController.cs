using Microsoft.AspNetCore.Mvc;

using MVMedia.Api.Repositories.Interfaces;
using MVMedia.API.Models;

namespace MVMedia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : Controller
{
 
    private readonly IMediaRepository _mediaRepository;

    public MediaController(IMediaRepository mediaRepository)
    {
        _mediaRepository = mediaRepository;
    }

    [HttpGet("GetAllMedia")]
    public async Task<ActionResult<IEnumerable<Media>>> GetAllMedia()
    {
        // This method should call the repository to get all media
        return Ok(await _mediaRepository.GetAllMedia());
    }

    [HttpGet("GetMedia/{id}")]
    public async Task<ActionResult<Media>> GetMediaById(int id)
    {
        // This method should call the repository to get media by ID
        var media = await _mediaRepository.GetMediaById(id);
        if (media == null)
        {
            return NotFound($"Media with id {id} not found!");
        }
        return Ok(media);
    }

    [HttpGet("GetMediasByClient/{clientId}")]
    public async Task<ActionResult<IEnumerable<Media>>> GetMediasByClientId(int clientId)
    {
        // This method should call the repository to get media by client ID
        var medias = await _mediaRepository.GetMediaByClientId(clientId);
        if (medias == null || !medias.Any())
        {
            return NotFound($"No media found for client with id {clientId}!");
        }
        return Ok(medias);
    }

    [HttpPost]
    public async Task<ActionResult<Media>> AddMedia(Media media)
    {
        // This method should call the repository to add a new media
        _mediaRepository.AddMedia(media);
        if (await _mediaRepository.SaveAllAsync())
        {
            return Ok(media);
        }
        return BadRequest("Failed to add media");
    }

}
