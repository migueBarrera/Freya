namespace Models.Core.Companies;

public class CompanyCreateRequest
{
    public string Name { get; set; } = string.Empty;

    public IEnumerable<Guid> SubscriptionIds { get; set; } = Enumerable.Empty<Guid>();
}
