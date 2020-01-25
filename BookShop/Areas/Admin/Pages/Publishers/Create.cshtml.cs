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
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _UW;
        public CreateModel(IUnitOfWork uw)
        {
            _UW = uw;
        }
        [BindProperty]
        public Publisher Publisher { get; set; }
        public IActionResult OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _UW.BaseRepository<Publisher>().CreateAsync(Publisher);
                await _UW.Commit();
                return RedirectToPage("./Index");
            }
            return Page();
        }

    }
}