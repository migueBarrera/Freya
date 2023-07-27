namespace Freya.Features.Clinics;

public partial class ClinicsPage
{
    public ClinicsPage(ClinicsViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
