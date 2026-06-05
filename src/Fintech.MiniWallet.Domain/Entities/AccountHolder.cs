namespace Fintech.MiniWallet.Domain.Entities;

public class AccountHolder
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; init; }
    public required string Document { get; init; }
}