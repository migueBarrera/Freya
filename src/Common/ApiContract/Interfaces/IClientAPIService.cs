using Models.Core.Clients;
using Models.Core.Multimedia;

namespace ApiContract.Interfaces;

public interface IClientAPIService
{
    [Post("/api/client/v1/signup")]
    Task<ClientSignUpResponse> SignUpAsync([Body] ClientSignUpRequest request);
    
    [Post("/api/client/v1/SignUpForClinic")]
    Task<ClientSignUpResponse> SignUpForClinicAsync([Body] ClientSignUpRequestForClinic request);

    [Post("/api/client/v1/signin")]
    Task<ClientSignInResponse> SignInAsync([Body] ClientSignInRequest request);

    [Get("/api/client/v1/{id}/clinic/{clinicId}")]
    Task<ClientDetailResponse> GetAsync(Guid id, Guid clinicId);

    [Post("/api/client/v1/change_password")]
    Task ChangePass([Body] ClientChangePassRequest request);

    [Post("/api/client/v1/recoverPass")]
    Task ForgotPass([Body] ClientForgotPassRequest request);

    [Post("/api/client/v1/{clientId}/refresh")]
    Task<ClientSignInResponse> RefreshClient(Guid clientId);

    [Post("/api/client/v1/checkStateForRegister")]
    Task<CheckClientStateForRegisterResponse> CheckEmployeeStateForRegister(CheckClientStateForRegisterResquest request);

    [Post("/api/client/v1/assignToClinic")]
    Task AssignToClinic(AssignClientToClinicRequest request);

    [Get("/api/client/v1/{clientId}/ultrasound/{clinicId}")]
    Task<IEnumerable<ImageMultimedia>> GetUltrasounds(Guid clientId, Guid clinicId);
    
    [Get("/api/client/v1/{clientId}/videos/{clinicId}")]
    Task<IEnumerable<VideoMultimedia>> GetVideos(Guid clientId, Guid clinicId);
    
    [Get("/api/client/v1/{clientId}/sounds/{clinicId}")]
    Task<IEnumerable<SoundMultimedia>> GetSounds(Guid clientId, Guid clinicId);

    [Patch("/api/client/v1/{clientId}")]
    Task UpdateClient(Guid clientId, [Body] ClientUpdateRequest request);


    [Delete("/api/client/v1/{clientId}/clinic/{clinicId}")]
    Task DeleteClientAsosiation(Guid clientId, Guid clinicId);

    [Post("/api/client/v1/updatePlan")]
    Task UpdateClientPlan(ClientUpdatePlanRequest request);
}
