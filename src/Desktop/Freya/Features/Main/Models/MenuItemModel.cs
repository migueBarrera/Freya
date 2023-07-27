using System.Windows.Input;
using System.Windows.Media;

namespace Freya.Features.Main.Models;

public class MenuItemModel
{
    public string Name { get; set; } = string.Empty;

    public ImageSource? Icon { get; set; }

    public Visibility Visibility { get; set; } = Visibility.Visible;

    public ICommand? Action { get; set; }
}
