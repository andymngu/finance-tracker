// src/ClearWealth.Application/Stubs/StubTransactionRepository.cs
using ClearWealth.Application.Interfaces;
using ClearWealth.Domain.Entities;

namespace ClearWealth.Application.Stubs;

public class StubTransactionRepository : ITransactionRepository
{
    private static readonly Guid _checkingId =
        Guid.Parse("00000000-0000-0000-0000-000000000010");

    private static readonly List<Transaction> _transactions = new()
    {
        new() { AccountId = _checkingId, Amount = -84.32m,
                Description = "Whole Foods", Category = TransactionCategory.Groceries,
                Date = DateTime.UtcNow.AddDays(-1) },
        new() { AccountId = _checkingId, Amount = 2450.00m,
                Description = "Payroll Deposit", Category = TransactionCategory.Income,
                Date = DateTime.UtcNow.AddDays(-2) },
        new() { AccountId = _checkingId, Amount = -62.10m,
                Description = "Shell Gas Station", Category = TransactionCategory.Transport,
                Date = DateTime.UtcNow.AddDays(-3) },
        new() { AccountId = _checkingId, Amount = -418.00m,
                Description = "Honda Auto Payment", Category = TransactionCategory.LoanPayment,
                Date = DateTime.UtcNow.AddDays(-6) },
        new() { AccountId = _checkingId, Amount = -7.45m,
                Description = "Starbucks", Category = TransactionCategory.Dining,
                Date = DateTime.UtcNow.AddDays(-4) },
    };

    public Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId) =>
        Task.FromResult(_transactions.Where(t => t.AccountId == accountId));
}