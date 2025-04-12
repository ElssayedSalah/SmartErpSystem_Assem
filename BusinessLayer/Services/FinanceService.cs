using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Reposetories;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessLayer.Services
{
    public class FinanceService<T1, T2> : BaseService<T1>, IFinanceService<T1, T2> where T1 : class where T2 : class
    {
        private readonly IFinanceRepository<T1, T2> _FinanceRepository;

        public FinanceService(IFinanceRepository<T1, T2> FinanceRepository, IBaseReposetory<T1> _BaseReposetory) :base(_BaseReposetory)
        {
            _FinanceRepository = FinanceRepository;

        }

        public void AddEntry(T1 entry)
        {
            _FinanceRepository.AddEntry(entry);
        }

        public void UpdateEntry(T1 master, List<T2> oldDetails)
        {
            _FinanceRepository.UpdateEntry(master, oldDetails);
        }
        public List<T2> GetEntryDetails(Expression<Func<T2, bool>> condetion = null)
        {
            return _FinanceRepository.GetEntryDetails(condetion);
        }

        public int GetLastEntryNumber(Expression<Func<T1, object>> orderBy = null, Expression<Func<T1, bool>> condetion = null)
        {
            return _FinanceRepository.GetLastEntryNumber(orderBy, condetion);

        }
    }
}
