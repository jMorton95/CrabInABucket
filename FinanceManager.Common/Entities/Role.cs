using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Common.DataEntities;

public class Role : Entity
{
    [Required] public string Name { get; set; }
}