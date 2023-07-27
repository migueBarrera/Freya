using System;
using System.Windows;
using System.Windows.Controls;

namespace Freya.Desktop.Dialogs.Core;

public class CoreModalDialog : UserControl
{
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(CoreModalDialog), new PropertyMetadata(string.Empty));


    public static readonly DependencyProperty SubTitleProperty =
        DependencyProperty.Register("SubTitle", typeof(string), typeof(CoreModalDialog), new PropertyMetadata(string.Empty));

    public CoreModalDialog()
    {
        DialogId = Guid.NewGuid();
    }

    public Guid DialogId { get; private set; }

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public string SubTitle
    {
        get { return (string)GetValue(SubTitleProperty); }
        set { SetValue(SubTitleProperty, value); }
    }
}
