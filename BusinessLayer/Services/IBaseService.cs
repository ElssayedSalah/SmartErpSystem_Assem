using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
   public interface IBaseService<T> where T : class
    {
        T GetById(int Id);
        List<T> GetAll();
        void Add(T entity);         
        Task AddRange(List<T> entity);
        void Update(T entity);
        int Delete(T entity);
        int DeleteMany(List<T> entitys);
        List<T> GetAllWithInclude(string[] includes = null);
        public T GetByIdWithInclude(int Id, string[] includes = null);
        int GetLastCode(Expression<Func<T, object>> orderBy = null, Expression<Func<T, bool>> condetion = null);
        bool IsExistRecord(Expression<Func<T, bool>> condetion = null);
        List<T> GetWithCondetion(Expression<Func<T, bool>> condetion = null);
        //List<T> GetViewResult(Expression<Func<T, bool>> condetion = null);

    }
}
