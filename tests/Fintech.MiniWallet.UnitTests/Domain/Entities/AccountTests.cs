using Fintech.MiniWallet.Domain.Entities;
using Fintech.MiniWallet.Domain.ValueObjects;
using FluentAssertions;

namespace Fintech.MiniWallet.UnitTests.Domain.Entities;

public class AccountTests
{
    [Fact]
    public void Should_Have_Zero_Balance_When_Account_Is_Created()
    {
        Account account = AccountInitializer(new AccountObject());

        account.Balance.Should().Be(new Money(0));
    }

    [Fact]
    public void Should_Have_An_Account_Holder_When_Account_Is_Created()
    {
        string accountHolderName = "Jhon Doe";
        string accountDocument = "12345678900";

        AccountObject accountObject = new AccountObject();
        accountObject.Holder = new AccountHolder { Name = accountHolderName, Document = accountDocument };

        Account account = AccountInitializer(accountObject);

        account.Holder.Name.Should().Be(accountHolderName);
    }

    [Fact]
    public void Should_Increase_Balance_When_Depositing()
    {
        Account account = AccountInitializer(new AccountObject());

        Money firstValueToDeposit = new Money(10m);
        Money secondValueDeposit = new Money(25m);

        account.Deposit(firstValueToDeposit);
        account.Deposit(secondValueDeposit);

        account.Balance.Should().Be(new Money(35m));
    }

    [Fact]
    public void Should_Change_Origin_Account_Balance_When_Transfering()
    {
        AccountObject firstAccountObject = new AccountObject();
        firstAccountObject.Balance = new Money(10m);

        AccountObject secondAccountObject = new AccountObject();
        secondAccountObject.Balance = new Money(10m);

        Account originAccount = AccountInitializer(firstAccountObject);
        Account destinationAccount = AccountInitializer(secondAccountObject);

        Money valueToTransfer = new Money(5m);
        originAccount.Transfer(destinationAccount, valueToTransfer);

        originAccount.Balance.Should().Be(new Money(5m));
        destinationAccount.Balance.Should().Be(new Money(15m));
    }

    [Fact]
    public void Should_Decrease_Balance_When_Withdrawing()
    {
        Account account = AccountInitializer(new AccountObject());
        account.Deposit(new Money(10m));

        account.Withdraw(new Money(5m));

        account.Balance.Should().Be(new Money(5m));
    }

    [Fact]
    public void Should_Throw_ArgumentException_When_Withdrawing_Exceeding_Value()
    {
        AccountObject accountObject = new AccountObject();
        Account account = AccountInitializer(accountObject);

        Money valueToWithdraw = new Money(20m);
        Action act = () => account.Withdraw(valueToWithdraw);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Should_Throw_When_Transferring_To_Same_Account()
    {
        AccountObject accountObject = new AccountObject();
        Account account = AccountInitializer(accountObject);

        Action act = () => account.Transfer(account, new Money(5m));

        act.Should().Throw<ArgumentException>();
    }

    private Account AccountInitializer(AccountObject accountObject)
    {
        Account initializedAccount = new Account { Holder = accountObject.Holder };

        if (accountObject.Balance.Amount > 0)
            initializedAccount.Deposit(accountObject.Balance);

        return initializedAccount;
    }
}

public class AccountObject
{
    public AccountHolder Holder { get; set; } = new AccountHolder { Name = "Unknown", Document = "12345678900" };
    public Money Balance { get; set; } = new Money(0);
}