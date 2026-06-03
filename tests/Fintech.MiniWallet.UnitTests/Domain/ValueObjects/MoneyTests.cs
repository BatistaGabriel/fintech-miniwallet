using Fintech.MiniWallet.Domain.ValueObjects;
using FluentAssertions;

namespace Fintech.MiniWallet.UnitTests.Domain.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Should_Throw_If_Money_Is_Negative()
    {
        Action act = () => new Money(-10);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Should_Be_Equal_When_Same_Value()
    {
        Money primaryMoney = new Money(10);
        Money secondaryMoney = new Money(10);

        primaryMoney.Should().Be(secondaryMoney);
    }

    [Fact]
    public void Should_Return_The_Sum_Of_Two_Money_Values()
    {
        Money primaryMoney = new Money(10);
        Money secondaryMoney = new Money(20);

        Money result = primaryMoney + secondaryMoney;
        result.Should().Be(new Money(30));
    }
}