namespace Freya.Validations.Rules;

public class MinLengthRule : IValidationRule<string>
{
    private int minLength;

    public MinLengthRule(string message, int minLength)
    {
        ValidationMessage = message;
        this.minLength = minLength;
    }

    public string ValidationMessage { get; set; }

    public bool Check(string value)
    {
        return value.Length >= minLength;
    }
}
