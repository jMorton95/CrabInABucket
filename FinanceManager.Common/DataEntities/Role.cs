using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Core.DataEntities;

public class Role : BaseModel
{
    [Required] public string Name { get; set; }
}