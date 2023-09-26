using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrabInABucket.Data.Models;

public class BaseModel
{
    [Required, Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    public int RowVersion { get; set; } = 1;
}