namespace Models.Core.Clients;

public class AssignClientToClinicRequest
{
    public Guid ClinicId { get; set; } = Guid.Empty;
    public string ClientEmail { get; set; } = string.Empty;
}
