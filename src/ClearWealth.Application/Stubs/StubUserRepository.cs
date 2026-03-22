using ClearWealth.Application.Interfaces;
using ClearWealth.Domain.Entities;

namespace ClearWealth.Application.Stubs;

// Hardcoded in-memory user data — swap for EF Core in Phase 1
public class StubUserRepository : IUserRepository
{
    private static readonly List<User> _users = new()
    {
        new()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Email = "demo@clearwealth.com",
            Name = "Demo User",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password")
        }
    };

    public Task<User?> GetByEmailAsync(string email) =>
        Task.FromResult(_users.FirstOrDefault(u => u.Email == email));

    public Task<User?> GetByIdAsync(Guid id) =>
        Task.FromResult(_users.FirstOrDefault(u => u.Id == id));

    public Task AddAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }
}
