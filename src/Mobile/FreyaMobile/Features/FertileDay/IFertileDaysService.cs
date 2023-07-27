namespace FreyaMobile.Features.FertileDay;

public interface IFertileDaysService
{
    IEnumerable<DateTime> CalculateFertyleDays(DateTime lastPeriod, int cicleDays);
}
