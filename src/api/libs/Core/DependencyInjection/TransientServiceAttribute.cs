namespace Sisa.Abstractions;

/// <summary>
/// Marks the class as a dependency.
/// </summary>
/// <param name="key"></param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class TransientServiceAttribute(string key = "") : Attribute
{
    /// <summary>
    /// The key of the dependency.
    /// </summary>
    public string Key { get; } = key;
}

/// <summary>
/// Marks the class as a dependency.
/// </summary>
/// <typeparam name="TServiceType"></typeparam>
/// <param name="key"></param>
/// <param name=""></param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class TransientServiceAttribute<TServiceType>(string key = "") : TransientServiceAttribute(key)
{
    /// <summary>
    /// The service type to register the dependency as.
    /// </summary>
    /// <typeparam name="TServiceType"></typeparam>
    public Type ServiceType { get; } = typeof(TServiceType);
}