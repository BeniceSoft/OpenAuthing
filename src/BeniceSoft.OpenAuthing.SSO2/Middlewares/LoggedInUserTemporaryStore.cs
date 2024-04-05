using System.Collections.Concurrent;
using BeniceSoft.OpenAuthing.Entities.Users;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Middlewares;

public record UserLoginInfo(User User, string Password, bool RememberMe, string? ReturnUrl = null);

public interface ILoggedInUserTemporaryStore
{
    Task<string> AddAsync(UserLoginInfo user);

    Task<UserLoginInfo?> PopAsync(string key);
}

public class InMemoryLoggedInUserTemporaryStore : ILoggedInUserTemporaryStore, ISingletonDependency
{
    private static readonly ConcurrentDictionary<string, UserLoginInfo> Users = new();

    public Task<string> AddAsync(UserLoginInfo user)
    {
        var id = Guid.NewGuid().ToString("n");

        if (!Users.TryAdd(id, user))
        {
            throw new InvalidOperationException("Could not add the user to the temporary store!");
        }

        return Task.FromResult(id);
    }

    public Task<UserLoginInfo?> PopAsync(string key)
    {
        if (Users.TryGetValue(key, out var user))
        {
            Users.TryRemove(key, out _);
        }

        return Task.FromResult(user);
    }
}