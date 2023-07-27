using Freya.Validations;
using Freya.Validations.Rules;

namespace FreyaManager.Features.Companies.Models;

public class UpdateCompanyValitable : BaseValidatable
{
    public UpdateCompanyValitable()
    {
        this.Name = new ValidatableObject<string>() { Value = string.Empty, };

        Name.Validations.Add(new NotEmptyRule("El nombre no puede estar vacio"));
        Name.Validations.Add(new MaxLengthRule($"El nombre no puede superar los {250} caracteres", 250));
    }

    public ValidatableObject<string> Name { get; private set; } = new ValidatableObject<string>();

    public override IEnumerable<ValidatableObject<string>> ValidatablesProperties()
    {
        return new ValidatableObject<string>[] { this.Name };
    }
}
