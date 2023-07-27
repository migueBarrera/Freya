using System.Windows.Controls;

namespace Freya.Desktop.Core.Controls;

public partial class PageButtons : UserControl
{
    public PageButtons()
    {
        InitializeComponent();
    }

    public bool HasPrevious
    {
        get { return (bool)GetValue(HasPreviousProperty); }
        set { SetValue(HasPreviousProperty, value); }
    }

    public static readonly DependencyProperty HasPreviousProperty =
        DependencyProperty.Register(nameof(HasPrevious), typeof(bool), typeof(PageButtons), new PropertyMetadata(false));
    
    public bool HasNext
    {
        get { return (bool)GetValue(HasNextProperty); }
        set { SetValue(HasNextProperty, value); }
    }

    public static readonly DependencyProperty HasNextProperty =
        DependencyProperty.Register(nameof(HasNext), typeof(bool), typeof(PageButtons), new PropertyMetadata(false));

    public int CurrentPage
    {
        get { return (int)GetValue(CurrentPageProperty); }
        set { SetValue(CurrentPageProperty, value); }
    }

    public static readonly DependencyProperty CurrentPageProperty =
        DependencyProperty.Register(nameof(CurrentPage), typeof(int), typeof(PageButtons), new PropertyMetadata(1));
    
    public int TotalPages
    {
        get { return (int)GetValue(TotalPagesProperty); }
        set { SetValue(TotalPagesProperty, value); }
    }

    public static readonly DependencyProperty TotalPagesProperty =
        DependencyProperty.Register(nameof(TotalPages), typeof(int), typeof(PageButtons), new PropertyMetadata(1));
}
