using System.ComponentModel.DataAnnotations;

namespace MVMedia.Adm.Models;

public class ClientViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "The Name field is required.")]
    public string Name { get; set; }
    public string? CPF { get; set; }
    public string? CNPJ { get; set; }
    [Required(ErrorMessage = "The Email field is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The Phone field is required.")]
    public string Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public virtual IEnumerable<MediaViewModel>? Medias { get; set; }
}
