using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using System;
using DoubleV.Models;

namespace DoubleV
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(df => df.UserId);
            
            modelBuilder.Entity<User>()
                .Property<string>("PartitionKey")
                .HasField("User_partitionKey")
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .HasKey(df => df.RolId);
            
            modelBuilder.Entity<Role>()
                .Property<string>("PartitionKey")
                .HasField("Role_partitionKey")
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Todo>()
                .HasKey(df => df.TodoId);
            
            modelBuilder.Entity<Todo>()
                .Property<string>("PartitionKey")
                .HasField("Todo_partitionKey")
                .HasMaxLength(255)
                .IsRequired();

            // Handle relationships manually
            modelBuilder.Entity<User>()
                .HasOne(df => df.Rol)
                .WithMany(f => f.Users);

            modelBuilder.Entity<Todo>()
                .HasOne(df => df.User)
                .WithMany(f => f.Todos);            
        }
    }
}
