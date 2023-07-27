namespace ApiContract;

public static class ApiExceptionHelper
{
    public static bool IsACustomException(Exception exception, out ErrorModel errorModel)
    {
        errorModel = new ErrorModel(ErrorType.INVALID_PARAMETERS);

        if (exception is ApiException apiException)
        {
            try
            {
                var regex = new Regex(string.Join("|", Enum.GetNames(typeof(ErrorType))));

                if (regex.IsMatch(apiException.Content!))
                {
                    errorModel.ErrorCode = (ErrorType)Enum.Parse(typeof(ErrorType), regex.Match(apiException.Content!).Value);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        return false;
    }
}
