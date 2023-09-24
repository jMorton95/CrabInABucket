using System.ComponentModel.DataAnnotations;

namespace CrabInABucket.Models;

public class BaseModel
{
    [Required, Key]
    public Guid Id { get; set; }
    
    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    
    [Required]
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
    
    [Required]
    public int RowVersion { get; set; } = 1;
}