using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public class BookShopContext : DbContext
    {
        public BookShopContext(DbContextOptions<BookShopContext> options):base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=(local);Database=BookShopDB;Trusted_Connection=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Translator_Book>().HasKey(t => new { t.BookID, t.TranslatorID });
            modelBuilder.Entity<Category_Book>().HasKey(t => new { t.BookID, t.CategoryID });
            modelBuilder.Entity<Author_Book>().HasKey(t => new { t.BookID, t.AuthorID });
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories  { get; set; }
        public DbSet<Category_Book> Category_Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Author_Book> Author_Books { get; set; }
        public DbSet<Order_Book> Order_Books { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Translator> Translators { get; set; }
        public DbSet<Translator_Book> Translator_Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

    }
}
