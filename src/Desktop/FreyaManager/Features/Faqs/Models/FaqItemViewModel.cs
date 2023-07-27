using Models.Core.FAQ;

namespace FreyaManager.Features.Faqs.Models;

public class FaqItemViewModel : CoreViewModel
{
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;
    private readonly ISessionService sessionService;
    private readonly FaqService faqService;
    private bool isOpen;

    public FAQ? Data { get; set; }

    public FaqItemViewModel(
        IDialogService dialogService,
        INavigationService navigationService,
        FaqService faqService,
        ISessionService sessionService)
    {
        OpenCommand = new AsyncCommand(OpenCommandExecute);
        DeleteCommand = new AsyncCommand(DeleteCommandExecute);
        EditCommand = new AsyncCommand(EditCommandExecute);
        this.dialogService = dialogService;
        this.navigationService = navigationService;
        this.faqService = faqService;
        this.sessionService = sessionService;
    }

    public string Main { get; set; } = string.Empty;

    public string Desc { get; set; } = string.Empty;

    private Task OpenCommandExecute()
    {
        IsOpen = !IsOpen;
        return Task.CompletedTask;
    }
    
    private async Task DeleteCommandExecute()
    {
        await dialogService.ShowMessageYesNo(
            "Borrar pregunta",
            "¿Estas seguro que quieres eliminar esta pregunta frecuente?",
            new AsyncCommand(UserWantDeleteThisQuestions));
    }

    private async Task UserWantDeleteThisQuestions()
    {
        IsBusy = true;
        var resultDelete = await faqService.Delete(Data!.Id);
        IsBusy = false;
        if (resultDelete)
        {
            (Parent as FaqViewModel)?.RemoveItem(Data.Id);
        }
    }

    private async Task EditCommandExecute()
    {
        sessionService.Save(SESSION.KEY_FAQ_SELECTED, Data);
        await navigationService.NavigateTo(typeof(EditFaqPage));
    }

    public override Task OnAppearingAsync(object? parameter = null)
    {
        Data = parameter is FAQ ? (FAQ)parameter : new FAQ();
        Main = Data.GetTitle();
        Desc = Data.GetDesc();
        return base.OnAppearingAsync(parameter);
    }

    public bool IsOpen { get => isOpen; set => SetAndRaisePropertyChanged(ref isOpen , value); }

    public IAsyncCommand OpenCommand { get; set; }

    public IAsyncCommand DeleteCommand { get; set; }

    public IAsyncCommand EditCommand { get; set; }
}
