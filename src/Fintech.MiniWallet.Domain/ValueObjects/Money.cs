namespace Fintech.MiniWallet.Domain.ValueObjects;

public record Money
{
    public decimal Amount {get; }

    public Money(decimal amount)
    {
        if(amount < 0)
        {
            throw new ArgumentException("Invalid value declared for the property `Amount`, please provide a valid value.");
        }

        Amount = amount;
    }

    public static Money operator + (Money first, Money second)
    {
        decimal summedValues = decimal.Add(first.Amount, second.Amount);
        Money result = new Money(summedValues);
        
        return result;
    }
}