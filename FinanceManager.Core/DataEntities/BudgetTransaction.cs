using System.ComponentModel.DataAnnotations;
using FinanceManager.Core.DataEntities;

namespace FinanceManager.Core.DataEntities;

public class BudgetTransaction : BaseModel
{
    [Required] public decimal Amount { get; set; }
    
    public virtual Account Account { get; set; }
}