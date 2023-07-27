namespace Freya.Desktop.Core.Features.Dialogs;

public partial class ViewPlayerDialog
{
    public ViewPlayerDialog(Uri videoUri)
    {
        InitializeComponent();
		mePlayer.Source = videoUri;
        mePlayer.Play();
        mePlayer.Pause();
    }

	private void btnPlay_Click(object sender, RoutedEventArgs e)
	{
		mePlayer.Play();
	}

	private void btnPause_Click(object sender, RoutedEventArgs e)
	{
		mePlayer.Pause();
	}

	private void btnStop_Click(object sender, RoutedEventArgs e)
	{
		mePlayer.Stop();
	}
}
