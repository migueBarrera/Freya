namespace Models.Core.Clients;

public class ClientUpdateRequest
{
    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public DateTime? PregnancyDate { get; set; }
}
