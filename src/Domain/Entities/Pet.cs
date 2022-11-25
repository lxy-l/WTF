using System.ComponentModel.DataAnnotations;

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

    public Pet(string name, DateTimeOffset birthday, PetType petType, ICollection<User> user,int id=default):base(id)
    {
        Name = name;
        Birthday = birthday;
        PetType = petType;
        User = user;
    }

    [StringLength(255)]
    public string Name { get; set; }

    public DateTimeOffset Birthday { get; set; }

    public PetType PetType { get; set; }

    public ICollection<User> User { get; set; }
}
