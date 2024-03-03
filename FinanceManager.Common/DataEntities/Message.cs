namespace FinanceManager.Core.DataEntities;

public class Message : BaseModel
{
    public User Sender { get; set; }
    
    public User Recipient { get; set; }
    
    public string MessageContent { get; set; }
}