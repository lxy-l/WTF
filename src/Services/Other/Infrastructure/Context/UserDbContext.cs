using Infrastructure.Core.Extend;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

/// <summary>
/// 第一次启动慢的优化方案：https://learn.microsoft.com/zh-cn/ef/core/performance/advanced-performance-topics?tabs=with-di%2Cwith-constant#compiled-models
/// </summary>
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