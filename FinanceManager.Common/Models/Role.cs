using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Common.Models;

public class Role : BaseModel
{
    [Required] public string Name { get; set; }
}