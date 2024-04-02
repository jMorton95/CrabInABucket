namespace FinanceManager.Common.Entities;

public class Message : Entity
{
    public User Sender { get; set; }
    
    public User Recipient { get; set; }
    
    public string MessageContent { get; set; }
}