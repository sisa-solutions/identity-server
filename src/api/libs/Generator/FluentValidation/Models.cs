namespace Sisa.Generator.FluentValidation;

public record TemplateModel
{
    public string ProjectName { get; set; } = null!;
    public List<RenderModel> ScopedServices { get; set; } = [];
}

public record RenderModel
{
    public string InterfaceName { get; set; } = null!;
    public string ImplementName { get; set; } = null!;
}

public record CollectionModel : RenderModel
{
}
