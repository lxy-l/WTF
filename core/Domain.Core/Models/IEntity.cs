namespace Domain.Core.Models;

/// <summary>
/// 实体接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IEntity<TKey> where TKey : struct
{
    public TKey Id { get; set; }
}
