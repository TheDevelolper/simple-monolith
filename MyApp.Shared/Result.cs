namespace MyApp.Shared;

/// <summary>
/// Class representing results that do not have data.
/// </summary>
/// <param name="Status">flag indicating the status of an operation</param>
/// <param name="ErrorMessage"></param>
public record Result(ResultStatus Status = ResultStatus.Error, string? ErrorMessage = "An Unknown error occured")
{
    
    public static Result Failure(ResultStatus status, string? errorMessage)
    {
        return new Result(status, errorMessage);
    }
    
    public static Result Success()
    {
        return new Result(ResultStatus.Success);
    }
    
}

/// <summary>
/// Class representing results which contain data.
/// </summary>
/// <param name="Status">flag indicating the status of an operation</param>
/// <param name="Value">the result value</param>
/// <param name="ErrorMessage">any error messages to be returned</param>
/// <typeparam name="T"></typeparam>
public record Result<T>(ResultStatus Status, T? Value, string? ErrorMessage = ""): Result(Status, ErrorMessage: ErrorMessage)
{
    //todo: Consider adding a technical error (for logging) and a user friendly error.
    public new static Result<T> Failure(ResultStatus status, string? errorMessage = "")
    {
        return new Result<T>(status, default, errorMessage);
    }
    
    public static Result<T> Success(T value)
    {
        return new Result<T>(ResultStatus.Success, value);
    }
}

public enum ResultStatus
{
    Success,
    NotFound,
    ValidationError,
    Conflict,
    Unauthorized,
    Error
}