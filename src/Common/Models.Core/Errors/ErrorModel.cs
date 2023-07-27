namespace Models.Core.Errors;

public class ErrorModel
{
    public ErrorModel(ErrorType errorCode)
    {
        ErrorCode = errorCode;
    }

    public ErrorType ErrorCode { get; set; }
}
