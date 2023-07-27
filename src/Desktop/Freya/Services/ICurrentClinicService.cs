namespace Freya.Services;

public interface ICurrentClinicService
{
    EventHandler<ClinicResponse?>? OnCurrentClinicSelectedChanged { get; set; }

    Task Init(List<ClinicResponse> clinics);

    void AddClinic(ClinicResponse clinicResponse);

    void RemoveClinic(Guid id);

    List<ClinicResponse> Clinics { get; set; }

    ClinicResponse? CurrentClinic { get; set; }
}
