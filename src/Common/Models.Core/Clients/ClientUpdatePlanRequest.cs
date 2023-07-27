namespace Models.Core.Clients;

public class ClientUpdatePlanRequest
{
    public Guid ClientId { get; set; }

    public Guid ClinicId { get; set; }
}
