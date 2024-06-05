using System;
using System.Collections.Generic;
using iteam.Libo.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace iteam.Libo.Api;

public partial class LiboContext : DbContext
{
    public LiboContext()
    {
    }

    public LiboContext(DbContextOptions<LiboContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Role> Roles { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=Libo;Trusted_Connection=True; Encrypt=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Title).HasMaxLength(200);
            entity.Property(a => a.Author).HasMaxLength(200);
            entity.Property(a => a.Description).HasMaxLength(1000);
            entity.Property(a => a.Isbn).HasMaxLength(20);
            entity.Property(a => a.Url).HasMaxLength(100);
            entity.HasOne(a => a.Category)
                .WithMany(c => c.Articles)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.HasOne(i => i.Article)
                .WithMany(a => a.Items)
                .HasForeignKey(i => i.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

        });


        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(d => d.BorrowedDate).HasMaxLength(200);
            entity.Property(a => a.ReturnDate).HasMaxLength(1000);

            entity.HasOne(l => l.BorrowedItem)
                .WithMany(i => i.Loans)
                .HasForeignKey(l => l.BorrowedItemId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(l => l.Borrower)
                .WithMany(u => u.Loans)
                .HasForeignKey(l => l.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict);

        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(a => a.Name).HasMaxLength(200);
            entity.Property(a => a.Description).HasMaxLength(1000);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId);
            entity.Property(u => u.Name).IsRequired();
            entity.Property(u => u.Phone).IsRequired();
            entity.Property(u => u.Email).IsRequired();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();
            entity.HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);
        });


    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
