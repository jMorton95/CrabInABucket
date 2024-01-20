using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Core.DataEntities;

public class Transaction : BaseModel
{
    [Required]
    public decimal Amount { get; set; }

    public bool RecurringTransaction { get; set; } = false;
    
    public virtual Account RecipientAccount { get; set; }
    
    public virtual Account? SenderAccount { get; set; }
}