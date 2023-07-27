namespace FreyaManager.Features.Companies;

public partial class CompaniesPage
{
    public CompaniesPage(CompaniesViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
