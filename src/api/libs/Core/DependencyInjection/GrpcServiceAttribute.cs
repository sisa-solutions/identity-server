namespace Sisa.Abstractions;

/// <summary>
/// Marks the class as a grpc service.
/// </summary>
/// <param name="key"></param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class GrpcServiceAttribute() : Attribute
{
}
