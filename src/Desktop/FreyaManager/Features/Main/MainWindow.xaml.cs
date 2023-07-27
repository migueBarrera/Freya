using System.Threading;

namespace FreyaManager.Features.Main;

public partial class MainWindow : Window, IHostedService
{
    private readonly Application application;
    private readonly IServiceProvider serviceProvider;

    public MainWindow(
        Application application,
        MainViewModel viewModel,
        IServiceProvider serviceProvider)
    {
        DataContext = viewModel;
        this.application = application;
        this.serviceProvider = serviceProvider;
        InitializeComponent();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (!cancellationToken.IsCancellationRequested)
        {
            return this.Dispatcher.InvokeAsync(() =>
            {
                this.application.MainWindow = this;
                var navigationService = serviceProvider.GetService<INavigationService>();
                navigationService?.Init(frame.NavigationService).Wait();
                this.Show();
                this.Activate();
            }).Task;
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
