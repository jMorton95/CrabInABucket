using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Common.Models;

public class Account : BaseModel
{
    [Required] public string Name { get; set; }
    public IEnumerable<BudgetTransaction> BudgetTransactions { get; set; }
}