namespace Freya.Validations;

public static class ValidateHelper
{
    public static bool IsValidPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            return false;
        }

        // TODO
        // var regex = @"^(?<countryCode>[\+][1-9]{1}[0-9]{0,2}\s)?(?<areaCode>0?[1-9]\d{0,4})(?<number>\s[1-9][\d]{5,12})(?<extension>\sx\d{0,4})?$";
        // return Regex.IsMatch(phone, regex, RegexOptions.Compiled);
        return true;
    }

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            // Normalize the domain
            email = Regex.Replace(
                email,
                @"(@)(.+)$",
                DomainMapper,
                RegexOptions.Compiled,
                TimeSpan.FromMilliseconds(200));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(
                email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))"
                + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    private static string DomainMapper(Match match)
    {
        // Use IdnMapping class to convert Unicode domain names.
        var idn = new IdnMapping();

        // Pull out and process domain name (throws ArgumentException on invalid)
        var domainName = idn.GetAscii(match.Groups[2].Value);

        return match.Groups[1].Value + domainName;
    }
}
