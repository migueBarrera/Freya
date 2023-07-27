using Models.Core.FAQ;

namespace FreyaManager.Features.Faqs
{
    public class EditFaqViewModel : CoreViewModel
    {
        private readonly INavigationService navigationService;
        private readonly ISessionService sessionService;
        private readonly FaqService faqService;
        private NewFaqModel faqModel;

        public EditFaqViewModel(
        INavigationService navigationService,
        FaqService faqService,
        ISessionService sessionService)
        {
            Title = "Editar pregunta frecuente";
            ShowBackButton = true;
            BackCommand = new AsyncCommand(BackCommandExecute);
            SaveFaqCommand = new AsyncCommand(SaveFaqCommandExecute);
            this.navigationService = navigationService;

            faqModel = new NewFaqModel();
            this.faqService = faqService;
            this.sessionService = sessionService;
        }

        public NewFaqModel FaqModel { get => faqModel; set => SetAndRaisePropertyChanged(ref faqModel, value); }

        public IEnumerable<int> OrderList => Enumerable.Range(1, 20);

        public override Task OnAppearingAsync(object? parameter = null)
        {
            var faq = sessionService.Get<FAQ>(SESSION.KEY_FAQ_SELECTED);
            FaqModel = NewFaqModel.ConvertTo(faq);
            return base.OnAppearingAsync(parameter);
        }

        private async Task SaveFaqCommandExecute()
        {
            IsBusy = true;
            var result = await faqService.Update(FaqModel);
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

        public IAsyncCommand SaveFaqCommand { get; set; }
    }
}
