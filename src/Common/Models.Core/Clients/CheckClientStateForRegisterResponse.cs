namespace Models.Core.Clients;

public class CheckClientStateForRegisterResponse
{
    public NewClientState NewClientState{ get; set; }

    public CheckClientStateForRegisterResponse(NewClientState newClientState)
    {
        NewClientState = newClientState;
    }
}
