using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using MVMedia.Api.DTOs;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.API.Models;

namespace MVMedia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : Controller
{
 
    private readonly IMediaRepository _mediaRepository;
    private readonly IMapper _mapper;
    private readonly IClientRepository _clientRepository;

    public MediaController(IMediaRepository mediaRepository, IMapper mapper, IClientRepository clientRepository)
    {
        _mediaRepository = mediaRepository;
        _mapper = mapper;
        _clientRepository = clientRepository;
    }

    [HttpGet("GetAllMedia")]
    public async Task<ActionResult<IEnumerable<Media>>> GetAllMedia()
    {

        var medias = await _mediaRepository.GetAllMedia();
        var mediasDTO = _mapper.Map<IEnumerable<MediaGetDTO>>(medias);
        // This method should call the repository to get all media
        return Ok(mediasDTO);
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

        var mediaDTO = _mapper.Map<MediaGetDTO>(media);
        return Ok(mediaDTO);
    }

    [HttpGet("GetMediasByCliente/{id}")]
    public async Task<ActionResult<ClientWithMediaDTO>> GetClientWithMedias(int id)
    {
        var client = await _clientRepository.GetClientById(id);
        var medias = await _mediaRepository.GetMediaByClientId(id);

        var dto = new ClientWithMediaDTO
        {
            Client = _mapper.Map<ClientSummaryDTO>(client),
            Medias = _mapper.Map<List<MediaListItemDTO>>(medias)
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<Media>> AddMedia(MediaAddDTO mediaDTO)
    {
        // This method should call the repository to add a new media
        var media = _mapper.Map<Media>(mediaDTO);
                
        _mediaRepository.AddMedia(media);
        if (await _mediaRepository.SaveAllAsync())
        {
            return Ok(media);
        }
        return BadRequest("Failed to add media");
    }
    [HttpPut]
    public async Task<ActionResult<Media>> UpdateMedia([FromBody] MediaUpdateDTO mediaDTO)
    {
        if(mediaDTO.Id <= 0)
            return BadRequest("Is not possible to update a media without an ID");
        var mediaExistent = await _mediaRepository.GetMediaById(mediaDTO.Id);
        if (mediaExistent == null)
            return NotFound($"Media with id {mediaDTO.Id} not found!");
        if(mediaDTO == null)
            return BadRequest("Invalid media data");
        await _mediaRepository.UpdateMedia(mediaDTO);
        return Ok(mediaDTO);
    }

}
