using Microsoft.AspNetCore.Mvc;
using MVMedia.Api.DTOs;
using MVMedia.Api.Models;
using MVMedia.Api.Services.Interfaces;

namespace MVMedia.Api.Controllers;



[ApiController]
[Route("api/[controller]")]
public class MediaFileController : ControllerBase
{
    private readonly IMediaFileService _mediaFileService;

    public MediaFileController(IMediaFileService mediaFileService)
    {
        _mediaFileService = mediaFileService;
    }

    [HttpPost("AddMediaFile")]
    public async Task<ActionResult<MediaFile>> AddMediaFile([FromForm] MediaFileUploadDTO dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("Nenhum arquivo enviado.");

        var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
        var mediaFile = new MediaFile
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            FileName = fileName,
            FileSize = dto.File.Length,
            UploadedAt = DateTime.UtcNow,
            IsPublic = dto.IsPublic,
            IsActive = true,
            ClientId = dto.ClientId
        };

        // Salva no banco
        var mediaFileAdded = await _mediaFileService.AddMediaFile(mediaFile);
        if (mediaFileAdded == null)
            return BadRequest("Falha ao adicionar arquivo de mídia.");

        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Videos");
        Directory.CreateDirectory(uploadPath);
        var filePath = Path.Combine(uploadPath, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await dto.File.CopyToAsync(stream);
        }

        return Ok(mediaFileAdded);
    }

    [HttpGet("ListActiveMediaFiles")]
    public async Task<ActionResult<IEnumerable<MediaFile>>> ListActiveMediaFiles()
    {
        var allMediaFiles = await _mediaFileService.GetAllMediaFiles();
        var activeMediaFiles = allMediaFiles.Where(m => m.IsActive).ToList();
        return Ok(activeMediaFiles);
    }

    [HttpGet("GetMediaFileByClientId/{clientId}")]
    public async Task<ActionResult<ClientWithMediaFileDTO>> GetMediaFileByClientId(int clientId)
    {
        var clientMediaFiles = await _mediaFileService.GetAllMediaByClientId(clientId);
        if (clientMediaFiles == null)
            return NotFound("Nenhum arquivo de mídia encontrado para o cliente especificado.");
        return Ok(clientMediaFiles);
    }

    [HttpGet("ListMediaUris")]
    public async Task<ActionResult<IEnumerable<string>>> ListMediaUris()
    {
        var allMediaFiles = await _mediaFileService.GetAllMediaFiles();
        var activeMediaFiles = allMediaFiles.Where(m => m.IsActive).ToList();
        var baseUrl = $"{Request.Scheme}://{Request.Host}/Videos/";
        var uris = activeMediaFiles.Select(m => baseUrl + m.FileName).ToList();
        return Ok(uris);
    }

    [HttpPut("UpdateMediaFile/{id}")]
    public async Task<ActionResult<MediaFile>> UpdateMediaFile(Guid id, [FromForm] MediaFileUploadDTO dto)
    {
        var existingMediaFile = await _mediaFileService.GetMediaFileById(id);
        var oldFileName = existingMediaFile?.FileName;
        if (existingMediaFile == null)
            return NotFound("Arquivo de mídia não encontrado.");

        // Atualiza os campos do modelo
        existingMediaFile.Title = dto.Title;
        existingMediaFile.Description = dto.Description;
        existingMediaFile.IsPublic = dto.IsPublic;
        existingMediaFile.UpdatedAt = DateTime.UtcNow;
        existingMediaFile.ClientId = dto.ClientId;

        // Se um novo arquivo foi enviado, atualiza o arquivo físico e o nome
        if (dto.File != null && dto.File.Length > 0)
        {
            var newFileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Videos");
            Directory.CreateDirectory(uploadPath);
            var newFilePath = Path.Combine(uploadPath, newFileName);

            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            existingMediaFile.FileName = newFileName;
            existingMediaFile.FileSize = dto.File.Length;
        }

        var updatedMediaFile = await _mediaFileService.UpdateMediaFile(existingMediaFile, oldFileName);
        return Ok(updatedMediaFile);
    }


}
