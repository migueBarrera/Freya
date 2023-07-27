using Models.Core.Common;

namespace FreyaApi.Infrastructure.Mappers;

public static class ClinicMapper
{
    public static ClinicResponse ConverToClient(ClinicTable clinic)
    {
        var subscription = clinic.SubscriptionPayments.FirstOrDefault();
        return new ClinicResponse()
        {
            Id = clinic.Id,
            CompanyId = clinic.CompanyId,
            Name = clinic.Name,
            Email = clinic.Email,
            Adress = clinic.Adress,
            NIF = clinic.NIF,
            Phone = clinic.Phone,
            CurrentSubscription = subscription != null ? SubscriptionsMapper.ConverTo(subscription) : null,
        };
    }
    
    public static ClinicDetailResponse ConverToClientDetail(ClinicTable clinic, IQueryable<ClientTable> clients, IQueryable<EmployeeTable> employees)
    {
        var subscription = clinic.SubscriptionPayments.FirstOrDefault();

        PagedModel<EmployeeResponse> employessPaged = PagedModel<EmployeeResponse>.ToPagedList(
                employees.Select(x => EmployeeMapper.ConverToResponse(x)),
                1,
                PageModelConst.PageSizeDefault,
                string.Empty);
        
        PagedModel<ClientResponse> clientsPaged = PagedModel<ClientResponse>.ToPagedList(
                clients.Select(x => ClientMapper.ConverToClientResponse(x)),
                1,
                PageModelConst.PageSizeDefault,
                string.Empty);

        return new ClinicDetailResponse()
        {
            Id = clinic.Id,
            CompanyId = clinic.CompanyId,
            Name = clinic.Name,
            Email = clinic.Email,
            Adress = clinic.Adress,
            NIF = clinic.NIF,
            Phone = clinic.Phone,
            Employees = employessPaged,
            Clients = clientsPaged,
            CurrentSubscription = subscription != null ? SubscriptionsMapper.ConverTo(subscription) : null,
        };
    }

    public static ClinicTable ConverToClient(ClinicCreateRequest clinic)
    {
        return new ClinicTable()
        {
            CompanyId = clinic.CompanyId,
            Name = clinic.Name,
            Email = clinic.Email,
            Adress = clinic.Adress,
            NIF = clinic.NIF,
            Phone = clinic.Phone,
        };
    }

    public static Clinic ConverTo(ClinicTable clinic)
    {
        return new Clinic()
        {
            CompanyId = clinic.CompanyId,
            Name = clinic.Name,
            Id = clinic.Id,
        };
    }

    public static ClinicCreateResponse ToCreateResponse(ClinicTable clinic)
    {
        return new ClinicCreateResponse()
        {
            CompanyId = clinic.CompanyId,
            Name = clinic.Name,
            Id = clinic.Id,
        };
    }
}
