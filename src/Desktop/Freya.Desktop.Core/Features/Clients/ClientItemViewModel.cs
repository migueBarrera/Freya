using Models.Core.Clients;
using MvvmHelpers.Interfaces;

namespace Freya.Desktop.Core.Features.Clients;

public class ClientItemViewModel : ObservableObject, INavigationAwareViewModel
{
    private string name = string.Empty;
    private string image = string.Empty;
    private string email = string.Empty;

    public ClientResponse? Client { get; private set; }

    public ClientItemViewModel()
    {
    }

    public string Name { get => name; set => SetAndRaisePropertyChanged(ref name, value); }

    public string Image { get => image; set => SetAndRaisePropertyChanged(ref image, value); }

    public string Email { get => email; set => SetAndRaisePropertyChanged(ref email, value); }

    public IAsyncCommand<ClientResponse>? ViewProfileCommand { set; get; }

    public Task OnAppearingAsync(object? parameter = null)
    {
        Client = (parameter as ClientResponse) ?? new ClientResponse();
        name = $"{Client.Name} {Client.LastName}";
        image = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460__340.png";
        Email = Client.Email;
        return Task.CompletedTask;
    }

    public Task OnDisappearingAsync()
    {
        return Task.CompletedTask;
    }
}
