using Microsoft.CodeAnalysis;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Scriban;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Text.RegularExpressions;

namespace Sisa.Generator.Mediator;

[Generator]
public sealed class MediatorExtensionsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // 1. Filter step
        IncrementalValuesProvider<DependencyCollectionModel> classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (source, _) => IsSyntaxTargetForGeneration(source), // select classes that are target for generation
                transform: static (ctx, _) => GetTargetForGeneration(ctx)); // get classes with interfaces

        // 2. Combine the selected classes with the `Compilation`
        IncrementalValueProvider<(Compilation compilation, ImmutableArray<DependencyCollectionModel> classes)> compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        // 3. Generate the source using the compilation and classes
        context.RegisterSourceOutput(compilationAndClasses,
            (ctx, source) => Execute(source.compilation, source.classes, ctx));
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode syntaxNode)
    {
        return syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
            && !classDeclarationSyntax.Modifiers.Any(x => Constants.INVALID_MODIFIERS.Contains(x.Kind()))

            && !classDeclarationSyntax.AttributeLists
                .SelectMany(x => x.Attributes)
                .Select(x => x.Name.ToString())
                .Any(x => DependencyInjection.Constants.REGEX_CLASSES.Any(y => Regex.IsMatch(x, y)))

            && classDeclarationSyntax?.BaseList?.Types.Count > 0
            && classDeclarationSyntax.BaseList.Types
                .Any(x => Constants.REQUEST_HANDLERS.Any(y => x.ToString().StartsWith(y)));
    }

    private static DependencyCollectionModel GetTargetForGeneration(GeneratorSyntaxContext context)
    {
        DependencyCollectionModel model = new();

        ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

        if (classSymbol is null)
            return model;

        INamedTypeSymbol? @interface = classSymbol.Interfaces
            .FirstOrDefault(x => Constants.REQUEST_HANDLERS.Any(y => x.Name == y));

        if (@interface is null)
            return model;


        var requestType = @interface.Name switch
        {
            Constants.COMMAND_HANDLER => RequestType.COMMAND,
            Constants.QUERY_HANDLER => RequestType.QUERY,
            Constants.EVENT_HANDLER => RequestType.EVENT,
            _ => throw new NotImplementedException()
        };

        model.RequestType = requestType;

        ITypeSymbol[] arguments = @interface
            .TypeArguments
            .ToArray() ?? new ITypeSymbol[0];

        if (arguments.Count() < 1)
            return model;

        model.InterfaceName = $"global::{@interface.ContainingNamespace.ToDisplayString()}.{@interface.Name}";

        model.InterfaceName += $"<{string.Join(", ", arguments.Select(x => $"global::{x.ToDisplayString()}"))}>";
        model.ImplementName = $"global::{classSymbol.ToDisplayString()}";

        return model;
    }

    private void Execute(
        Compilation compilation,
        IEnumerable<DependencyCollectionModel> collectedModels,
        SourceProductionContext context)
    {
        if (!collectedModels.Any())
            return;

        var projectName = (compilation.AssemblyName ?? string.Empty)
                .Split('.')
                .Last();

        DependencyTemplateModel model = new()
        {
            ProjectName = projectName,

            Commands = collectedModels
                .Where(c => c.RequestType == RequestType.COMMAND)
                .Select(c => new DependencyRenderModel()
                {
                    InterfaceName = c.InterfaceName,
                    ImplementName = c.ImplementName,
                }).ToList(),

            Queries = collectedModels
                .Where(c => c.RequestType == RequestType.QUERY)
                .Select(c => new DependencyRenderModel()
                {
                    InterfaceName = c.InterfaceName,
                    ImplementName = c.ImplementName,
                }).ToList(),

            Events = collectedModels
                .Where(c => c.RequestType == RequestType.EVENT)
                .Select(c => new DependencyRenderModel()
                {
                    InterfaceName = c.InterfaceName,
                    ImplementName = c.ImplementName,
                }).ToList()
        };

        string sourceCode = RenderSourceCode(Constants.MEDIATOR_EXTENSIONS_TEMPLATE_PATH, model);

        context.AddSource(Constants.MEDIATOR_EXTENSIONS_OUTPUT_PATH, SourceText.From(sourceCode, Encoding.UTF8));
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
