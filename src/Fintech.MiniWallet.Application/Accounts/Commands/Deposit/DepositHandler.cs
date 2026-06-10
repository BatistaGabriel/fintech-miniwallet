using Fintech.MiniWallet.Application.Accounts.Commands.Deposit;
using Fintech.MiniWallet.Domain.Entities;
using Fintech.MiniWallet.Domain.Interfaces;
using Fintech.MiniWallet.Domain.ValueObjects;
using MediatR;

namespace Fintech.MiniWallet.Application.Accounts.Commands.Deposit;

public class DepositHandler : IRequestHandler<DepositCommand, Unit>
{
    private readonly IAccountRepository _repository;

    public DepositHandler(IAccountRepository accountRepository)
    {
        _repository = accountRepository;
    }

    public async Task<Unit> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetByIdAsync(request.AccountId, cancellationToken) ?? throw new InvalidOperationException("Something is wrong with the informed account, try again.");

        account.Deposit(new Money(request.Amount));
        await _repository.UpdateAsync(account, cancellationToken);

        return Unit.Value;
    }
}