using Fintech.MiniWallet.Domain.ValueObjects;

namespace Fintech.MiniWallet.Domain.Entities;

public class Account
{
    public Guid Id {get; init;} = Guid.NewGuid();
    public required AccountHolder Holder {get; init;}
    public Money Balance {get; private set;} = new Money(0);
    private readonly List<Transaction> _transactions = new();
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    public void Deposit(Money value)
    {
        ValidateTransactionValue(value);
        Balance += value;

        _transactions.Add(new Transaction
        {
            Amount = value,
            Type = Enums.TransactionType.Deposit,
            OriginAccount = this
        });
    }
    public void Transfer(Account destinationAccount, Money value)
    {
        if (Id == destinationAccount.Id)
            throw new ArgumentException("Invalid account, please check the account and try again.");

        ValidateTransactionValue(value);
        ValidateBalanceTransactionAvailability(value, Balance);

        Balance -= value;
        destinationAccount.Deposit(value);

        _transactions.Add(new Transaction
        {
            Amount = value,
            Type = Enums.TransactionType.Transfer,
            OriginAccount = this,
            DestinationAccount = destinationAccount
        });
    }
    public void Withdraw(Money value)
    {
        ValidateBalanceTransactionAvailability(value, Balance);
        Balance -= value;

        _transactions.Add(new Transaction
        {
            Amount = value,
            Type = Enums.TransactionType.Withdraw,
            OriginAccount = this
        });
    }

    private static void ValidateTransactionValue(Money transactionValue)
    {
        if(transactionValue.Amount <= 0)
        {
            throw new ArgumentException("Transaction value must be greater than zero.");
        }
    }

    private static void ValidateBalanceTransactionAvailability(Money transactionValue, Money balance)
    {
        if(transactionValue.Amount > balance.Amount)
        {
            throw new ArgumentException("Insuficient funds, check your balance and try again.");
        }
    }
}