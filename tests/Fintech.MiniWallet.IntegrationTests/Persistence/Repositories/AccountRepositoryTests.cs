using Fintech.MiniWallet.Domain.Entities;
using Fintech.MiniWallet.Domain.ValueObjects;
using Fintech.MiniWallet.Infrastructure.Persistence;
using Fintech.MiniWallet.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Fintech.MiniWallet.IntegrationTests.Persistence.Repositories;

public class AccountRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine").Build();

    private MiniWalletDbContext _context = null!;

    public async Task DisposeAsync() => await _postgres.DisposeAsync();

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<MiniWalletDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
            .EnableSensitiveDataLogging()
            .Options;

        _context = new MiniWalletDbContext(options);

        await _context.Database.EnsureCreatedAsync();
    }

    [Fact]
    public async Task Should_Persist_Account_And_Update_Balance()
    {
        var repository = new AccountRepository(_context);

        Account account = AccountInitializer(new AccountObject());

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        account.Deposit(new Money(100m));
        await repository.UpdateAsync(account, CancellationToken.None);

        _context.ChangeTracker.Clear();

        var result = await repository.GetByIdAsync(account.Id, CancellationToken.None);
        result.Should().NotBeNull();
        result!.Balance.Should().Be(new Money(100m));
    }

    private Account AccountInitializer(AccountObject accountObject)
    {
        Account initializedAccount = new Account { Holder = accountObject.Holder };

        if (accountObject.Balance.Amount > 0)
            initializedAccount.Deposit(accountObject.Balance);

        return initializedAccount;
    }
}

public class AccountObject
{
    public AccountHolder Holder { get; set; } = new AccountHolder { Name = "Unknown", Document = "12345678900" };
    public Money Balance { get; set; } = new Money(0);
}