namespace FreyaApi.Controllers.Services;

public class ClinicControllerService
{
    private readonly IClinicRepository clinicRepository;
    private readonly ISubscriptionPaymentRepository subscriptionPaymentRepository;
    private readonly IPaymentSesionService paymentSesionService;
    private readonly ISubscriptionPlanRepository subscriptionPlanRepository;
    private readonly MultimediaService multimediaService;

    public ClinicControllerService(
        IClinicRepository clinicRepository,
        MultimediaService multimediaService,
        ISubscriptionPaymentRepository subscriptionPaymentRepository,
        IPaymentSesionService paymentSesionService,
        ISubscriptionPlanRepository subscriptionPlanRepository)
    {
        this.clinicRepository = clinicRepository;
        this.multimediaService = multimediaService;
        this.subscriptionPaymentRepository = subscriptionPaymentRepository;
        this.paymentSesionService = paymentSesionService;
        this.subscriptionPlanRepository = subscriptionPlanRepository;
    }

    public async Task<IActionResult> Create(ControllerBase controller, ClinicCreateRequest request)
    {
        ClinicTable clinic = ClinicMapper.ConverToClient(request);
        var created = await clinicRepository.Create(clinic);

        return created ?
            controller.Created(string.Empty, ClinicMapper.ToCreateResponse(clinic)) :
            controller.BadRequest();
    }

    public async Task<ActionResult<ClinicDetailResponse>> Details(ControllerBase controller, Guid id)
    {
        ClinicTable? clinic = clinicRepository.GetClinic(id);

        if (clinic == null)
        {
            return controller.NotFound();
        }

        var employees = clinicRepository.GetEmployeeForClinic(id);
        var clients = clinicRepository.GetClientsForClinic(id);

        var response = ClinicMapper.ConverToClientDetail(clinic, clients, employees);

        var payments = await subscriptionPaymentRepository.GetPaymentsByClinic(clinic.Id);
        if (payments.Any())
        {
            response.SubscriptionPaymentResponses = payments.Select(x => SubscriptionsMapper.ConverTo(x)).ToList();
            response.CurrentSubscription = response.SubscriptionPaymentResponses.FirstOrDefault();
        }

        IEnumerable<SubscriptionPlanTable> paymentsPlans = await subscriptionPlanRepository.GetPlansForCompany(clinic.CompanyId);

        response.PaymentsPlansForCompanies = paymentsPlans.Select(x => SubscriptionsMapper.ConverTo(x));

        return controller.Ok(response);
    }

    public ActionResult<PagedModel<EmployeeResponse>> GetEmployees(ClinicController controller, Guid id, PaginationFilter paginationFilter)
    {
        var employees = clinicRepository.GetEmployeesByClinic(id, paginationFilter.SearchArgument);

        if (employees == null)
        {
            return controller.NotFound();
        }

        PagedModel<EmployeeResponse> response = PagedModel<EmployeeResponse>.ToPagedList(
                employees.Select(x =>
                        EmployeeMapper.ConverToResponse(
                       x)),
                paginationFilter.PageNumber,
                paginationFilter.PageSize,
                paginationFilter.SearchArgument);

        return controller.Ok(response);
    }

    public async Task<ActionResult> UpdateClinic(ClinicController controller, UpdateClinicRequest request)
    {
        var updated = await clinicRepository.Update(request);

        return updated ?
            controller.Ok() :
            controller.NotFound();
    }

    public ActionResult<PagedModel<ClientResponse>> GetClientsForClinic(ClinicController controller, Guid id, PaginationFilter paginationFilter)
    {
        var clients = clinicRepository.GetClientWithClinicInformation(id, paginationFilter.SearchArgument);

        PagedModel<ClientResponse> response = PagedModel<ClientResponse>.ToPagedList(
                clients.Select(x =>
                        ClientMapper.ConverToClientResponse(x)),
                paginationFilter.PageNumber,
                paginationFilter.PageSize,
                paginationFilter.SearchArgument);

        return controller.Ok(response);
    }

    public async Task<IActionResult> DeleteClinic(ClinicController controller, Guid clinicId)
    {
        var paymentPlan = await subscriptionPaymentRepository.GetCurrentActiveClinicSubscription(clinicId);
        if (paymentPlan != null)
        {
            var cancelOnStripe = await paymentSesionService.CancelSubscriptionNow(paymentPlan.StripeSubscriptionId);
            if (!cancelOnStripe)
            {
                return controller.NotFound();
            }

            await subscriptionPaymentRepository.UpdateState(paymentPlan.Id, SubscriptionStates.REJECTED_BY_EMPLOYEE);
        }

        var deleted = await clinicRepository.DeleteClinic(clinicId);
        if (!deleted)
        {
            return controller.NotFound();
        }

        var clientsIds = await clinicRepository.GetAllClientsIdFromClinic(clinicId);

        foreach (var clientId in clientsIds)
        {
            await multimediaService.DeleteAllMultimediaOfAClientOnAClinic(clientId, clinicId);
        }

        return controller.Ok();
    }
}
