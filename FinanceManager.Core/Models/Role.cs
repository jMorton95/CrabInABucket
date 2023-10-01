using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Core.Models;

public class Role : BaseModel
{
    [Required] public string Name { get; set; }
}