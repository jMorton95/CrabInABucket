namespace FinanceManager.Common.DataEntities;

public class Friendship : Entity
{
    public IEnumerable<UserFriendship> UserFriendships { get; set; }

    public bool IsAccepted { get; set; } = false;

    public bool IsPending { get; set; } = true;
    
    public IEnumerable<Message> Messages { get; set; }
}

