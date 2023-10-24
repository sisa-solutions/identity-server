namespace System;

public interface IDomainException
{
    StatusCode StatusCode { get; }
    string ErrorCode { get; }
    string Message { get; }

    IDictionary<string, string[]> Errors { get; }
}
