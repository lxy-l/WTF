using Microsoft.EntityFrameworkCore;

namespace InfrastructureTests.Data;

public class TestDbContext : DbContext
{
    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestInfo> TestInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Test>(x =>
        {
            x.HasKey(x => x.Id);
            x.HasIndex(x => x.Id).IsUnique();
            x.HasMany(x => x.TestInfos)
                .WithOne();
        });

        modelBuilder.Entity<TestInfo>(x =>
        {
            x.HasKey(x => x.Id);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseInMemoryDatabase("Test");

        //base.OnConfiguring(optionsBuilder);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}