// src/ClearWealth.Application/Services/TransactionService.cs
using ClearWealth.Application.Interfaces;
using ClearWealth.Domain.Entities;

namespace ClearWealth.Application.Services;

public class TransactionService
{
    private readonly ITransactionRepository _repo;
    private readonly IAccountRepository _accounts;

    public TransactionService(ITransactionRepository repo, IAccountRepository accounts)
    {
        _repo = repo;
        _accounts = accounts;
    }

    public Task<IEnumerable<Transaction>> GetRecentAsync(Guid accountId) =>
        _repo.GetByAccountIdAsync(accountId);

    public async Task<IEnumerable<Transaction>> GetAllForUserAsync(Guid userId)
    {
        var accounts = await _accounts.GetByUserIdAsync(userId);
        var tasks = accounts.Select(a => _repo.GetByAccountIdAsync(a.Id));
        var results = await Task.WhenAll(tasks);
        return results.SelectMany(t => t).OrderByDescending(t => t.Date);
    }

    public async Task<IEnumerable<CategorySummary>> GetSpendingByCategory(Guid userId)
    {
        var transactions = await GetAllForUserAsync(userId);

        return transactions
            .Where(t => t.Amount < 0)
            .GroupBy(t => t.Category)
            .Select(g => new CategorySummary(
                Category: g.Key,
                TotalSpent: Math.Abs(g.Sum(t => t.Amount)),
                Count: g.Count()))
            .OrderByDescending(s => s.TotalSpent);
    }

    public async Task<CashFlowSummary> GetMonthlyCashFlowAsync(Guid userId)
    {
        var transactions = await GetAllForUserAsync(userId);

        var thisMonth = transactions.Where(t =>
            t.Date.Year == DateTime.UtcNow.Year &&
            t.Date.Month == DateTime.UtcNow.Month);

        var income = thisMonth.Where(t => t.Amount > 0).Sum(t => t.Amount);
        var spending = thisMonth.Where(t => t.Amount < 0).Sum(t => Math.Abs(t.Amount));

        return new CashFlowSummary(income, spending, income - spending);
    }
}

public record CategorySummary(TransactionCategory Category, decimal TotalSpent, int Count);
public record CashFlowSummary(decimal Income, decimal Spending, decimal Net);