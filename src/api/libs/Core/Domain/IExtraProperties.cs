namespace Sisa.Abstractions;

public interface IExtraProperties { }

public interface IExtraProperties<TExtraProperties> : IExtraProperties
{
    TExtraProperties ExtraProperties { get; }
}
