using MediatR;

namespace Fintech.MiniWallet.Application.Accounts.Commands.Transfer;

public record TransferCommand(Guid OriginAccountId, Guid DestinationAccountId, decimal Amount) : IRequest<Unit>;