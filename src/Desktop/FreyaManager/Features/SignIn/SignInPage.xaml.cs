namespace FreyaManager.Features.SignIn;

public partial class SignInPage
{
    public SignInPage(
        SignInViewModel viewModel, 
        AppVersionService appVersionService,
        IConfiguration configuration)
    {
        DataContext = viewModel;
        InitializeComponent();

        bool isProductionEvironment = configuration.GetValue<bool>("IsProductionEvironment");
        string textDevelopEvironment = isProductionEvironment ? string.Empty : "Dev";

        var version = appVersionService.GetAppVersion();
        VersionTag.Text = $"{version.Major}.{version.Minor}.{version.Build} v {textDevelopEvironment}";
    }
}
