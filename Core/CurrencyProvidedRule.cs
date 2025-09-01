namespace Core;

public sealed class CurrencyProvidedRule : IValidationRule
{
    public string Code => "currency.required";

    public string Description => "Currency must be provided.";

    public ValidationError? Validate(Payment payment)
    {
        return string.IsNullOrWhiteSpace(payment.Amount.Currency)
        ? new ValidationError(Code, Description) : (ValidationError?)null;
    }
}
