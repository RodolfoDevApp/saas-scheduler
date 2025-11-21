namespace BuildingBlocks.Application;

public record Error(string Code, string Message)
{
    public static Error None => new("None", string.Empty);
}

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(string code, string message) => new(false, new Error(code, message));

    public static Result<T> Success<T>(T value) => new(value, true, Error.None);

    public static Result<T> Failure<T>(string code, string message) => new(default!, false, new Error(code, message));
}

public class Result<T> : Result
{
    private readonly T _value = default!;

    internal Result(T value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    public T Value
    {
        get
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException("Cannot access the value of a failed result.");
            }

            return _value;
        }
    }
}
