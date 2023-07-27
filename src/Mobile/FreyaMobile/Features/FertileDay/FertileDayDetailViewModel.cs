using FreyaMobile.Core.Framework;

namespace FreyaMobile.Features.FertileDay;

public class FertileDayDetailViewModel : CoreViewModel
{
    private FertyleDaysCapsule fertyleDays;
    private readonly ISessionService sessionService;

    public FertileDayDetailViewModel(ISessionService sessionService)
    {
        this.sessionService = sessionService;
    }

    public FertyleDaysCapsule FertyleDays { get => fertyleDays; set => SetAndRaisePropertyChanged(ref fertyleDays, value); }

    public override Task OnAppearingAsync()
    {
        FertyleDays = sessionService.Get<FertyleDaysCapsule>(SESSION.FERTILE_DAYS_CAPSULE);
        return base.OnAppearingAsync();
    }
}

