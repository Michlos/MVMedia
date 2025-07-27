using MVMedia.API.Models;

using System.ComponentModel.DataAnnotations;

namespace MVMedia.Api.DTOs;

public class MediaGetDTO
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    public bool IsActive { get; set; } = true;
    [Required]
    [MaxLength(500)]
    public string MediaUrl { get; set; } // URL to access the media file
    public int ClientId { get; set; }

}
