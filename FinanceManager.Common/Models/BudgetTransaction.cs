using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Common.Models;

public class BudgetTransaction : BaseModel
{
    [Required] decimal Amount { get; set; }
}