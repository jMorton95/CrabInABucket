using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Common.DataEntities;

public class Account : Entity
{
    [Required] public string Name { get; set; }
    
    public decimal Balance { get; set; } = 0;
    public ICollection<RecurringTransaction> RecurringTransactions { get; set; }
    public User User { get; set; }
}