using Microsoft.Web.WebView2.Core;

namespace Freya.Features.Subscriptions;

public partial class CheckoutPage
{
    public CheckoutPage(CheckoutViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();

        _ = InitializeAsync();
    }

    protected override void PageLoaded(object sender, RoutedEventArgs e)
    {
        base.PageLoaded(sender, e);
        webView.Source = ViewModel.Source;
    }

    private CheckoutViewModel ViewModel => ((CheckoutViewModel)DataContext);

    async Task InitializeAsync()
    {
        // must create a data folder if running out of a secured folder that can't write like Program Files
        var env = await CoreWebView2Environment.CreateAsync(userDataFolder: Path.Combine(Path.GetTempPath(), "MarkdownMonster_Browser"));

        // NOTE: this waits until the first page is navigated - then continues
        //       executing the next line of code!
        await webView.EnsureCoreWebView2Async(env);

        //if (Model.Options.AutoOpenDevTools)
        //    webView.CoreWebView2.OpenDevToolsWindow();

        // Almost always need this event for something    
        //webView.NavigationCompleted += Completed;

        //// set the initial URL
        //webView.Source = new Uri("https://buy.stripe.com/test_cN2eYw2y5aaP9uE6oo");
    }

    private void Completed(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {

    }

    private void wbSample_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
    {

    }

    private void wbSample_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
    {

    }

    private void webView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
    {

    }

    private void webView_ContentLoading(object sender, CoreWebView2ContentLoadingEventArgs e)
    {
        //ViewModel.IsBusy = false;
    }

    private void webView_Loaded(object sender, RoutedEventArgs e)
    {
        //ViewModel.IsBusy = true;
    }
}
