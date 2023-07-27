using Freya.Validations;
using Freya.Validations.Rules;
using Models.Core.Employees;

namespace FreyaManager.Features.Employees;

public class NewEmployeeValidatable : BaseValidatable
{
    public NewEmployeeValidatable()
    {
        this.EmployeeName = new ValidatableObject<string>() { Value = string.Empty, };
        this.EmployeeEmail = new ValidatableObject<string>() { Value = string.Empty, };
        this.EmployeeLastName = new ValidatableObject<string>() { Value = string.Empty, };

        EmployeeEmail.Validations.Add(new EmailValidationRule("Revise el email"));
        EmployeeLastName.Validations.Add(new NotEmptyRule("Revise el apellido"));
        EmployeeEmail.Validations.Add(new NotEmptyRule("Revise el nombre"));
    }

    public ValidatableObject<string> EmployeeName { get; private set; } = new ValidatableObject<string>();
    public ValidatableObject<string> EmployeeEmail { get; private set; } = new ValidatableObject<string>();
    public ValidatableObject<string> EmployeeLastName { get; private set; } = new ValidatableObject<string>();
    public ValidatableObject<EmployeeRol> RolSelected { get; private set; } = new ValidatableObject<EmployeeRol>();

    public override IEnumerable<ValidatableObject<string>> ValidatablesProperties()
    {
        return new ValidatableObject<string>[] { this.EmployeeName, this.EmployeeEmail, this.EmployeeLastName };
    }
}

public class CapsuleRol
{
    public string Text { get; set; } = string.Empty;
    public EmployeeRol Rol { get; set; }
}
