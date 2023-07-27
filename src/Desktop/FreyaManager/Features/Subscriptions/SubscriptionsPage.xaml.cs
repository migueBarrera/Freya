namespace FreyaManager.Features.Subscriptions
{
    public partial class SubscriptionsPage
    {
        public SubscriptionsPage(SubscriptionsViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
