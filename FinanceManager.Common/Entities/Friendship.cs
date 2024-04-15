namespace FinanceManager.Common.Entities;

public class Friendship : Entity
{
    public ICollection<UserFriendship> UserFriendships { get; set; }

    public bool IsAccepted { get; set; } = false;

    public bool IsPending { get; set; } = true;
    
    public ICollection<Message> Messages { get; set; }
}

