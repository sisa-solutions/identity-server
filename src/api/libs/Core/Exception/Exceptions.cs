namespace System;

public class BadRequestException : DomainException
{
    public BadRequestException(string errorCode, string message)
        : base(StatusCode.BAD_REQUEST, errorCode, message)
    {
    }

    public BadRequestException(string errorCode, string message, IDictionary<string, string[]> errors)
        : base(StatusCode.BAD_REQUEST, errorCode, message, errors)
    {
    }
}

public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string errorCode, string message)
        : base(StatusCode.UNAUTHORIZED, errorCode, message)
    {
    }

    public UnauthorizedException(string errorCode, string message, IDictionary<string, string[]> errors)
        : base(StatusCode.UNAUTHORIZED, errorCode, message, errors)
    {
    }
}

public class PaymentRequiredException : DomainException
{
    public PaymentRequiredException(string errorCode, string message)
        : base(StatusCode.PAYMENT_REQUIRED, errorCode, message)
    {
    }

    public PaymentRequiredException(string errorCode, string message, IDictionary<string, string[]> errors)
        : base(StatusCode.PAYMENT_REQUIRED, errorCode, message, errors)
    {
    }
}

public class ForbiddenException : DomainException
{
    public ForbiddenException(string errorCode, string message)
        : base(StatusCode.FORBIDDEN, errorCode, message)
    {
    }

    public ForbiddenException(string errorCode, string message, IDictionary<string, string[]> errors)
        : base(StatusCode.FORBIDDEN, errorCode, message, errors)
    {
    }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string errorCode, string message)
        : base(StatusCode.NOT_FOUND, errorCode, message)
    {
    }

    public NotFoundException(string errorCode, string message, IDictionary<string, string[]> errors)
        : base(StatusCode.NOT_FOUND, errorCode, message, errors)
    {
    }
}

public class ConflictException : DomainException
{
    public ConflictException(string errorCode, string message)
        : base(StatusCode.CONFLICT, errorCode, message)
    {
    }

    public ConflictException(string errorCode, string message, IDictionary<string, string[]> errors)
        : base(StatusCode.CONFLICT, errorCode, message, errors)
    {
    }
}

public class InternalServerErrorException : DomainException
{
    public InternalServerErrorException(string errorCode, string message)
        : base(StatusCode.INTERNAL_SERVER_ERROR, errorCode, message)
    {
    }

    public InternalServerErrorException(string errorCode, string message, IDictionary<string, string[]> errors)
        : base(StatusCode.INTERNAL_SERVER_ERROR, errorCode, message, errors)
    {
    }
}
