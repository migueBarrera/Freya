namespace FreyaManager.Features.Faqs;

public partial class NewFaqPage
{
    public NewFaqPage(NewFaqViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
