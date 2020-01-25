using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BookShop.Models;
using BookShop.Models.Repositories;
using BookShop.Models.UnitOfWork;
using BookShop.ViewModel;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BooksController : Controller
    {
        private readonly IUnitOfWork _UW;
        private readonly IHostingEnvironment _env;
        public BooksController(IUnitOfWork UW, BookRepository repository, IHostingEnvironment env)
        {
            _UW = UW;
            _env = env;
        }

        public async Task<IActionResult> Index(string title = "", int pageSize = 5, int page = 1, string sortExpression = "Title")
        {

            ViewBag.AuthorID = new SelectList(_UW.BaseRepository<Author>().FindAll().Select(a => new { ID = a.AuthorID, Name = a.FirstName + " " + a.LastName }), "Name", "Name");
            ViewBag.LanguageID = new SelectList(_UW.BaseRepository<Language>().FindAll(), "LanguageName", "LanguageName");
            ViewBag.PublisherID = new SelectList(_UW.BaseRepository<Publisher>().FindAll(), "PublisherName", "PublisherName");
            ViewBag.TranslatorID = new SelectList(_UW.BaseRepository<Translator>().FindAll().Select(a => new { ID = a.TranslatorID, Name = a.FirstName + " " + a.LastName }), "Name", "Name");
            ViewBag.Categories = _UW.BooksRepository.GetAllCategories();


            title = string.IsNullOrEmpty(title) ? "" : title.Trim();

            var books = await (from b in _UW._context.Books.Include(p => p.Publisher)
                               where (b.Delete == false && b.Title.Contains(title))
                               join ab in _UW._context.Author_Books on b.BookID equals ab.BookID
                               join a in _UW._context.Authors on ab.AuthorID equals a.AuthorID
                               select new BookIndexViewModel
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
            var VM = new List<BookIndexViewModel>();
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
                BookIndexViewModel temp = new BookIndexViewModel
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

            int[] Size = { 5, 10, 15, 20, 50, 100 };
            ViewBag.Size = new SelectList(Size, pageSize);

            ViewBag.RowNumber = (page - 1) * pageSize + 1;

            var Paging = PagingList.Create(VM, pageSize, page, sortExpression, "Title");
            Paging.RouteValue = new Microsoft.AspNetCore.Routing.RouteValueDictionary
            {
                { "pageSize",pageSize},
                { "title",title},
            };
            return View(Paging);
        }

        public async Task<IActionResult> AdvanceSearch(string title = "", string isbn = "", string language = "", string category = "",
            string author = "", string publisher = "", string translator = "")
        {



            title = string.IsNullOrEmpty(title) ? "" : title.Trim();
            isbn = string.IsNullOrEmpty(isbn) ? "" : isbn.Trim();
            language = string.IsNullOrEmpty(language) ? "" : language;
            publisher = string.IsNullOrEmpty(publisher) ? "" : publisher;
            author = string.IsNullOrEmpty(author) ? "" : author;
            translator = string.IsNullOrEmpty(translator) ? "" : translator;
            category = string.IsNullOrEmpty(category) ? "" : category;

            var books = await (from b in _UW._context.Books.Include(p => p.Publisher)
                               join ab in _UW._context.Author_Books on b.BookID equals ab.BookID into b_ab
                               from bab in b_ab.DefaultIfEmpty()
                               join a in _UW._context.Authors on bab.AuthorID equals a.AuthorID into bab_a
                               from baba in bab_a.DefaultIfEmpty()

                               join tb in _UW._context.Translator_Books on b.BookID equals tb.BookID into b_tb
                               from btb in b_tb.DefaultIfEmpty()
                               join t in _UW._context.Translators on btb.TranslatorID equals t.TranslatorID into btb_t
                               from btbt in btb_t.DefaultIfEmpty()

                               join cb in _UW._context.Category_Books on b.BookID equals cb.BookID into b_cb
                               from bcb in b_cb.DefaultIfEmpty()
                               join c in _UW._context.Categories on bcb.CategoryID equals c.CategoryID into bcb_c
                               from bcbc in bcb_c.DefaultIfEmpty()

                               where (b.Delete == false && b.Title.Contains(title) && b.ISBN.Contains(isbn)
                               && b.Language.LanguageName.Contains(language) && b.Publisher.PublisherName.Contains(publisher))


                               select new
                               {
                                   TranslatorName = btb != null ? btbt.FirstName + " " + btbt.LastName : "",
                                   CategoryName = bcb != null ? bcbc.CategoryName : "",
                                   b.BookID,
                                   b.Title,
                                   b.ISBN,
                                   b.IsPublish,
                                   b.Price,
                                   b.Language.LanguageName,
                                   AuthorName = baba.FirstName + " " + baba.LastName,
                                   b.Publisher.PublisherName,
                                   b.PublishDate,
                                   b.Stock,
                                   b.File,
                               }).Where(a => a.AuthorName.Contains(author) && (a.TranslatorName.Contains(translator)) && (a.CategoryName.Contains(category)))
                               .GroupBy(g => g.BookID).Select(s => new { BookID = s.Key, BookInfo = s })
                               .Distinct().AsNoTracking().ToListAsync();

            var VM = new List<BookIndexViewModel>();
            foreach (var book in books)
            {
                var authors = "";
                foreach (var info in book.BookInfo.Select(a => a.AuthorName).Distinct())
                {
                    if (authors == "")
                    {
                        authors = info;
                    }
                    else
                    {
                        authors += " - " + info;
                    }
                }
                var translators = "";
                foreach (var translatorName in book.BookInfo.Select(t => t.TranslatorName).Distinct())
                {
                    if (translators == "")
                    {
                        translators = translatorName;
                    }
                    else
                    {
                        translators += " - " + translatorName;
                    }
                }
                var categories = "";
                foreach (var categoryName in book.BookInfo.Select(c => c.CategoryName).Distinct())
                {
                    if (categories == "")
                    {
                        categories = categoryName;
                    }
                    else
                    {
                        categories += " - " + categoryName;
                    }
                }
                BookIndexViewModel temp = new BookIndexViewModel
                {
                    AuthorName = authors,
                    CategoryName = categories,
                    TranslatorName = translators,
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
            return View(VM);
        }

        public IActionResult Create()
        {

            ViewBag.AuthorID = new SelectList(_UW.BaseRepository<Author>().FindAll().Select(a => new { ID = a.AuthorID, Name = a.FirstName + " " + a.LastName }), "ID", "Name");
            ViewBag.LanguageID = new SelectList(_UW.BaseRepository<Language>().FindAll(), "LanguageID", "LanguageName");
            ViewBag.PublisherID = new SelectList(_UW.BaseRepository<Publisher>().FindAll(), "PublisherID", "PublisherName");
            ViewBag.TranslatorID = new SelectList(_UW.BaseRepository<Translator>().FindAll().Select(a => new { ID = a.TranslatorID, Name = a.FirstName + " " + a.LastName }), "ID", "Name");

            BookCreateEditViewModel vm = new BookCreateEditViewModel(_UW.BooksRepository.GetAllCategories());

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreateEditViewModel viewModel)
        {


            ViewBag.AuthorID = new SelectList(_UW.BaseRepository<Author>().FindAll().Select(a => new { ID = a.AuthorID, Name = a.FirstName + " " + a.LastName }), "ID", "Name");
            ViewBag.LanguageID = new SelectList(_UW.BaseRepository<Language>().FindAll(), "LanguageID", "LanguageName");
            ViewBag.PublisherID = new SelectList(_UW.BaseRepository<Publisher>().FindAll(), "PublisherID", "PublisherName");
            ViewBag.TranslatorID = new SelectList(_UW.BaseRepository<Translator>().FindAll().Select(a => new { ID = a.TranslatorID, Name = a.FirstName + " " + a.LastName }), "ID", "Name");

            viewModel.Categories = _UW.BooksRepository.GetAllCategories();

            if (ModelState.IsValid)
            {
                try
                {
                    DateTime? pDate = null;
                    if (viewModel.IsPublish)
                    {
                        pDate = DateTime.Now;
                    }
                    Book book = new Book
                    {
                        Delete = false,
                        Title = viewModel.Title,
                        ISBN = viewModel.ISBN,
                        IsPublish = viewModel.IsPublish,
                        Weight = viewModel.Weight,
                        NumOfPage = viewModel.NumOfPage,
                        Price = viewModel.Price,
                        PublishDate = pDate,
                        Stock = viewModel.Stock,
                        Summary = viewModel.Summary,
                        PublishYear = viewModel.PublishYear,
                        LanguageID = viewModel.LanguageID,
                        PublisherID = viewModel.PublisherID,
                    };

                    if (viewModel.File != null)
                    {
                        var BookFileDirectory = Path.Combine(_env.WebRootPath, "BookFile");
                        if (!Directory.Exists(BookFileDirectory))
                        {
                            Directory.CreateDirectory(BookFileDirectory);
                        }
                        var fileName = string.Concat(Guid.NewGuid().ToString(), Path.GetExtension(viewModel.File.FileName));
                        var path = Path.Combine(BookFileDirectory, fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await viewModel.File.CopyToAsync(stream);
                        }
                        book.File = fileName;
                    }

                    if (viewModel.Image != null)
                    {
                        using (var memory = new MemoryStream())
                        {
                            await viewModel.Image.CopyToAsync(memory);
                            using (var img = new MagickImage(memory.ToArray()))
                            {
                                img.Quality = 50;
                                book.Image = img.ToByteArray();
                            }
                        }
                    }

                    await _UW.BaseRepository<Book>().CreateAsync(book);

                    if (viewModel.AuthorID != null)
                    {
                        var temps = new List<Author_Book>();
                        foreach (var item in viewModel.AuthorID)
                        {
                            var temp = new Author_Book() { AuthorID = item, BookID = book.BookID };
                            temps.Add(temp);
                        }
                        await _UW.BaseRepository<Author_Book>().CreateRangeAsync(temps);
                    }
                    if (viewModel.CategoryID != null)
                    {
                        var temps = new List<Category_Book>();
                        foreach (var item in viewModel.CategoryID)
                        {
                            var temp = new Category_Book() { CategoryID = item, BookID = book.BookID };
                            temps.Add(temp);
                        }
                        await _UW.BaseRepository<Category_Book>().CreateRangeAsync(temps);
                    }
                    if (viewModel.TranslatorID != null)
                    {
                        var temps = new List<Translator_Book>();
                        foreach (var item in viewModel.TranslatorID)
                        {
                            var temp = new Translator_Book() { TranslatorID = item, BookID = book.BookID };
                            temps.Add(temp);
                        }
                        await _UW.BaseRepository<Translator_Book>().CreateRangeAsync(temps);
                    }

                    TempData["success"] = "ذخیره کتاب با موفقیت انجام شد.";
                    TempData["error"] = null;
                    await _UW.Commit();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    TempData["success"] = null;
                    TempData["error"] = "در ذخیره کتاب خطایی رخ داده است.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {

            ViewBag.AuthorID = new SelectList(_UW.BaseRepository<Author>().FindAll().Select(a => new { ID = a.AuthorID, Name = a.FirstName + " " + a.LastName }), "ID", "Name");
            ViewBag.LanguageID = new SelectList(_UW.BaseRepository<Language>().FindAll(), "LanguageID", "LanguageName");
            ViewBag.PublisherID = new SelectList(_UW.BaseRepository<Publisher>().FindAll(), "PublisherID", "PublisherName");
            ViewBag.TranslatorID = new SelectList(_UW.BaseRepository<Translator>().FindAll().Select(a => new { ID = a.TranslatorID, Name = a.FirstName + " " + a.LastName }), "ID", "Name");


            if (id == null)
            {
                return NotFound();
            }
            var book = await (from b in _UW._context.Books
                              where (b.BookID == id)
                              select new BookCreateEditViewModel
                              {
                                  BookID = b.BookID,
                                  Title = b.Title,
                                  ISBN = b.ISBN,
                                  IsPublish = b.IsPublish,
                                  NumOfPage = b.NumOfPage,
                                  Price = b.Price,
                                  Stock = b.Stock,
                                  Summary = b.Summary,
                                  PublishYear = b.PublishYear,
                                  Weight = b.Weight,
                                  PublishDate = b.PublishDate,
                                  LanguageID = b.LanguageID,
                                  PublisherID = b.PublisherID,
                                  ImageByte = b.Image,

                              }).AsNoTracking().FirstOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }
            else
            {
                book.AuthorID = await (from a in _UW._context.Author_Books where a.BookID == id select a.AuthorID).ToArrayAsync();
                book.TranslatorID = await (from t in _UW._context.Translator_Books where t.BookID == id select t.TranslatorID).ToArrayAsync();
                book.CategoryID = await (from c in _UW._context.Category_Books where c.BookID == id select c.CategoryID).ToArrayAsync();
                book.Categories = _UW.BooksRepository.GetAllCategories();

                return View(book);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookCreateEditViewModel viewModel)
        {

            ViewBag.AuthorID = new SelectList(_UW.BaseRepository<Author>().FindAll().Select(a => new { ID = a.AuthorID, Name = a.FirstName + " " + a.LastName }), "ID", "Name");
            ViewBag.LanguageID = new SelectList(_UW.BaseRepository<Language>().FindAll(), "LanguageID", "LanguageName");
            ViewBag.PublisherID = new SelectList(_UW.BaseRepository<Publisher>().FindAll(), "PublisherID", "PublisherName");
            ViewBag.TranslatorID = new SelectList(_UW.BaseRepository<Translator>().FindAll().Select(a => new { ID = a.TranslatorID, Name = a.FirstName + " " + a.LastName }), "ID", "Name");

            viewModel.Categories = _UW.BooksRepository.GetAllCategories();

            if (ModelState.IsValid)
            {
                try
                {
                    DateTime? pDate = null;
                    if (viewModel.PublishDate == null && viewModel.IsPublish == true)
                    {
                        pDate = DateTime.Now;
                    }
                    Book book = new Book
                    {
                        BookID = viewModel.BookID,
                        Delete = false,
                        Title = viewModel.Title,
                        ISBN = viewModel.ISBN,
                        IsPublish = viewModel.IsPublish,
                        Weight = viewModel.Weight,
                        NumOfPage = viewModel.NumOfPage,
                        Price = viewModel.Price,
                        PublishDate = pDate,
                        Stock = viewModel.Stock,
                        Summary = viewModel.Summary,
                        PublishYear = viewModel.PublishYear,
                        LanguageID = viewModel.LanguageID,
                        PublisherID = viewModel.PublisherID,
                    };

                    if (viewModel.File != null)
                    {
                        var BookFileDirectory = Path.Combine(_env.WebRootPath, "BookFile");
                        if (!Directory.Exists(BookFileDirectory))
                        {
                            Directory.CreateDirectory(BookFileDirectory);
                        }
                        var fileName = string.Concat(Guid.NewGuid().ToString(), Path.GetExtension(viewModel.File.FileName));
                        var path = Path.Combine(BookFileDirectory, fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await viewModel.File.CopyToAsync(stream);
                        }
                        book.File = fileName;
                    }


                    if (viewModel.Image != null)
                    {
                        var memory = new MemoryStream();
                        await viewModel.Image.CopyToAsync(memory);

                        using (var img = new MagickImage(memory.ToArray()))
                        {
                            img.Quality = 50;
                            book.Image = img.ToByteArray();
                        }

                    }

                    _UW.BaseRepository<Book>().Update(book);

                    if (viewModel.AuthorID != null)
                    {
                        var all = await (from a in _UW._context.Author_Books where (a.BookID == book.BookID) select a.AuthorID).ToArrayAsync();
                        var del = all.Except(viewModel.AuthorID).ToArray();
                        var add = viewModel.AuthorID.Except(all).ToArray();
                        var tempsAdd = new List<Author_Book>();
                        foreach (var i in add)
                        {
                            var temp = new Author_Book { BookID = book.BookID, AuthorID = i };
                            tempsAdd.Add(temp);
                        }
                        await _UW.BaseRepository<Author_Book>().CreateRangeAsync(tempsAdd);

                        var tempsDel = new List<Author_Book>();
                        foreach (var i in del)
                        {
                            var temp = new Author_Book { BookID = book.BookID, AuthorID = i };
                            tempsDel.Add(temp);
                        }
                        _UW.BaseRepository<Author_Book>().DeleteRange(tempsDel);
                    }
                    if (viewModel.CategoryID != null)
                    {
                        var all = await (from a in _UW._context.Category_Books where (a.BookID == book.BookID) select a.CategoryID).ToArrayAsync();
                        var del = all.Except(viewModel.CategoryID).ToArray();
                        var add = viewModel.CategoryID.Except(all).ToArray();
                        await _UW.BaseRepository<Category_Book>().CreateRangeAsync((from n in add select new Category_Book { BookID = book.BookID, CategoryID = n }).ToList());
                        _UW.BaseRepository<Category_Book>().DeleteRange((from n in del select new Category_Book { BookID = book.BookID, CategoryID = n }).ToList());
                    }
                    if (viewModel.TranslatorID != null)
                    {
                        var all =await (from t in _UW._context.Translator_Books where (t.BookID == book.BookID) select t.TranslatorID).ToArrayAsync();
                        var del = all.Except(viewModel.TranslatorID).ToArray();
                        var add = viewModel.TranslatorID.Except(all);
                        await _UW.BaseRepository<Translator_Book>().CreateRangeAsync((from t in add select new Translator_Book { BookID = book.BookID, TranslatorID = t }).ToList());
                        _UW.BaseRepository<Translator_Book>().DeleteRange((from t in del select new Translator_Book { TranslatorID = t, BookID = book.BookID }).ToList());
                    }

                    TempData["success"] = "به‌روز‌رسانی کتاب با موفقیت انجام شد.";
                    TempData["error"] = null;
                    await _UW.Commit();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    TempData["success"] = null;
                    TempData["error"] = "در به‌روز‌رسانی کتاب خطایی رخ داده است.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            else
            {
                var book = await _UW._context.Books.Include(l => l.Language).Include(p => p.Publisher)
                    .FromSql("SELECT * FROM BookInfo WHERE BookID={0}", id).FirstOrDefaultAsync();
                if (book==null)
                {
                    return NotFound();
                }
                else
                {
                    ViewBag.Author =await (from n in _UW._context.Author_Books.Include(n=>n.Author) where (n.BookID == id) select n.Author.FirstName+" "+n.Author.LastName).AsNoTracking().ToListAsync();
                    ViewBag.Category =await (from n in _UW._context.Category_Books.Include(n=>n.Category) where (n.BookID == id) select n.Category.CategoryName).AsNoTracking().ToListAsync();
                    ViewBag.Translator =await (from n in _UW._context.Translator_Books.Include(n=>n.Translator) where (n.BookID == id) select n.Translator.FirstName + " " + n.Translator.LastName).AsNoTracking().ToListAsync();

                    return View(book);
                }
            }
        }

        public async Task<IActionResult> Download(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await _UW.BaseRepository<Book>().FindByIDAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            var path = Path.Combine(_env.WebRootPath, "BookFile", book.File);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/pdf", "چند صفحه از کتاب " + book.Title);
        }

        public async Task<IActionResult> ShowImage(int? id)
        {
            var book = await _UW.BaseRepository<Book>().FindByIDAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            MemoryStream memory = new MemoryStream(book.Image);

            return new FileStreamResult(memory, "image/png");
        }

    }
}