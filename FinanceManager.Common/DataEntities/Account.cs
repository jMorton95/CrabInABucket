using System.ComponentModel.DataAnnotations;
using FinanceManager.Core.DataEntities;

namespace FinanceManager.Core.DataEntities;

public class Account : BaseModel
{
    [Required] public string Name { get; set; }
    public IEnumerable<BudgetTransaction> BudgetTransactions { get; set; }
    public virtual User User { get; set; }
}