// src/ClearWealth.Application/Interfaces/ITransactionRepository.cs
using ClearWealth.Domain.Entities;

namespace ClearWealth.Application.Interfaces;

public interface ITransactionRepository
{
	Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);
}