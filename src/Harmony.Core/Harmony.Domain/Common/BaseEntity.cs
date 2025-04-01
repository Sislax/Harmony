namespace Harmony.Domain.Common;

public abstract class BaseEntity<T>
{
    public required T Id { get; set; }
}
