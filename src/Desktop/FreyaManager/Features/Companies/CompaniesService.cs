using ApiContract.Interfaces;
using Models.Core.Common;
using Models.Core.Companies;
using Refit;

namespace FreyaManager.Features.Companies;

public class CompaniesService
{
    private readonly ICompanyAPIService companyAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly INavigationService navigationService;
    private readonly ISessionService sessionService;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly ILogger<CompaniesService> logger;

    public CompaniesService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        ILogger<CompaniesService> logger,
        ISessionService sessionService,
        INavigationService navigationService,
        ITranslatorService translatorService,
        IDialogService dialogService)
    {
        this.companyAPIService = refitService.InitRefitInstance<ICompanyAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
        this.sessionService = sessionService;
        this.navigationService = navigationService;
        this.translatorService = translatorService;
        this.dialogService = dialogService;
    }

    public async Task<bool> CreateCompany(string name, IEnumerable<Guid> subscriptionIds)
    {
        var result = await taskHelperFactory.
                            CreateInternetAccessViewModelInstance(logger).
                            TryExecuteAsync(
                            () => companyAPIService.CreateAsync(new CompanyCreateRequest()
                            {
                                Name = name,
                                SubscriptionIds = subscriptionIds,
                            }));

        return result.IsSuccess;
    }

    public async Task<PagedModel<CompanyModel>> GetCompanies(PaginationFilter paginationFilter)
    {
        var result = await taskHelperFactory.
                            CreateInternetAccessViewModelInstance(logger).
                            TryExecuteAsync(
                            () => companyAPIService.GetCompaniesAsync(paginationFilter));

        if (result.IsSuccess)
        {
            var list = PagedModel<CompanyModel>.EmptyFrom(result.Value);
            foreach (var item in result.Value.Items)
            {
                list.Items.Add(new CompanyModel(item, this.navigationService, this.sessionService));
            }

            return list;
        }
        else
        {
            return PagedModel<CompanyModel>.Empty();
        }
    }

    public async Task<CompanyDetailResponse> GetCompany(Guid id)
    {
        var result = await taskHelperFactory.
                            CreateInternetAccessViewModelInstance(logger).
                            TryExecuteAsync(
                            () => companyAPIService.GetCompanyAsync(id));

        if (result.IsSuccess)
        {
            return result.Value;
        }
        else
        {
            return new CompanyDetailResponse();
        }
    }

    public async Task<bool> UpdateCompany(Guid id, string name)
    {
        var request = new UpdateCommpanyRequest()
        {
            Name = name,
            Id = id,
        };

        var result = await taskHelperFactory.
                            CreateInternetAccessViewModelInstance(logger).
                            TryExecuteAsync(
                            () => companyAPIService.UpdateCompany(request));

        return result.IsSuccess;
    }

    public async Task<bool> DeleteCompany(Guid id)
    {
        var result = await taskHelperFactory.
                           CreateInternetAccessViewModelInstance(logger).
                           WithErrorHandling(OnDeleteCompanyError).
                           TryExecuteAsync(
                           () => companyAPIService.Delete(id));

        return result.IsSuccess;
    }

    private async Task<bool> OnDeleteCompanyError(Exception arg)
    {
        if (arg is ApiException apiException)
        {
            if (apiException.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                await dialogService.ShowMessage(
                    translatorService.Translate("error"),
                    "Debes eliminar antes todas las clinicas de la compañia");
            }
            else
            {
                await dialogService.ShowMessage(
                    translatorService.Translate("error"),
                    string.Empty);
            }
            
            return true;
        }

        return false;
    }
}
