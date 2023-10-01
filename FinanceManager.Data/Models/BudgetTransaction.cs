using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Data.Models;

public class BudgetTransaction : BaseModel
{
    [Required] decimal Amount { get; set; }
}