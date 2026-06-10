using MediatR;

namespace Fintech.MiniWallet.Application.Accounts.Commands.Deposit;

public record DepositCommand(Guid AccountId, decimal Amount) : IRequest<Unit>;