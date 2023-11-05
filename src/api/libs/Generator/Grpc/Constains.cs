using Microsoft.CodeAnalysis.CSharp;

using Sisa.Abstractions;

namespace Sisa.Generator.Grpc;

public static class Constants
{
    public static readonly SyntaxKind[] INVALID_MODIFIERS = [
        SyntaxKind.AbstractKeyword,
        SyntaxKind.StaticKeyword
    ];

    public const string GRPC_SERVICE = nameof(GrpcServiceAttribute);

    public const string EXTENSIONS_TEMPLATE_PATH = "Sisa.Generator.Grpc.Templates.GrpcExtensions.sbn-cs";
    public const string EXTENSIONS_OUTPUT_PATH = "GrpcExtensions.g.cs";
}
