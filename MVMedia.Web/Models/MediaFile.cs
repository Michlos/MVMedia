using System.Text.Json.Serialization;

namespace MVMedia.Web.Models;

public class MediaFile
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
    public bool IsPublic { get; set; }
    public bool IsActive { get; set; }
    public int ClientId { get; set; }
}
