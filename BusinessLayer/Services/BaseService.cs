using DataAccessLayer.Reposetories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IBaseReposetory<T> _BaseReposetory;
        public BaseService(IBaseReposetory<T> BaseReposetory)
        {
            _BaseReposetory = BaseReposetory;

        }
        public void Add(T entity)
        {
            _BaseReposetory.Add(entity);
        }
        
        public async Task AddRange(List<T> entity)
        {
            foreach (var item in entity)
            {
                _BaseReposetory.Add(item);
            }
            
        }

      

        public int Delete(T entity)
        {
          return _BaseReposetory.Delete(entity);

        }
        public int DeleteMany(List<T> entitys)
        {
            return _BaseReposetory.DeleteMany(entitys);

        }
        public List<T> GetAll()
        {
          return _BaseReposetory.GetAll();

        }

        public List<T> GetAllWithInclude(string[] includes = null)
        {
          return _BaseReposetory.GetAllWithInclude(includes);

        }

        public T GetById(int Id)
        {
            return _BaseReposetory.GetById(Id);

        }

        public T GetByIdWithInclude(int Id, string[] includes = null)
        {
            return _BaseReposetory.GetByIdWithInclude(Id, includes);
        }
        //public void AddInventoryTransaction(T master)
        //{
        //    _BaseReposetory.AddInventoryTransaction(master);
        //}
        //public List<T> GetInventoryTransactionDetails(Expression<Func<T, bool>> condetion = null)
        //{
        //    return _BaseReposetory.GetInventoryTransactionDetails(condetion);
        //}

        public int GetLastCode(Expression<Func<T, object>> orderBy = null, Expression<Func<T, bool>> condetion = null)
        {
          return _BaseReposetory.GetLastCode(orderBy, condetion);

        }

        public bool IsExistRecord(Expression<Func<T, bool>> condetion = null)
        {
          return  _BaseReposetory.IsExistRecord(condetion);

        }

        public void Update(T entity)
        {
            _BaseReposetory.Update(entity);

        }

        public List<T> GetWithCondetion( Expression<Func<T, bool>> condetion = null)
        {
            return _BaseReposetory.GetWithCondetion(condetion);

        }

        //public List<T> GetViewResult(Expression<Func<T, bool>> condetion = null)
        //{
        //    return _BaseReposetory.GetViewResult(condetion);
        //}
    }
}
