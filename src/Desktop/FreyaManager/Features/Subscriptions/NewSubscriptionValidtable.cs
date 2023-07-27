using Freya.Validations;
using Freya.Validations.Rules;

namespace FreyaManager.Features.Subscriptions;

public class NewSubscriptionValidtable : BaseValidatable
{
    public NewSubscriptionValidtable()
    {
        this.Name = new ValidatableObject<string>() { Value = string.Empty, };
        this.PriceMonthly = new ValidatableObject<string>() { Value = string.Empty, };
        this.PriceAnnual = new ValidatableObject<string>() { Value = string.Empty, };
        this.Description = new ValidatableObject<string>() { Value = string.Empty, };
        this.HasAnnualPrice = new ValidatableObject<bool>() { Value = false, };
        //this.Size = new ValidatableObject<string>() { Value = string.Empty, };

        Name.Validations.Add(new NotEmptyRule("El nombre no puede estar vacio"));
        Name.Validations.Add(new MaxLengthRule($"El nombre no puede superar los {250} caracteres", 250));
        Description.Validations.Add(new MaxLengthRule($"La descripción no puede superar los {1700} caracteres", 1700));
        Description.Validations.Add(new NotEmptyRule("La descripción no puede estar vacio"));
        PriceMonthly.Validations.Add(new NotEqualToZeroRule("El precio no puede ser 0"));
        PriceMonthly.Validations.Add(new MinLongValue(99,"El precio minimo es de 1€"));
        //Size.Validations.Add(new NotEqualToZeroRule("La tamaño no puede ser 0"));
    }

    public ValidatableObject<string> Name { get; private set; } = new ValidatableObject<string>();

    public ValidatableObject<string> Description { get; private set; } = new ValidatableObject<string>();

    public ValidatableObject<string> PriceMonthly { get; private set; } = new ValidatableObject<string>();

    public ValidatableObject<bool> HasAnnualPrice { get; private set; } = new ValidatableObject<bool>();

    public ValidatableObject<string> PriceAnnual { get; private set; } = new ValidatableObject<string>();

    public ValidatableObject<bool> AllowPromotionCodes { get; private set; } = new ValidatableObject<bool>();

    //public ValidatableObject<string> Size { get; private set; } = new ValidatableObject<string>();

    public override IEnumerable<ValidatableObject<string>> ValidatablesProperties()
    {
        return new ValidatableObject<string>[] { this.Name, this.PriceMonthly, this.Description };
    }
}
