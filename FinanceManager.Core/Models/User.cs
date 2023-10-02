using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Core.Models;

public class User : BaseModel
{
    [Required, StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
    
    public IEnumerable<Account> Accounts { get; set; }
    
    public IEnumerable<UserRole> Roles { get; set; }
}