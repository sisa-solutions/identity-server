namespace Sisa.Generator.Mediator;

public enum RequestType
{
    COMMAND,
    QUERY,
    EVENT
}

public record TemplateModel
{
    public string ProjectName { get; set; } = null!;

    public List<RenderModel> Requests { get; set; } = [];
}

public record RenderModel
{
    public string RequestName { get; set; } = null!;
    public string? ResponseName { get; set; }
}

public record CollectionModel : RenderModel
{
    public RequestType RequestType { get; set; }
}


public record DependencyTemplateModel
{
    public string ProjectName { get; set; } = null!;
    public List<DependencyRenderModel> Commands { get; set; } = [];
    public List<DependencyRenderModel> Queries { get; set; } = [];
    public List<DependencyRenderModel> Events { get; set; } = [];
}

public record DependencyRenderModel
{
    public string? InterfaceName { get; set; }
    public string ImplementName { get; set; } = null!;
}

public record DependencyCollectionModel : DependencyRenderModel
{
    public RequestType RequestType { get; set; }
}