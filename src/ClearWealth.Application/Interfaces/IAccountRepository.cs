using ClearWealth.Domain.Entities;

namespace ClearWealth.Application.Interfaces;

public interface IAccountRepository
{
    Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId);
    Task AddAsync(Account account);
}