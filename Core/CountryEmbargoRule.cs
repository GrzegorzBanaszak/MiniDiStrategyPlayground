using Core;

namespace Core;

public sealed class CountryEmbargoRule : IValidationRule
{
    public string Code => "country.embargo";

    public string Description => "Payments from {0} are not allowed.";
    private string[] CountryList;

    public CountryEmbargoRule(string[] countryList)
    {
        CountryList = countryList;
    }

    public ValidationError? Validate(Payment payment)
    {
        if (string.IsNullOrWhiteSpace(payment.CountryCode))
            return null;

        if (CountryList.Contains(payment.CountryCode))
            return new ValidationError(Code, string.Format(Description, payment.CountryCode));

        return null;
    }
}
