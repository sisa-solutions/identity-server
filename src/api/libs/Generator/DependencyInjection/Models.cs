namespace Sisa.Generator.DependencyInjection;

public enum RegisterType
{
    TRANSIENT,
    SCOPED,
    SINGLETON
}

public record TemplateModel
{
    public string ProjectName { get; set; } = null!;
    public List<RenderModel> TransientServices { get; set; } = [];
    public List<RenderModel> ScopedServices { get; set; } = [];
    public List<RenderModel> SingletonServices { get; set; } = [];
}

public record RenderModel
{
    public string? InterfaceName { get; set; }
    public string ImplementName { get; set; } = null!;
    public string? Key { get; set; }
}

public record CollectionModel : RenderModel
{
    public RegisterType RegisterType { get; set; }
}
