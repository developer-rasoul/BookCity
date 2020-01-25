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
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _UW;
        public EditModel(IUnitOfWork uw)
        {
            _UW = uw;
        }
        [BindProperty]
        public Publisher Publisher { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Publisher = await _UW.BaseRepository<Publisher>().FindByIDAsync(id);

            if (Publisher == null) return NotFound();

            return Page();


        }

        public async Task<IActionResult> OnPostAsync()
        {
            var orgin = await _UW.BaseRepository<Publisher>().FindByIDAsync(Publisher.PublisherID);

            if (ModelState.IsValid)
            {
                if (orgin.PublisherID!=Publisher.PublisherID)
                {
                    return NotFound();
                }
                orgin.PublisherName = Publisher.PublisherName;
                await _UW.Commit();
                return RedirectToPage("./Index");
            }
            return Page();
        }
    }
}