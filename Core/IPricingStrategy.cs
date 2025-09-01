namespace Core;

public interface IPricingStrategy
{
    Money Calculate(Payment payment);
    string Name { get; } 
}

