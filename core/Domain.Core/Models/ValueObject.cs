namespace Domain.Core.Models;

/// <summary>
/// 值对象
/// </summary>
public abstract record ValueObject<T> where T : ValueObject<T>;
