using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Core.Models;
/// <summary>
/// 实体
/// </summary>
[Serializable]
public abstract class Entity<TKey> : IEntity<TKey> where TKey : struct
{
    /// <summary>
    /// 唯一标识
    /// </summary>
    [Key]
    public TKey Id { get; set; }

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="id"></param>
    protected Entity(TKey id)
    {
        Id = id;
    }

    ///// <summary>
    ///// 唯一标识
    ///// </summary>
    //[Key]
    //public TKey Id { get; private set; }

    /// <summary>
    /// Equals
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public override bool Equals(object? entity)
    {
        if (entity is null)
        {
            return false;
        }
        if (entity is not Entity<TKey> entity1)
        {
            return false;
        }
        return this == entity1;
    }

    /// <summary>
    /// GetHashCode
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// ==
    /// </summary>
    /// <param name="entity1"></param>
    /// <param name="entity2"></param>
    /// <returns></returns>
    public static bool operator ==(Entity<TKey>? entity1, Entity<TKey>? entity2)
    {
        if (entity1 is null && entity2 is null)
            return true;
        if (entity1 is null || entity2 is null)
            return false;
        if (entity1.Id.Equals(default(TKey)))
            return false;
        return entity1.Id.Equals(entity2.Id);
    }

    /// <summary>
    /// !=
    /// </summary>
    /// <param name="entity1"></param>
    /// <param name="entity2"></param>
    /// <returns></returns>
    public static bool operator !=(Entity<TKey> entity1, Entity<TKey> entity2)
    {
        return !(entity1 == entity2);
    }
}
