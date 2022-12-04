using System.Runtime.Serialization;

namespace Application.Core.MyException;

/// <summary>
/// 自定义异常类
/// </summary>
public abstract class MyException : Exception
{
    protected MyException()
    {
    }

    protected MyException(string? message) : base(message)
    {
    }

    protected MyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected MyException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
