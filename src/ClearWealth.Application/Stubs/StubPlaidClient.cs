// src/ClearWealth.Application/Stubs/StubPlaidClient.cs
using ClearWealth.Application.Interfaces;

namespace ClearWealth.Application.Stubs;

public class StubPlaidClient : IPlaidClient
{
    public Task<string> ExchangePublicTokenAsync(string publicToken)
    {
        // Always returns a fake access token — real Plaid call goes here in Phase 1
        return Task.FromResult("access-sandbox-stub-token-abc123");
    }
}