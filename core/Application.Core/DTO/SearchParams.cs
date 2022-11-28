using System.ComponentModel.DataAnnotations;

namespace Application.Core.DTO;

[Serializable]
public class SearchParams
{

    /// <summary>
    /// 展示字段
    /// </summary>
    public string? Select { get; set; }

    /// <summary>
    /// 筛选条件
    /// <!--Name="string"&&TestInfos.Count>0-->
    /// </summary>
    public string? Filters { get; set; }

    /// <summary>
    /// 排序条件(Name ASC,Id DESC)
    /// </summary>
    [RegularExpression(@"(\w*\s[ASC|DESC]+,*){1,100}", ErrorMessage ="排序参数有误！")]
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