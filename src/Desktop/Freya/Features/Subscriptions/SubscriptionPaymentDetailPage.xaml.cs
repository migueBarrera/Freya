
namespace Freya.Features.Subscriptions;

public partial class SubscriptionPaymentDetailPage
{
    public SubscriptionPaymentDetailPage(SubscriptionPaymentDetailViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
