namespace ApiContract.Refit;

public interface IRefitService
{
    T InitRefitInstance<T>(bool isAutenticated = false);
}