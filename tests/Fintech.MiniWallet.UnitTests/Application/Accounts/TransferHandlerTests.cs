using Fintech.MiniWallet.Application.Accounts.Commands.Transfer;
using Fintech.MiniWallet.Domain.Entities;
using Fintech.MiniWallet.Domain.Interfaces;
using Fintech.MiniWallet.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace Fintech.MiniWallet.UnitTests.Application.Accounts;

public class TransferHandlerTests
{
    private readonly IAccountRepository _repository;
    private readonly TransferHandler _handler;

    public TransferHandlerTests()
    {
        _repository = Substitute.For<IAccountRepository>();
        _handler = new TransferHandler(_repository);
    }

    [Fact]
    public async Task Should_Execute_Transfer_Succesfully()
    {
        var originAccount = new Account
        {
            Holder = new AccountHolder
            {
                Name = "Jhon Doe",
                Document = "123"
            }
        };
        originAccount.Deposit(new Money(100m));

        var destinationAccount = new Account
        {
            Holder = new AccountHolder
            {
                Name = "Jane Doe",
                Document = "123"
            }
        };

        _repository.GetByIdAsync(originAccount.Id,Arg.Any<CancellationToken>()).Returns(originAccount);
        _repository.GetByIdAsync(destinationAccount.Id,Arg.Any<CancellationToken>()).Returns(destinationAccount);

        var command = new TransferCommand(originAccount.Id, destinationAccount.Id, 50m);

        await _handler.Handle(command, CancellationToken.None);

        originAccount.Balance.Should().Be(new Money(50m));
        destinationAccount.Balance.Should().Be(new Money(50m));

        await _repository.Received(1).UpdateAsync(originAccount,Arg.Any<CancellationToken>());
        await _repository.Received(1).UpdateAsync(destinationAccount,Arg.Any<CancellationToken>());
    }
}