using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using TodoApp.DAL.Entities;

namespace TodoApp.DAL;

public partial class TodoDbContext : DbContext
{
    public TodoDbContext()
    {
    }

    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Tag> Tags { get; set; }
    public virtual DbSet<Todo> Todos { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", true, true)
                   .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"];

        return strConn;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Tag entity configuration
        // 1 to Many relationship between Tag and Todo
        modelBuilder.Entity<Tag>(entity =>
        {
            // Configure the primary key
            entity.HasKey(e => e.TagId).HasName("PK_Tags");

            // Entity properties configuration
            entity.Property(e => e.TagId).HasDefaultValueSql("NEWID()");
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.TagName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");

            // Indexes configuration
            entity.HasIndex(e => new { e.UserId, e.TagName })
                .IsUnique()
                .HasDatabaseName("IX_Tag_UserId_TagName");

            // Relationship configuration
            // One Tag has many Todos
            entity.HasMany(d => d.Todos)
                .WithOne(p => p.Tag)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK_Todos_Tags");

            // Many Tags belong to one User
            entity.HasOne(d => d.User)
                .WithMany(p => p.Tags)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tags_Users");
        });
        #endregion

        #region Todo entity configuration
        // Many to 1 relationship between Todo and User
        modelBuilder.Entity<Todo>(entity =>
        {
            // Configure the primary key
            entity.HasKey(e => e.TodoId).HasName("PK_Todos");

            // Entity properties configuration
            entity.Property(e => e.TodoId).HasDefaultValueSql("NEWID()");
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            entity.Property(e => e.IsImportant).HasDefaultValue(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");

            // Indexes configuration
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.TagId);
            entity.HasIndex(e => e.IsCompleted);

            // Relationship configuration
            // Many Todos belong to one User
            entity.HasOne(d => d.User)
                .WithMany(p => p.Todos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Todos_Users");
        });
        #endregion

        #region User entity configuration
        // 1 to Many relationship between User and Tag
        modelBuilder.Entity<User>(entity =>
        {
            // Configure the primary key
            entity.HasKey(e => e.UserId).HasName("PK_Users");

            // Entity properties configuration
            entity.Property(e => e.UserId).HasDefaultValueSql("NEWID()");
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CoverImageUrl).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");

            // Indexes configuration
            entity.HasIndex(e => e.Username)
                .IsUnique()
                .HasDatabaseName("IX_Users_Username");
            entity.HasIndex(e => e.Email);

            // Configure the primary key
            entity.HasMany(d => d.Tags)
                .WithOne(p => p.User)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tags_Users");
        });
        #endregion

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
