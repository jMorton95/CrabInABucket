using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Core.DataEntities;

public class Friendship : BaseModel
{
    public IEnumerable<UserFriendship> UserFriendships { get; set; }

    public bool IsAccepted { get; set; } = false;

    public bool IsPending { get; set; } = true;
    
    public IEnumerable<Message> Messages { get; set; }
}

