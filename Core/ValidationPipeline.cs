namespace Core;

public sealed class ValidationPipeline
{
    public IReadOnlyCollection<IValidationRule> ValidationRules { get; private set; }
    public bool StopOnFirstError { get; private set; }

    public ValidationPipeline(IReadOnlyCollection<IValidationRule> validationRules, bool stopOnFirstError = false)
    {
        ValidationRules = validationRules;
        StopOnFirstError = stopOnFirstError;
    }

    public IReadOnlyList<ValidationError> Validate(Payment payment)
    {
        List<ValidationError> errors = new List<ValidationError>();

        foreach (var rule in ValidationRules)
        {
            var error = rule.Validate(payment);
            if (error != null)
            {
                errors.Add(error.Value);

                if (StopOnFirstError) return errors;

            }
        }

        return errors;
    }
}