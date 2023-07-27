using Models.Core.Clinics;

namespace Freya.Desktop.Core.Features.Clinics;

public interface IEditClinicService
{
    Task<bool> EditClinic(UpdateClinicRequest clinic);
}
