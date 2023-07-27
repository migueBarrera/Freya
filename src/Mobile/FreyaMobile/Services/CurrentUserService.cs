using System.Text.Json;

namespace FreyaMobile.Services;

public class CurrentUserService : ICurrentUserService
{
    const string userPreferencesKey = nameof(userPreferencesKey);
    const string clinicPreferencesKey = nameof(clinicPreferencesKey);

    private readonly ILoggingService logger;

    public CurrentUserService(ILoggingService logger)
    {
        this.logger = logger;
    }

    public Task<bool> HasClientDataAsync()
    {
        var user = GetClient();
        var userHasAsosietedToAnyClinic = user != null && user.Clinics != null && user.Clinics.Any();
        return Task.FromResult(userHasAsosietedToAnyClinic);
    }

    public Task<bool> IsLoggedAsync()
    {
        return Task.FromResult(GetClient() != null);
    }

    public Task SetUserAsync(Client? user)
    {
        if (user == null)
        {
            Preferences.Default.Set(userPreferencesKey, string.Empty);
        }
        else
        {
            var clientJson = JsonSerializer.Serialize(user);
            Preferences.Default.Set(userPreferencesKey, clientJson);
        }

        return Task.CompletedTask;
    }

    public Task<Client?> GetCurrentUserAsync()
    {
        return Task.FromResult(GetClient());
    }

    private Client? GetClient()
    {
        var clientJson = Preferences.Default.Get<string>(userPreferencesKey, string.Empty);
        if (string.IsNullOrEmpty(clientJson))
        {
            return null;
        }

        return JsonSerializer.Deserialize<Client>(clientJson);
    }

    public Clinic? GetCurrentClinic()
    {
        var clientJson = Preferences.Default.Get<string>(clinicPreferencesKey, string.Empty);
        if (string.IsNullOrEmpty(clientJson))
        {
            return null;
        }

        return JsonSerializer.Deserialize<Clinic>(clientJson);
    }

    public Task SetClinicSelectedAsync(Clinic? clinic)
    {
        try
        {
            if (clinic == null)
            {
                Preferences.Default.Set(clinicPreferencesKey, string.Empty);
            }
            else
            {
                var userString = JsonSerializer.Serialize(clinic);
                Preferences.Default.Set(clinicPreferencesKey, userString);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex);
        }

        return Task.CompletedTask;
    }
}
