using FluentValidation;
using FluentValidation.Results;

using Grpc.Core;
using Grpc.Core.Interceptors;
using GrpcStatusCode = Grpc.Core.StatusCode;

using Microsoft.Extensions.Logging;

using Sisa.Grpc.Helpers;
using Sisa.Extensions;
using System.Text.Json;

namespace Sisa.Grpc.Interceptors;

public class ExceptionInterceptor(
    IServiceProvider serviceProvider,
    ILogger<ExceptionInterceptor> logger) : Interceptor
{
    public override TResponse BlockingUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        string requestName = request.GetGenericTypeName();

        try
        {
            logger.LogInformation("Handling BlockingUnary request {requestName} ({@request})", requestName, request);

            Validate(request);

            TResponse response = base.BlockingUnaryCall(request, context, continuation);

            logger.LogInformation("BlockingUnary request {request} handled - response: ({@response})", requestName, response);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling BlockingUnary request {requestName} ({@request})", requestName, request);

            throw HandleException(ex);
        }
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        string requestName = request.GetGenericTypeName();

        try
        {
            logger.LogInformation("Handling UnaryServer request {requestName} ({@request})", requestName, request);

            await ValidateAsync(request, context.CancellationToken);

            TResponse response = await base.UnaryServerHandler(request, context, continuation);

            logger.LogInformation("UnaryServer request {requestName} handled - response: ({@response})", requestName, response);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling UnaryServer request {requestName} ({@request})", requestName, request);

            throw HandleException(ex);
        }
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        string requestName = request.GetGenericTypeName();

        try
        {
            logger.LogInformation("Handling AsyncUnary request {requestName} ({@request})", requestName, request);

            Validate(request);

            var response = base.AsyncUnaryCall(request, context, continuation);

            logger.LogInformation("AsyncUnary request {request} handled", requestName);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling AsyncUnary request {requestName} ({@request})", requestName, request);

            throw HandleException(ex);
        }
    }

    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        string requestName = typeof(TRequest).GetGenericTypeName();

        try
        {
            logger.LogInformation("Handling ClientStreamingServer request {requestName}", requestName);

            TResponse response = await base.ClientStreamingServerHandler(requestStream, context, continuation);

            logger.LogInformation("ClientStreamingServer request {requestName} handled", requestName);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling ClientStreamingServer request {requestName}", requestName);

            throw HandleException(ex);
        }
    }

    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        string requestName = typeof(TRequest).GetGenericTypeName();

        try
        {
            logger.LogInformation("Handling AsyncClientStreaming request {requestName}", requestName);

            var response = base.AsyncClientStreamingCall(context, continuation);

            logger.LogInformation("AsyncClientStreaming request {requestName} handled", requestName);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling AsyncClientStreaming request {requestName}", requestName);

            throw HandleException(ex);
        }
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        string requestName = request.GetGenericTypeName();

        try
        {
            logger.LogInformation("Handling ServerStreamingServer request {requestName} ({@request})", requestName, request);

            await ValidateAsync(request);

            await base.ServerStreamingServerHandler(request, responseStream, context, continuation);

            logger.LogInformation("ServerStreamingServer request {requestName} handled", requestName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling ServerStreamingServer request {requestName} ({@request})", requestName, request);

            throw HandleException(ex);
        }
    }

    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        string requestName = request.GetGenericTypeName();

        try
        {
            logger.LogInformation("Handling AsyncServerStreaming request {requestName} ({@request})", requestName, request);

            Validate(request);

            var response = base.AsyncServerStreamingCall(request, context, continuation);

            logger.LogInformation("AsyncServerStreaming request {requestName} handled", requestName);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling AsyncServerStreaming request {requestName} ({@request})", requestName, request);

            throw HandleException(ex);
        }
    }

    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        string requestName = typeof(TResponse).GetGenericTypeName();

        try
        {
            logger.LogInformation("Handling DuplexStreamingServer request {requestName}", requestName);

            await base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);

            logger.LogInformation("DuplexStreamingServer request {requestName} handled", requestName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling DuplexStreaming request {requestName}", typeof(TRequest).GetGenericTypeName());

            throw HandleException(ex);
        }
    }

    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var requestName = typeof(TRequest).GetGenericTypeName();

        try
        {
            logger.LogInformation("Handling AsyncDuplexStreaming request {requestName}", requestName);

            var response = base.AsyncDuplexStreamingCall(context, continuation);

            logger.LogInformation("AsyncDuplexStreaming request {requestName} handled", requestName);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling AsyncDuplexStreaming request {requestName}", requestName);

            throw HandleException(ex);
        }
    }

    private void Validate<TRequest>(TRequest request)
    {
        IValidator<TRequest>? validator = serviceProvider.GetService(typeof(IValidator<TRequest>)) as IValidator<TRequest>;

        if (validator is not null)
        {
            var requestName = request!.GetGenericTypeName();

            logger.LogInformation("Validating request {requestName} ({@request})", requestName, request);

            var validationResult = validator!.Validate(request);

            logger.LogInformation("Request {requestName} validated - result ({@validationResult})", requestName, validationResult);

            HandleValidationException(validationResult);
        }
    }

    private async Task ValidateAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default)
    {
        IValidator<TRequest>? validator = serviceProvider.GetService(typeof(IValidator<TRequest>)) as IValidator<TRequest>;

        if (validator is not null)
        {
            var requestName = request!.GetGenericTypeName();

            logger.LogInformation("Validating request {requestName} ({@request})", requestName, request);

            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            logger.LogInformation("Request {requestName} validated - result ({@validationResult})", requestName, validationResult);

            HandleValidationException(validationResult);
        }
    }

    private static void HandleValidationException(ValidationResult validationResult)
    {
        if (validationResult.IsValid)
            return;

        throw new DomainException(400, "400", "One or more validation errors occurred", validationResult.ToDictionary());
    }

    private static RpcException HandleException(Exception ex)
    {
        RpcException rpcException;

        if (ex is IDomainException domainException)
        {
            rpcException = new RpcException(
                new Status(domainException.ToGrpcStatusCode(), domainException.Message, ex),
                new Metadata
                {
                    { "x-error-code", domainException.ErrorCode },
                    { "x-errors", JsonSerializer.Serialize(domainException.Errors) },
                }
            );
        }
        else
        {
            rpcException = new RpcException(
                new Status(GrpcStatusCode.Internal, "An error occurred while processing the request", ex),
                new Metadata
                {
                { "x-error-code", "500" },
                }
            );
        }

        return rpcException;
    }
}
