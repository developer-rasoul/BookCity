using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Models;
using BookShop.Models.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Areas.Admin.Pages.Publishers
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _UW;
        public IndexModel(IUnitOfWork uw)
        {
            _UW = uw;
        }
        public int count { get; set; }
        public int TotalPages =>(int)Math.Ceiling(count / (pageSize*1.0));
        public int pageSize { get; set; } = 3;
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public bool Next => CurrentPage < TotalPages ;
        public bool Previous => CurrentPage > 1 ;
        public IEnumerable<Publisher> Publishers { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            count = _UW.BaseRepository<Publisher>().Count();
            Publishers = await _UW.BaseRepository<Publisher>().Paging(CurrentPage,pageSize);
            return Page();
        }


        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null) return NotFound();
            var Publisher = await _UW.BaseRepository<Publisher>().FindByIDAsync(id);
            if (Publisher == null) return NotFound();

            _UW.BaseRepository<Publisher>().Delete(Publisher);
            await _UW.Commit();
            return RedirectToPage("./Index");
        }

    }
}