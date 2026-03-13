using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVMedia.Adm.Models;

public class CompanyViewModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(18)]
    [DataType(DataType.Text)]
    public required string CNPJ { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public required string Email { get; set; }

    [Required]
    [MaxLength(15)]
    public required string Phone { get; set; }
    
    [MaxLength(255)]
    public required string Address { get; set; }

    [MaxLength(90)]
    public required string City { get; set; }

    [MaxLength(2)]
    public required string State { get; set; }

    [MaxLength(10)]
    [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "Invalid Zip Code format. Use XXXXX-XXX.")]
    public string? ZipCode { get; set; }

    [ScaffoldColumn(false)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Invalid Date format. Use YYYY-MM-DD.")]
    public DateTime? CreatedAt { get; set; }

    
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Invalid Date format. Use YYYY-MM-DD.")]
    public DateTime? UpdatedAt { get; set; }

    [NotMapped]
    public virtual IEnumerable<ClientViewModel>? Clients { get; set; }
}
