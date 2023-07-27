using Models.Core.Clients;

namespace Freya.Desktop.Core.Features.Clients.Models;

public class EditClientModel : ObservableObject
{
    private Guid id = Guid.Empty;
    private string clientName = string.Empty;
    private string clientEmail = string.Empty;
    private string clientLastName = string.Empty;
    private string clientPhone = string.Empty;

    public string ClientName { get => clientName; set => SetAndRaisePropertyChanged(ref clientName, value); }
    public string ClientEmail { get => clientEmail; set => SetAndRaisePropertyChanged(ref clientEmail, value); }
    public string ClientPhone { get => clientPhone; set => SetAndRaisePropertyChanged(ref clientPhone, value); }
    public string ClientLastName { get => clientLastName; set => SetAndRaisePropertyChanged(ref clientLastName, value); }
    public Guid Id { get => id; set => id = value; }

    internal ClientUpdateRequest ToEditRequest()
    {
        return new ClientUpdateRequest()
        {
            Name = ClientName,
            LastName = ClientLastName,
            Phone = clientPhone.Replace(" ",string.Empty),
        };
    }

    internal bool Validate()
    {
        return true;
    }
}
