using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;


public class UserMap : IEntityTypeConfiguration<User>
{
    /*
        总结：一对一关系以及值对象映射需要手动在这里配置
              而多对多，一对多只需要配置导航属性(仅限于生成数据库的时候,可以生成不会报错!)
              如果要配置其他操作例如级联删除，则需要在这里配置
        建议：Domain层不用数据注解的原因就是如果使用注解将会造成Doamin层依赖EFCore框架，超出了Domian层的职责
              尽量能配置的关系全部配置，以免新增，删除等其他操作报错！
     */
    public void Configure(EntityTypeBuilder<User> builder)
    {
        //值对象 需要在EFCore中手动映射
        builder.OwnsOne(x => x.Address);


        /*
            配置一对一关系
            将生成一个外键在从表UserInfo中
            （必须在从表手动创建一个字段作为外键，不然EFCore会报错）
         */
        builder
            .HasOne(b => b.UserInfo)
            .WithOne(i => i.User)
            .HasForeignKey<UserInfo>(b => b.UserId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);

        //必须配置删除行为，不然EFCore会报错
        builder.HasMany(x => x.Cars)
            .WithOne(x => x.User)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);

        //解决多对多关系删除时中间关系表不删除的问题，需要这样配置或者手动创建关系表维护
        builder
            .HasMany(x => x.Pets)
            .WithMany(x => x.User)
            .UsingEntity<Dictionary<string, object>>(
        "UserPets",
        j => j
            .HasOne<Pet>()
            .WithMany()
            .HasForeignKey("PetId")
            .HasConstraintName("FK_UserPet_PetId")
            .OnDelete(DeleteBehavior.Cascade)
            ,
        j => j
            .HasOne<User>()
            .WithMany()
            .HasForeignKey("UserId")
            .HasConstraintName("FK_UserPet_UserId")
            .OnDelete(DeleteBehavior.Cascade));
    }
}