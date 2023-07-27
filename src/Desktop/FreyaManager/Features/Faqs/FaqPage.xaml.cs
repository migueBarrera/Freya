namespace FreyaManager.Features.Faqs;

public partial class FaqPage
{
    public FaqPage(FaqViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
