using System.ComponentModel.DataAnnotations;

namespace CrabInABucket.DataContext.Models;

public class User : BaseModel
{
    [Required]
    public string? Username { get; set; }
    
    public string? Firstname { get; set; }
    
    public string? LastName { get; set; }
    
    public string? Password { get; set; }
}