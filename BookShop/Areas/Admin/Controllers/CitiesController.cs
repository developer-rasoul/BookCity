using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CitiesController : Controller
    {
        private readonly BookShopContext _context;
        public CitiesController(BookShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cities = await _context.Cities.Include(a=>a.province).ToListAsync();
            return View(cities);
        }

        public IActionResult Create()
        {
            ViewBag.Provinces = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(City city)
        {
            ViewBag.Provinces = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName");

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Cities.AddAsync(city);
                    await _context.SaveChangesAsync();
                    ViewData["Error"] = null;
                    ViewData["Success"] = "استان جدید با موفقیت ذخیره شد.";
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewData["Error"] = "در ذخیره سازی اطلاعات مشکلی یه وجود آمده، دوباره سعی کنید ...";
                    ViewData["Success"] = null;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(city);
            }
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewBag.Provinces = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName");
            return View(city);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(City city)
        {
            ViewBag.Provinces = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName");

            if (ModelState.IsValid)
            {
                try
                {
                     _context.Cities.Update(city);
                    await _context.SaveChangesAsync();
                    ViewData["Error"] = null;
                    ViewData["Success"] = "استان جدید با موفقیت ذخیره شد.";
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewData["Error"] = "در ذخیره سازی اطلاعات مشکلی یه وجود آمده، دوباره سعی کنید ...";
                    ViewData["Success"] = null;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(city);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            ViewData["Error"] = null;
            ViewData["Success"] = "استان " + city.CityName + " با موفقیت حذف شد.";
            return RedirectToAction("Index");
        }
    }
}