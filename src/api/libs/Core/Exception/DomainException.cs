namespace System;

public class DomainException(int statusCode, string errorCode, string message)
    : Exception(message), IDomainException
{
    public int StatusCode { get; init; } = statusCode;
    public string ErrorCode { get; init; } = errorCode;

    public IDictionary<string, string[]> Errors { get; init; } = new Dictionary<string, string[]>();

    public DomainException(int statusCode, string errorCode, string message, IDictionary<string, string[]> errors)
        : this(statusCode, errorCode, message)
    {
        Errors = errors;
    }
}
