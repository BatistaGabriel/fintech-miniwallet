using Fintech.MiniWallet.Domain.Entities;
using Fintech.MiniWallet.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fintech.MiniWallet.Infrastructure.Persistence.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly MiniWalletDbContext _context;

    public AccountRepository(MiniWalletDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Accounts
            .Include(a => a.Holder)
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Account> AddAsync(Account account, CancellationToken cancellationToken)
    {
        await _context.AddAsync(account, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return account;
    }

    public async Task UpdateAsync(Account account, CancellationToken cancellationToken)
    {
        foreach (var transaction in account.Transactions)
        {
            if (_context.Entry(transaction).State == EntityState.Detached)
                _context.Add(transaction);
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}