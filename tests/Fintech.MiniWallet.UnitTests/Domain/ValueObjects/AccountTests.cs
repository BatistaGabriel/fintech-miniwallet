using FluentAssertions;

namespace Fintech.MiniWallet.UnitTests.Domain.ValueObjects;

public class AccountTests
{
    [Fact]
    public void Should_Have_Zero_Balance_When_Account_Is_Created()
    {
        Account account = new Account();
        
        account.Balance.Should().Be(0);
    }

    [Fact]
    public void Should_Have_An_Account_Holder_When_Account_Is_Created()
    {
        Account account = new Account();
        
        account.Holder.Should().NotBeEmpty();
    }

    [Fact]
    public void Should_Increase_Balance_When_Depositing()
    {
        Account account = new Account();
        decimal firstValueToDeposit = 10m;
        decimal secondValueDeposit = 25m;

        account.Deposit(decimal.Add(firstValueToDeposit,secondValueDeposit));

        account.Balance.Should().Be(35m);
    }

    [Fact]
    public void Should_Change_Origin_Account_Balance_When_Transfering()
    {
        Account originAccount = new Account();
        Account destinationAccount = new Account();
        decimal valueToDeposit = 10m;
        decimal valueToTransfer = 5m;

        originAccount.Deposit(valueToDeposit);
        destinationAccount.Deposit(valueToDeposit);

        originAccount.Transfer(destinationAccount, valueToDeposit);

        originAccount.Balance.Should().Be(5m);
        destinationAccount.Balance.Should().Be(15m);
    }

    [Fact]
    public void Should_Decrease_Balance_When_Withdrawing()
    {
        Account account = new Account();
        decimal valueToDeposit = 10m;
        decimal valueToWithdraw = 5m;

        account.Deposit(valueToDeposit);
        account.Withdraw(valueToWithdraw);

        account.Balance.Should().Be(5m);
    }

    [Fact]
    public void Should_Throw_ArgumentException_When_Withdrawing_Exceeding_Value()
    {
        Account account = new Account();
        decimal valueToDeposit = 10m;
        decimal valueToWithdraw = 20m;

        account.Deposit(valueToDeposit);
        account.Withdraw(valueToWithdraw);

        account.Should().Throw<ArgumentException>();
    }
}