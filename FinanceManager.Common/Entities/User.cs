using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Common.Entities;

public class User : Entity
{
    [Required, StringLength(100, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public DateTime LastOnline { get; set; }
    
    public ICollection<Account> Accounts { get; set; }
    
    public ICollection<UserRole> Roles { get; set; }
    
    public ICollection<UserFriendship> UserFriendships { get; set; }
}