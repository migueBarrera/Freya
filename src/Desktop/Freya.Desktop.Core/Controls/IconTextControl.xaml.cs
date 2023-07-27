using System.Windows.Controls;
using System.Windows.Media;

namespace Freya.Desktop.Core.Controls
{
    public partial class IconTextControl : Grid
    {
        public static readonly DependencyProperty StartImageSourceProperty =
            DependencyProperty.Register(
                "StartImageSource",
                typeof(ImageSource),
                typeof(IconTextControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty EndImageSourceProperty =
            DependencyProperty.Register(
                "EndImageSource",
                typeof(ImageSource),
                typeof(IconTextControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(IconTextControl),
                new PropertyMetadata("Placeholder"));

        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register(
                "ImageWidth",
                typeof(double),
                typeof(IconTextControl),
                new PropertyMetadata(49d));

        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register(
                "FontFamily",
                typeof(FontFamily),
                typeof(IconTextControl),
                new PropertyMetadata(Application.Current.Resources["RobotoRegular"]));

        public static readonly DependencyProperty FontSizeProperty =
         DependencyProperty.Register(
             "FontSize",
             typeof(double),
             typeof(IconTextControl),
             new PropertyMetadata(16d));

        public IconTextControl()
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

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
    }
}
