using System.Text;

namespace Models.Core.Common;

public class ApiInfo
{
    public string Version { get; set; } = string.Empty;

    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.AppendLine($"Version: {Version}");

        return builder.ToString();
    }
}
