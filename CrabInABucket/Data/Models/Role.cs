using System.ComponentModel.DataAnnotations;

namespace CrabInABucket.Data.Models;

public class Role : BaseModel
{
    [Required] public string Name { get; set; }
}