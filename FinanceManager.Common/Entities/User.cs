using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Common.Entities;

public class User : Entity
{
    [Required, StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public DateTime LastOnline { get; set; }
    
    public IEnumerable<Account> Accounts { get; set; }
    
    public IEnumerable<UserRole> Roles { get; set; }
    
    public IEnumerable<UserFriendship> UserFriendships { get; set; }
}