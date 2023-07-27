namespace Freya.Features.Clinics.Services
{
    public interface ICreateNewClinicService
    {
        Task<ClinicCreateResponse?> CreateClinic(ClinicCreateRequest clinic);
    }
}