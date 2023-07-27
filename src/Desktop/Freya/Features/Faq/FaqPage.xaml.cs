namespace Features.Faq;

public partial class FaqPage
{
    public FaqPage(FaqViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
