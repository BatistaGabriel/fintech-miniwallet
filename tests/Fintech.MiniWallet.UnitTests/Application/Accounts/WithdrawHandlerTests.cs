using Fintech.MiniWallet.Application.Accounts.Commands.Withdraw;
using Fintech.MiniWallet.Domain.Entities;
using Fintech.MiniWallet.Domain.Interfaces;
using Fintech.MiniWallet.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace Fintech.MiniWallet.UnitTests.Application.Accounts;

public class WithdrawHandlerTests
{
    private readonly IAccountRepository _repository;
    private readonly WithdrawHandler _handler;

    public WithdrawHandlerTests()
    {
        _repository = Substitute.For<IAccountRepository>();
        _handler = new WithdrawHandler(_repository);
    }

    [Fact]
    public async Task Should_Execute_Withdraw_Succesfully()
    {
        var account = new Account
        {
            Holder = new AccountHolder
            {
                Name = "Jhon Doe",
                Document = "123"
            }
        };

        account.Deposit(new Money(100m));

        _repository.GetByIdAsync(account.Id, Arg.Any<CancellationToken>()).Returns(account);

        var command = new WithdrawCommand(account.Id, 50m);
        await _handler.Handle(command, CancellationToken.None);

        account.Balance.Should().Be(new Money(50m));

        await _repository.Received(1).UpdateAsync(account, Arg.Any<CancellationToken>());
    }
}