namespace Freya.Validations.Rules;

public class MinLongValue : IValidationRule<string>
{
    private long minValue;
    public MinLongValue(long minValue, string validationMessage)
    {
        ValidationMessage = validationMessage;
        this.minValue = minValue;
    }

    public string ValidationMessage { get; set; } = string.Empty;

    public bool Check(string value)
    {
        if (long.TryParse(value, out var result))
        {
            return result > minValue;
        }
        else
        {
            return false;
        }
    }
}
