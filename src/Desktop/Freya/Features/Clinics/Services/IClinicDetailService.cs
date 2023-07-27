using Freya.Desktop.Core.Services;

namespace Freya.Features.Clinics.Services;

public interface IClinicDetailService
{
    Task<bool> CancelSubscription(Guid id);

    Task<bool> DeleteClinic(Guid id);

    Task<ClinicDetailResponse> GetClinic(Guid id);
}