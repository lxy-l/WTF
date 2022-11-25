using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// 汽车类
/// </summary>
public class Car : Entity<int>
{
    public Car(string name, string description, User user,int id=default) : this(id)
    {
        Name = name;
        Description = description;
        User = user;
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
    public User User { get; set; }
}
