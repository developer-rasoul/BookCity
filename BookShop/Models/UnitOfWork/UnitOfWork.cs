using BookShop.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public BookShopContext _context { get; }
        private IBooksRepository _IBookRepository;
        public UnitOfWork(BookShopContext context)
        {
            _context = context;
        }

        public IRepositoryBase<TEntity> BaseRepository<TEntity>() where TEntity : class
        {
            IRepositoryBase<TEntity> repositoryBase = new RepositoryBase<TEntity, BookShopContext>(_context);
            return repositoryBase;
        }

        public IBooksRepository BooksRepository
        {
            get
            {
                if (_IBookRepository == null)
                {
                    _IBookRepository = new BookRepository(_context);
                }
                return _IBookRepository;
            }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }

}
