using Freya.Validations;

namespace FreyaMobile.Features.FertileDay;

public class FertileDayModelValidatable : BaseValidatable
{
    public FertileDayModelValidatable(int fertileDay)
    {
        CicleDays = new ValidatableObject<string>() { Value = fertileDay.ToString(), };
    }

    public ValidatableObject<string> CicleDays { get; private set; } = new ValidatableObject<string>();

    public override IEnumerable<ValidatableObject<string>> ValidatablesProperties()
    {
        return new ValidatableObject<string>[] { CicleDays };
    }
}
