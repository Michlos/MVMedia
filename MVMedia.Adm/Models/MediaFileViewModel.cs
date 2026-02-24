using System.ComponentModel.DataAnnotations;

namespace MVMedia.Adm.Models;

public class MediaFileViewModel
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public string? FileName { get; set; }
    public long FileSize { get; set; } // Size in bytes
    public bool IsPublic { get; set; }
    public bool IsActive { get; set; }

    public IFormFile File { get; set; }
    public int ClientId { get; set; }
    public string? ClientName { get; set; }
    public int CompanyId { get; set; }
    //public string? CompanyName { get; set; }
}
