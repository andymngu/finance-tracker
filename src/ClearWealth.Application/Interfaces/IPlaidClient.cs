namespace ClearWealth.Application.Interfaces;

// Stub — real Plaid SDK drops in here in Phase 1
public interface IPlaidClient
{
    Task<string> ExchangePublicTokenAsync(string publicToken);
}