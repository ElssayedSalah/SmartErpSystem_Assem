using DataAccessLayer.Entities.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
   public interface IFinanceService<T1, T2> : IBaseService<T1> where T1 : class where T2 : class
    {
        void AddEntry(T1 entry);
        void UpdateEntry(T1 master, List<T2> oldDetails);
        int GetLastEntryNumber(Expression<Func<T1, object>> orderBy = null, Expression<Func<T1, bool>> condetion = null);
        public List<T2> GetEntryDetails(Expression<Func<T2, bool>> condetion = null);
    }
}
