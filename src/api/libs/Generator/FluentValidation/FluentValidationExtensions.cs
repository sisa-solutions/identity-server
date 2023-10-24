using Microsoft.CodeAnalysis;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Scriban;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Sisa.Generator.FluentValidation;

[Generator]
public sealed class FluentValidationExtensions : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // 1. Filter step
        IncrementalValuesProvider<CollectionModel> classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (source, _) => IsSyntaxTargetForGeneration(source), // select classes that are target for generation
                transform: static (ctx, _) => GetTargetForGeneration(ctx)); // get classes with interfaces

        // 2. Combine the selected classes with the `Compilation`
        IncrementalValueProvider<(Compilation compilation, ImmutableArray<CollectionModel> classes)> compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        // 3. Generate the source using the compilation and classes
        context.RegisterSourceOutput(compilationAndClasses,
            (ctx, source) => Execute(source.classes.Where(x => !string.IsNullOrEmpty(x.InterfaceName)), ctx));
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode syntaxNode)
    {
        return syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
            && !classDeclarationSyntax.Modifiers.Any(x => Constants.INVALID_MODIFIERS.Contains(x.Kind()))

            && classDeclarationSyntax?.BaseList?.Types.Count > 0
            && classDeclarationSyntax.BaseList.Types
                .Any(x => x.ToString().StartsWith(Constants.ABSTRACT_VALIDATOR));
    }

    private static CollectionModel GetTargetForGeneration(GeneratorSyntaxContext context)
    {
        CollectionModel model = new();

        ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

        if (classSymbol is null)
            return model;

        INamedTypeSymbol? @interface = classSymbol.AllInterfaces
            .FirstOrDefault(x => x.Name.StartsWith(Constants.INTERFACE_NAME));

        if (@interface is null)
            return model;

        ITypeSymbol[] arguments = @interface.TypeArguments
            .ToArray() ?? new ITypeSymbol[0];

        if (arguments.Length < 1)
            return model;

        model.InterfaceName = $"global::{@interface.ContainingNamespace.ToDisplayString()}.{@interface.Name}";

        model.InterfaceName += $"<global::{arguments[0].ToDisplayString()}>";
        model.ImplementName = $"global::{classSymbol.ToDisplayString()}";

        return model;
    }

    private void Execute(
        IEnumerable<CollectionModel> collectedModels,
        SourceProductionContext context)
    {
        if (!collectedModels.Any())
            return;

        TemplateModel model = new()
        {
            ScopedServices = [.. collectedModels]
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
