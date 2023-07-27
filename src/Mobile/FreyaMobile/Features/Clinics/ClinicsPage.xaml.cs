namespace FreyaMobile.Features.Clinics;

public partial class ClinicsPage
{
    public ClinicsPage(ClinicsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}