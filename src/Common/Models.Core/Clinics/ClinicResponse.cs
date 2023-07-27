namespace Models.Core.Clinics;

public class ClinicResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Adress { get; set; } = string.Empty;

    public string NIF { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public Guid CompanyId { get; set; }

    public ICollection<EmployeeResponse> Employees { get; set; } = new List<EmployeeResponse>();

    public ICollection<ClientResponse> Clients { get; set; } = new List<ClientResponse>();

    public ICollection<SubscriptionPaymentResponse> SubscriptionPaymentResponses { get; set; } = new List<SubscriptionPaymentResponse>();

    public SubscriptionPaymentResponse? CurrentSubscription { get; set; }

    public IEnumerable<PaymentsPlansForCompanyResponse> PaymentsPlansForCompanies { get; set; } = Enumerable.Empty<PaymentsPlansForCompanyResponse>();

    public override string ToString()
    {
        return Name;
    }
}
