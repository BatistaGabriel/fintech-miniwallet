using Fintech.MiniWallet.Application;
using Fintech.MiniWallet.Application.Accounts.Commands.Deposit;
using Fintech.MiniWallet.Domain.Entities;
using Fintech.MiniWallet.Domain.Interfaces;
using Fintech.MiniWallet.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace Fintech.MiniWallet.UnitTests.Application.Accounts;

public class DepositHandlerTests
{
    private readonly IAccountRepository _repository;
    private readonly DepositHandler _handler;

    public DepositHandlerTests()
    {
        _repository = Substitute.For<IAccountRepository>();
        _handler = new DepositHandler(_repository);
    }

    [Fact]
    public async Task Should_Execute_Deposit_Succesfully()
    {
        var account = new Account
        {
            Holder = new AccountHolder
            {
                Name = "Jhon Doe",
                Document = "123"
            }
        };
        _repository.GetByIdAsync(account.Id, Arg.Any<CancellationToken>()).Returns(account);

        var command = new DepositCommand(account.Id, 100m);

        await _handler.Handle(command, CancellationToken.None);

        account.Balance.Should().Be(new Money(100m));
        await _repository.Received(1).UpdateAsync(account, Arg.Any<CancellationToken>());
    }
}