namespace FreyaManager.Features.Faqs;

public class FaqViewModel : CoreViewModel
{
    private readonly INavigationService navigationService;
    private readonly IServiceProvider serviceProvider;
    private readonly FaqService faqService;
    private ObservableCollection<FaqItemViewModel> faqs;

    public FaqViewModel(
        INavigationService navigationService,
        FaqService faqService,
        IServiceProvider serviceProvider)
    {
        Title = "Preguntas frecuentes";
        faqs = new ObservableCollection<FaqItemViewModel>();
        NewFaqCommand = new AsyncCommand(NewFaqCommandExecute);
        this.navigationService = navigationService;
        this.faqService = faqService;
        this.serviceProvider = serviceProvider;
    }

    private Task NewFaqCommandExecute()
    {
        return navigationService.NavigateTo(typeof(NewFaqPage));
    }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync(parameter);
        await GetFaqs();
    }

    private async Task GetFaqs()
    {
        IsBusy = true;
        var items = await faqService.GetAll();
        Faqs.Clear();
        IsBusy = false;
        foreach (var item in items)
        {
            var vm = serviceProvider.GetService<FaqItemViewModel>();
            if (vm != null)
            {
                await vm.OnAppearingAsync(item);
                vm.Parent = this;
                Faqs.Add(vm);
            }
        }
    }

    public void RemoveItem(Guid id)
    {
        var itemForRemove = Faqs.Where(x => x.Data!.Id == id).FirstOrDefault();
        if (itemForRemove != null)
        {
            Faqs.Remove(itemForRemove);
        }
    }

    public IAsyncCommand NewFaqCommand { get; set; }

    public ObservableCollection<FaqItemViewModel> Faqs { get => faqs; set => SetAndRaisePropertyChanged(ref faqs, value); }
}
