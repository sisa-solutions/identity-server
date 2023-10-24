using Microsoft.CodeAnalysis.CSharp;

namespace Sisa.Generator.FluentValidation;

public static class Constants
{
    public static readonly SyntaxKind[] INVALID_MODIFIERS = [
        SyntaxKind.AbstractKeyword,
        SyntaxKind.StaticKeyword];


    public static readonly string[] REGEX_CLASSES =
    [
        "^AbstractValidator(<.*>)?$"
    ];

    public const string ABSTRACT_VALIDATOR = "AbstractValidator";
    public const string INTERFACE_NAME = "IValidator";
    public const string NAMESPACE = "FluentValidation";

    public static readonly string[] DEPENDENCY_ATTRIBUTES =
    [
        ABSTRACT_VALIDATOR,
    ];

    public const string EXTENSIONS_TEMPLATE_PATH = "Sisa.Generator.FluentValidation.Templates.FluentValidationExtensions.sbn-cs";
    public const string EXTENSIONS_OUTPUT_PATH = "FluentValidationExtensions.g.cs";
}
