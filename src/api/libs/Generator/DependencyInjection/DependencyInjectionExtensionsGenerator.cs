using Microsoft.CodeAnalysis;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Scriban;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Sisa.Generator.DependencyInjection;

[Generator]
public sealed class DependenciesResolverExtensionsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // 1. Filter step
        IncrementalValuesProvider<IEnumerable<CollectionModel>> classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (source, _) => IsSyntaxTargetForGeneration(source), // select classes that are target for generation
                transform: static (ctx, _) => GetTargetForGeneration(ctx)); // get classes with interfaces

        // 2. Combine the selected classes with the `Compilation`
        IncrementalValueProvider<(Compilation compilation, ImmutableArray<IEnumerable<CollectionModel>> classes)> compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        // 3. Generate the source using the compilation and classes
        context.RegisterSourceOutput(compilationAndClasses,
            (ctx, source) =>
            {
                var models = source.classes.SelectMany(x => x.Select(y => y));

                Execute(source.compilation, models, ctx);
            });
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
                .Any(x => Constants.REGEX_CLASSES.Any(y => Regex.IsMatch(x, y)));
    }

    private static IEnumerable<CollectionModel> GetTargetForGeneration(GeneratorSyntaxContext context)
    {
        ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;
        List<CollectionModel> models = [];

        foreach (AttributeListSyntax attributeListSyntax in classDeclarationSyntax.AttributeLists)
        {
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                    continue;

                if (!Constants.DEPENDENCY_ATTRIBUTES.Contains(attributeSymbol.ContainingType.Name))
                    continue;

                INamedTypeSymbol? symbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

                if (symbol is null)
                    continue;

                var registerType = attributeSymbol.ContainingType.Name switch
                {
                    Constants.TRANSIENT_SERVICE => RegisterType.TRANSIENT,
                    Constants.SCOPED_SERVICE => RegisterType.SCOPED,
                    Constants.SINGLETON_SERVICE => RegisterType.SINGLETON,
                    _ => throw new NotImplementedException()
                };

                TypeSyntax[] arguments = attributeSyntax
                    .DescendantNodes()
                    .OfType<GenericNameSyntax>()?
                    .FirstOrDefault()?.TypeArgumentList.Arguments
                    .ToArray() ?? new TypeSyntax[0];

                string? key = attributeSyntax.ArgumentList?.Arguments.FirstOrDefault()?.ToString();

                CollectionModel collectionModel = new()
                {
                    ImplementName = symbol.ToDisplayString(),
                    RegisterType = registerType,
                    Key = key
                };

                // no arguments
                if (arguments.Length == 0)
                {
                    // No arguments and no interfaces
                    if (symbol.Interfaces.Length == 0)
                    {
                        // Self registration
                        models.Add(collectionModel);

                        continue;
                    }

                    // No arguments and one interface
                    if (symbol.Interfaces.Length == 1)
                    {
                        // interface registration
                        var interfaceSymbol = symbol.Interfaces[0];

                        collectionModel.InterfaceName = BuildInterfaceString(interfaceSymbol);

                        models.Add(collectionModel);

                        continue;
                    }

                    // No arguments and more than one interface
                    // Don't allow to register more than one interface without specifying the argument
                    throw new Exception("Invalid class declaration");
                }

                // Have one argument, but no interfaces ==> no need specify argument
                if (symbol.Interfaces.Length == 0)
                {
                    throw new Exception("Invalid class declaration");
                }

                // Have one argument, and one interface
                // Need to check if the argument is the same as the interface
                // Or the argument is the same as the class implements the interface
                // Or the argument are not the same as both ==> throw exception
                if (symbol.Interfaces.Length > 0)
                {
                    string argumentName = arguments[0].ToString();

                    if (symbol.Name == argumentName)
                    {
                        // self registration
                        models.Add(collectionModel);

                        continue;
                    }

                    var interfaceSymbol = symbol.Interfaces.FirstOrDefault(x => x.Name == argumentName);

                    if (interfaceSymbol is not null)
                    {
                        // interface registration
                        collectionModel.InterfaceName = BuildInterfaceString(interfaceSymbol);

                        models.Add(collectionModel);

                        continue;
                    }

                    throw new Exception("Invalid class declaration");
                }
            }
        }

        return models;
    }

    private static string BuildInterfaceString(INamedTypeSymbol symbol)
    {
        var interfaceName = $"{symbol.ContainingNamespace.ToDisplayString()}.{symbol.Name}";

        if (symbol.TypeArguments.Length > 0)
        {
            interfaceName += $"<{string.Join(", ", symbol.TypeArguments.Select(x => $"global::{x.ToDisplayString()}"))}>";
        }

        return interfaceName;
    }

    private void Execute(
        Compilation compilation,
        IEnumerable<CollectionModel> collectedModels,
        SourceProductionContext context)
    {
        if (!collectedModels.Any())
            return;

        TemplateModel model = new()
        {
            ProjectName = (compilation.AssemblyName ?? string.Empty)
                .Split('.')
                .Last(),

            TransientServices = collectedModels
                .Where(c => c.RegisterType == RegisterType.TRANSIENT)
                .Select(c => new RenderModel()
                {
                    InterfaceName = c.InterfaceName,
                    ImplementName = c.ImplementName,
                    Key = c.Key
                }).ToList(),

            ScopedServices = collectedModels
                .Where(c => c.RegisterType == RegisterType.SCOPED)
                .Select(c => new RenderModel()
                {
                    InterfaceName = c.InterfaceName,
                    ImplementName = c.ImplementName,
                    Key = c.Key
                }).ToList(),

            SingletonServices = collectedModels
                .Where(c => c.RegisterType == RegisterType.SINGLETON)
                .Select(c => new RenderModel()
                {
                    InterfaceName = c.InterfaceName,
                    Key = c.Key
                }).ToList()
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
