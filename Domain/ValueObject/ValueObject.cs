using Microsoft.EntityFrameworkCore;

namespace Domain.ValueObject
{
    /// <summary>
    /// 值对象
    /// </summary>
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        //public override bool Equals(object? obj)
        //{
        //    var valueObject = obj as T;
        //    return !ReferenceEquals(valueObject, null) && EqualsCore(valueObject);
        //}

        //protected abstract bool EqualsCore(T other);

        //public override int GetHashCode()
        //{
        //    return GetHashCodeCore();
        //}

        //protected abstract int GetHashCodeCore();

        //public static bool operator ==(ValueObject<T> value1, ValueObject<T> value2)
        //{
        //    if (ReferenceEquals(value1, null) && ReferenceEquals(value2, null))
        //        return true;

        //    if (ReferenceEquals(value1, null) || ReferenceEquals(value2, null))
        //        return false;

        //    return value1.Equals(value2);
        //}

        //public static bool operator !=(ValueObject<T> value1, ValueObject<T> value2)
        //{
        //    return !(value1 == value2);
        //}

        //public virtual T Clone()
        //{
        //    return (T)MemberwiseClone();
        //}

    }
}

