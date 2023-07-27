using System.Windows.Controls;
using System.Windows.Media;

namespace Freya.Desktop.Core.Controls;

public partial class TextBoxWithPlaceHolder : UserControl
{
    public static readonly DependencyProperty PlaceHolderProperty =
               DependencyProperty.Register(
                   "PlaceHolder",
                   typeof(string),
                   typeof(TextBoxWithPlaceHolder),
                   new PropertyMetadata("Placeholder"));
    
    public static readonly DependencyProperty TitleProperty =
               DependencyProperty.Register(
                   "Title",
                   typeof(string),
                   typeof(TextBoxWithPlaceHolder),
                   new PropertyMetadata(""));

    public static readonly DependencyProperty TextColorBrushProperty =
       DependencyProperty.Register(
           "TextColorBrush",
           typeof(Brush),
           typeof(TextBoxWithPlaceHolder),
           new PropertyMetadata(null));

    public static readonly DependencyProperty SVGSourceProperty =
      DependencyProperty.Register(
          "SVGSource",
          typeof(ImageSource),
          typeof(TextBoxWithPlaceHolder),
          new PropertyMetadata(null));

    public static readonly DependencyProperty TextProperty =
       DependencyProperty.Register(
           "Text",
           typeof(string),
           typeof(TextBoxWithPlaceHolder),
           new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty EndSVGProperty =
        DependencyProperty.Register(
            "EndSVG",
            typeof(ImageSource),
            typeof(TextBoxWithPlaceHolder),
            new PropertyMetadata(null));

    public static readonly DependencyProperty IsPasswordProperty =
        DependencyProperty.Register(
            "IsPassword",
            typeof(bool),
            typeof(TextBoxWithPlaceHolder),
            new PropertyMetadata(false, OnIsPasswordChanged));

    public static readonly DependencyProperty TextBoxBackgroundProperty =
       DependencyProperty.Register(
           "TextBoxBackground",
           typeof(Color),
           typeof(TextBoxWithPlaceHolder),
           new PropertyMetadata(null));

    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(TextBoxWithPlaceHolder),
            new PropertyMetadata(null));

    public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register(
            "IsReadOnly",
            typeof(bool),
            typeof(TextBoxWithPlaceHolder),
            new PropertyMetadata(false));

    private static readonly DependencyProperty IsInPasswordModeProperty =
        DependencyProperty.Register(
            nameof(IsInPasswordMode),
            typeof(bool),
            typeof(TextBoxWithPlaceHolder),
            new PropertyMetadata(false));
    
    private static readonly DependencyProperty TextWrappingProperty =
        DependencyProperty.Register(
            nameof(TextWrapping),
            typeof(TextWrapping),
            typeof(TextBoxWithPlaceHolder),
            new PropertyMetadata(TextWrapping.NoWrap));

    public TextBoxWithPlaceHolder()
    {
        InitializeComponent();
        this.textBox.KeyUp += TextBox_KeyUp;
    }

    public bool IsPassword
    {
        get { return (bool)GetValue(IsPasswordProperty); }
        set { SetValue(IsPasswordProperty, value); }
    }

    public string PlaceHolder
    {
        get { return (string)GetValue(PlaceHolderProperty); }
        set { SetValue(PlaceHolderProperty, value); }
    }

    public Brush TextColorBrush
    {
        get { return (Brush)GetValue(TextColorBrushProperty); }
        set { SetValue(TextColorBrushProperty, value); }
    }

    public ImageSource SVGSource
    {
        get { return (ImageSource)GetValue(SVGSourceProperty); }
        set { SetValue(SVGSourceProperty, value); }
    }

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }
    
    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public ImageSource EndSVG
    {
        get { return (ImageSource)GetValue(EndSVGProperty); }
        set { SetValue(EndSVGProperty, value); }
    }

    public Color TextBoxBackground
    {
        get { return (Color)GetValue(TextBoxBackgroundProperty); }
        set { SetValue(TextBoxBackgroundProperty, value); }
    }

    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    public bool IsReadOnly
    {
        get { return (bool)GetValue(IsReadOnlyProperty); }
        set { SetValue(IsReadOnlyProperty, value); }
    }

    public TextWrapping TextWrapping
    {
        get { return (TextWrapping)GetValue(TextWrappingProperty); }
        set { SetValue(TextWrappingProperty, value); }
    }

    private bool IsInPasswordMode
    {
        get { return (bool)GetValue(IsInPasswordModeProperty); }
        set { SetValue(IsInPasswordModeProperty, value); }
    }

    private static void OnIsPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (TextBoxWithPlaceHolder)d;
        control.IsInPasswordMode = control.IsPassword;
    }

    private void TextBox_KeyUp(object sender, KeyEventArgs e)
    {
        if (Command != null)
        {
            Command.Execute(textBox.Text);
        }
    }

    private void OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (Command != null)
        {
            Command.Execute(textBox.Text);
        }
    }

    private void ChangeStateClick(object sender, RoutedEventArgs e)
    {
        if (this.IsPassword)
        {
            IsInPasswordMode = !IsInPasswordMode;
        }
    }
}
