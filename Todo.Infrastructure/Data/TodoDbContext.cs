using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Domain.Entities;

namespace Todo.Infrastructure.Persistences
{
    public class TodoDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Domain.Entities.Task> Tasks { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure entity relationships and constraints here if needed

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(256);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.UserName).IsUnique();
            });

            // Task configuration
            modelBuilder.Entity<Domain.Entities.Task>(entity =>
            {
                entity.HasKey(e => e.TaskId);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Tasks)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Tag)
                    .WithMany(t => t.Tasks)
                    .HasForeignKey(e => e.TagId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Tag configuration
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.TagId);
                entity.Property(e => e.TagName).IsRequired().HasMaxLength(50);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Tags)
                    .HasForeignKey(e => e.User.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
