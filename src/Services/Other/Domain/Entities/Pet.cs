using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Crafty.Domain.Core.Models;
using Domain.Enum;

namespace Domain.Entities;

/// <summary>
/// 宠物类
/// </summary>
public class Pet : Entity<int>
{
    private Pet(int id) : base(id)
    {
    }

    public Pet(string name, DateTimeOffset birthday, PetType petType,int id=default):base(id)
    {
        Name = name;
        Birthday = birthday;
        PetType = petType;
    }

    /// <summary>
    /// 名称
    /// </summary>
    [StringLength(255)]
    public string Name { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public DateTimeOffset Birthday { get; set; }

    /// <summary>
    /// 宠物类型
    /// </summary>
    public PetType PetType { get; set; }

    [JsonIgnore]
    public ICollection<User>? User { get; set; }
}
