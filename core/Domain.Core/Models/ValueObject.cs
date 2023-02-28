using System.Collections.Generic;
using System.Linq;

namespace Domain.Core.Models;

/*
 * 1. 为什么不适用record代替ValueObject
 *https://enterprisecraftsmanship.com/posts/csharp-records-value-objects/
 * 2. 值对象的实现
 *https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects#value-object-implementation-in-c
 */

///// <summary>
///// 值对象
///// <typeparamref name="T"/>
///// </summary>
//public abstract record ValueObject<T> where T : ValueObject<T>;

///// <summary>
///// 值对象(基于Class实现)
///// </summary>
//public abstract class ValueObject
//{
//    protected static bool EqualOperator(ValueObject left, ValueObject right)
//    {
//        if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
//        {
//            return false;
//        }
//        return ReferenceEquals(left, null) || left.Equals(right);
//    }

//    protected static bool NotEqualOperator(ValueObject left, ValueObject right)
//    {
//        return !(EqualOperator(left, right));
//    }

//    protected abstract IEnumerable<object> GetEqualityComponents();

//    public override bool Equals(object? obj)
//    {
//        if (obj == null || obj.GetType() != GetType())
//        {
//            return false;
//        }

//        var other = (ValueObject)obj;

//        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
//    }

//    public override int GetHashCode()
//    {
//        //return GetEqualityComponents()
//        //    .Select(x => x != null ? x.GetHashCode() : 0)
//        //    .Aggregate((x, y) => x ^ y);
//        return GetEqualityComponents().Aggregate(0, (hash, value) => hash ^ EqualityComparer<object>.Default.GetHashCode(value));
//    }

//    public static bool operator ==(ValueObject one, ValueObject two)
//    {
//        return EqualOperator(one, two);
//    }

//    public static bool operator !=(ValueObject one, ValueObject two)
//    {
//        return NotEqualOperator(one, two);
//    }


//    public ValueObject? GetCopy()
//    {
//        return MemberwiseClone() as ValueObject;
//    }
//}

/// <summary>
/// 值对象（基于record实现）
/// </summary>
public abstract record ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override int GetHashCode()
    {
        return GetEqualityComponents().Aggregate(0, (hash, value) => hash ^ EqualityComparer<object>.Default.GetHashCode(value));
    }

    public ValueObject With(params (string PropertyName, object PropertyValue)[] changes)
    {
        var copy = this with { };
        foreach (var (PropertyName, PropertyValue) in changes)
        {
            var property = GetType().GetProperty(PropertyName);
            property?.SetValue(copy, PropertyValue);
        }
        return copy;
    }
}