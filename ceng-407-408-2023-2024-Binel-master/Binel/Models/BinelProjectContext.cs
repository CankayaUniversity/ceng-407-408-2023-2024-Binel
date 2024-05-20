using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Binel.Models;

public partial class BinelProjectContext : DbContext
{
    public BinelProjectContext()
    {
    }

    public BinelProjectContext(DbContextOptions<BinelProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<DonateLog> DonateLogs { get; set; }

    public virtual DbSet<DonatePost> DonatePosts { get; set; }

    public virtual DbSet<FileUrl> FileUrls { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=sqlcon");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("category_ID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(30)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<DonateLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK_Donate_Logs_1");

            entity.ToTable("Donate_Logs");

            entity.Property(e => e.LogId).HasColumnName("log_ID");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.DonateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("donateDate");
            entity.Property(e => e.DonateId).HasColumnName("donate_ID");

            entity.HasOne(d => d.Donate).WithMany(p => p.DonateLogs)
                .HasForeignKey(d => d.DonateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Donate_Logs_Donate_Posts");
        });

        modelBuilder.Entity<DonatePost>(entity =>
        {
            entity.HasKey(e => e.DonateId);

            entity.ToTable("Donate_Posts");

            entity.Property(e => e.DonateId).HasColumnName("donate_ID");
            entity.Property(e => e.DonateText)
                .HasColumnType("ntext")
                .HasColumnName("donateText");
            entity.Property(e => e.MaxLimit).HasColumnName("maxLimit");
            entity.Property(e => e.MinLimit).HasColumnName("minLimit");
            entity.Property(e => e.OrganizationId).HasColumnName("organization_ID");
            entity.Property(e => e.PublishDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("publishDate");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.TotalDonate).HasColumnName("totalDonate");

            entity.HasOne(d => d.Organization).WithMany(p => p.DonatePosts)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Donate_Posts_Organizations");

            entity.HasMany(d => d.Categories).WithMany(p => p.Donates)
                .UsingEntity<Dictionary<string, object>>(
                    "DonateCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Donate_Categories_Categories"),
                    l => l.HasOne<DonatePost>().WithMany()
                        .HasForeignKey("DonateId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Donate_Categories_Donate_Posts"),
                    j =>
                    {
                        j.HasKey("DonateId", "CategoryId").HasName("PK_Donate_Categories_1");
                        j.ToTable("Donate_Categories");
                        j.IndexerProperty<int>("DonateId").HasColumnName("donate_ID");
                        j.IndexerProperty<int>("CategoryId").HasColumnName("category_ID");
                    });

            entity.HasMany(d => d.Files).WithMany(p => p.Donates)
                .UsingEntity<Dictionary<string, object>>(
                    "MediaDonateFile",
                    r => r.HasOne<FileUrl>().WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Media_Donate_Files_File_URLs"),
                    l => l.HasOne<DonatePost>().WithMany()
                        .HasForeignKey("DonateId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Media_Donate_Files_Donate_Posts"),
                    j =>
                    {
                        j.HasKey("DonateId", "FileId");
                        j.ToTable("Media_Donate_Files");
                        j.IndexerProperty<int>("DonateId").HasColumnName("donate_ID");
                        j.IndexerProperty<int>("FileId").HasColumnName("file_ID");
                    });
        });

        modelBuilder.Entity<FileUrl>(entity =>
        {
            entity.HasKey(e => e.FileId);

            entity.ToTable("File_URLs");

            entity.Property(e => e.FileId).HasColumnName("file_ID");
            entity.Property(e => e.FileUrl1)
                .HasMaxLength(200)
                .HasColumnName("fileURL");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.Property(e => e.OrganizationId).HasColumnName("organization_ID");
            entity.Property(e => e.Biography)
                .HasColumnType("ntext")
                .HasColumnName("biography");
            entity.Property(e => e.OrganizationName)
                .HasMaxLength(100)
                .HasColumnName("organizationName");
            entity.Property(e => e.OrganizationTitle)
                .HasMaxLength(100)
                .HasColumnName("organizationTitle");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.Property(e => e.PostId).HasColumnName("post_ID");
            entity.Property(e => e.ExternalPlatform)
                .HasColumnType("ntext")
                .HasColumnName("externalPlatform");
            entity.Property(e => e.OrganizationId).HasColumnName("organization_ID");
            entity.Property(e => e.PostText)
                .HasColumnType("ntext")
                .HasColumnName("postText");
            entity.Property(e => e.PublishDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("publishDate");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.Organization).WithMany(p => p.Posts)
                .HasForeignKey(d => d.OrganizationId)
                .HasConstraintName("FK_Posts_Organizations");

            entity.HasMany(d => d.Files).WithMany(p => p.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "MediaPostFile",
                    r => r.HasOne<FileUrl>().WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Media_Post_Files_File_URLs"),
                    l => l.HasOne<Post>().WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Media_Post_Files_Posts"),
                    j =>
                    {
                        j.HasKey("PostId", "FileId");
                        j.ToTable("Media_Post_Files");
                        j.IndexerProperty<int>("PostId").HasColumnName("post_ID");
                        j.IndexerProperty<int>("FileId").HasColumnName("file_ID");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("user_ID");
            entity.Property(e => e.Birth).HasColumnName("birth");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.OrganizationId).HasColumnName("organization_ID");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(256)
                .HasColumnName("passwordHash");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.Surname)
                .HasMaxLength(30)
                .HasColumnName("surname");

            entity.HasOne(d => d.Organization).WithMany(p => p.Users)
                .HasForeignKey(d => d.OrganizationId)
                .HasConstraintName("FK_Users_Organizations");

            entity.HasMany(d => d.Logs).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "DonateUsersLog",
                    r => r.HasOne<DonateLog>().WithMany()
                        .HasForeignKey("LogId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Donate_Users_Logs_Donate_Logs"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Donate_Users_Logs_Users"),
                    j =>
                    {
                        j.HasKey("UserId", "LogId");
                        j.ToTable("Donate_Users_Logs");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_ID");
                        j.IndexerProperty<int>("LogId").HasColumnName("log_ID");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
