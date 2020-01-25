using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookShop.Models.Repositories
{
    public class RepositoryBase<TEntity, TContext> : IRepositoryBase<TEntity> where TEntity : class where TContext : BookShopContext
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> dbset;
        public RepositoryBase(TContext context)
        {
            _context = context;
            dbset = _context.Set<TEntity>();
        }

        public IEnumerable<TEntity> FindAll() => dbset.AsNoTracking().ToList();
        public async Task<IEnumerable<TEntity>> FindAllAsync() => await dbset.AsNoTracking().ToListAsync();

        public TEntity FindByID(object id) => dbset.Find(id);
        public async Task<TEntity> FindByIDAsync(object id) => await dbset.FindAsync(id);

        public async Task<IEnumerable<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> where = null
            , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> order = null, params Expression<Func<TEntity, object>>[] include)
        {
            IQueryable<TEntity> temp = dbset;
            foreach (var item in include)
            {
                temp.Include(item);
            }
            if (where != null)
            {
                temp = temp.Where(where);
            }
            if (order != null)
            {
                temp = order(temp);
            }
            return await temp.ToListAsync();
        }



        public async Task CreateAsync(TEntity entity)
        {
            await dbset.AddAsync(entity);
        }
        public async Task CreateRangeAsync(List<TEntity> entity)
        {
            await dbset.AddRangeAsync(entity);
        }
        public void Create(TEntity entity)
        {
            dbset.Add(entity);
        }
        public void CreateRange(List<TEntity> entity)
        {
            dbset.AddRange(entity);
        }

        public void Update(TEntity entity)
        {
            dbset.Update(entity);
        }
        public void UpdateRange(List<TEntity> entity)
        {
            dbset.UpdateRange(entity);
        }


        public void Delete(TEntity entity)
        {
            dbset.Remove(entity);
        }
        public void DeleteRange(List<TEntity> entity)
        {
            dbset.RemoveRange(entity);
        }


        //////////////////////////////////////////////////////////////////////////////////

        public int Count()
        {
            return dbset.Count();
        }

        public async Task<IEnumerable<TEntity>> Paging(int currentPage, int pageSize)
        {
            var temp = await dbset.Skip((currentPage - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            return temp;
        }

    }
}
