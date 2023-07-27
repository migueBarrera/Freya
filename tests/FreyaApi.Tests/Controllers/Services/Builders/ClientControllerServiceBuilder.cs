namespace FreyaApi.Tests.Controllers.Services.Builders;

internal class ClientControllerServiceBuilder
{
    private JwtSecurityTokenService jwtService;
    private Mock<IClientRepository> clientRepository;
    private Mock<IEmailService> emailService;
    private Mock<IClinicRepository> clinicRepository;
    private Mock<IImageRepository> imageRepository;
    private Mock<IVideoRepository> videoRepository;
    private Mock<ISoundRepository> soundRepository;
    private Mock<MultimediaService> multimediaService;
    private Mock<ISubscriptionPaymentRepository> subscriptionPaymentRepository;

    private List<ClientTable> clients = new List<ClientTable>();

    public ClientControllerServiceBuilder()
    {

        jwtService = new JwtSecurityTokenServiceBuilder().Build();
        emailService = new Mock<IEmailService>();
        clinicRepository = new Mock<IClinicRepository>();
        soundRepository = new Mock<ISoundRepository>();
        videoRepository = new Mock<IVideoRepository>();
        imageRepository = new Mock<IImageRepository>();
        multimediaService = new Mock<MultimediaService>(null!, null!, null!, null!);
        clientRepository = new Mock<IClientRepository>();
        subscriptionPaymentRepository = new Mock<ISubscriptionPaymentRepository>();
    }

    internal ClientControllerService Build()
    {
        if (clients.Any())
        {
            foreach (ClientTable client in clients)
            {
                clientRepository
                    .Setup(x => x.GetClientByEmail(It.Is<string>(s => s.Equals(client.Email))))
                    .ReturnsAsync(client);

                clientRepository
                    .Setup(x => x.ClientExist(It.Is<string>(s => s.Equals(client.Email))))
                    .Returns(true);
            }
        }

        clientRepository
                    .Setup(x => x.Create(It.IsAny<ClientTable>()))
                    .ReturnsAsync(new ClientTable());

        return new ClientControllerService(
            clientRepository.Object,
            jwtService,
            emailService.Object,
            multimediaService.Object,
            clinicRepository.Object,
            subscriptionPaymentRepository.Object,
            videoRepository.Object,
            imageRepository.Object,
            soundRepository.Object);
    }

    internal ClientControllerServiceBuilder WithClient(string email, string pass)
    {
        clients.Add(new ClientTable()
        {
            Id = Guid.NewGuid(),
            Email = email,
            Pass = pass,
        });

        return this;
    }
}
