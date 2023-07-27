using System.Windows.Controls;
namespace Freya.Features.Main;

public partial class HeaderControl : UserControl
{

    public static readonly DependencyProperty ShowClinicSelectorProperty =
   DependencyProperty.Register("ShowClinicSelector", typeof(bool), typeof(HeaderControl), new PropertyMetadata(false));
    
    public static readonly DependencyProperty TitleProperty =
   DependencyProperty.Register("Title", typeof(string), typeof(HeaderControl), new PropertyMetadata(string.Empty));

    public HeaderControl()
    {
        InitializeComponent();
    }

    public bool ShowClinicSelector
    {
        get { return (bool)GetValue(ShowClinicSelectorProperty); }
        set { SetValue(ShowClinicSelectorProperty, value); }
    }
    
    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    private void Image_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        TooltipPopup.IsOpen = false;
        TooltipPopup.IsOpen = true;
    }
}
