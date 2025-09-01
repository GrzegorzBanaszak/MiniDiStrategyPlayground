namespace Core;

public readonly record struct ValidationError(string Code,string Message)
{
    public override string ToString() => $"[{Code}] {Message}";
}