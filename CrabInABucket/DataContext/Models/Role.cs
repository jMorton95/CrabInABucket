using System.ComponentModel.DataAnnotations;

namespace CrabInABucket.DataContext.Models;

public class Role : BaseModel
{
    [Required] string Name { get; set; }
}