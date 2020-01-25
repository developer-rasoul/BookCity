using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.ViewModel
{
    public class BookIndexViewModel
    {

        public int BookID { get; set; }
        [Display(Name = "عنوان کتاب")]
        public string Title { get; set; }
        public string File { get; set; }
        public string ISBN { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public string Summury { get; set; }
        public IFormFile Image { get; set; }
        public byte[] ImageByte { get; set; }
        public bool IsPublish { get; set; }
        public DateTime? PublishDate { get; set; }

        public string LanguageName { get; set; }
        public string PublisherName { get; set; }
        public string AuthorName { get; set; }
        public string TranslatorName { get; set; }
        public string CategoryName { get; set; }

    }
    public class BookIndexHomeViewModel
    {

        public int BookID { get; set; }
        [Display(Name = "عنوان کتاب")]
        public string Title { get; set; }
        public string File { get; set; }
        public string ISBN { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public string Summury { get; set; }
        public IFormFile Image { get; set; }
        public byte[] ImageByte { get; set; }
        public bool IsPublish { get; set; }
        public DateTime? PublishDate { get; set; }

        public string LanguageName { get; set; }
        public string PublisherName { get; set; }
        public string AuthorName { get; set; }
        public string TranslatorName { get; set; }
        public string CategoryName { get; set; }

    }
    public class BookCreateEditViewModel
    {
        public BookCreateEditViewModel()
        {

        }
        public BookCreateEditViewModel(List<BooksTreeCategoryViewModel> categories)
        {
            Categories = categories;
        }
        public List<BooksTreeCategoryViewModel> Categories { get; set; }

        public int BookID { get; set; }

        [Display(Name ="عنوان کتاب")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید.")]
        public string Title { get; set; }

        [Display(Name = "خلاصه")]
        public string Summary { get; set; }
        public DateTime? PublishDate { get; set; }
        [Display(Name = "فایل کتاب")]
        public IFormFile File { get; set; }

        [Display(Name ="شابک")]
        public string ISBN { get; set; }

        [Display(Name ="قیمت")]
        public int Price { get; set; }

        [Display(Name ="تعداد صفحه")]
        public int NumOfPage { get; set; }

        [Display(Name ="موجودی")]
        public int Stock { get; set; }

        [Display(Name ="عکس کتاب")]
        public IFormFile Image { get; set; }
        public byte[] ImageByte { get; set; }

        [Display(Name = "وزن")]
        public int Weight { get; set; }

        [Display(Name ="کتاب در سایت منتشر شود.")]
        public bool IsPublish { get; set; }

        [Display(Name ="سال انتشار")]
        public int PublishYear { get; set; }


        [Display(Name = "زبان")]
        [Required(ErrorMessage = "لطفا {0} را انتخاب کنید.")]
        public int LanguageID { get; set; }

        [Display(Name ="ناشر")]
        [Required(ErrorMessage ="لطفا {0} را انتخاب کنید.")]
        public int PublisherID { get; set; }
        

        [Display(Name ="نویسنده")]
        [Required(ErrorMessage ="لطفا {0} را انتخاب کنید.")]
        public int[] AuthorID { get; set; }

        [Display(Name ="دسته‌بندی کتاب")]
        public int[] CategoryID { get; set; }

        [Display(Name ="مترجم")]
        public int[] TranslatorID { get; set; }
    }

    public class BooksTreeCategoryViewModel
    {
        public BooksTreeCategoryViewModel()
        {
            subs = new List<BooksTreeCategoryViewModel>();
        }
        public int id { get; set; }
        public string title { get; set; }

        public int CategoryBooksCount { get; set; }
        public List<BooksTreeCategoryViewModel> subs { get; set; }
    }
}
