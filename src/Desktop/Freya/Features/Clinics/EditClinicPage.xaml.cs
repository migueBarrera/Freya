namespace Features.Clinics;

public partial class EditClinicPage 
{
    public EditClinicPage(EditClinicViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
