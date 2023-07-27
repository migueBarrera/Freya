namespace FreyaApi.Repository.Interfaces;

public interface IHelperReposiory
{
    Task<bool> RecreateDatabase(bool delete);
}
