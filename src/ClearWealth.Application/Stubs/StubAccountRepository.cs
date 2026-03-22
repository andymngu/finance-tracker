// src/ClearWealth.Application/Stubs/StubAccountRepository.cs
using ClearWealth.Application.Interfaces;
using ClearWealth.Domain.Entities;

namespace ClearWealth.Application.Stubs;

// Hardcoded in-memory data — swap for EF Core in Phase 1
public class StubAccountRepository : IAccountRepository
{
    private static readonly Guid _userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    private static readonly List<Account> _accounts = new()
    {
        new() { UserId = _userId, InstitutionName = "SoFi",
                AccountName = "Checking", Type = AccountType.Checking,
                Balance = 6240.18m },
        new() { UserId = _userId, InstitutionName = "Bank of America",
                AccountName = "Savings", Type = AccountType.Savings,
                Balance = 18110.44m },
        new() { UserId = _userId, InstitutionName = "Chase",
                AccountName = "Sapphire Card", Type = AccountType.CreditCard,
                Balance = -1847.22m },
        new() { UserId = _userId, InstitutionName = "Honda Financial",
                AccountName = "Civic Auto Loan", Type = AccountType.AutoLoan,
                Balance = -18440.00m },
        new() { UserId = _userId, InstitutionName = "OSAP",
                AccountName = "Student Loan", Type = AccountType.StudentLoan,
                Balance = -11930.00m },
    };

    public Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId) =>
        Task.FromResult(_accounts.Where(a => a.UserId == userId));

    public Task AddAsync(Account account)
    {
        _accounts.Add(account);
        return Task.CompletedTask;
    }
}