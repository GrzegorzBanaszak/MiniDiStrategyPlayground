using Core;
using FluentAssertions;

public class ValidationRulesTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-0.01)]
    public void AmountPositiveRule_rejects_non_positive_amounts(decimal invalid)
    {
        // Arrange
        var rule = new AmountPositiveRule();
        var p = new Payment(new Money(invalid, "PLN"), PaymentMethod.Transfer);

        // Act
        var error = rule.Validate(p);

        // Assert
        error.Should().NotBeNull();
        error!.Value.Code.Should().Be("amount.positive");
    }

    [Fact]
    public void AmountPositiveRule_accepts_positive_amount()
    {
        var rule = new AmountPositiveRule();
        var p = new Payment(new Money(1.00m, "PLN"), PaymentMethod.Transfer);

        var error = rule.Validate(p);

        error.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void CurrencyProvidedRule_requires_non_empty_currency(string bad)
    {
        var rule = new CurrencyProvidedRule();
        var p = new Payment(new Money(10m, bad), PaymentMethod.Blik);

        var error = rule.Validate(p);

        error.Should().NotBeNull();
        error!.Value.Code.Should().Be("currency.required");
    }

    [Theory]
    [InlineData("pl")]  // lower
    [InlineData("POL")] // 3 chars
    [InlineData("1P")]  // digit
    public void CountryCodeIso2Rule_validates_iso2_when_present(string bad)
    {
        var rule = new CountryCodeIso2Rule();
        var p = new Payment(new Money(10m, "PLN"), PaymentMethod.Cash, bad);

        var error = rule.Validate(p);

        error.Should().NotBeNull();
        error!.Value.Code.Should().Be("country.iso2");
    }

    [Fact]
    public void CountryCodeIso2Rule_ignores_when_missing()
    {
        var rule = new CountryCodeIso2Rule();
        var p = new Payment(new Money(10m, "PLN"), PaymentMethod.Cash, null);

        var error = rule.Validate(p);

        error.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("12")]     // too short
    [InlineData("12345")]  // too long
    [InlineData("12a")]    // non-digit
    public void CardCvvRule_requires_3_to_4_digits_for_card(string? cvv)
    {
        var rule = new CardCvvRule();
        var meta = cvv is null ? null : new Dictionary<string, string> { ["cvv"] = cvv };
        var p = new Payment(new Money(100m, "PLN"), PaymentMethod.Card, "PL", meta);

        var error = rule.Validate(p);

        error.Should().NotBeNull();
        error!.Value.Code.Should().Be("card.cvv");
    }

    [Theory]
    [InlineData("123")]
    [InlineData("1234")]
    public void CardCvvRule_accepts_3_or_4_digits_for_card(string cvv)
    {
        var rule = new CardCvvRule();
        var meta = new Dictionary<string, string> { ["cvv"] = cvv };
        var p = new Payment(new Money(100m, "PLN"), PaymentMethod.Card, "PL", meta);

        var error = rule.Validate(p);

        error.Should().BeNull();
    }

    [Fact]
    public void CardCvvRule_does_nothing_for_non_card_methods()
    {
        var rule = new CardCvvRule();
        var p = new Payment(new Money(100m, "PLN"), PaymentMethod.Transfer);

        var error = rule.Validate(p);

        error.Should().BeNull();
    }

    [Fact]
    public void CountryEmbargoRule_blocks_configured_countries()
    {
        var rule = new CountryEmbargoRule(new[] { "RU", "KP", "IR" });
        var p = new Payment(new Money(5m, "PLN"), PaymentMethod.Cash, "RU");

        var error = rule.Validate(p);

        error.Should().NotBeNull();
        error!.Value.Code.Should().Be("country.embargo");
        error.Value.Message.Should().Contain("RU");
    }

    [Fact]
    public void CountryEmbargoRule_allows_not_listed_countries()
    {
        var rule = new CountryEmbargoRule(new[] { "RU", "KP", "IR" });
        var p = new Payment(new Money(5m, "PLN"), PaymentMethod.Cash, "PL");

        var error = rule.Validate(p);

        error.Should().BeNull();
    }
}
