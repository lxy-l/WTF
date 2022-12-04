using Infrastructure.Core.Extend;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class UserDbContext:DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var types = EfEntityInfo.GetEntityTypes(typeof(Domain.Register).Assembly);
        foreach (var entityType in types)
        {
            modelBuilder.Entity(entityType);
        }
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