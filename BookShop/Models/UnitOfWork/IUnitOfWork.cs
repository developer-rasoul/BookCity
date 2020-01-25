using BookShop.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models.UnitOfWork
{
    public interface IUnitOfWork
    {
        BookShopContext _context { get; }

        IBooksRepository BooksRepository { get; }

        IRepositoryBase<TEntity> BaseRepository<TEntity>() where TEntity : class;
        
        Task Commit();
    }
}
