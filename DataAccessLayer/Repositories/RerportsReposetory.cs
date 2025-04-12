
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccessLayer.Data;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.Financial;

namespace DataAccessLayer.Reposetories
{
    public class RerportsReposetory : IRerportsReposetory
    {
        protected AppDbContext _context;
        public RerportsReposetory(AppDbContext context)
        {
            _context = context;
        }  
        public List<OpenBalanceReportView> GetWithCondetion(Expression<Func<OpenBalanceReportView, bool>> condetion = null)
        {
            IQueryable<OpenBalanceReportView> query = _context.Set<OpenBalanceReportView>();
            var result = query.Where(condetion).ToList();
            return result;

        }       
        public List<OpenBalanceReportView> GetOpenBalanceReportView(Expression<Func<OpenBalanceReportView, bool>> condetion = null)
        {
            IQueryable<OpenBalanceReportView> query = _context.Set<OpenBalanceReportView>().Where(condetion);
           
            return query.AsNoTracking().ToList();
        }
        public List<ItemDataReportView> GetItemDataReportView(Expression<Func<ItemDataReportView, bool>> condetion = null)
        {
            IQueryable<ItemDataReportView> query = _context.Set<ItemDataReportView>().Where(condetion);
           
            return query.AsNoTracking().ToList();
        } 
        public List<ItemBalanceQuantityAndValueReportView> GetItemBalanceQuantityAndValueReportView(Expression<Func<ItemBalanceQuantityAndValueReportView, bool>> condetion = null)
        {
            IQueryable<ItemBalanceQuantityAndValueReportView> query = _context.Set<ItemBalanceQuantityAndValueReportView>().Where(condetion);
           
            return query.AsNoTracking().ToList();
        }

        public List<ItemCartReportView> GetItemCartReportView(Expression<Func<ItemCartReportView, bool>> condetion = null)
        {
            IQueryable<ItemCartReportView> query = _context.Set<ItemCartReportView>().Where(condetion);

            return query.AsNoTracking().ToList();
        }
         public List<ItemPurshasAnaysisReportView> GetItemPurshasAnaysisReportView(Expression<Func<ItemPurshasAnaysisReportView, bool>> condetion = null)
        {
            IQueryable<ItemPurshasAnaysisReportView> query = _context.Set<ItemPurshasAnaysisReportView>().Where(condetion);

            return query.AsNoTracking().ToList();
        } 
        public List<ItemSalesAnaysisReportView> GetItemSalesAnaysisReportView(Expression<Func<ItemSalesAnaysisReportView, bool>> condetion = null)
        {
            IQueryable<ItemSalesAnaysisReportView> query = _context.Set<ItemSalesAnaysisReportView>().Where(condetion);

            return query.AsNoTracking().ToList();
        }
        public List<AlAstazAccountReportView> GetAlAstazAccountReportView(Expression<Func<AlAstazAccountReportView, bool>> condetion = null)
        {
            IQueryable<AlAstazAccountReportView> query = _context.Set<AlAstazAccountReportView>().Where(condetion);

            return query.AsNoTracking().ToList();
        }



    }
}
