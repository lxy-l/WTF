﻿using Domain.Entities;

namespace Domain.AggregateRoots;

/// <summary>
/// 聚合根
/// </summary>
public abstract class AggregateRoot<TKey> : Entity<TKey>
{
    protected AggregateRoot(TKey id) : base(id)
    {
    }
}