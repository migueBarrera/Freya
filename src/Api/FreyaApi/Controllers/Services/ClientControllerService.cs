namespace FreyaApi.Controllers.Services;

public class ClientControllerService
{
    private readonly IClientRepository clientRepository;
    private readonly IClinicRepository clinicRepository;
    private readonly ISubscriptionPaymentRepository subscriptionPaymentRepository;
    private readonly JwtSecurityTokenService jwtSecurityToken;
    private readonly IEmailService emailService;
    private readonly MultimediaService multimediaService;
    private readonly IVideoRepository videoRepository;
    private readonly IImageRepository imageRepository;
    private readonly ISoundRepository soundRepository;

    public ClientControllerService(
        IClientRepository clientRepository,
        JwtSecurityTokenService jwtSecurityToken,
        IEmailService emailService,
        MultimediaService multimediaService,
        IClinicRepository clinicRepository,
        ISubscriptionPaymentRepository subscriptionPaymentRepository,
        IVideoRepository videoRepository,
        IImageRepository imageRepository,
        ISoundRepository soundRepository)
    {
        this.clientRepository = clientRepository;
        this.jwtSecurityToken = jwtSecurityToken;
        this.emailService = emailService;
        this.multimediaService = multimediaService;
        this.clinicRepository = clinicRepository;
        this.subscriptionPaymentRepository = subscriptionPaymentRepository;
        this.videoRepository = videoRepository;
        this.imageRepository = imageRepository;
        this.soundRepository = soundRepository;
    }

    public async Task<ActionResult<ClientSignInResponse>> RefresClient(ControllerBase controller, Guid id)
    {
        var client = clientRepository.GetClientById(id);

        if (client == null)
        {
            return controller.NotFound();
        }

        var clinics = await clientRepository.GetClinicsForClient(client.Id);

        var response = ClientMapper.ConvertToClientSignInResponse(
            client!,
            jwtSecurityToken.BuildToken(client!),
            jwtSecurityToken.BuildRefreshToken(client!),
            clinics);

        return controller.Ok(response);
    }

    public async Task<ActionResult> UpdatePlan(ControllerBase controller, ClientUpdatePlanRequest request)
    {
        var subcriptionForClinic = await subscriptionPaymentRepository.GetCurrentActiveClinicSubscription(request.ClinicId);

        if (subcriptionForClinic == null
            || subcriptionForClinic.State != SubscriptionStates.ACTIVE)
        {
            return controller.BadRequest(new ErrorModel(ErrorType.CLINIC_NOT_HAS_A_VALID_SUBCRIPTION));
        }

        ClinicClient? clinicClientRelation = await clinicRepository.GetClientClinicRelation(request.ClientId, request.ClinicId);

        if (clinicClientRelation == null)
        {
            return controller.BadRequest();
        }

        bool newPlanHasMoreSpace = clinicClientRelation.SubscriptionPlanSize < subcriptionForClinic.SubscriptionPlan?.SubscriptionProduct?.Size;
        if (!newPlanHasMoreSpace)
        {
            return controller.BadRequest();
        }

        bool updatedPlan = await clinicRepository.UpdateClientPlan(clinicClientRelation.ClinicId, clinicClientRelation.ClientId, subcriptionForClinic.SubscriptionPlanId);
        if (updatedPlan)
        {
            return controller.Ok();
        }
        else
        {
            return controller.NotFound();
        }
    }

    public ActionResult<CheckClientStateForRegisterResponse> CheckClientStateForRegister(ControllerBase controller, CheckClientStateForRegisterResquest request)
    {
        if (!clientRepository.ClientExist(request.ClientEmail))
        {
            return controller.Ok(new CheckClientStateForRegisterResponse(NewClientState.CLIENT_NOT_REGISTERED));
        }

        bool employeeIsAsignedForClinic = clientRepository.ClienHasIncludeOnAClinic(request.ClientEmail, request.ClinicId);

        if (employeeIsAsignedForClinic)
        {
            return controller.Ok(new CheckClientStateForRegisterResponse(NewClientState.CLIENT_REGISTERED_AND_ASIGNED));
        }
        else
        {
            return controller.Ok(new CheckClientStateForRegisterResponse(NewClientState.CLIENT_REGISTERED_BUT_NOT_ASIGNED));
        }
    }

