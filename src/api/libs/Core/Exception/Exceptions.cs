namespace System;

public class BadRequestException : DomainException
{
    public BadRequestException(string errorCode, string message)
        : base(400, errorCode, message)
    {
    }

    public BadRequestException(string errorCode, string message, IDictionary<string, string[]> errors)
        : base(400, errorCode, message, errors)
    {
    }
}

public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string errorCode, string message)
        : base(401, errorCode, message)
    {
    }

    public UnauthorizedException(string errorCode, string message, IDictionary<string, string[]> errors)
        : base(401, errorCode, message, errors)
    {
    }
}

public class PaymentRequiredException : DomainException
{
    public PaymentRequiredException(string errorCode, string message)
        : base(402, errorCode, message)
    {
    }

    public PaymentRequiredException(string errorCode, string message, IDictionary<string, string[]> errors)
        : base(402, errorCode, message, errors)
    {
    }
}

public class ForbiddenException : DomainException
{
    public ForbiddenException(string errorCode, string message)
        : base(403, errorCode, message)
    {
    }

    public ForbiddenException(string errorCode, string message, IDictionary<string, string[]> errors)
        : base(403, errorCode, message, errors)
    {
    }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string errorCode, string message)
        : base(404, errorCode, message)
    {
    }

    public NotFoundException(string errorCode, string message, IDictionary<string, string[]> errors)
        : base(404, errorCode, message, errors)
    {
    }
}

public class ConflictException : DomainException
{
    public ConflictException(string errorCode, string message)
        : base(409, errorCode, message)
    {
    }

    public ConflictException(string errorCode, string message, IDictionary<string, string[]> errors)
        : base(409, errorCode, message, errors)
    {
    }
}

public class InternalServerErrorException : DomainException
{
    public InternalServerErrorException(string errorCode, string message)
        : base(500, errorCode, message)
    {
    }

    public InternalServerErrorException(string errorCode, string message, IDictionary<string, string[]> errors)
        : base(500, errorCode, message, errors)
    {
    }
}
