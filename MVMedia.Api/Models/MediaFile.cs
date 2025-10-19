using MVMedia.API.Models;
using System.ComponentModel.DataAnnotations;

namespace MVMedia.Api.Models;

public class MediaFile
{
    [Key]
    public Guid Id { get; set; } //UUID PRIMARY KEY

    //NÃO PRECISA MANTER POIS O FILEPATH JÁ ESTÁ NO APPSETTINGS.JSON
    //[Required]
    //[MaxLength(500)]
    //public string FilePath { get; set; }

    [Required]
    [MaxLength(100)]
    public string FileName { get; set; }

    public long FileSize { get; set; } // Size in bytes

    public DateTime UploadedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsPublic { get; set; }
    public bool IsActive { get; set; }
    public int MediaId { get; set; }
    public virtual Media Media { get; set; }

}
