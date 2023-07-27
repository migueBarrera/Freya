using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;

namespace Freya.Features.Main;

/// <summary>
/// Interaction logic for LateralMenu.xaml
/// </summary>
public partial class LateralMenu : UserControl
{
    public static readonly DependencyProperty ShowBackButtonProperty =
   DependencyProperty.Register("ShowBackButton", typeof(bool), typeof(LateralMenu), new PropertyMetadata(false));

    public LateralMenu()
    {
        InitializeComponent();

        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        VersionTag.Text = $"{version} v";
    }

    public bool ShowBackButton
    {
        get { return (bool)GetValue(ShowBackButtonProperty); }
        set { SetValue(ShowBackButtonProperty, value); }
    }

    private LateralMenuViewModel ViewModel
    {
        get { return (LateralMenuViewModel)DataContext; }
    }

    private void Grid_TouchUp(object sender, TouchEventArgs e)
    {
        InvokeShowMenuCommand();
    }

    private void InvokeShowMenuCommand()
    {
        if (ViewModel.ShowMenuCommand != null)
        {
            ViewModel.ShowMenuCommand.Execute(null);
        }
    }

    private void Grid_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        InvokeShowMenuCommand();
    }

    private async void Grid_back_MouseUp(object sender, MouseButtonEventArgs e)
    {
        await ViewModel.GoBack();
    }

    private async void Grid_back_TouchUp(object sender, TouchEventArgs e)
    {
        await ViewModel.GoBack();
    }
}
