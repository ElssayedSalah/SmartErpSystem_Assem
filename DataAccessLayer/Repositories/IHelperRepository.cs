using DataAccessLayer.Data;
using DataAccessLayer.Entities.Financial;
using System;

namespace DataAccessLayer.Repositories
{
    public interface IHelperRepository
    {
        void PrepareNewAccount(Account Account);
        /// <summary>
        /// حساب الرصيد الحالي للصنف في المخزن والفرع في فترة خلال السنة المالية 
        /// </summary>
        /// <param name="ItemId"></param>
        /// <param name="FinancialPeriodId"></param>
        /// <param name="CompanyId"></param>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <param name="CurruntTransaction"> رقم الحركة الحالي ليتم اثتثناء الحركة في حالة التعديل</param>
        /// <returns></returns>
        decimal GetItemBalance(int ItemId, int BranchId, int StoreId, int FinancialPeriodId, int CompanyId, DateTime From, DateTime To, int CurruntTransaction=0);
    }
}
