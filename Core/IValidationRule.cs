namespace Core;

public interface IValidationRule
{
    public string Code { get; }
    string Description { get; }
    ValidationError? Validate(Payment payment);
}