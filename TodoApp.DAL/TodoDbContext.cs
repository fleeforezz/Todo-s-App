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

    public virtual DbSet<Category> Categories { get; set; }

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
        var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];

        return strConn;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B1246DB5F");

            entity.Property(e => e.CategoryId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Categories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Categorie__UserI__3E52440B");
        });

        modelBuilder.Entity<Todo>(entity =>
        {
            entity.HasKey(e => e.TodoId).HasName("PK__Todos__958625526120A31E");

            entity.Property(e => e.TodoId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            entity.Property(e => e.ReminderTime).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Todos)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Todos__CategoryI__4BAC3F29");

            entity.HasOne(d => d.User).WithMany(p => p.Todos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Todos__UserId__4AB81AF0");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CE8284299");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4F3C06022").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534B8041FFD").IsUnique();

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
