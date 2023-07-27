using System.Windows.Controls;

namespace Freya.Desktop.Core.Controls;

public partial class LoaderView : UserControl
{
    public static readonly DependencyProperty IsRunningProperty =
        DependencyProperty.Register("IsRunning", typeof(bool), typeof(LoaderView), new PropertyMetadata(false));

    public LoaderView()
    {
        InitializeComponent();
    }

    public bool IsRunning
    {
        get { return (bool)GetValue(IsRunningProperty); }
        set { SetValue(IsRunningProperty, value); }
    }
}
