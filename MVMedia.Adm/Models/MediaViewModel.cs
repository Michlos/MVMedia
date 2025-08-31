using System.ComponentModel.DataAnnotations;

namespace MVMedia.Adm.Models;

public class MediaViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public string MediaUrl { get; set; }
    public string? Notes { get; set; }
    public int ClientId { get; set; }
}