    public async Task<IActionResult> AssignToClinic(ControllerBase controller, AssignClientToClinicRequest request)
    {
        if (!clientRepository.ClientExist(request.ClientEmail))
        {
            return controller.NotFound();
        }

        if (!clinicRepository.ClinicExist(request.ClinicId))
        {
            return controller.NotFound();
        }

        var subcriptionForClinic = await subscriptionPaymentRepository.GetCurrentActiveClinicSubscription(request.ClinicId);

        if (subcriptionForClinic == null
            || subcriptionForClinic.State != SubscriptionStates.ACTIVE)
        {
            return controller.BadRequest(new ErrorModel(ErrorType.CLINIC_NOT_HAS_A_VALID_SUBCRIPTION));
        }

        Guid? clientID = await clientRepository.GetClientIdByEmail(request.ClientEmail);

        var relation = new ClinicClient()
        {
            ClientId = clientID ?? Guid.Empty,
            ClinicId = request.ClinicId,
            SubscriptionPlanId = subcriptionForClinic?.SubscriptionPlanId ?? Guid.Empty,
            SubscriptionPlanSize = subcriptionForClinic?.SubscriptionPlan?.SubscriptionProduct?.Size ?? 0,
        };

        await clinicRepository.AddClientToClinic(relation);

        return controller.Ok();
    }

    public async Task<IActionResult> RemoveClientFromClinic(ControllerBase controller, Guid clientId, Guid clinicId)
    {
        if (!clientRepository.ClientExist(clientId) || !clinicRepository.ClinicExist(clinicId))
        {
            return controller.NotFound();
        }

        var removed = await clinicRepository.RemoveClientFromClinic(clinicId, clientId);
        if (!removed)
        {
            return controller.NotFound();
        }

        await multimediaService.DeleteAllMultimediaOfAClientOnAClinic(clientId, clinicId);

        return controller.Ok();
    }

    public async Task<ActionResult> RecoverPassword(ControllerBase controller, ClientForgotPassRequest request)
    {
        if (!clientRepository.ClientExist(request.Email))
        {
            return controller.NotFound();
        }
        var newPass = PassGeneratorHelper.Generate();

        bool updated = await clientRepository.UpdatePass(request.Email, Hash.Create(newPass));

        if (updated)
        {
            emailService.SendMailChangePassClient(request.Email, newPass);
        }

        return controller.Ok();
    }

    public async Task<IActionResult> GetVideos(ControllerBase controller, Guid id, Guid clinicId)
    {
        IEnumerable<VideoTable> videos = await videoRepository.GetVideos(id, clinicId);

        return controller.Ok(videos);
    }

    public async Task<IActionResult> GetUltrasounds(ControllerBase controller, Guid id, Guid clinicId)
    {
        IEnumerable<UltrasoundTable> items = await imageRepository.GetImages(id, clinicId);

        return controller.Ok(items);
    }

    public async Task<IActionResult> GetSounds(ControllerBase controller, Guid id, Guid clinicId)
    {
        IEnumerable<SoundTable> items = await soundRepository.GetSounds(id, clinicId);

        return controller.Ok(items);
    }
    public async Task<ActionResult> UpdateClient(ControllerBase controller, Guid id, ClientUpdateRequest request)
    {
        bool updated = await clientRepository.UpdateClient(id, request);
        if (!updated)
        {
            return controller.NotFound();
        }

        return controller.Ok();
    }
    public async Task<ActionResult> ChangePass(ControllerBase controller, ClientChangePassRequest request)
    {
        ClientTable? client = clientRepository.GetClientById(request.ClientId);

        if (client == null)
        {
            return controller.NotFound();
        }

        if (!Hash.Validate(request.ActualPassword, client.Pass))
        {
            return controller.NotFound();
        }

        bool updated = await clientRepository.UpdatePass(request.ClientId, Hash.Create(request.Password));

        return controller.Ok();
    }

