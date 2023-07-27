using Models.Core.Clients;

namespace Freya.Desktop.Core.Features.Clients.Models;

public class ClientDetailModel : ObservableObject
{
    private long size;

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public DateTime? PregnancyDate { get; set; }

    public long Size { get => size; set => SetAndRaisePropertyChanged(ref size, value); }

    public long SubscriptionPlanSize { get; set; }

    public static ClientDetailResponse To(ClientDetailModel model)
    {
        return new ClientDetailResponse()
        {
            Id = model.Id,
            Email = model.Email,
            LastName = model.LastName,
            Name = model.Name,
            Phone = model.Phone,
            SubscriptionPlanSize = model.SubscriptionPlanSize,
            Size = model.Size,
        };
    }

    public static ClientDetailModel To(ClientResponse model)
    {
        return new ClientDetailModel()
        {
            Id = model.Id,
            Email = model.Email,
            LastName = model.LastName,
            Name = model.Name,
            Phone = model.Phone,
            //SubscriptionPlanSize = model.SubscriptionPlanSize,
            //Size = model.Size,
        };
    }

    public static ClientDetailModel To(ClientDetailResponse model)
    {
        return new ClientDetailModel()
        {
            Id = model.Id,
            Email = model.Email,
            LastName = model.LastName,
            Name = model.Name,
            Phone = model.Phone,
            SubscriptionPlanSize = model.SubscriptionPlanSize,
            Size = model.Size,
        };
    }
}