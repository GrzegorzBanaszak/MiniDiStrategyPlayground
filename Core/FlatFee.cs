namespace Core;


public sealed class FlatFee(Money money) : IPricingStrategy
{
    public string Name => "FlatFee";

    public Money Calculate(Payment payment)
    {
        if (money.Currency != payment.Amount.Currency)
            throw new InvalidOperationException("Currency mismatch between fee and payment.");
            
        return money;
    }
}
