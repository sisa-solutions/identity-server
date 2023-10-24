using Microsoft.CodeAnalysis.CSharp;

using Sisa.Abstractions;

namespace Sisa.Generator.Mediator;

public static class Constants
{
    public static readonly SyntaxKind[] INVALID_MODIFIERS = [
        SyntaxKind.AbstractKeyword,
        SyntaxKind.StaticKeyword];

    public static readonly string[] REGEX_CLASSES =
    [
        "ICommandHandler<w+,s*w+>",
    ];

    public const string COMMAND_HANDLER = nameof(ICommandHandler);
    public const string QUERY_HANDLER = nameof(IQueryHandler);
    public const string EVENT_HANDLER = nameof(IEventHandler);

    public static readonly string[] REQUEST_HANDLERS =
    [
        COMMAND_HANDLER,
        QUERY_HANDLER,
        EVENT_HANDLER
    ];

    public const string COMMAND_DISPATCHER_EXTENSIONS_TEMPLATE_PATH = "Sisa.Generator.Mediator.Templates.CommandDispatcherExtensions.sbn-cs";
    public const string QUERY_DISPATCHER_EXTENSIONS_TEMPLATE_PATH = "Sisa.Generator.Mediator.Templates.QueryDispatcherExtensions.sbn-cs";
    public const string EVENT_DISPATCHER_EXTENSIONS_TEMPLATE_PATH = "Sisa.Generator.Mediator.Templates.EventDispatcherExtensions.sbn-cs";
    public const string MEDIATOR_EXTENSIONS_TEMPLATE_PATH = "Sisa.Generator.Mediator.Templates.MediatorExtensions.sbn-cs";

    public const string COMMAND_DISPATCHER_EXTENSIONS_OUTPUT_PATH = "CommandDispatcherExtensions.g.cs";
    public const string QUERY_DISPATCHER_EXTENSIONS_OUTPUT_PATH = "QueryDispatcherExtensions.g.cs";
    public const string EVENT_DISPATCHER_EXTENSIONS_OUTPUT_PATH = "EventDispatcherExtensions.g.cs";
    public const string MEDIATOR_EXTENSIONS_OUTPUT_PATH = "MediatorExtensions.g.cs";
}
