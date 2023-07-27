namespace Freya.Validations;

public interface IValidatableInput
{
    bool IsValid { get; }

    void MarkAsValid();

    void MarkAsInvalid();
}
