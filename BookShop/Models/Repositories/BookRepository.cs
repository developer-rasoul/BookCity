using BookShop.Models.UnitOfWork;
using BookShop.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models.Repositories
{
    public class BookRepository:IBooksRepository
    {
        private readonly BookShopContext _context;
        public BookRepository(BookShopContext context)
        {
            _context = context;
        }
        public List<BooksTreeCategoryViewModel> GetAllCategories()
        {
            var Categories = (from c in _context.Categories where (c.ParentID == null) 
                              select new BooksTreeCategoryViewModel
                              {
                                  id=c.CategoryID,title=c.CategoryName,
                              }).ToList();

            foreach (var category in Categories)
            {
                FindAllSubCategories(category);
                category.CategoryBooksCount = this.CategoryBooksCount(category.id);
            }

            return Categories;
        }

        public void FindAllSubCategories(BooksTreeCategoryViewModel category)
        {
            var subs = (from c in _context.Categories
                        where (c.ParentID == category.id)
                        select new BooksTreeCategoryViewModel
                        {
                            id = c.CategoryID,
                            title = c.CategoryName
                        }).ToList();
            foreach (var sub in subs)
            {
                FindAllSubCategories(sub);
                category.subs.Add(sub);
            }
        }

        public async Task<IEnumerable<BookIndexHomeViewModel>> GetAllPublishBook(string title = "")
        {
            title = title == null ? "" : title.Trim();
            var books = await (from b in _context.Books.Include(p => p.Publisher)
                               where (b.Delete == false && b.IsPublish == true && b.Title.Contains(title))
                               join ab in _context.Author_Books on b.BookID equals ab.BookID
                               join a in _context.Authors on ab.AuthorID equals a.AuthorID
                               select new BookIndexHomeViewModel
                               {
                                   BookID = b.BookID,
                                   Title = b.Title,
                                   ISBN = b.ISBN,
                                   IsPublish = b.IsPublish,
                                   Price = b.Price,
                                   LanguageName = b.Language.LanguageName,
                                   AuthorName = a.FirstName + " " + a.LastName,
                                   PublisherName = b.Publisher.PublisherName,
                                   PublishDate = b.PublishDate,
                                   Stock = b.Stock,
                                   File = b.File,
                               }).GroupBy(g => g.BookID).Select(s => new { BookID = s.Key, BookInfo = s }).ToListAsync();
            var VM = new List<BookIndexHomeViewModel>();
            foreach (var book in books)
            {
                var authors = "";
                foreach (var info in book.BookInfo)
                {
                    if (authors == "")
                    {
                        authors = info.AuthorName;
                    }
                    else
                    {
                        authors += " - " + info.AuthorName;
                    }
                }
                BookIndexHomeViewModel temp = new BookIndexHomeViewModel
                {
                    AuthorName = authors,
                    Title = book.BookInfo.First().Title,
                    ISBN = book.BookInfo.First().ISBN,
                    Stock = book.BookInfo.First().Stock,
                    IsPublish = book.BookInfo.First().IsPublish,
                    PublisherName = book.BookInfo.First().PublisherName,
                    LanguageName = book.BookInfo.First().LanguageName,
                    Price = book.BookInfo.First().Price,
                    BookID = book.BookID,
                    PublishDate = book.BookInfo.First().PublishDate,
                    File = book.BookInfo.First().File,
                };

                VM.Add(temp);
            }
            return VM;
        }

        public async Task<IEnumerable<BookIndexHomeViewModel>> GetLastTopPublishBook(int Top = 5)
        {


            var books = await (from b in _context.Books.Include(p => p.Publisher)
                               .Where(b=> b.Delete == false && b.IsPublish == true)
                               .OrderByDescending(b => b.PublishDate).Take(Top)

                               join ab in _context.Author_Books on b.BookID equals ab.BookID
                               join a in _context.Authors on ab.AuthorID equals a.AuthorID
                               select new BookIndexHomeViewModel
                               {
                                   BookID = b.BookID,
                                   ImageByte=b.Image,
                                   Title = b.Title,
                                   ISBN = b.ISBN,
                                   IsPublish = b.IsPublish,
                                   Price = b.Price,
                                   LanguageName = b.Language.LanguageName,
                                   AuthorName = a.FirstName + " " + a.LastName,
                                   PublisherName = b.Publisher.PublisherName,
                                   PublishDate = b.PublishDate,
                                   Stock = b.Stock,
                                   Summury=b.Summary,
                                   File = b.File,
                               }).GroupBy(g => g.BookID).Select(s => new { BookID = s.Key, BookInfo = s }).ToListAsync();
            var VM = new List<BookIndexHomeViewModel>();
            foreach (var book in books)
            {
                var authors = "";
                foreach (var info in book.BookInfo)
                {
                    if (authors == "")
                    {
                        authors = info.AuthorName;
                    }
                    else
                    {
                        authors += " - " + info.AuthorName;
                    }
                }
                BookIndexHomeViewModel temp = new BookIndexHomeViewModel
                {
                    AuthorName = authors,
                    Title = book.BookInfo.First().Title,
                    ISBN = book.BookInfo.First().ISBN,
                    Stock = book.BookInfo.First().Stock,
                    IsPublish = book.BookInfo.First().IsPublish,
                    PublisherName = book.BookInfo.First().PublisherName,
                    LanguageName = book.BookInfo.First().LanguageName,
                    Price = book.BookInfo.First().Price,
                    BookID = book.BookID,
                    Summury = book.BookInfo.First().Summury,
                    ImageByte =book.BookInfo.First().ImageByte,
                    PublishDate = book.BookInfo.First().PublishDate,
                    File = book.BookInfo.First().File,
                };

                VM.Add(temp);
            }
            return VM;
        }

        public int CategoryBooksCount(int categoryID)
        {
            var catCount =(from n in _context.Category_Books
                           .Include(b=>b.Book)
                           where (n.CategoryID == categoryID && n.Book.Delete == false && n.Book.IsPublish==true)
                           select n.BookID).Count();
            return catCount;
        }
        public async Task<IEnumerable<BookIndexHomeViewModel>> FindBooksOfCategory(int catID)
        {


            var books = await (from b in _context.Category_Books.Include(b => b.Book)
                               .Where(b => b.Book.Delete == false && b.Book.IsPublish == true && b.CategoryID==catID)

                               join ab in _context.Author_Books on b.BookID equals ab.BookID
                               join a in _context.Authors on ab.AuthorID equals a.AuthorID
                               select new BookIndexHomeViewModel
                               {
                                   BookID = b.Book.BookID,
                                   ImageByte = b.Book.Image,
                                   Title = b.Book.Title,
                                   ISBN = b.Book.ISBN,
                                   IsPublish = b.Book.IsPublish,
                                   Price = b.Book.Price,
                                   LanguageName = b.Book.Language.LanguageName,
                                   AuthorName = a.FirstName + " " + a.LastName,
                                   PublisherName = b.Book.Publisher.PublisherName,
                                   PublishDate = b.Book.PublishDate,
                                   Stock = b.Book.Stock,
                                   Summury = b.Book.Summary,
                                   File = b.Book.File,
                               }).GroupBy(g => g.BookID).Select(s => new { BookID = s.Key, BookInfo = s }).ToListAsync();
            var VM = new List<BookIndexHomeViewModel>();
            foreach (var book in books)
            {
                var authors = "";
                foreach (var info in book.BookInfo)
                {
                    if (authors == "")
                    {
                        authors = info.AuthorName;
                    }
                    else
                    {
                        authors += " - " + info.AuthorName;
                    }
                }
                BookIndexHomeViewModel temp = new BookIndexHomeViewModel
                {
                    AuthorName = authors,
                    Title = book.BookInfo.First().Title,
                    ISBN = book.BookInfo.First().ISBN,
                    Stock = book.BookInfo.First().Stock,
                    IsPublish = book.BookInfo.First().IsPublish,
                    PublisherName = book.BookInfo.First().PublisherName,
                    LanguageName = book.BookInfo.First().LanguageName,
                    Price = book.BookInfo.First().Price,
                    BookID = book.BookID,
                    Summury = book.BookInfo.First().Summury,
                    ImageByte = book.BookInfo.First().ImageByte,
                    PublishDate = book.BookInfo.First().PublishDate,
                    File = book.BookInfo.First().File,
                };

                VM.Add(temp);
            }
            return VM;
        }

    }
}
