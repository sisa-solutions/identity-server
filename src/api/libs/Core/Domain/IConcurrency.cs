namespace Sisa.Abstractions;

public interface IConcurrencyToken
{
    byte[] ConcurrencyToken { get; }
}
