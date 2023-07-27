namespace Models.Core.Companies;

public class UpdateCommpanyRequest
{
    public Guid Id { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;
}
