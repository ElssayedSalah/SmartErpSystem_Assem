using DataAccessLayer.Reposetories;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
   public class InventoryService<T1,T2>:BaseService<T1>, IInventoryService<T1,T2> where T1 : class where T2 : class
    {
        private readonly IInventoryRepository<T1,T2> _InventoryRepository;

        public InventoryService(IInventoryRepository<T1, T2> InventoryRepository, IBaseReposetory<T1> _BaseReposetory) :base(_BaseReposetory)
        {
            _InventoryRepository = InventoryRepository;

        }
        public void AddInventoryTransaction(T1 master)
        {
            _InventoryRepository.AddInventoryTransaction(master);
        }

        public List<T2> GetInventoryTransactionDetails(Expression<Func<T2, bool>> condetion = null)
        {
            return _InventoryRepository.GetInventoryTransactionDetails(condetion);
        }

        public void UpdateInventoryTransaction(T1 master, List<T2> oldDetails)
        {
            _InventoryRepository.UpdateInventoryTransaction(master,oldDetails);
        }
        public T2 GetInventoryTransactionDetailsById(int Id)
        {
            return _InventoryRepository.GetInventoryTransactionDetailsById(Id);

        }
        public void DeleteInventoryTransactionDetail(T2 detail)
        {
            _InventoryRepository.DeleteInventoryTransactionDetail(detail);
        }

        public List<T1> GetInventoryTransactions(Expression<Func<T1, bool>> condetion = null)
        {
           return _InventoryRepository.GetInventoryTransactions(condetion);
        }

        public List<T1> GetInventoryWithInclude(Expression<Func<T1, bool>> condetion = null, string[] includes = null)
        {
            return _InventoryRepository.GetInventoryWithInclude(condetion, includes);
        }


    }
}
