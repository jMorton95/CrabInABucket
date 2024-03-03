using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Core.DataEntities;

public class Friendship : BaseModel
{
    public IEnumerable<UserFriendship> UserFriendships { get; set; }

    public bool IsConfirmed { get; set; } = false;
    
    public IEnumerable<Message> Messages { get; set; }
}

