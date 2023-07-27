namespace FreyaApi.Infrastructure.Models;

[Table("AppManagers")]
public class AppManagerTable
{
    [Key]
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Pass { get; set; } = string.Empty;
}
