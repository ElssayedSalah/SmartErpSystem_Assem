using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Reposetories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BusinessLayer.Services
{
    public class RerportsBusinessService: IRerportsBusinessService
    {
        private readonly IRerportsReposetory _RerportsReposetory;
        public RerportsBusinessService(IRerportsReposetory RerportsReposetory)
        {
            _RerportsReposetory = RerportsReposetory;

        }        
        public List<OpenBalanceReportView> GetOpenBalanceReportView(Expression<Func<OpenBalanceReportView, bool>> condetion = null)
        {
            return _RerportsReposetory.GetOpenBalanceReportView(condetion);
        }
        public List<ItemDataReportView> GetItemDataReportView(Expression<Func<ItemDataReportView, bool>> condetion = null)
        {
            return _RerportsReposetory.GetItemDataReportView(condetion);
        }

       public List<ItemBalanceQuantityAndValueReportView> GetItemBalanceQuantityAndValueReportView(Expression<Func<ItemBalanceQuantityAndValueReportView, bool>> condetion = null)
        {
            return _RerportsReposetory.GetItemBalanceQuantityAndValueReportView(condetion);
        }

        public List<ItemCartReportView> GetItemCartReportView(Expression<Func<ItemCartReportView, bool>> condetion = null)
        {
            return _RerportsReposetory.GetItemCartReportView(condetion);
        } 
        public List<ItemPurshasAnaysisReportView> GetItemPurshasAnaysisReportView(Expression<Func<ItemPurshasAnaysisReportView, bool>> condetion = null)
        {
            return _RerportsReposetory.GetItemPurshasAnaysisReportView(condetion);
        }
        public List<ItemSalesAnaysisReportView> GetItemSalesAnaysisReportView(Expression<Func<ItemSalesAnaysisReportView, bool>> condetion = null)
        {
            return _RerportsReposetory.GetItemSalesAnaysisReportView(condetion);
        } 
        public List<AlAstazAccountReportView> GetAlAstazAccountReportView(Expression<Func<AlAstazAccountReportView, bool>> condetion = null)
        {
            return _RerportsReposetory.GetAlAstazAccountReportView(condetion);
        }



















    }
}
