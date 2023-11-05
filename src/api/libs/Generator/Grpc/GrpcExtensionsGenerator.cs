using Microsoft.CodeAnalysis;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Scriban;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Sisa.Generator.Grpc;

[Generator]
public sealed class GrpcExtensionsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // 1. Filter step
        IncrementalValuesProvider<string> classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (source, _) => IsSyntaxTargetForGeneration(source), // select classes that are target for generation
                transform: static (ctx, _) => GetTargetForGeneration(ctx)); // get classes with interfaces

        // 2. Combine the selected classes with the `Compilation`
        IncrementalValueProvider<(Compilation compilation, ImmutableArray<string> classes)> compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        // 3. Generate the source using the compilation and classes
        context.RegisterSourceOutput(compilationAndClasses,
            (ctx, source) => Execute(source.classes.Where(x => !string.IsNullOrEmpty(x)), ctx));
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode syntaxNode)
    {
        // TODO: Better validation
        return syntaxNode is ClassDeclarationSyntax { AttributeLists.Count: > 0 } classDeclarationSyntax

            && !classDeclarationSyntax.Modifiers
                .Any(x => Constants.INVALID_MODIFIERS.Contains(x.Kind()))

            && classDeclarationSyntax.AttributeLists
                .SelectMany(x => x.Attributes)
                .Select(x => x.Name.ToString())
                .Any(x => $"{x}Attribute" == Constants.GRPC_SERVICE);
    }

    private static string GetTargetForGeneration(GeneratorSyntaxContext context)
    {
        ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

        if (classSymbol is null)
            return string.Empty;

        return classSymbol.ToDisplayString();


        // foreach (AttributeListSyntax attributeListSyntax in classDeclarationSyntax.AttributeLists)
        // {
        //     var attr = attributeListSyntax.Attributes.FirstOrDefault(x => x.Name.ToString() == Constants.GRPC_SERVICE);

        //     if(attr is null)
        //         continue;

        //     return attr.ToFullString();

        //     // foreach (var attributeSyntax in attributeListSyntax.Attributes)
        //     // {
        //     //     if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
        //     //         continue;

        //     //     if (attributeSymbol.ContainingType.Name != Constants.GRPC_SERVICE)
        //     //         continue;

        //     //     INamedTypeSymbol? symbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

        //     //     if (symbol is null)
        //     //         continue;

        //     //     return symbol.ToDisplayString();
        //     // }
        // }
    }

    private void Execute(
        IEnumerable<string> collectedModels,
        SourceProductionContext context)
    {
        if (!collectedModels.Any())
            return;

        TemplateModel model = new()
        {
            Services = [.. collectedModels]
        };

        string sourceCode = RenderSourceCode(Constants.EXTENSIONS_TEMPLATE_PATH, model);

        context.AddSource(Constants.EXTENSIONS_OUTPUT_PATH, SourceText.From(sourceCode, Encoding.UTF8));
    }

    private string RenderSourceCode(string templatePath, object? model = null)
    {
        using var resourceStream = GetType().Assembly.GetManifestResourceStream(templatePath);
        using var streamReader = new StreamReader(resourceStream!);

        var template = Template.Parse(streamReader.ReadToEnd());
        var sourceCode = template.Render(model);

        return sourceCode ?? string.Empty;
    }
}
