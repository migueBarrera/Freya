namespace FreyaManager.Features.Subscriptions;

public partial class NewSubscriptionPage
{
    public NewSubscriptionPage(NewSubscriptionViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
