using Microsoft.CodeAnalysis;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Scriban;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Sisa.Generator.Mediator;

[Generator]
public sealed class DispatcherExtensionsGenerator : IIncrementalGenerator
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
            (ctx, source) =>
            {
                var models = source.classes.Where(x => !string.IsNullOrEmpty(x.RequestName));

                Execute(source.compilation, models, ctx);
            });
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode syntaxNode)
    {
        return syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
            && !classDeclarationSyntax.Modifiers.Any(x => Constants.INVALID_MODIFIERS.Contains(x.Kind()))

            && classDeclarationSyntax?.BaseList?.Types.Count > 0
            && classDeclarationSyntax.BaseList.Types
                .Any(x => Constants.REQUEST_HANDLERS.Any(y => x.ToString().StartsWith(y)));
    }

    private static CollectionModel GetTargetForGeneration(GeneratorSyntaxContext context)
    {
        CollectionModel model = new();

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

        model.RequestName = arguments[0].ToDisplayString();

        if (arguments.Count() < 2)
            return model;

        model.ResponseName = arguments[1].ToDisplayString();

        return model;
    }

    private void Execute(
        Compilation compilation,
        IEnumerable<CollectionModel> collectedModels,
        SourceProductionContext context)
    {
        if (!collectedModels.Any())
            return;

        var projectName = (compilation.AssemblyName ?? string.Empty)
                .Split('.')
                .Last();

        TemplateModel commandModel = new()
        {
            ProjectName = projectName,

            Requests = collectedModels
                .Where(x => x.RequestType == RequestType.COMMAND)
                .Select(c => new RenderModel()
                {
                    RequestName = c.RequestName,
                    ResponseName = c.ResponseName,
                }).ToList(),
        };

        TemplateModel queryModel = new()
        {
            ProjectName = projectName,

            Requests = collectedModels
                .Where(x => x.RequestType == RequestType.QUERY)
                .Select(c => new RenderModel()
                {
                    RequestName = c.RequestName,
                    ResponseName = c.ResponseName,
                }).ToList(),
        };

        TemplateModel eventModel = new()
        {
            ProjectName = projectName,

            Requests = collectedModels
                .Where(x => x.RequestType == RequestType.EVENT)
                .Select(c => new RenderModel()
                {
                    RequestName = c.RequestName,
                    ResponseName = c.ResponseName,
                }).ToList(),
        };

        if (commandModel.Requests.Any())
        {
            string commandSourceCode = RenderSourceCode(Constants.COMMAND_DISPATCHER_EXTENSIONS_TEMPLATE_PATH, commandModel);
            context.AddSource(Constants.COMMAND_DISPATCHER_EXTENSIONS_OUTPUT_PATH, SourceText.From(commandSourceCode, Encoding.UTF8));
        }

        if (queryModel.Requests.Any())
        {
            string querySourceCode = RenderSourceCode(Constants.QUERY_DISPATCHER_EXTENSIONS_TEMPLATE_PATH, queryModel);
            context.AddSource(Constants.QUERY_DISPATCHER_EXTENSIONS_OUTPUT_PATH, SourceText.From(querySourceCode, Encoding.UTF8));
        }

        if (eventModel.Requests.Any())
        {
            string eventSourceCode = RenderSourceCode(Constants.EVENT_DISPATCHER_EXTENSIONS_TEMPLATE_PATH, eventModel);
            context.AddSource(Constants.EVENT_DISPATCHER_EXTENSIONS_OUTPUT_PATH, SourceText.From(eventSourceCode, Encoding.UTF8));
        }
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
