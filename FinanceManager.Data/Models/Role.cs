using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Data.Models;

public class Role : BaseModel
{
    [Required] public string Name { get; set; }
}