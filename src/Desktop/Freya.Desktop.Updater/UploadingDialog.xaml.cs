namespace Freya.Desktop.Updater;

public partial class UploadingDialog  : CoreModalDialog
{
    public UploadingDialog(UploadingDialogViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();

        Loaded += DialogLoaded;
    }

    private async void DialogLoaded(object sender, RoutedEventArgs e)
    {
        await ((UploadingDialogViewModel)DataContext).Initialize();
    }
}
