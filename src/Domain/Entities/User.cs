using System.ComponentModel.DataAnnotations;

using Domain.AggregateRoots;

namespace Domain.Entities;

public class User : AggregateRoot<int>
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
    public string Password { get; set; }

    /// <summary>
    /// 身份信息
    /// </summary>
    public virtual UserInfo? UserInfo { get; set; }

    public User(string name, string password, UserInfo? userInfo, int id = default) : base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        UserInfo = userInfo;
    }

    private User(int id = default) : base(id)
    {

    }
}