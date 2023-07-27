using System.Windows.Controls;

namespace FreyaManager.Features.Main
{
    /// <summary>
    /// Interaction logic for HeaderControlç.xaml
    /// </summary>
    public partial class HeaderControl : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
       DependencyProperty.Register("Title", typeof(string), typeof(HeaderControl), new PropertyMetadata(string.Empty));

        public HeaderControl()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
    }
}
