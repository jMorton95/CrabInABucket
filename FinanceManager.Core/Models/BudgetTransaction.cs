using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Core.Models;

public class BudgetTransaction : BaseModel
{
    [Required] decimal Amount { get; set; }
    
    public virtual Account Account { get; set; }
}