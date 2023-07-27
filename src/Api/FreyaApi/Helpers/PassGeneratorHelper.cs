namespace FreyaApi.Helpers;

public static class PassGeneratorHelper
{
    public static string Generate()
    {
        return RandomPassword();
    }

    // Generate a random password of a given length (optional)  
    private static string RandomPassword()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(RandomString(2, true));
        builder.Append(RandomNumber(100, 999));
        //builder.Append(RandomString(2, false));
        return builder.ToString();
    }

    private static int RandomNumber(int min, int max)
    {
        // Generate a random number  
        Random random = new Random();
        // Any random integer   
        return random.Next(min, max);
    }

    private static string RandomString(int size, bool lowerCase)
    {
        StringBuilder builder = new StringBuilder();
        Random random = new Random();
        char ch;
        for (int i = 0; i < size; i++)
        {
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            builder.Append(ch);
        }
        if (lowerCase)
            return builder.ToString().ToLower();
        return builder.ToString();
    }
}
