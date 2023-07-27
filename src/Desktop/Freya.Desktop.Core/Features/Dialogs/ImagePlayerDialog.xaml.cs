using System.Windows.Media.Imaging;

namespace Freya.Desktop.Core.Features.Dialogs;

public partial class ImagePlayerDialog
{
    public ImagePlayerDialog(Uri imageUri)
    {
        InitializeComponent();
        image.Source = new BitmapImage(imageUri);
    }
}
