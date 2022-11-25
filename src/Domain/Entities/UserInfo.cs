﻿using System.ComponentModel.DataAnnotations;

using Domain.ValueObject;

namespace Domain.Entities;

/// <summary>
/// 用户信息类
/// </summary>
public class UserInfo:Entity<int>
{
    /// <summary>
    /// 姓名
    /// </summary>
    [Required]
    [StringLength(55)]
    public string Name { get; set; }

    /// <summary>
    /// 出生年月
    /// </summary>
    [Required]
    public DateTimeOffset Birthday { get; set; }

    /// <summary>
    /// 身份编号
    /// </summary>
    [Required]
    [StringLength(20)]
    public string CardId { get; set; }
    
    /// <summary>
    /// 关联用户id
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// 对应人员
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// 户籍地址
    /// </summary>
    [Required]
    public Address Address { get; set; }

    public UserInfo(string name, DateTimeOffset birthday, string cardId, Address address,int id =default):base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Birthday = birthday;
        CardId = cardId ?? throw new ArgumentNullException(nameof(cardId));
        Address = address ?? throw new ArgumentNullException(nameof(address));
    }

    private UserInfo(int id) : base(id)
    {
    }
}
