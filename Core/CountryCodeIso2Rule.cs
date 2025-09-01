using System.Text.RegularExpressions;

namespace Core;

public sealed class CountryCodeIso2Rule : IValidationRule
{
    public string Code => "country.iso2";

    public string Description => "CountryCode must be a 2-letter uppercase ISO code, e.g., 'PL'.";

    public ValidationError? Validate(Payment payment)
    {
        if (string.IsNullOrWhiteSpace(payment.CountryCode))
            return null;

        if (!Regex.IsMatch(payment.CountryCode, "^[A-Z]{2}$"))
            return new ValidationError(Code, Description);

        return null;
    }
}
