namespace ClearWealth.Domain.Entities;

public class Account
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string InstitutionName { get; set; } = "";
    public string AccountName { get; set; } = "";
    public AccountType Type { get; set; }
    public decimal Balance { get; set; }
    public DateTime LastSyncedAt { get; set; } = DateTime.UtcNow;
}

public enum AccountType { Checking, Savings, CreditCard, AutoLoan, StudentLoan }