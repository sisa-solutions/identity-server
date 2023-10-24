namespace Sisa.Abstractions;

/// <summary>
/// Marks the class as a dependency.
/// </summary>
/// <param name="key"></param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class SingletonServiceAttribute(string key = "") : Attribute
{
    /// <summary>
    /// The key of the dependency.
    /// </summary>
    public string Key { get; } = key;
}

/// <summary>
/// Marks the class as a dependency.
/// </summary>
/// <typeparam name="TService"></typeparam>
/// <param name="key"></param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class SingletonServiceAttribute<TService>(string key = "") : SingletonServiceAttribute(key)
{
    /// <summary>
    /// The service type to register the dependency as.
    /// </summary>
    public Type ServiceType { get; } = typeof(TService);
}