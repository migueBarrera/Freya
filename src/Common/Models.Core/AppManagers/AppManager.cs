namespace Models.Core.AppManagers;

public class AppManager
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Pass { get; set; } = string.Empty;
}
