using System.ComponentModel.DataAnnotations;

namespace CrabInABucket.DataContext.Models;

public class User : BaseModel
{
    [Required] public string? Username { get; set; }

    [Required] public string? Password { get; set; }
}