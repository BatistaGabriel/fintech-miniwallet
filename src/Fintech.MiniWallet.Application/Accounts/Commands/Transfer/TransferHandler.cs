using Fintech.MiniWallet.Domain.Interfaces;
using Fintech.MiniWallet.Domain.ValueObjects;
using MediatR;

namespace Fintech.MiniWallet.Application.Accounts.Commands.Transfer;

public class TransferHandler : IRequestHandler<TransferCommand, Unit>
{
    private readonly IAccountRepository _repository;

    public TransferHandler(IAccountRepository accountRepository)
    {
        _repository = accountRepository;
    }

    public async Task<Unit> Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        var originAccount = await _repository.GetByIdAsync(request.OriginAccountId, cancellationToken) ?? throw new InvalidOperationException("Something is wrong with the informed origin account, try again.");
        var destinationAccount = await _repository.GetByIdAsync(request.DestinationAccountId, cancellationToken) ?? throw new InvalidOperationException("Something is wrong with the informed destination account, try again.");

        originAccount.Transfer(destinationAccount, new Money(request.Amount));
        await _repository.UpdateAsync(originAccount, cancellationToken);
        await _repository.UpdateAsync(destinationAccount, cancellationToken);

        return Unit.Value;
    }
}