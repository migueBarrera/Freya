using System.Windows.Controls;
using System.Windows.Media;

namespace Freya.Desktop.Core.Controls;

public partial class TextImageButton : Button
{
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(TextImageButton),
            new PropertyMetadata("default text"));

    public static readonly DependencyProperty StartImageSourceProperty =
         DependencyProperty.Register(
             "StartImageSource",
             typeof(ImageSource),
             typeof(TextImageButton),
             new PropertyMetadata(null));

    public static readonly DependencyProperty EndImageSourceProperty =
        DependencyProperty.Register(
            "EndImageSource",
            typeof(ImageSource),
            typeof(TextImageButton),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ImageWidthProperty =
        DependencyProperty.Register(
            "ImageWidth",
            typeof(double),
            typeof(TextImageButton),
            new PropertyMetadata(49d));

    public TextImageButton()
    {
        InitializeComponent();
    }

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public ImageSource StartImageSource
    {
        get { return (ImageSource)GetValue(StartImageSourceProperty); }
        set { SetValue(StartImageSourceProperty, value); }
    }

    public ImageSource EndImageSource
    {
        get { return (ImageSource)GetValue(EndImageSourceProperty); }
        set { SetValue(EndImageSourceProperty, value); }
    }

    public double ImageWidth
    {
        get { return (double)GetValue(ImageWidthProperty); }
        set { SetValue(ImageWidthProperty, value); }
    }
}
