namespace CrabInABucket.Data.Models;

public class UserRole : BaseModel
{
    public User User { get; set; }
    public Role Role { get; set; }
}