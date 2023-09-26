using System.ComponentModel.DataAnnotations;

namespace CrabInABucket.Data.Models;

public class Role : BaseModel
{
    [Required] string Name { get; set; }
}