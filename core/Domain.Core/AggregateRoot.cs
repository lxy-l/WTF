namespace Domain.Core;

/// <summary>
/// 聚合根
/// </summary>
public abstract class AggregateRoot<TKey> : Entity<TKey> where TKey : struct
{
    protected AggregateRoot(TKey id) : base(id)
    {
    }
}
