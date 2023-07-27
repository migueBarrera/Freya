using OperationResult;
using Refit;
using static OperationResult.Helpers;

namespace FreyaMobile.Features.Clients.Services;

public class EditClientService : IEditClientService
{
    private readonly IClientDataService clientDataService;
    private readonly DialogService dialogService;

    public EditClientService(DialogService dialogService, IClientDataService clientDataService)
    {
        this.dialogService = dialogService;
        this.clientDataService = clientDataService;
    }

    public async Task<Result<bool>> UpdateUser(UpdateUserValidatable updateUser)
    {
        var request = new ClientUpdateRequest()
        {
            Name = updateUser.Name.Value,
            LastName = updateUser.LastName.Value,
            Phone = updateUser.Phone.Value,
            PregnancyDate = CalculatePregnancyDateFromWeeks(updateUser.Weeks),
        };

        var userResponse = await clientDataService.UpdateUser(request, OnUpdateUserErrorHandler);

        if (userResponse.IsError)
        {
            return Error();
        }

        return true;
    }

    private async Task<bool> OnUpdateUserErrorHandler(Exception exception)
    {
        if (exception is ApiException apiException && apiException.StatusCode == System.Net.HttpStatusCode.NotFound)
{
            await dialogService.DisplayAlert("Algo salio mal", string.Empty);
            return true;
        }
        else
        {
            return false;
        }
    }

    //TODO MOVER FUERA
    private static DateTime? CalculatePregnancyDateFromWeeks(int weeks)
    {
        if (weeks < 1)
        {
            return null;
        }

        var today = DateTime.UtcNow;
        return today.AddDays(-(weeks * 7));
    }
}
