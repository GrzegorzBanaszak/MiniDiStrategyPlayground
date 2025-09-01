
namespace Core;

public sealed class AmountPositiveRule : IValidationRule
{
    public string Code => "amount.positive";
    public string Description => "Payment amount must be greater than 0.";

    public ValidationError? Validate(Payment payment)
    {
        return payment.Amount.Amount <= 0m
        ? new ValidationError(Code, Description) : (ValidationError?)null;
    }
}
