namespace Sisa.Generator;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class GrpcResponseAttribute(Type? singleType, Type? listType) : Attribute
{
    public Type? SingleType { get; private set; } = singleType;
    public Type? ListType { get; private set; } = listType;
}
