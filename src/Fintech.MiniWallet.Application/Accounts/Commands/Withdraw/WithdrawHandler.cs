using Fintech.MiniWallet.Domain.Interfaces;
using Fintech.MiniWallet.Domain.ValueObjects;
using MediatR;

namespace Fintech.MiniWallet.Application.Accounts.Commands.Withdraw;

public class WithdrawHandler : IRequestHandler<WithdrawCommand, Unit>
{
    private readonly IAccountRepository _repository;

    public WithdrawHandler(IAccountRepository accountRepository)
    {
        _repository = accountRepository;
    }

    public async Task<Unit> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetByIdAsync(request.AccountId, cancellationToken) ?? throw new InvalidOperationException("Something is wrong with the informed account, try again.");

        account.Withdraw(new Money(request.Amount));
        await _repository.UpdateAsync(account, cancellationToken);

        return Unit.Value;
    }
}