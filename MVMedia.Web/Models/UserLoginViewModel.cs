using System.ComponentModel.DataAnnotations;

namespace MVMedia.Web.Models;

public class UserLoginViewModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
