using System.ComponentModel.DataAnnotations;

namespace CrabInABucket.DataContext.Models;

public class Account : BaseModel
{
    [Required] public string Name { get; set; }
    public IEnumerable<BudgetTransaction> BudgetTransactions { get; set; }
}