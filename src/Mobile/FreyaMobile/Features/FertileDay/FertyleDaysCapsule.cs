namespace FreyaMobile.Features.FertileDay;

public class FertyleDaysCapsule
{
    public IEnumerable<DateTime> FertyleDays { get; set; }
    public DateTime StartLastPeriod { get; set; }
    public int CicleDays { get; set; }
}
