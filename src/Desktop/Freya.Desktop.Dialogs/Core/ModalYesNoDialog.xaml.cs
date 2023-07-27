using System.Windows.Input;

namespace Freya.Desktop.Dialogs.Core;

public partial class ModalYesNoDialog
{
    public ModalYesNoDialog()
    {
        InitializeComponent();
    }

    private ModalYesNoViewModel? viewModel => DataContext as ModalYesNoViewModel;

    public ICommand? YesCommand { get; set; }

    public ICommand? NoCommand { get; set; }

    private void Button_Yes_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (YesCommand != null)
        {
            YesCommand.Execute(sender);
        }

        viewModel?.Close(this.DialogId);
    }

    private void Button_No_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (NoCommand != null)
        {
            NoCommand.Execute(sender);
        }

        viewModel?.Close(this.DialogId);
    }
}
