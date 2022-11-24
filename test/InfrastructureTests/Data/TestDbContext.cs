using Microsoft.EntityFrameworkCore;

namespace InfrastructureTests.Data
{
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
                //x.HasMany(x => x.TestInfos)
                //.WithOne();
            });
            //modelBuilder.Entity<Test>().HasData(new Test("A3", DateTime.UtcNow, 3));
            //modelBuilder.Entity<Test>().HasData(new[]
            //    {
            //        new Test(1)
            //        {
            //            Name="A1",
            //            DateTime=DateTime.Now,
            //            TestInfos=new List<TestInfo>
            //            {
            //                new TestInfo(1,"B1"),
            //                new TestInfo(2,"B2")
            //            }
            //        },
            //        new Test(2)
            //        {
            //            Name="A2",
            //            DateTime=DateTime.Now,
            //            TestInfos=new List<TestInfo>
            //            {
            //                new TestInfo(1,"B1"),
            //                new TestInfo(2,"B2")
            //            }
            //        },
            //        new Test("A3",DateTime.UtcNow,null,3),
            //        new Test(4) { Name="A4",DateTime=DateTime.Now},
            //        new Test(5) { Name="A5",DateTime=DateTime.Now},
            //        new Test(6) { Name="A6",DateTime=DateTime.Now},
            //        new Test(7) { Name="A7",DateTime=DateTime.Now},
            //        new Test(8) { Name="A8",DateTime=DateTime.Now}
            //    });

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
}
