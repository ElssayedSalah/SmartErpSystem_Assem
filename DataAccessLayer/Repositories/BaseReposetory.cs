
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccessLayer.Data;

namespace DataAccessLayer.Reposetories
{
    public class BaseReposetory<T> : IBaseReposetory<T> where T : class
    {
        protected AppDbContext _context;
        public BaseReposetory(AppDbContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public int Delete(T entity)
        {
            _context.Set<T>().Remove(entity); 
             return _context.SaveChanges();

        }

        public int DeleteMany(List<T> entitys)
        {
            _context.Set<T>().RemoveRange(entitys);
            return _context.SaveChanges();

        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();            

        }

        public List<T> GetAllWithInclude(string[] includes = null)
        {
            IQueryable<T> query= _context.Set<T>();
            if (includes!=null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }
            return query.ToList();

        }

        public T GetById(int Id)
        {
            return _context.Set<T>().Find(Id);
        }
        public T GetByIdWithInclude(int Id,string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>().Find(Id) as IQueryable<T>;
            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }
            return query.FirstOrDefault();

        }

        public int GetLastCode(Expression<Func<T, object>> orderBy = null, Expression<Func<T, bool>> condetion = null) 
        {
            int LastCode = 0;
            IQueryable<T> query = _context.Set<T>();
            if (condetion!=null)
            {
                query = query.Where(condetion);
            }
            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }
            var LastRecord = query.LastOrDefault();
            if (LastRecord != null)
            {
                LastCode = (int)LastRecord.GetType().GetProperty("Code").GetValue(LastRecord) + 1;
            }
            else
            {
                LastCode = 1;
            }
            return  LastCode;
        }       
       
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();

        }
        //public void AddInventoryTransaction(T master)
        //{
        //    using (var Trans = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            Add(master);                  
        //            _context.SaveChanges();
        //            Trans.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            Trans.Rollback();
        //        }

        //    }   
        //}

        public List<T> GetWithCondetion(Expression<Func<T, bool>> condetion = null)
        {
            IQueryable<T> query = _context.Set<T>();
            var result = query.Where(condetion).ToList();
            return result;

        }


        public bool IsExistRecord(Expression<Func<T, bool>> condetion = null)
        {
            bool IsExist = false;
            IQueryable<T> query = _context.Set<T>();
            var Record = query.Where(condetion).FirstOrDefault();
            if (Record!=null)
            {
                IsExist = true;
            }
            return IsExist;
        }

        //public List<T> GetViewResult(Expression<Func<T, bool>> condetion = null)
        //{
        //    IQueryable<T> query = _context.Set<T>().Where(condetion);
           
        //    return query.AsNoTracking().ToList();
        //}

    }
}
