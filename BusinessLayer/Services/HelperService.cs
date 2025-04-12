using DataAccessLayer.Data;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Repositories;
using System;
using System.Linq;

namespace BusinessLayer.Services
{
    public class HelperService: IHelperRepository
    {
        protected AppDbContext _context;
        public HelperService(AppDbContext context)
        {
            _context = context;
        }

        public void PrepareNewAccount(Account Account)
        {
            var allAccounts = _context.Accounts.ToList();

            var parentAccount = allAccounts.FirstOrDefault(x => x.Id == Account.ParentId);
            var accountCode = allAccounts.Count(x => x.ParentId == Account.ParentId) + 1;
            var AccountsLevels = _context.AccountSetting.ToList();
            if (parentAccount != null)
            {
                Account.Level = parentAccount.Level + 1;
                Account.AccountCode = parentAccount.AccountCode;
                if (parentAccount != null)
                {
                    Account.LastLevelInTree = parentAccount.Level + 1 == AccountsLevels.Count-1;
                    Account.ParentId = parentAccount.Id;
                    Account.AccountNatureId = parentAccount.AccountNatureId;
                    Account.PostTo = parentAccount.PostTo;
                    Account.PostType = parentAccount.PostType;

                }
            }
            var length = 0;
            if (AccountsLevels.Count > Account.Level)
                length = AccountsLevels[Account.Level].Length;
            Account.AccountCode += accountCode.ToString(new string('0', length));
        }

       
        public decimal GetItemBalance(int ItemId,int BranchId ,int StoreId, int FinancialPeriodId,int CompanyId,DateTime From ,DateTime To,int CurruntTransaction=0)
        {
            decimal balance = 0;
            var transactions = _context.Transaction_InvMaster.Where(x=>x.BranchId == BranchId && x.StoreId== StoreId &&x.FinancialPeriodId==FinancialPeriodId && x.CompanyId==CompanyId&&x.DocDate.Date>=From&& x.DocDate.Date<=To && x.Id != CurruntTransaction).ToList();
            var transactionsIds = transactions.Select(x=>x.Id).ToList();
            var details = _context.Transaction_InvDetails.Where(x=>transactionsIds.Contains(x.MasterId)).ToList();
            var documents = _context.Documents.Where(x=>x.DocSign!=0).ToList();
            var taransactionsAffectBalance= (from t in transactions
                                             join d in documents on t.DocTypeId equals d.DocTypeId
                                             join i in details on t.Id equals i.MasterId
                                             select new {i.Quntity,d.DocSign }).ToList();
            if (taransactionsAffectBalance.Count>0)
            {
                balance = taransactionsAffectBalance.Sum(x=>x.Quntity * x.DocSign);
            }


            return balance;
            
        }
    }
}
