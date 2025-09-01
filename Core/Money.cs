namespace Core;

public readonly record struct Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    // Przykładowa metoda pomocnicza
    public Money Round(int digits = 2) =>
        new(decimal.Round(Amount, digits), Currency);

    public override string ToString() => $"{Amount:0.00} {Currency}";
}