using DataAccessLayer.Reposetories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
   public interface IFinanceRepository<T1, T2> where T1 : class where T2 : class
    {
        void AddEntry(T1 entry);
        public List<T2> GetEntryDetails(Expression<Func<T2, bool>> condetion = null);

        void UpdateEntry(T1 master, List<T2> oldDetails);

        int GetLastEntryNumber(Expression<Func<T1, object>> orderBy = null, Expression<Func<T1, bool>> condetion = null);


    }
}
