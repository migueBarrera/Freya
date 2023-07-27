namespace Freya.Features.Clinics;

public partial class ClinicDetailPage
{
    public ClinicDetailPage(ClinicDetailViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
