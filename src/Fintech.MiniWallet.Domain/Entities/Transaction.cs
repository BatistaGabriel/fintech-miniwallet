using Fintech.MiniWallet.Domain.Enums;
using Fintech.MiniWallet.Domain.ValueObjects;

namespace Fintech.MiniWallet.Domain.Entities;

public class Transaction
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime Date { get; init; } = DateTime.UtcNow;
    public required Money Amount { get; init; }
    public required TransactionType Type { get; init; }
    public required Account OriginAccount { get; init; }
    public Account? DestinationAccount { get; init; }
}