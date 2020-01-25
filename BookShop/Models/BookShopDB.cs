using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models
{
    [Table("BookInfo")]
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string File { get; set; }
        public string ISBN { get; set; }
        public int Price { get; set; }
        public int NumOfPage { get; set; }
        public int Stock { get; set; }
        [Column(TypeName = "image")]
        public byte[] Image { get; set; }
        public int Weight { get; set; }
        public bool Delete { get; set; }
        public bool IsPublish { get; set; }
        public DateTime? PublishDate { get; set; }
        public int PublishYear { get; set; }


        /// /////////////////////////////////////////////////////////


        public int LanguageID { get; set; }
        public Language Language { get => LazyLoader.Load(this, ref _language); set => _language = value; }

        public int PublisherID { get; set; }
        public Publisher Publisher { get => LazyLoader.Load(this, ref _publisher); set => _publisher = value; }

        public List<Author_Book> Author_Books { get; set; }
        public List<Translator_Book> Translator_Books { get; set; }

        public List<Order_Book> Order_Books { get; set; }
        public List<Category_Book> Category_Books { get; set; }



        /// /////////////////////////////////////////////////////////
        /// 

        private Language _language;
        private Publisher _publisher;

        public ILazyLoader LazyLoader { get; set; }
        public Book()
        {
        }
        private Book(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

    }

    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Display(Name = "نام دسته بندی")]
        //[Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
        public string CategoryName { get; set; }


        /// /////////////////////////////////////////////////////////

        [ForeignKey("category")]
        public int? ParentID { get; set; }
        [Display(Name = "دسته بندی پدر")]
        public Category category { get; set; }
        public List<Category> Categories { get; set; }

        public List<Category_Book> Category_Books { get; set; }
    }

    public class Category_Book
    {
        public int BookID { get; set; }
        public int CategoryID { get; set; }
        public Book Book { get; set; }
        public Category Category { get; set; }
    }

    public class Language
    {
        [Key]
        public int LanguageID { get; set; }

        [Display(Name = "زبان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string LanguageName { get; set; }



        /// /////////////////////////////////////////////////////////


        public List<Book> Books { get; set; }
    }

    public class Publisher
    {
        [Key]
        public int PublisherID { get; set; }

        [Display(Name = "نام ناشر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string PublisherName { get; set; }



        /// /////////////////////////////////////////////////////////


        public List<Book> Books { get; set; }
    }

    public class Discount
    {
        [Key, ForeignKey("Book")]
        public int BookID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public byte Percent { get; set; }

        // ////////////////////////////////////////////////////////////////
        public Book Book { get; set; }
    }

    public class Author
    {
        [Key]
        public int AuthorID { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string LastName { get; set; }

        // ////////////////////////////////////////////////////////////////

        public List<Author_Book> Author_Books
        {
            get => LazyLoader.Load(this, ref _Author_Books);
            set => _Author_Books = value;
        }

        // ////////////////////////////////////////////////////////////////

        private List<Author_Book> _Author_Books;
        private ILazyLoader LazyLoader { get; set; }

        public Author()
        {
        }
        private Author(ILazyLoader lazy)
        {
            LazyLoader = lazy;
        }
    }

    public class Author_Book
    {
        public int BookID { get; set; }
        public int AuthorID { get; set; }
        public Book Book { get => LazyLoader.Load(this, ref _book); set => _book = value; }
        public Author Author { get; set; }
        //////////////////////////////////////////////////////
        ///
        private Book _book;
        public ILazyLoader LazyLoader { get; set; }
        public Author_Book()
        {
        }
        private Author_Book(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }
    }

    public class Translator
    {
        [Key]
        public int TranslatorID { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string LastName { get; set; }

        // ////////////////////////////////////////////////////////////////
        public List<Translator_Book> Author_Books { get; set; }
    }

    public class Translator_Book
    {
        public int BookID { get; set; }
        public int TranslatorID { get; set; }
        public Book Book { get; set; }
        public Translator Translator { get; set; }
    }

    public class Customer
    {
        [Key]
        public string CustomerID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode1 { get; set; }
        public string PostalCode2 { get; set; }
        public string Tell { get; set; }
        public string Image { get; set; }
        // ////////////////////////////////////////////////////////////////

        public List<Order> Orders { get; set; }
        public City City1 { get; set; }
        public City City2 { get; set; }

    }

    public class City
    {
        [Key]
        public int CityID { get; set; }

        [Display(Name = "نام شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]

        public string CityName { get; set; }

        // ////////////////////////////////////////////////////////////////
        [Display(Name = "نام استان")]
        [Required(ErrorMessage = "لطفا {0} را انتخاب کنید.")]
        [ForeignKey("province")]
        public int provinceID { get; set; }
        public Province province { get; set; }
        [InverseProperty("City1")]
        public List<Customer> Customers1 { get; set; }
        [InverseProperty("City2")]
        public List<Customer> Customers2 { get; set; }
    }

    public class Province
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProvinceID { get; set; }
        [Display(Name = "نام استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string ProvinceName { get; set; }

        // ////////////////////////////////////////////////////////////////

        public List<City> Cities { get; set; }
    }

    public class Order
    {
        [Key]
        public string OrderID { get; set; }
        public long AmountPaid { get; set; }
        public string DispatchNumber { get; set; }
        public DateTime BuyDate { get; set; }
        // ////////////////////////////////////////////////////////////////

        public OrderStatus OrderStatus { get; set; }
        public Customer Customer { get; set; }
        public List<Order_Book> Order_Books { get; set; }
    }

    public class OrderStatus
    {
        [Key]
        public string OrderStatusID { get; set; }
        public string OrderStatusName { get; set; }
        // ////////////////////////////////////////////////////////////////

        public List<Order> Orders { get; set; }
    }

    public class Order_Book
    {
        [Key]
        public int Order_BookID { get; set; }
        public int BookID { get; set; }
        public string OrderID { get; set; }
        public Book Book { get; set; }
        public Order Order { get; set; }
    }

}
