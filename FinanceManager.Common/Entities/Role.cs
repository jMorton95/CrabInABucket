using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Common.Entities;

public class Role : Entity
{
    [Required] public string Name { get; set; }
}