using System.ComponentModel.DataAnnotations;

namespace CrabInABucket.DataContext.Models;

public class BaseModel
{
    [Required, Key]
    public int Id { get; set; }
    
    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    
    [Required]
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
    
    [Required]
    public int RowVersion { get; set; } = 1;
}