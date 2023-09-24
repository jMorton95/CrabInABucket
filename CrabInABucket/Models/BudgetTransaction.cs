using System.ComponentModel.DataAnnotations;

namespace CrabInABucket.Models;

public class BudgetTransaction : BaseModel
{
    [Required] decimal Amount { get; set; }
}