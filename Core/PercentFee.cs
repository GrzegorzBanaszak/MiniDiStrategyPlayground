namespace Core;

public sealed class PercentFee(decimal percent) : IPricingStrategy
{
    public string Name => "PercentFee";

    public Money Calculate(Payment payment)
    {
        var feeAmount = payment.Amount.Amount * (percent / 100m);
        return new Money(feeAmount, payment.Amount.Currency);
    }
}
