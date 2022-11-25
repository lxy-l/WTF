using System.ComponentModel.DataAnnotations;

using Domain.Core;

namespace Domain.ValueObject;

/// <summary>
/// 地址
/// </summary>
public record Address : ValueObject<Address>
{
    /// <summary>
    /// 街道
    /// </summary>
    [StringLength(255)]
    public string Street { get; set; }
    

    /// <summary>
    /// 城市
    /// </summary>
    [StringLength(255)]
    public string City { get; set; }

    /// <summary>
    /// 国家
    /// </summary>
    [StringLength(255)]
    public string Country { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="country"></param>
    /// <param name="city"></param>
    /// <param name="street"></param>
    public Address(string country,string city,string street)
    {
        Country = country;
        City = city;
        Street = street;
    }
}