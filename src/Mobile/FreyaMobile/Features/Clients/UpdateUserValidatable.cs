namespace FreyaMobile.Features.Clients;

public class UpdateUserValidatable : BaseValidatable
{
    public UpdateUserValidatable()
    {
        Name = new ValidatableObject<string>() { Value = string.Empty, };
        LastName = new ValidatableObject<string>() { Value = string.Empty, };
        Phone = new ValidatableObject<string>() { Value = string.Empty, };
    }

    public ValidatableObject<string> LastName { get; private set; } = new ValidatableObject<string>();

    public ValidatableObject<string> Name { get; private set; } = new ValidatableObject<string>();

    public ValidatableObject<string> Phone { get; private set; } = new ValidatableObject<string>();
    public int Weeks { get; set; }

    public override IEnumerable<ValidatableObject<string>> ValidatablesProperties()
    {
        return new ValidatableObject<string>[] { Name, Phone, LastName };
    }
}
