using GrpcStatusCode = Grpc.Core.StatusCode;

namespace Sisa.Grpc.Helpers;

public static partial class StatusCodeHelper
{
    public static GrpcStatusCode ToGrpcStatusCode(this int statusCode)
    {
        return statusCode switch
        {
            400 => GrpcStatusCode.InvalidArgument,
            401 => GrpcStatusCode.Unauthenticated,
            402 => GrpcStatusCode.PermissionDenied,
            403 => GrpcStatusCode.PermissionDenied,
            404 => GrpcStatusCode.NotFound,
            409 => GrpcStatusCode.AlreadyExists,
            500 => GrpcStatusCode.Internal,
            _ => GrpcStatusCode.Unknown,
        };
    }

    public static GrpcStatusCode ToGrpcStatusCode(this IDomainException domainException)
    {
        return domainException.StatusCode.ToGrpcStatusCode();
    }
}
