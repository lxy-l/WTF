using System.ComponentModel.DataAnnotations;

using Domain.Core.Models;

namespace Domain.ValueObjects;

/// <summary>
/// 地址
/// </summary>
public class Address : ValueObject
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

    /// <summary>
    /// 可以更加细化控制值比较
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Country;
        yield return City;
        yield return Street;
    }
}