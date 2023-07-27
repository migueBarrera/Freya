namespace FreyaMobile.Features.FertileDay;

public class FertileDaysService : IFertileDaysService
{
    private const int DAYSFERTYLES = 6;

    public IEnumerable<DateTime> CalculateFertyleDays(DateTime startLastPeriod, int cicleDays)
    {
        var fertileDays = new List<DateTime>();
        var days = 31 - cicleDays;

        var endLastPeriod = startLastPeriod.AddDays(days);

        var difDays = (endLastPeriod - startLastPeriod).Days;

        var startFertilePeriodDay = difDays / 2 - 3;

        for (int i = 0; i < DAYSFERTYLES; i++)
        {
            fertileDays.Add(startLastPeriod.AddDays(startFertilePeriodDay));
            startFertilePeriodDay += 1;
        }

        return fertileDays;
    }
}
