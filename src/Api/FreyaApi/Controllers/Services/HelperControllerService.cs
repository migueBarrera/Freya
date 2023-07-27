using System.Reflection;

namespace FreyaApi.Controllers.Services;

public class HelperControllerService
{
    private IHostEnvironment _hostingEnv;
    private readonly IHelperReposiory helperReposiory;
    private readonly IAppManagerRepository appManagerRepository;

    public HelperControllerService(
        IHelperReposiory helperReposiory,
        IAppManagerRepository appManagerRepository,
        IHostEnvironment hostingEnv)
    {
        this.helperReposiory = helperReposiory;
        this.appManagerRepository = appManagerRepository;
        _hostingEnv = hostingEnv;
    }

    public async Task<ActionResult> RecreateDatabase(ControllerBase controller, bool delete)
    {
        try
        {
            var recreated = await helperReposiory.RecreateDatabase(delete);
            if (recreated)
            {
                await appManagerRepository.Create(new AppManagerTable()
                {
                    Email = "mbmdevelop@gmail.com",
                    Pass = Hash.Create("123456"),
                });
                await appManagerRepository.Create(new AppManagerTable()
                {
                    Email = "gutimoreno90@gmail.com",
                    Pass = Hash.Create("123456"),
                });
            }
        }
        catch (Exception e)
        {
            return controller.BadRequest(e);
        }

        return controller.Ok();
    }

    internal ActionResult<ApiInfo> Info(HelperController helperController)
    {
        var version = GetAppVersion();

        var versionText = $"{version.Major}.{version.Minor}.{version.Build}";
        if (_hostingEnv.IsDevelopment())
        {
            versionText = $"{versionText} Dev";
        }

        return helperController.Ok(new ApiInfo()
        {
            Version = versionText,
        });
    }

    private Version GetAppVersion()
    {
        Version defaultVersion = new Version(1, 0, 0, 0);

        var assembly = Assembly.GetAssembly(typeof(HelperControllerService));
        var version = assembly?.GetName().Version;
        return version ?? defaultVersion;
    }
}
