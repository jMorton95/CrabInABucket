using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Common.Entities;

public interface IEntity
{
    Guid Id { get; }
}

public class Entity : IEntity
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

    public bool WasSimulated { get; set; } = false;
}