using System.Windows;

namespace Freya.Desktop.Dialogs.Core;


public partial class SimpleDialog 
{
    public SimpleDialog()
    {
        InitializeComponent();
    }

    private SimpleViewModel? viewModel => DataContext as SimpleViewModel;

    private void Button_Yes_Click(object sender, RoutedEventArgs e)
    {
        viewModel?.Close(this.DialogId);
    }
}
