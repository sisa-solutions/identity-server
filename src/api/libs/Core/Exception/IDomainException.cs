namespace System;

public interface IDomainException
{
    int StatusCode { get; }
    string ErrorCode { get; }
    string Message { get; }

    IDictionary<string, string[]> Errors { get; }
}
