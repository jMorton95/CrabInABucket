using System.ComponentModel.DataAnnotations;

namespace CrabInABucket.Data.Models;

public class BudgetTransaction : BaseModel
{
    [Required] decimal Amount { get; set; }
}