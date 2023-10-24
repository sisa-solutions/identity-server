using Microsoft.CodeAnalysis.CSharp;

using Sisa.Abstractions;

namespace Sisa.Generator.DependencyInjection;

public static class Constants
{
    public static readonly SyntaxKind[] INVALID_MODIFIERS = [
        SyntaxKind.AbstractKeyword,
        SyntaxKind.StaticKeyword];


    public static readonly string[] REGEX_CLASSES =
    [
        "^TransientService(<.*>)?$",
        "^ScopedService(<.*>)?$",
        "^SingletonService(<.*>)?$"
    ];

    public const string TRANSIENT_SERVICE = nameof(TransientServiceAttribute);
    public const string SCOPED_SERVICE = nameof(ScopedServiceAttribute);
    public const string SINGLETON_SERVICE = nameof(SingletonServiceAttribute);
    public const string KEY_ARGUMENT = "Key";

    public static readonly string[] DEPENDENCY_ATTRIBUTES =
    [
        TRANSIENT_SERVICE,
        SCOPED_SERVICE,
        SINGLETON_SERVICE
    ];

    public const string EXTENSIONS_TEMPLATE_PATH = "Sisa.Generator.DependencyInjection.Templates.DependencyInjectionExtensions.sbn-cs";
    public const string EXTENSIONS_OUTPUT_PATH = "DependenciesResolverExtensions.g.cs";
}
