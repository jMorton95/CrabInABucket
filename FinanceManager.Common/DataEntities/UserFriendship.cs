using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Core.DataEntities;

public class UserFriendship : BaseModel
{
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    
    [ForeignKey("FriendshipId")]
    public Guid FriendshipId { get; set; }
    
    public Friendship Friendship { get; set; }
    
    public User User { get; set; }
}