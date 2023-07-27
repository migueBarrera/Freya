using System.Diagnostics.CodeAnalysis;
using System.Timers;
using System.Windows.Controls;

namespace Freya.Desktop.Core.Controls;

public partial class SearchControl : UserControl
{
    private readonly TimeSpan MinThreholdTime = TimeSpan.FromMilliseconds(700);
    private System.Timers.Timer timer;

    public static readonly DependencyProperty PlaceHolderProperty =
         DependencyProperty.Register(
             nameof(PlaceHolder),
             typeof(string),
             typeof(SearchControl),
             new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
       nameof(Text),
       typeof(string),
       typeof(SearchControl),
       new PropertyMetadata(string.Empty));


    public ICommand SearchCommand
    {
        get { return (ICommand)GetValue(SearchCommandProperty); }
        set { SetValue(SearchCommandProperty, value); }
    }

    public static readonly DependencyProperty SearchCommandProperty =
        DependencyProperty.Register(nameof(SearchCommand), typeof(ICommand), typeof(SearchControl), new PropertyMetadata(null));


    public SearchControl()
    {
        InitializeComponent();

        SetTimer();
    }

    [MemberNotNull(nameof(timer))]
    private void SetTimer()
    {
        // Create a timer with a two second interval.
        timer = new System.Timers.Timer(MinThreholdTime);
        // Hook up the Elapsed event for the timer. 
        timer.Elapsed += OnTimedEvent;
        timer.AutoReset = false;
        timer.Enabled = false;
    }

    private void OnTimedEvent(object? sender, ElapsedEventArgs e)
    {
        Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                          e.SignalTime);
        this.Dispatcher.Invoke(() =>
        {
            SearchCommand?.Execute(Text);
        });
    }

    public string PlaceHolder
    {
        get { return (string)GetValue(PlaceHolderProperty); }
        set { SetValue(PlaceHolderProperty, value); }
    }

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    private void ClearButtonClicked(object sender, RoutedEventArgs e)
    {
        Text = string.Empty;
    }

    private void textBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        timer.Stop();
        timer.Start();
    }
}
