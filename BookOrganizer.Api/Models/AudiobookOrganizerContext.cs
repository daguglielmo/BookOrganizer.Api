using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookOrganizer.Api.Models;

public partial class AudiobookOrganizerContext : DbContext
{
    public AudiobookOrganizerContext()
    {
    }

    public AudiobookOrganizerContext(DbContextOptions<AudiobookOrganizerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookPath> BookPaths { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(System.Environment.GetEnvironmentVariable("ConnectionString"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.OrganizerAuthorId).HasName("PK_AuthorTable");

            entity.Property(e => e.AuthorImageId)
                .HasMaxLength(11)
                .IsFixedLength();
            entity.Property(e => e.OpenLibraryAuthorId)
                .HasMaxLength(11)
                .IsFixedLength();
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.OrganizerBookId).HasName("PK_BookTable");

            entity.Property(e => e.Asin)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.AuthorKey)
                .HasMaxLength(11)
                .IsFixedLength();
            entity.Property(e => e.CoverId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.FirstPublishYear)
                .HasMaxLength(4)
                .IsFixedLength();
            entity.Property(e => e.OpenLibraryBookId)
                .HasMaxLength(11)
                .IsFixedLength();
            entity.Property(e => e.OpenLibraryWorksLink)
                .HasMaxLength(18)
                .IsFixedLength();
        });

        modelBuilder.Entity<BookPath>(entity =>
        {
            entity.HasKey(e => e.PathId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
