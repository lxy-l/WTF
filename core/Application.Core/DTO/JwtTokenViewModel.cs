namespace Application.Core.DTO;

[Serializable]
public record JwtTokenViewModel
{
    /// <summary>
    /// token
    /// </summary>
    public string? AccessToken { get; set; }
    /// <summary>
    /// 有效期
    /// </summary>
    public double Expires { get; set; }
    /// <summary>
    /// token类型
    /// </summary>
    public string? TokenType { get; set; }
}