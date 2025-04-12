using DataAccessLayer.Reposetories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
   public interface IInventoryRepository<T1,T2> where T1 : class where T2 : class
    {
        void AddInventoryTransaction(T1 master);
        void UpdateInventoryTransaction(T1 master, List<T2> oldDetails);
        public List<T2> GetInventoryTransactionDetails(Expression<Func<T2, bool>> condetion = null);
        public T2 GetInventoryTransactionDetailsById(int Id);
        public void DeleteInventoryTransactionDetail(T2 detail);
        public List<T1> GetInventoryTransactions(Expression<Func<T1, bool>> condetion = null);
        public List<T1> GetInventoryWithInclude(Expression<Func<T1, bool>> condetion = null, string[] includes = null);
    }
}
