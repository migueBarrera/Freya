namespace Freya.Validations;

public abstract class BaseValidatable
{
    public abstract IEnumerable<ValidatableObject<string>> ValidatablesProperties();

    public bool IsValid()
    {
        var validatablesProperties = ValidatablesProperties();
        foreach (var validatable in validatablesProperties)
        {
            validatable.Validate();
        }
        var isValidItems = validatablesProperties.Select(property => property.IsValid);

        return isValidItems.All(isValidItem => isValidItem);
    }

    public bool HasError(out string error)
    {
        error = string.Empty;

        if (IsValid())
        {
            return false;
        }

        error = AppendErrors();

        return true;
    }

    private string AppendErrors()
    {
        var validatablesProperties = ValidatablesProperties();
        var errors = validatablesProperties
            .Where(property => property.Errors != null)
            .SelectMany(property => property.Errors);

        var errorBuilder = new StringBuilder();

        foreach (var errorObject in errors)
        {
            if (errorObject is string error)
            {
                errorBuilder.AppendLine(error.ToString());
            }
        }

        return errorBuilder.ToString();
    }
}
