namespace FreyaMobile.Features.Clients.ChangePass;

public class ChangePassValidatable : BaseValidatable
{
    public ChangePassValidatable()
    {
        this.NewPass = new ValidatableObject<string>() { Value = string.Empty, };
        this.ActualPass = new ValidatableObject<string>() { Value = string.Empty, };
        this.NewPassRepeat = new ValidatableObject<string>() { Value = string.Empty, };
    }

    public ValidatableObject<string> NewPass { get; private set; } = new ValidatableObject<string>();

    public ValidatableObject<string> NewPassRepeat { get; private set; } = new ValidatableObject<string>();

    public ValidatableObject<string> ActualPass { get; private set; } = new ValidatableObject<string>();

    public override IEnumerable<ValidatableObject<string>> ValidatablesProperties()
    {
        return new ValidatableObject<string>[] { this.NewPass, this.ActualPass,this.NewPassRepeat };
    }
}
