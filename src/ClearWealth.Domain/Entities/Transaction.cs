namespace ClearWealth.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = "";
    public TransactionCategory Category { get; set; }
    public DateTime Date { get; set; }
}

public enum TransactionCategory
{
    Income, Groceries, Transport, Dining,
    Subscriptions, LoanPayment, Other
}