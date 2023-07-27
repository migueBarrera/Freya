using FreyaMobile.Core.Framework;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;

namespace FreyaMobile.Features.FertileDay;

public class FertileDayViewModel : CoreViewModel
{
    private readonly IFertileDaysService fertileDaysService;
    private readonly ISessionService sessionService;
    private DateTime selectedDate;
    private FertileDayModelValidatable fertileDayModel;

    public FertileDayViewModel(
        IFertileDaysService fertileDaysService, ISessionService sessionService)
    {
        this.fertileDaysService = fertileDaysService;

        selectedDate = DateTime.UtcNow;
        fertileDayModel = new FertileDayModelValidatable(4);

        CalculateFertileDaysCommand = new AsyncCommand(CalculateFertileDaysAsync);
        this.sessionService = sessionService;
    }

    public IAsyncCommand CalculateFertileDaysCommand { get; set; }

    public DateTime SelectedDate { get => selectedDate; set => SetAndRaisePropertyChanged(ref selectedDate, value); }

    public FertileDayModelValidatable FertileDayModel { get => fertileDayModel; set => SetAndRaisePropertyChanged(ref fertileDayModel, value); }

    private async Task CalculateFertileDaysAsync()
    {
        if (fertileDayModel.HasError(out var error))
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                error,
                "Cerrar");
            return;
        }

        var cicleDaysInt = int.Parse(fertileDayModel.CicleDays.Value);

        var fertileDays = fertileDaysService.CalculateFertyleDays(selectedDate, cicleDaysInt);

        var capsule = new FertyleDaysCapsule()
        {
            CicleDays = cicleDaysInt,
            FertyleDays = fertileDays,
            StartLastPeriod = selectedDate,
        };

        //var page = new FertileDayDetailPage
        //{
        //    Parameter = new FertyleDaysCapsule()
        //    {
        //        CicleDays = cicleDaysInt,
        //        FertyleDays = fertileDays,
        //        StartLastPeriod = selectedDate,
        //    }
        //};

        sessionService.Save(SESSION.FERTILE_DAYS_CAPSULE, capsule);
        await Shell.Current.GoToAsync(nameof(FertileDayDetailPage));
        //await App.Current.MainPage.Navigation.PushAsync(page);
    }
}