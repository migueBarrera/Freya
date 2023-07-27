namespace Freya.Validations;

public class ValidatableObject<T> : INotifyPropertyChanged
{
    private List<object> errors;
    private T innerValue = default!;
    private bool isValid;
    public event PropertyChangedEventHandler? PropertyChanged;

    public List<IValidationRule<T>> Validations { get; }

    public ValidatableObject()
    {
        isValid = true;
        Validations = new List<IValidationRule<T>>();
        errors = new List<object>();
    }

    public List<object> Errors
    {
        get => errors;

        set => this.SetAndRaisePropertyChanged(ref errors, value);
    }

    public T Value
    {
        get => innerValue;

        set
        {
            this.SetAndRaisePropertyChanged(ref innerValue, value);
        }
    }

    /// <inheritdoc/>
    public bool IsValid
    {
        get => isValid;

        set => this.SetAndRaisePropertyChanged(ref isValid, value);
    }

    public bool Validate()
    {
        Errors.Clear();

        var errors = Validations.Where(v => !v.Check(Value))
            .Select(v => v.ValidationMessage);

        foreach (var error in errors)
        {
            Errors.Add(error);
        }

        IsValid = !Errors.Any();

        return IsValid;
    }

    protected bool SetAndRaisePropertyChanged<TRef>(
    ref TRef field, TRef value, [CallerMemberName] string? propertyName = null)
    {
        if (field == null && value == null)
        {
            return false;
        }

        if (field == null || !field.Equals(value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        return false;
    }
}
