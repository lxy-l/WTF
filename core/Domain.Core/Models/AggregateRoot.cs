namespace Domain.Core.Models;

/// <summary>
/// 聚合根
/// </summary>
public abstract class AggregateRoot<TKey> : Entity<TKey>,IAggregateRoot where TKey : struct
{
    protected AggregateRoot(TKey id) : base(id)
    {

    }
}

/// <summary>
/// 聚合根
/// </summary>
public interface IAggregateRoot { }