using Models.Core.Clinics;
using Models.Core.Common;

namespace Freya.Desktop.Core.Features.Clinics
{
    public interface IClinicService
    {
        Task<bool> CreateClinic(ClinicCreateRequest clinic);

        Task<ClinicDetailResponse> GetClinic(Guid id);

        Task<PagedModel<ClinicResponse>> GetClinicsByCompanyId(Guid id, PaginationFilter PaginationFilter);

        Task<bool> DeleteClinic(Guid id);
    }
}