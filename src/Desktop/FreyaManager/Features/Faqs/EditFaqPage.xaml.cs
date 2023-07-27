namespace FreyaManager.Features.Faqs;

public partial class EditFaqPage
{
    public EditFaqPage(EditFaqViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
