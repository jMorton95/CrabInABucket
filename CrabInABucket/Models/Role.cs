using System.ComponentModel.DataAnnotations;

namespace CrabInABucket.Models;

public class Role : BaseModel
{
    [Required] string Name { get; set; }
}