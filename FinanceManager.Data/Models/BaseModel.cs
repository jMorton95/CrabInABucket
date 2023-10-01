using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Data.Models;

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
    
    public Guid? CreatedBy { get; set; }
    
    public Guid? EditedBy { get; set; }
}