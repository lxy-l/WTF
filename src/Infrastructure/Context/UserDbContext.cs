using System.Reflection;

using Domain.Entities;

using Infrastructure.Core.Extend;
using Infrastructure.Mappings;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class UserDbContext:DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    //public DbSet<User>? Users { get; set; }

    //public DbSet<UserInfo>? UserInfos { get; set; }

    //public DbSet<Pet> Pets { get; set; }

    //public DbSet<Car> Cars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var Types = EFEntityInfo.GetEntityTypes(typeof(Domain.Register).Assembly);
        foreach (var entityType in Types)
        {
            modelBuilder.Entity(entityType);
        }
        //modelBuilder.ApplyConfiguration(new UserMap());
        //modelBuilder.ApplyConfiguration(new UserInfoMap());
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    public override ValueTask DisposeAsync()
    {
        return base.DisposeAsync();
    }
}