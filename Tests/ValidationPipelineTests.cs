using Core;
using FluentAssertions;

public class ValidationPipelineTests
{
    [Fact]
    public void Pipeline_collects_multiple_errors()
    {
        // Arrange: amount <= 0, missing currency, bad country & missing cvv (card)
        var rules = new IValidationRule[]
        {
            new AmountPositiveRule(),
            new CurrencyProvidedRule(),
            new CountryCodeIso2Rule(),
            new CardCvvRule()
        };
        var pipeline = new ValidationPipeline(rules);

        var meta = new Dictionary<string, string>(); // brak cvv
        var p = new Payment(new Money(0m, ""), PaymentMethod.Card, "pl", meta);

        // Act
        var errors = pipeline.Validate(p);

        // Assert (kolejność zgodna z kolejnością reguł)
        errors.Should().HaveCount(4);
        errors[0].Code.Should().Be("amount.positive");
        errors[1].Code.Should().Be("currency.required");
        errors[2].Code.Should().Be("country.iso2");
        errors[3].Code.Should().Be("card.cvv");
    }

    [Fact]
    public void Pipeline_can_stop_on_first_error()
    {
        var rules = new IValidationRule[]
        {
            new AmountPositiveRule(),
            new CurrencyProvidedRule()
        };
        var pipeline = new ValidationPipeline(rules, stopOnFirstError: true);

        var p = new Payment(new Money(0m, ""), PaymentMethod.Blik);

        var errors = pipeline.Validate(p);

        errors.Should().HaveCount(1);
        errors[0].Code.Should().Be("amount.positive");
    }

    [Fact]
    public void Pipeline_returns_empty_list_when_all_ok()
    {
        var rules = new IValidationRule[]
        {
            new AmountPositiveRule(),
            new CurrencyProvidedRule(),
            new CountryCodeIso2Rule(),
            new CardCvvRule()
        };
        var pipeline = new ValidationPipeline(rules);

        var meta = new Dictionary<string, string> { ["cvv"] = "123" };
        var p = new Payment(new Money(10m, "PLN"), PaymentMethod.Card, "PL", meta);

        var errors = pipeline.Validate(p);

        errors.Should().BeEmpty();
    }
}
