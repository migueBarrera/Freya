namespace ApiContract.Interfaces;

public interface IHelperAPIService
{
    [Get("/api/helper/v1/reset")]
    Task ResetAllDatabase([Query] bool delete);
    
    [Get("/api/helper/v1/info")]
    Task<ApiInfo> GetApiInfo();
}
