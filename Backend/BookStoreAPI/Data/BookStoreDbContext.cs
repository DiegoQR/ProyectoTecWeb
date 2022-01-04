using BookStoreAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Data
{
    public class BookStoreDbContext : DbContext
    {
        public DbSet<EditorialEntity> Editorials { get; set; }
        public DbSet<BookEntity> Books { get; set; }

        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EditorialEntity>().ToTable("Editorials");
            modelBuilder.Entity<EditorialEntity>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<EditorialEntity>().HasMany(e => e.Books).WithOne(b => b.Editorial);

            modelBuilder.Entity<BookEntity>().ToTable("Books");
            modelBuilder.Entity<BookEntity>().Property(b => b.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<BookEntity>().HasOne(b => b.Editorial).WithMany(e => e.Books);
        }
    }
}
