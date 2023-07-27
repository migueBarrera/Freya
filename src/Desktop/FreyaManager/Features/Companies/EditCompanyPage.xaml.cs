namespace FreyaManager.Features.Companies;

public partial class EditCompanyPage
{
    public EditCompanyPage(EditCompanyViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
