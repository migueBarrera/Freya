using Microsoft.AspNetCore.Mvc;
using Models.Core.Clinics;

namespace FreyaApi.Controllers.Services;

public class CompaniesControllerService
{
    private readonly IProductsPaymentsService productsPaymentsService;
    private readonly ICompaniesRepository companiesRepository;
    private readonly IClinicRepository clinicRepository;
    private readonly IEmployeeRepository employeeRepository;
    private readonly ISubscriptionProductRepository subscriptionProductRepository;
    private readonly ISubscriptionPlanRepository subscriptionPlanRepository;
    private readonly IConfiguration configuration;

    public CompaniesControllerService(
        IProductsPaymentsService productsPaymentsService,
        ICompaniesRepository companiesRepository,
        ISubscriptionProductRepository subscriptionProductRepository,
        ISubscriptionPlanRepository subscriptionPlanRepository,
        IClinicRepository clinicRepository,
        IEmployeeRepository employeeRepository,
        IConfiguration configuration)
    {
        this.productsPaymentsService = productsPaymentsService;
        this.companiesRepository = companiesRepository;
        this.subscriptionProductRepository = subscriptionProductRepository;
        this.subscriptionPlanRepository = subscriptionPlanRepository;
        this.clinicRepository = clinicRepository;
        this.employeeRepository = employeeRepository;
        this.configuration = configuration;
    }

    public ActionResult<PagedModel<Company>> Index(ControllerBase controller, PaginationFilter paginationFilter)
    {
        IQueryable<Company> companies = companiesRepository.GetCompanies();

        var response = PagedModel<Company>.ToPagedList(companies, paginationFilter.PageNumber, paginationFilter.PageSize);

        return controller.Ok(response);
    }

    public async Task<ActionResult<CompanyDetailResponse>> Details(ControllerBase controller, Guid id)
    {
        CompanyTable? company = await companiesRepository.GetCompanyById(id);

        if (company == null)
        {
            return controller.NotFound();
        }

        var employees = companiesRepository.GetEmployeeForCompany(id);
        var clinics = companiesRepository.GetClinicsForCompany(id);

        PagedModel<EmployeeResponse> employessPaged = PagedModel<EmployeeResponse>.ToPagedList(
               employees.Select(x => EmployeeMapper.ConverToResponse(x)),
               1,
               PageModelConst.PageSizeDefault,
               string.Empty);

        PagedModel<ClinicResponse> clinicPaged = PagedModel<ClinicResponse>.ToPagedList(
                clinics.Select(x => ClinicMapper.ConverToClient(x)),
                1,
                PageModelConst.PageSizeDefault,
                string.Empty);

        IEnumerable<SubscriptionPlanTable> paymentsPlans = await subscriptionPlanRepository.GetPlansForCompany(id);

        var response = new CompanyDetailResponse()
        {
            Id = id,
            Created = company.Created,
            Name = company.Name,
            Employees = employessPaged,
            Clinics = clinicPaged,
            PaymentsPlansForCompanyResponses = paymentsPlans.Select(SubscriptionsMapper.ConverTo),
        };

        return controller.Ok(response);
    }

    public ActionResult<PagedModel<ClinicResponse>> GetClinicsByCompanyId(ControllerBase controller, Guid id, PaginationFilter paginationFilter)
    {
        IQueryable<ClinicTable> clinics = clinicRepository.GetAllClinicsByCompanyId(id, paginationFilter.SearchArgument);

        PagedModel<ClinicResponse> response = PagedModel<ClinicResponse>.ToPagedList(
                clinics.Select(clinic => ClinicMapper.ConverToClient(clinic)),
                paginationFilter.PageNumber,
                paginationFilter.PageSize,
                paginationFilter.SearchArgument);

        return controller.Ok(response);
    }

    public ActionResult<PagedModel<EmployeeResponse>> GetEmployeesByCompanyId(ControllerBase controller, Guid id, PaginationFilter paginationFilter)
    {
        IQueryable<EmployeeTable> clinics = employeeRepository.GetAllEmployeeByCompanyId(id, paginationFilter.SearchArgument);

        PagedModel<EmployeeResponse> response = PagedModel<EmployeeResponse>.ToPagedList(
                clinics.Select(clinic => EmployeeMapper.ConverToResponse(clinic)),
                paginationFilter.PageNumber,
                paginationFilter.PageSize,
                paginationFilter.SearchArgument); ;

        return controller.Ok(response);
    }

    public async Task<IActionResult> Create(ControllerBase controller, CompanyCreateRequest request)
    {
        var company = new CompanyTable()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
        };

        var created = await companiesRepository.CreateCompany(company);
        if (!created)
        {
            return controller.NotFound();
        }

        var baseuri = configuration.GetValue<string>("BaseUri");
        if (string.IsNullOrEmpty(baseuri))
        {
            return controller.NotFound();
        }

        var listSubscriptionPlan = await CreateSubscrptionPlan(request.SubscriptionIds, company, baseuri);

        await subscriptionPlanRepository.CreateSubscriptionPlans(listSubscriptionPlan);

        return controller.Created(string.Empty, company);
    }

    public async Task<ActionResult> UpdateCcompany(ControllerBase controller, UpdateCommpanyRequest request)
    {
        var updated = await companiesRepository.Update(request);

        return updated ?
        controller.Ok() :
            controller.NotFound();
    }


    private async Task<List<SubscriptionPlanTable>> CreateSubscrptionPlan(IEnumerable<Guid> subscriptionIds, CompanyTable company, string baseUri)
    {
        var listSubscriptionPlan = new List<SubscriptionPlanTable>();
        IEnumerable<SubscriptionProductTable> subscriptionProducts = await subscriptionProductRepository.GetSubscriptionProductsByIds(subscriptionIds);
        foreach (var subscription in subscriptionProducts)
        {
            var subsciptionPlanId = Guid.NewGuid();
            Uri paymentLinkMonthly = productsPaymentsService.GenerateLinkByPrice(subsciptionPlanId, subscription.PriceIdMonthly, subscription.AllowPromotionCodes, baseUri, isMonthly: true);
            Uri? paymentLinkAnnual = null;
            if (subscription.PriceIdAnnual != null)
            {
                paymentLinkAnnual = productsPaymentsService.GenerateLinkByPrice(subsciptionPlanId, subscription.PriceIdAnnual, subscription.AllowPromotionCodes, baseUri, isMonthly: false);
            }

            var subcriptionPlan = new SubscriptionPlanTable()
            {
                Id = subsciptionPlanId,
                CompanyId = company.Id,
                SubscriptionProductId = subscription.Id,
                IsActive = true,
                PaymentLinkMonthly = paymentLinkMonthly,
                PaymentLinkAnnual = paymentLinkAnnual,
            };
            listSubscriptionPlan.Add(subcriptionPlan);
        }

        return listSubscriptionPlan;
    }

    public async Task<ActionResult> DeleteCompany(ControllerBase controller, Guid companyId)
    {
        //De momento vamos a requerir que borre manualmente todas las clinicas una a una para evitar esa carga en el servidor de golpe
        var clinicsForCompany = companiesRepository.GetClinicsForCompany(companyId);
        if (clinicsForCompany.Any())
        {
            return controller.Conflict();
        }

        var deleted = await companiesRepository.DeleteCompany(companyId);
        if (!deleted)
        {
            return controller.NotFound();
        }

        return controller.Ok(companyId);
    }
}
