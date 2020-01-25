using BookShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models.Repositories
{
    public interface IBooksRepository
    {
       
        List<BooksTreeCategoryViewModel> GetAllCategories();
        void FindAllSubCategories(BooksTreeCategoryViewModel category);

        Task<IEnumerable<BookIndexHomeViewModel>> GetAllPublishBook(string title = "");
        Task<IEnumerable<BookIndexHomeViewModel>> GetLastTopPublishBook(int Top = 5);
        int CategoryBooksCount(int categoryID);
        Task<IEnumerable<BookIndexHomeViewModel>> FindBooksOfCategory(int catID);
    }
}
