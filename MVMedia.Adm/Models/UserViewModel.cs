using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MVMedia.Adm.Models;

public class UserViewModel
{
    //public int Id { get; private set; }
    //public string Name { get; private set; }

    
    public required string Username { get; set; }

    public required string Password { get; set; }

    //public string Email { get; private set; }
    //public bool IsActive { get; set; }
    //public bool IsAdmin { get; set; }
    //[JsonIgnore]
    //public string? Token { get; set; }
}
