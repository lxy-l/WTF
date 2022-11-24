using System.ComponentModel.DataAnnotations;

namespace Application.DTO;

[Serializable]
public class SearchParams
{
    /// <summary>
    /// 筛选条件
    /// </summary>
    public string? Filters { get; set; }

    /// <summary>
    /// 排序条件（Name ASC, Age DESC）
    /// </summary>
    public string? Sort { get; set; }

    /// <summary>
    /// 页码
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    /// <summary>
    /// 每页数据量
    /// </summary>
    [Required]
    public int PageSize { get; set; } = 20;
}