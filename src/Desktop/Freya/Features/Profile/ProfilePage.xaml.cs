namespace Freya.Features.Profile;

public partial class ProfilePage
{
    public ProfilePage(ProfileViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
