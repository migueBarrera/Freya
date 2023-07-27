namespace Models.Core.FAQ;

public class FAQ
{
    public Guid Id { get; set; }

    public int Order { get; set; }

    public string OriginalTitle { get; set; } = string.Empty;

    public string OriginalDesc { get; set; } = string.Empty;

    public string Title_es { get; set; } = string.Empty;

    public string Desc_es { get; set; } = string.Empty;
}
