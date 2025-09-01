namespace Core;

public readonly record struct Payment(
    Money Amount,
    PaymentMethod Method,
    string? CountryCode = null,
    IReadOnlyDictionary<string,string>? Metadata = null);