namespace FreyaManager.Features.Faqs;

public partial class SettingPage
{
    public SettingPage(SettingViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
