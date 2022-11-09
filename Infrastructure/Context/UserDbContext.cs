using Domain.Entities;

using Infrastructure.Mappings;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{

    public class UserDbContext:DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
         : base(options)
        {
        }

        public DbSet<User>? Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=localhost;Database=MyDataBase2;User ID=sa;Password=Lxy962921;Persist Security Info=True;MultipleActiveResultSets=True;Connect Timeout=15");
            base.OnConfiguring(optionsBuilder);
        }

        public override ValueTask DisposeAsync()
        {
            return base.DisposeAsync();
        }
    }
}
