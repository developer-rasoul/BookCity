using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookShop.Models;
using BookShop.Models.UnitOfWork;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorsController : Controller
    {
        private readonly IUnitOfWork _UW;

        public AuthorsController(IUnitOfWork context)
        {
            _UW = context;
        }

        // GET: Admin/Authors
        public async Task<IActionResult> Index()
        {
            //var author = await _UW.BaseRepository<Author>().FindAllAsync();
            var author = await _UW.BaseRepository<Author>().FindByCondition(null,a=>a.OrderByDescending(b=>b.FirstName));
            return View(author);
        }

        // GET: Admin/Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _UW.BaseRepository<Author>().FindByIDAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Admin/Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorID,FirstName,LastName")] Author author)
        {
            if (ModelState.IsValid)
            {
              await  _UW.BaseRepository<Author>().CreateAsync(author);
                await _UW.Commit();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Admin/Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _UW.BaseRepository<Author>().FindByIDAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Admin/Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorID,FirstName,LastName")] Author author)
        {
            if (id != author.AuthorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _UW.BaseRepository<Author>().Update(author);
                    await _UW.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_UW.BaseRepository<Author>().FindByID(author.AuthorID)==null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(author);
        }

        // GET: Admin/Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _UW.BaseRepository<Author>().FindByIDAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Admin/Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _UW.BaseRepository<Author>().FindByIDAsync(id);
            _UW.BaseRepository<Author>().Delete(author);
            await _UW.Commit();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ShowBooks(int? id)
        {
            if (id == null) return NotFound();
            var author =await _UW.BaseRepository<Author>().FindByIDAsync(id);
            if (author == null) return NotFound();
            else return View(author);
        }

    }
}
