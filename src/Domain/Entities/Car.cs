using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Domain.Core;

namespace Domain.Entities;

/// <summary>
/// 汽车类
/// </summary>
public class Car : Entity<int>
{
    public Car(string name, string description,int id=default) : this(id)
    {
        Name = name;
        Description = description;
    }

    private Car(int id) : base(id)
    {
    }

    /// <summary>
    /// 汽车名称
    /// </summary>
    [StringLength(255)]
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [StringLength(450)]
    public string Description { get; set; }

    /// <summary>
    /// 关联用户
    /// </summary>
    [JsonIgnore]
    public User? User { get; set; }
}
