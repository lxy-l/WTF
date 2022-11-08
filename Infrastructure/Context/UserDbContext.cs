﻿using Domain.Entities;

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
            base.OnConfiguring(optionsBuilder);
        }

        public override ValueTask DisposeAsync()
        {
            return base.DisposeAsync();
        }
    }
}