    public async Task<ActionResult<ClientDetailResponse>> ClientDetailsForAClinic(ControllerBase controller, Guid id, Guid clinicId)
    {
        ClientTable? client = clientRepository.GetClientByIdWithClinicRelation(id, clinicId);

        ClinicClient? clinicClientRelation = client?.ClinicClients?.FirstOrDefault();

        if (client == null || clinicClientRelation == null)
        {
            return controller.NotFound();
        }

        var actualSize = await clientRepository.GetSizeFromClientAndClinic(clinicClientRelation.ClientId, clinicClientRelation.ClinicId);
        var response = ClientMapper.ConvertToDetailResponse(client, clinicClientRelation, actualSize);

        var ultrasounds = await imageRepository.GetImages(client.Id, clinicId);

        var videos = await videoRepository.GetVideos(client.Id, clinicId);

        var sounds = await soundRepository.GetSounds(client.Id, clinicId);

        response.Videos = videos?.Select(item => MultimediaMapper.ConverTo(item))
                ?? Enumerable.Empty<VideoMultimedia>();
        response.Ultrasounds = ultrasounds?.Select(item => MultimediaMapper.ConverTo(item))
                ?? Enumerable.Empty<ImageMultimedia>();
        response.Sounds = sounds?.Select(item => MultimediaMapper.ConverTo(item))
            ?? Enumerable.Empty<SoundMultimedia>();

        return controller.Ok(response);
    }

    public async Task<ActionResult<ClientSignUpResponse>> SignUpForClinic(ControllerBase controller, ClientSignUpRequestForClinic request)
    {
        if (clientRepository.ClientExist(request.Email))
        {
            return controller.Conflict();
        }

        ClinicTable? clinic = null;

        var newPass = PassGeneratorHelper.Generate();
        var client = ClientMapper.ConverToClient(request, Hash.Create(newPass));

        var clientCreated = await clientRepository.Create(client);

        if (clientCreated == null)
        {
            return controller.BadRequest();
        }

        if (request.ClinicId != Guid.Empty)
        {
            var currentSubscription = await subscriptionPaymentRepository
                .GetCurrentActiveClinicSubscription(request.ClinicId);
            if (currentSubscription == null)
            {
                return controller.BadRequest(new ErrorModel(ErrorType.CLINIC_NOT_HAS_A_VALID_SUBCRIPTION));
            }

            await clinicRepository.AddClientToClinic(new ClinicClient()
            {
                ClientId = clientCreated.Id,
                ClinicId = request.ClinicId,
                SubscriptionPlanId = currentSubscription.SubscriptionPlanId,
                SubscriptionPlanSize = currentSubscription.SubscriptionPlan?.SubscriptionProduct?.Size ?? 0,
            });
        }

        emailService.SendMailWelcomeClient(request.Email, request.Name, newPass);

        IEnumerable<Clinic> clinics = Enumerable.Empty<Clinic>();
        if (clinic != null)
        {
            clinics = new List<Clinic>() { new Clinic()
                        {
                            Id = clinic.Id,
                            CompanyId = clinic.CompanyId,
                            Name = clinic.Name,
                        }
                    };
        }

        return controller.Created(
            "CreatedClient",
            ClientMapper.ConvertToClientSignUpResponse(
                clientCreated,
                string.Empty,
                string.Empty,
                clinics));
    }

    public async Task<ActionResult<ClientSignUpResponse>> SignUp(ControllerBase controller, ClientSignUpRequest request)
    {
        if (clientRepository.ClientExist(request.Email))
        {
            return controller.Conflict();
        }

        var client = ClientMapper.ConverToClient(
            request,
            Hash.Create(request.Pass));

        var clientCreated = await clientRepository.Create(client);

        if (clientCreated == null)
        {
            return controller.BadRequest();
        }

        emailService.SendMailWelcomeClient(client.Email, client.Name, request.Pass);

        return controller.Created(
            "CreatedClient",
            ClientMapper.ConvertToClientSignUpResponse(
                client,
                jwtSecurityToken.BuildToken(client),
                jwtSecurityToken.BuildRefreshToken(client),
                Enumerable.Empty<Clinic>()));
    }

    public async Task<ActionResult<ClientSignInResponse>> SignIn(ControllerBase controller, ClientSignInRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Pass))
        {
            return controller.NotFound();
        }

        var client = await clientRepository.GetClientByEmail(request.Email);

        if (client == null)
        {
            return controller.NotFound();
        }

        if (!Hash.Validate(request.Pass, client.Pass))
        {
            return controller.NotFound();
        }

        var clinics = await clientRepository.GetClinicsForClient(client.Id);

        var response = ClientMapper.ConvertToClientSignInResponse(
            client!,
            jwtSecurityToken.BuildToken(client!),
            jwtSecurityToken.BuildRefreshToken(client!),
            clinics);

        return controller.Ok(response);
    }
}
