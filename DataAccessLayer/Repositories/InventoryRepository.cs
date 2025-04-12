using DataAccessLayer.Data;
using DataAccessLayer.Reposetories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class InventoryRepository<T1, T2> : BaseReposetory<T1>, IInventoryRepository<T1, T2> where T1 : class where T2 : class
    {
        //protected AppDbContext _context;
        public InventoryRepository(AppDbContext context) : base(context)
        {
            //_context = context;
        }

        public void AddInventoryTransaction(T1 master)
        {
            using (var Trans = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Set<T1>().Add(master);
                    _context.SaveChanges();
                    Trans.Commit();
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                }

            }
        }

        public void UpdateInventoryTransaction(T1 master, List<T2> oldDetails)
        {
            using (var Trans = _context.Database.BeginTransaction())
            {
                try
                {
                    if (oldDetails != null)
                    {
                        _context.Set<T2>().RemoveRange(oldDetails);
                    }
                    _context.Set<T1>().Update(master);
                    _context.SaveChanges();
                    Trans.Commit();
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                }

            }
        }

        public List<T2> GetInventoryTransactionDetails(Expression<Func<T2, bool>> condetion = null)
        {
            IQueryable<T2> query = _context.Set<T2>();
            var Details = query.Where(condetion).ToList();

            return Details;

        }
        public T2 GetInventoryTransactionDetailsById(int Id)
        {
            return _context.Set<T2>().Find(Id);

        }
        public void DeleteInventoryTransactionDetail(T2 detail)
        {
            _context.Set<T2>().Remove(detail);
        }

        public List<T1> GetInventoryTransactions(Expression<Func<T1, bool>> condetion = null)
        {
            IQueryable<T1> query = _context.Set<T1>();
            var masters = query.Where(condetion).ToList();

            return masters;
        }

        public List<T1> GetInventoryWithInclude(Expression<Func<T1, bool>> condetion = null, string[] includes = null)
        {
            IQueryable<T1> query = _context.Set<T1>().Where(condetion);
            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }
            return query.ToList();
        }
    }
}
