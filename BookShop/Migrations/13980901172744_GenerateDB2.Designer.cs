﻿// <auto-generated />
using System;
using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BookShop.Migrations
{
    [DbContext(typeof(BookShopContext))]
    [Migration("13980901172744_GenerateDB2")]
    partial class GenerateDB2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BookShop.Models.Author", b =>
                {
                    b.Property<int>("AuthorID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("AuthorID");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("BookShop.Models.Author_Book", b =>
                {
                    b.Property<int>("BookID");

                    b.Property<int>("AuthorID");

                    b.HasKey("BookID", "AuthorID");

                    b.HasIndex("AuthorID");

                    b.ToTable("Author_Books");
                });

            modelBuilder.Entity("BookShop.Models.Book", b =>
                {
                    b.Property<int>("BookID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Delete");

                    b.Property<string>("File");

                    b.Property<string>("ISBN");

                    b.Property<byte[]>("Image")
                        .HasColumnType("image");

                    b.Property<bool>("IsPublish");

                    b.Property<int?>("LanguageID");

                    b.Property<int>("LangusgeID");

                    b.Property<int>("Price");

                    b.Property<DateTime?>("PublishDate");

                    b.Property<int>("PublishYear");

                    b.Property<int>("PublisherID");

                    b.Property<int>("Stock");

                    b.Property<string>("Summary");

                    b.Property<string>("Title");

                    b.Property<int>("Weight");

                    b.HasKey("BookID");

                    b.HasIndex("LanguageID");

                    b.HasIndex("PublisherID");

                    b.ToTable("BookInfo");
                });

            modelBuilder.Entity("BookShop.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName");

                    b.Property<int?>("ParentID");

                    b.HasKey("CategoryID");

                    b.HasIndex("ParentID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("BookShop.Models.Category_Book", b =>
                {
                    b.Property<int>("BookID");

                    b.Property<int>("CategoryID");

                    b.HasKey("BookID", "CategoryID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Category_Books");
                });

            modelBuilder.Entity("BookShop.Models.City", b =>
                {
                    b.Property<int>("CityID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CityName");

                    b.Property<int?>("ProvinceID");

                    b.HasKey("CityID");

                    b.HasIndex("ProvinceID");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("BookShop.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address1");

                    b.Property<string>("Address2");

                    b.Property<DateTime>("BirthDate");

                    b.Property<int?>("City1CityID");

                    b.Property<int?>("City2CityID");

                    b.Property<string>("FirstName");

                    b.Property<string>("Image");

                    b.Property<string>("LastName");

                    b.Property<string>("Mobile");

                    b.Property<string>("Tell");

                    b.HasKey("CustomerID");

                    b.HasIndex("City1CityID");

                    b.HasIndex("City2CityID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("BookShop.Models.Discount", b =>
                {
                    b.Property<int>("BookID");

                    b.Property<DateTime?>("EndDate");

                    b.Property<byte>("Percent");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("BookID");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("BookShop.Models.Language", b =>
                {
                    b.Property<int>("LanguageID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LanguageName");

                    b.HasKey("LanguageID");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("BookShop.Models.Order", b =>
                {
                    b.Property<string>("OrderID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AmountPaid");

                    b.Property<DateTime>("BuyDate");

                    b.Property<int?>("CustomerID");

                    b.Property<string>("DispatchNumber");

                    b.Property<string>("OrderStatusID");

                    b.HasKey("OrderID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("OrderStatusID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("BookShop.Models.OrderStatus", b =>
                {
                    b.Property<string>("OrderStatusID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("OrderStatusName");

                    b.HasKey("OrderStatusID");

                    b.ToTable("OrderStatuses");
                });

            modelBuilder.Entity("BookShop.Models.Order_Book", b =>
                {
                    b.Property<int>("Order_BookID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BookID");

                    b.Property<string>("OrderID");

                    b.HasKey("Order_BookID");

                    b.HasIndex("BookID");

                    b.HasIndex("OrderID");

                    b.ToTable("Order_Books");
                });

            modelBuilder.Entity("BookShop.Models.Province", b =>
                {
                    b.Property<int>("ProvinceID");

                    b.Property<string>("ProvinceName");

                    b.HasKey("ProvinceID");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("BookShop.Models.Publisher", b =>
                {
                    b.Property<int>("PublisherID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PublisherName");

                    b.HasKey("PublisherID");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("BookShop.Models.Translator", b =>
                {
                    b.Property<int>("TranslatorID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("TranslatorID");

                    b.ToTable("Translators");
                });

            modelBuilder.Entity("BookShop.Models.Translator_Book", b =>
                {
                    b.Property<int>("BookID");

                    b.Property<int>("TranslatorID");

                    b.HasKey("BookID", "TranslatorID");

                    b.HasIndex("TranslatorID");

                    b.ToTable("Translator_Books");
                });

            modelBuilder.Entity("BookShop.Models.Author_Book", b =>
                {
                    b.HasOne("BookShop.Models.Author", "Author")
                        .WithMany("Author_Books")
                        .HasForeignKey("AuthorID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookShop.Models.Book", "Book")
                        .WithMany("Author_Books")
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookShop.Models.Book", b =>
                {
                    b.HasOne("BookShop.Models.Language", "Language")
                        .WithMany("Books")
                        .HasForeignKey("LanguageID");

                    b.HasOne("BookShop.Models.Publisher", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookShop.Models.Category", b =>
                {
                    b.HasOne("BookShop.Models.Category", "category")
                        .WithMany("Categories")
                        .HasForeignKey("ParentID");
                });

            modelBuilder.Entity("BookShop.Models.Category_Book", b =>
                {
                    b.HasOne("BookShop.Models.Book", "Book")
                        .WithMany("Category_Books")
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookShop.Models.Category", "Category")
                        .WithMany("Category_Books")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookShop.Models.City", b =>
                {
                    b.HasOne("BookShop.Models.Province", "province")
                        .WithMany("Cities")
                        .HasForeignKey("ProvinceID");
                });

            modelBuilder.Entity("BookShop.Models.Customer", b =>
                {
                    b.HasOne("BookShop.Models.City", "City1")
                        .WithMany("Customers1")
                        .HasForeignKey("City1CityID");

                    b.HasOne("BookShop.Models.City", "City2")
                        .WithMany("Customers2")
                        .HasForeignKey("City2CityID");
                });

            modelBuilder.Entity("BookShop.Models.Discount", b =>
                {
                    b.HasOne("BookShop.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookShop.Models.Order", b =>
                {
                    b.HasOne("BookShop.Models.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerID");

                    b.HasOne("BookShop.Models.OrderStatus", "OrderStatus")
                        .WithMany("Orders")
                        .HasForeignKey("OrderStatusID");
                });

            modelBuilder.Entity("BookShop.Models.Order_Book", b =>
                {
                    b.HasOne("BookShop.Models.Book", "Book")
                        .WithMany("Order_Books")
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookShop.Models.Order", "Order")
                        .WithMany("Order_Books")
                        .HasForeignKey("OrderID");
                });

            modelBuilder.Entity("BookShop.Models.Translator_Book", b =>
                {
                    b.HasOne("BookShop.Models.Book", "Book")
                        .WithMany("Translator_Books")
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookShop.Models.Translator", "Translator")
                        .WithMany("Author_Books")
                        .HasForeignKey("TranslatorID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
