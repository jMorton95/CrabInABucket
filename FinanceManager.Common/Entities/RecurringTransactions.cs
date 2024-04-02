using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Common.Entities;

public class RecurringTransaction : Entity
{
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public string TransactionName { get; set; }
    
    /// <summary>
    /// Specifies the number of days between transactions.
    /// </summary>
    [Required]
    [Range(1, 28)]
    public int TransactionInterval { get; set; }
    
    public DateTime? LastTransactionDate { get; set; }
    
    public DateTime NextTransactionDate { get; set; }
    
    public virtual Guid RecipientAccountId { get; set; }
    
    public virtual Guid? SenderAccountId { get; set; }
}