using MediatR;

namespace Fintech.MiniWallet.Application.Accounts.Commands.Withdraw;

public record WithdrawCommand (Guid AccountId, decimal Amount) : IRequest<Unit>;