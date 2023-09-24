using System.ComponentModel.DataAnnotations;

namespace CrabInABucket.DataContext.Models;

public class BudgetTransaction : BaseModel
{
    [Required] decimal Amount { get; set; }
}