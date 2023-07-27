namespace FreyaApi.Infrastructure.Mappers;

public static class ClientMapper
{
    public static ClientDetailResponse ConvertToDetailResponse(
       ClientTable client,
       ClinicClient clinicRelation,
       long size)
    {
        return new ClientDetailResponse()
        {
            Email = client.Email,
            Id = client.Id,
            LastName = client.LastName,
            Name = client.Name,
            Phone = client.Phone,
            Size = size,
            SubscriptionPlanSize = clinicRelation.SubscriptionPlanSize,
            Created = clinicRelation.Created,
        };
    }

    public static ClientResponse ConverToClientResponse(ClientTable client)
    {
        return new ClientResponse()
        {
            Name = client.Name,
            Email = client.Email,
            LastName = client.LastName,
            Phone = client.Phone,
            Id = client.Id,
        };
    }   
    
    public static ClientTable ConverToClient(ClientSignUpRequest request, string hashedPass)
    {
        return new ClientTable()
        {
            Name = request.Name,
            Pass = hashedPass,
            Email = request.Email,
            LastName = request.LastName,
            Phone = request.Phone,
        };
    }
    
    public static ClientTable ConverToClient(ClientSignUpRequestForClinic request, string hashedPass)
    {
        return new ClientTable()
        {
            Name = request.Name,
            Pass = hashedPass,
            Email = request.Email,
            LastName = request.LastName,
            Phone = request.Phone,
        };
    }

    public static ClientSignUpResponse ConvertToClientSignUpResponse(
        ClientTable client, 
        string token, 
        string refreshToken, 
        IEnumerable<Clinic> enumerable)
    {
        return new ClientSignUpResponse()
        {
            Id = client.Id,
            Email = client.Email,
            Name = client.Name,
            Phone = client.Phone,
            LastName = client.LastName,
            Clinics = enumerable,
            Token = token,
            RefreshToken = refreshToken,
        };
    }
    
    public static ClientSignInResponse ConvertToClientSignInResponse(
        ClientTable client, 
        string token, 
        string refreshToken, 
        IEnumerable<ClinicTable> enumerable)
    {
        return new ClientSignInResponse()
        {
            Id = client.Id,
            Email = client.Email,
            Name = client.Name,
            Phone = client.Phone,
            LastName = client.LastName,
            Clinics = enumerable.Select(x => ClinicMapper.ConverTo(x)),
            Token = token,
            RefreshToken = refreshToken,
            Created = client.Created,
        };
    }
}
