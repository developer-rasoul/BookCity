using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProvincesController : Controller
    {
        private readonly BookShopContext _context;
        public ProvincesController(BookShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var Provinces = await _context.Provinces.ToListAsync();
            return View(Provinces);
        }

        public async Task<IActionResult> ShowCities(int? id)
        {
            if (id == null) return NotFound();
            var province = await _context.Provinces.SingleOrDefaultAsync(a => a.ProvinceID == id);
            await _context.Entry(province).Collection(p => p.Cities).LoadAsync();
            return View(province);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Province province)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    int temp = 0;
                    Random r = new Random();
                    var pid = await (from n in _context.Provinces select n.ProvinceID).ToArrayAsync();

                    do
                    {
                        temp = r.Next(1, 1000);
                    } while (pid.Contains(temp));

                    province.ProvinceID = temp;
                    await _context.Provinces.AddAsync(province);
                    await _context.SaveChangesAsync();
                    TempData["Error"] = null;
                    TempData["Success"] = "استان جدید با موفقیت ذخیره شد." ;
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["Error"] = "در ذخیره سازی اطلاعات مشکلی یه وجود آمده، دوباره سعی کنید ...";
                    TempData["Success"] = null;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(province);
            }
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var province = await _context.Provinces.FindAsync(id);
            if (province == null)
            {
                return NotFound();
            }
            return View(province);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Province province)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Provinces.Update(province);
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
                return View(province);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var province = await _context.Provinces.FindAsync(id);
            if (province == null)
            {
                return NotFound();
            }
            _context.Provinces.Remove(province);
            await _context.SaveChangesAsync();
            ViewData["Error"] = null;
            ViewData["Success"] = "استان " + province.ProvinceName + " با موفقیت حذف شد.";
            return RedirectToAction("Index");
        }
    }
}