using Fintech.MiniWallet.Domain.Entities;

namespace Fintech.MiniWallet.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Account> AddAsync(Account account, CancellationToken cancellationToken);
    Task UpdateAsync(Account account, CancellationToken cancellationToken);
}