namespace Models.Core.FAQ;

public static class FaqHelper
{
    public static string GetTitle(this FAQ fAQ)
    {
        var title = fAQ.OriginalTitle;
        var currentCulture = CultureInfo.CurrentCulture.Parent.Name;
        if (currentCulture.Equals("es", StringComparison.OrdinalIgnoreCase))
        {
            if (!string.IsNullOrWhiteSpace(fAQ.Title_es))
            {
                title = fAQ.Title_es;
            }
        }

        return title;
    }

    public static string GetDesc(this FAQ fAQ)
    {
        var desc = fAQ.OriginalDesc;
        var currentCulture = CultureInfo.CurrentCulture.Parent.Name;
        if (currentCulture.Equals("es", StringComparison.OrdinalIgnoreCase))
        {
            if (!string.IsNullOrWhiteSpace(fAQ.Desc_es))
            {
                desc = fAQ.Desc_es;
            }
        }

        return desc;
    }
}
