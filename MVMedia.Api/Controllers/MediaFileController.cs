using Microsoft.AspNetCore.Mvc;
using MVMedia.Api.Models;
using MVMedia.Api.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MVMedia.Api.Controllers;

public class MediaFileUploadDTO
{
    [Required]
    public IFormFile File { get; set; }
    public bool IsPublic { get; set; }
    public int MediaId { get; set; }
}

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
            FileName = fileName,
            FileSize = dto.File.Length,
            UploadedAt = DateTime.UtcNow,
            IsPublic = dto.IsPublic,
            IsActive = true,
            MediaId = dto.MediaId
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

    [HttpGet("ListMediaUris")]
    public async Task<ActionResult<IEnumerable<string>>> ListMediaUris()
    {
        var allMediaFiles = await _mediaFileService.GetAllMediaFiles();
        var activeMediaFiles = allMediaFiles.Where(m => m.IsActive).ToList();
        var baseUrl = $"{Request.Scheme}://{Request.Host}/Videos/";
        var uris = activeMediaFiles.Select(m => baseUrl + m.FileName).ToList();
        return Ok(uris);
    }
}
