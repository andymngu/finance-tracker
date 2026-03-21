// src/ClearWealth.Application/Services/AccountService.cs
using ClearWealth.Application.Interfaces;
using ClearWealth.Domain.Entities;

namespace ClearWealth.Application.Services;

public class AccountService
{
    private readonly IAccountRepository _accounts;

    public AccountService(IAccountRepository accounts) => _accounts = accounts;

    public async Task<IEnumerable<Account>> GetAccountsAsync(Guid userId) =>
        await _accounts.GetByUserIdAsync(userId);

    public async Task<NetWorthSummary> GetNetWorthAsync(Guid userId)
    {
        var accounts = await _accounts.GetByUserIdAsync(userId);
        var list = accounts.ToList();

        var assets = list.Where(a => a.Type is AccountType.Checking or AccountType.Savings)
                         .Sum(a => a.Balance);
        var debts = list.Where(a => a.Type is AccountType.CreditCard
                                          or AccountType.AutoLoan or AccountType.StudentLoan)
                         .Sum(a => Math.Abs(a.Balance));
        return new NetWorthSummary(assets, debts, assets - debts);
    }
}

public record NetWorthSummary(decimal Assets, decimal Debts, decimal NetWorth);