namespace FreyaMobile.Services.Interfaces;

public interface ICurrentUserService
{
    Task<bool> IsLoggedAsync();

    Task<bool> HasClientDataAsync();

    Task SetUserAsync(Client? user);

    Task<Client?> GetCurrentUserAsync();

    Clinic? GetCurrentClinic();

    Task SetClinicSelectedAsync(Clinic? clinic);
}
