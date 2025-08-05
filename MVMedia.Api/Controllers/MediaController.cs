using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using MVMedia.Api.DTOs;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.Api.Services;
using MVMedia.Api.Services.Interfaces;
using MVMedia.API.Models;

namespace MVMedia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : Controller
{

    private readonly IMediaSerivce _mediaService;


    public MediaController(IMediaSerivce mediaService)
    {
        _mediaService = mediaService;
    }

    [HttpGet("GetAllMedia")]
    public async Task<ActionResult<IEnumerable<MediaGetDTO>>> GetAllMedia()
    {

        var medias = await _mediaService.GetAllMedia();
        // This method should call the repository to get all media
        return Ok(medias);
    }

    [HttpGet("GetMedia/{id}")]
    public async Task<ActionResult<MediaGetDTO>> GetMediaById(int id)
    {
        // This method should call the repository to get media by ID
        var media = await _mediaService.GetMediaById(id);
        if (media == null)
        {
            return NotFound($"Media with id {id} not found!");
        }
        return Ok(media);
    }

    [HttpGet("GetMediasByCliente/{id}")]
    public async Task<ActionResult<ClientWithMediaDTO>> GetClientWithMedias(int id)
    {
        var clientWithMediasDTO = await _mediaService.GetMediaByClientId(id);
        return Ok(clientWithMediasDTO);
    }

    [HttpPost]
    public async Task<ActionResult<Media>> AddMedia(MediaAddDTO mediaDTO)
    {
        var mediaAdded = await _mediaService.AddMedia(mediaDTO);
        if (mediaAdded == null)
        {
            return BadRequest("Failed to add media");
        }
        return Ok(mediaAdded);

    }
    [HttpPut]
    public async Task<ActionResult<Media>> UpdateMedia([FromBody] MediaUpdateDTO mediaDTO)
    {
        if (mediaDTO.Id <= 0)
            return BadRequest("Is not possible to update a media without an ID");
        var mediaExistent = await _mediaService.GetMediaById(mediaDTO.Id);
        if (mediaExistent == null)
            return NotFound($"Media with id {mediaDTO.Id} not found!");
        if (mediaDTO == null)
            return BadRequest("Invalid media data");
        await _mediaService.UpdateMedia(mediaDTO);
        return Ok(mediaDTO);
    }

}
