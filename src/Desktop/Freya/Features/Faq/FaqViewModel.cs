namespace Freya.Features.Faq;

public class FaqViewModel : CoreViewModel
{
    public FaqService faqService;
    private readonly IServiceProvider serviceProvider;
    private readonly AppCenterAnalitics appCenterAnalitics;
    private ObservableCollection<FaqItemViewModel> faqs;

    public FaqViewModel(
        FaqService faqService,
        IServiceProvider serviceProvider,
        ITranslatorService translatorService,
        AppCenterAnalitics appCenterAnalitics)
    {
        Title = translatorService.Translate("page_title_faq");
        this.faqService = faqService;
        this.faqs = new ObservableCollection<FaqItemViewModel>();
        this.serviceProvider = serviceProvider;
        this.appCenterAnalitics = appCenterAnalitics;

        appCenterAnalitics.PageView(nameof(FaqViewModel));
    }

    public ObservableCollection<FaqItemViewModel> Faqs { get => faqs; set => SetAndRaisePropertyChanged(ref faqs, value); }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync(parameter);
        await GetFaqs();
    }

    private async Task GetFaqs()
    {
        var items = await faqService.GetAll();
        Faqs.Clear();
        foreach (var item in items)
        {
            var vm = serviceProvider.GetService<FaqItemViewModel>();
            if (vm != null)
            {
                await vm.OnAppearingAsync(item);
                Faqs.Add(vm);
            }
        }
    }
}
