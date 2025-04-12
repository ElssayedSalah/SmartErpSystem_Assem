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
    public class FinanceRepository<T1, T2> : BaseReposetory<T1>, IFinanceRepository<T1, T2> where T1 : class where T2 : class
    {       
        public FinanceRepository(AppDbContext context) : base(context)
        {
           
        }
        public void AddEntry(T1 entry)
        {
            using (var Trans = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Set<T1>().Add(entry);
                    _context.SaveChanges();
                    Trans.Commit();
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                }

            }
        }
        public void UpdateEntry(T1 master, List<T2> oldDetails)
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

        public List<T2> GetEntryDetails(Expression<Func<T2, bool>> condetion = null)
        {
            IQueryable<T2> query = _context.Set<T2>();
            var Details = query.Where(condetion).ToList();

            return Details;

        }


        public int GetLastEntryNumber(Expression<Func<T1, object>> orderBy = null, Expression<Func<T1, bool>> condetion = null)
        {
            int LastEntryNumber = 0;
            IQueryable<T1> query = _context.Set<T1>();
            if (condetion != null)
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
                LastEntryNumber = (int)LastRecord.GetType().GetProperty("EntryNumber").GetValue(LastRecord) + 1;
            }
            else
            {
                LastEntryNumber = 1;
            }
            return LastEntryNumber;
        }
    }
}
