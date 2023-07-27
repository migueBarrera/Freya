using System.Windows.Controls;
using System.Windows.Media;

namespace Freya.Desktop.Core.Controls;

public partial class IconButtonControl : Button
{
    public IconButtonControl()
    {
        InitializeComponent();
    }

    public double IconSize
    {
        get { return (double)GetValue(IconSizeProperty); }
        set { SetValue(IconSizeProperty, value); }
    }

    public static readonly DependencyProperty IconSizeProperty =
        DependencyProperty.Register("IconSize", typeof(double), typeof(IconButtonControl), new PropertyMetadata(40d));
    
    public double ButtonSize
    {
        get { return (double)GetValue(ButtonSizeProperty); }
        set { SetValue(ButtonSizeProperty, value); }
    }

    public static readonly DependencyProperty ButtonSizeProperty =
        DependencyProperty.Register("ButtonSize", typeof(double), typeof(IconButtonControl), new PropertyMetadata(18d));



    public ImageSource ImageSource
    {
        get { return (ImageSource)GetValue(ImageSourceProperty); }
        set { SetValue(ImageSourceProperty, value); }
    }

    public static readonly DependencyProperty ImageSourceProperty =
        DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(IconButtonControl), new PropertyMetadata(null));
    
    
    public ImageSource ImageSourcePressed
    {
        get { return (ImageSource)GetValue(ImageSourcePressedProperty); }
        set { SetValue(ImageSourcePressedProperty, value); }
    }

    public static readonly DependencyProperty ImageSourcePressedProperty =
        DependencyProperty.Register("ImageSourcePressed", typeof(ImageSource), typeof(IconButtonControl), new PropertyMetadata(null));
    
    public ImageSource ImageSourceDisabled
    {
        get { return (ImageSource)GetValue(ImageSourceDisabledProperty); }
        set { SetValue(ImageSourceDisabledProperty, value); }
    }

    public static readonly DependencyProperty ImageSourceDisabledProperty =
        DependencyProperty.Register("ImageSourceDisabled", typeof(ImageSource), typeof(IconButtonControl), new PropertyMetadata(null));


}
