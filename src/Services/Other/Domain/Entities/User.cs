using System.ComponentModel.DataAnnotations;
using Crafty.Domain.Core.Models;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// 用户类
/// </summary>
public class User : Entity<int>, IAggregateRoot
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; }

    /// <summary>
    /// 身份信息（一对一关系，即一个用户对应一个身份证，一个身份证仅对应一个人）
    /// </summary>
    public UserInfo? UserInfo { get; set; }

    /// <summary>
    /// 车（一对多关系，即一个用户可以有多辆车）
    /// </summary>
    public ICollection<Car>? Cars { get; set; }

    /// <summary>
    /// 宠物(多对多关系，即一个宠物可以有多个主人，一个人可以有多个宠物)
    /// </summary>
    public ICollection<Pet>? Pets { get; set; }

    /// <summary>
    /// 住址(值对象)
    /// </summary>
    public Address? Address { get; set; }

    private User(int id = default) : base(id)
    {

    }

    public User(string name, string passwordHash, UserInfo? userInfo, ICollection<Car>? cars, ICollection<Pet>? pets, Address? address, int id = default) : base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        UserInfo = userInfo;
        Cars = cars;
        Pets = pets;
        Address = address;
    }
}