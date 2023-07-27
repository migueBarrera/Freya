namespace Models.Core.Clients;

public class CheckClientStateForRegisterResquest
{
    public string ClientEmail { get; set; } = string.Empty;

    public Guid ClinicId { get; set; } = Guid.Empty;
}
