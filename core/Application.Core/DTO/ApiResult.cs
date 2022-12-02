namespace Application.Core;
/// <summary>
/// 异常信息类
/// </summary>
public record ExceptionResult
{
    /// <summary>
    /// 错误信息
    /// </summary>
    public string Message { get; set; }
}
