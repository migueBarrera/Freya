namespace FreyaManager.Features.Faqs;

public class NewFaqViewModel : CoreViewModel
{
    private readonly INavigationService navigationService;
    private readonly FaqService faqService;
    private NewFaqModel faqModel;

    public NewFaqViewModel(
        INavigationService navigationService, 
        FaqService faqService)
    {
        Title = "Nueva pregunta frecuente";
        ShowBackButton = true;
        BackCommand = new AsyncCommand(BackCommandExecute);
        CreateFaqCommand = new AsyncCommand(CreateFaqCommandExecute);
        this.navigationService = navigationService;

        faqModel = new NewFaqModel();
        this.faqService = faqService;
    }

    public NewFaqModel FaqModel { get => faqModel; set => SetAndRaisePropertyChanged(ref faqModel, value); }

    public IEnumerable<int> OrderList => Enumerable.Range(1, 20);

    private async Task CreateFaqCommandExecute()
    {
        IsBusy = true;
        var result = await faqService.Create(FaqModel);
        IsBusy = false;
        if (result)
        {
            BackCommand.Execute(null);
        }
    }

    private Task BackCommandExecute()
    {
        return navigationService.BackAsync();
    }

    public IAsyncCommand BackCommand { get; set; }

    public IAsyncCommand CreateFaqCommand { get; set; }
}
