using Models.Core.Common;

namespace Freya.Features.Clinics.Services
{
    public interface IClinicsService
    {
        Task<PagedModel<ClinicResponse>> GetClinicsAsync(PaginationFilter PaginationFilter);
    }
}