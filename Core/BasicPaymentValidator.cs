namespace Core;

/// <summary>
/// Minimalna walidacja Payment – bez DI, bez strategii.
/// </summary>

public sealed class BasicPaymentValidator
{
    /// <summary>
    /// Sprawdza podstawowe reguły. Zwraca listę błędów (pusta = brak błędów).
    /// </summary>
    public IReadOnlyList<string> Validate(Payment payment)
    {
        var errors = new List<string>();

        if (payment.Amount.Amount <= 0)
            errors.Add("Amount must be greater than 0.");

        if (string.IsNullOrWhiteSpace(payment.Amount.Currency))
            errors.Add("Currency must be provided.");

        if (!Enum.IsDefined(typeof(PaymentMethod), payment.Method))
            errors.Add("Payment method is invalid.");

        if (!string.IsNullOrWhiteSpace(payment.CountryCode))
        {
            var cc = payment.CountryCode!;
            // prosta walidacja ISO2: dokładnie 2 wielkie litery
            if (cc.Length != 2 || !cc.All(char.IsUpper) || !cc.All(char.IsLetter))
                errors.Add("CountryCode must be a 2-letter uppercase ISO code (e.g., 'PL').");
        }

        return errors;
    }
}