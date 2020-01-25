using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Models;
using BookShop.Models.Repositories;
using BookShop.Models.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _UW;
        public HomeController(IUnitOfWork UW)
        {
            _UW = UW;
        }


        public async Task<IActionResult> Index(string id,string title="")
        {
           
            if (id != null)
                ViewBag.ConfirmEmailAlert = "لینک فعال سازی حساب کاربری به ایمیل شما ارسال شد لطفا با کلیک روی این لینک حساب خود را فعال کنید.";


            var books =await _UW.BooksRepository.GetLastTopPublishBook(5);
            ViewBag.Categories = _UW.BooksRepository.GetAllCategories();

            return View(books);
        }


        public async Task<IActionResult> ShowBooksWithCategory(int? catID)
        {
            if (catID==null)
            {
                return NotFound();
            }
            var cat =await _UW.BaseRepository<Category>().FindByIDAsync(catID);
            if (cat == null)
            {
                return NotFound();
            }
            ViewBag.Category = cat;
            ViewBag.Categories = _UW.BooksRepository.GetAllCategories();
            var books = await _UW.BooksRepository.FindBooksOfCategory(catID.Value);

            return View(books);
        }


    }
}