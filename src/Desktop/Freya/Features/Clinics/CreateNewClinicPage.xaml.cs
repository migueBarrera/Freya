namespace Freya.Features.Clinics;

public partial class CreateNewClinicPage
{
    public CreateNewClinicPage(CreateNewClinicViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
