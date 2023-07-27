namespace Freya.Services;

public class CurrentClinicService : ICurrentClinicService
{
    private ClinicResponse? currentClinic;

    public CurrentClinicService()
    {
        Clinics = new List<ClinicResponse>();
    }

    public List<ClinicResponse> Clinics { get; set; }

    public ClinicResponse? CurrentClinic
    {
        get => currentClinic;
        set
        {
            currentClinic = value;
            OnCurrentClinicSelectedChanged?.Invoke(this, value);
        }
    }

    public EventHandler<ClinicResponse?>? OnCurrentClinicSelectedChanged { get; set; }

    public void AddClinic(ClinicResponse clinicResponse)
    {
        Clinics.Add(clinicResponse);
    }

    public Task Init(List<ClinicResponse> clinics)
    {
        Clinics = clinics;
        return Task.CompletedTask;
    }

    public void RemoveClinic(Guid id)
    {
        var itemForRemove = Clinics.FirstOrDefault(x => x.Id == id);
        if (itemForRemove != null)
        {
            Clinics.Remove(itemForRemove);
        }
    }
}
