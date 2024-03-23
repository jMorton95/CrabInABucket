using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Common.DataEntities;

public class Transaction : Entity
{
    [Required]
    public decimal Amount { get; set; }

    public bool RecurringTransaction { get; set; } = false;
    
    [ForeignKey("RecipientAccountId")]
    public Guid RecipientAccountId { get; set; }
    
    [ForeignKey("SenderAccountId")]
    public Guid? SenderAccountId { get; set; }
}