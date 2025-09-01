using Core;

namespace Core;

public sealed class CardCvvRule : IValidationRule
{
    public string Code => "card.cvv";

    public string Description => "Card payments require CVV (3–4 digits) in metadata under 'cvv'.";

    public ValidationError? Validate(Payment payment)
    {

        if (payment.Method != PaymentMethod.Card)
            return null;



        string? raw = null;
        if (payment.Metadata is not null)
        {
            if (!payment.Metadata.TryGetValue("cvv", out raw))
            {
                raw = payment.Metadata
                    .FirstOrDefault(kv => string.Equals(kv.Key, "cvv", StringComparison.OrdinalIgnoreCase))
                    .Value;
            }
        }

        var cvv = raw?.Trim();

        // Akceptuj wyłącznie 3 lub 4 ASCII cyfry
        bool isAsciiDigits = cvv is not null
            && (cvv.Length == 3 || cvv.Length == 4)
            && cvv.All(c => c is >= '0' and <= '9');

        return isAsciiDigits ? null : new ValidationError(Code, Description);
    }
}
