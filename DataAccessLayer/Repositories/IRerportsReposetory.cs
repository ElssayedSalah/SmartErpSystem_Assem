using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Reposetories
{
    public interface IRerportsReposetory
    {     

        List<OpenBalanceReportView> GetWithCondetion(Expression<Func<OpenBalanceReportView, bool>> condetion = null);
        List<OpenBalanceReportView> GetOpenBalanceReportView(Expression<Func<OpenBalanceReportView, bool>> condetion = null);
        List<ItemDataReportView> GetItemDataReportView(Expression<Func<ItemDataReportView, bool>> condetion = null);
        List<ItemBalanceQuantityAndValueReportView> GetItemBalanceQuantityAndValueReportView(Expression<Func<ItemBalanceQuantityAndValueReportView, bool>> condetion = null);
        List<ItemCartReportView> GetItemCartReportView(Expression<Func<ItemCartReportView, bool>> condetion = null);
        List<ItemPurshasAnaysisReportView> GetItemPurshasAnaysisReportView(Expression<Func<ItemPurshasAnaysisReportView, bool>> condetion = null);
        List<ItemSalesAnaysisReportView> GetItemSalesAnaysisReportView(Expression<Func<ItemSalesAnaysisReportView, bool>> condetion = null);
        List<AlAstazAccountReportView> GetAlAstazAccountReportView(Expression<Func<AlAstazAccountReportView, bool>> condetion = null);
    }
}
