using Models.Core.Clinics;

namespace Freya.Desktop.Core.Features.Clinics;

public class NewClinicModel : ObservableObject
{
    public NewClinicModel()
    {

    }

    public NewClinicModel(ClinicDetailResponse clinic)
    {
        Name = clinic.Name;
        Id = clinic.Id;
        Adress = clinic.Adress;
        NIF = clinic.NIF;
        Phone = clinic.Phone;
        Email = clinic.Email;
    }

    public Guid Id { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;

    public string Adress { get; set; } = string.Empty;

    public string NIF { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public ClinicCreateRequest ToRequest(Guid companyId)
    {
        return new ClinicCreateRequest()
        {
            Adress = Adress,
            Email = Email,
            Name = Name,
            NIF = NIF,
            Phone = Phone.Replace(" ", string.Empty),
            CompanyId = companyId,
        };
    }

    public UpdateClinicRequest ToEditRequest()
    {
        return new UpdateClinicRequest()
        {
            Id = Id,
            Adress = Adress,
            Email = Email,
            Name = Name,
            NIF = NIF,
            Phone = Phone.Replace(" ", string.Empty),
        };
    }
}
