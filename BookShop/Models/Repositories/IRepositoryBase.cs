using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookShop.Models.Repositories
{
    public interface IRepositoryBase<TEntity>
    {

         IEnumerable<TEntity> FindAll();
         Task<IEnumerable<TEntity>> FindAllAsync() ;

         TEntity FindByID(object id); 
        Task<TEntity> FindByIDAsync(object id);

        Task<IEnumerable<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> where = null
            , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> order = null, params Expression<Func<TEntity, object>>[] include);
      



        Task CreateAsync(TEntity entity);
       
        Task CreateRangeAsync(List<TEntity> entity);

       
        void Create(TEntity entity);

        void CreateRange(List<TEntity> entity);
       

        void Update(TEntity entity);
       
        void UpdateRange(List<TEntity> entity);
       

        void Delete(TEntity entity);
       
        void DeleteRange(List<TEntity> entity);

        // /////////////////////////////////////////////////////////////////

        int Count();

        Task<IEnumerable<TEntity>> Paging(int currentPage, int pageSize);


    }
}
