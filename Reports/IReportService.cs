using BusinessLayer.Models.Filters;
using BusinessLayer.Models.Reports.Finance;
using DataAccessLayer.Entities.Financial;
using DevExpress.XtraReports.UI;
using Reports.Model;
using Reports.Models.Inventory;
using System.Collections.Generic;

namespace BusinessLayer.Services
{
    public interface IReportService
    {
        List<SalesInvoiceDs> SalesInvoicePrint(int InvId);
        List<SalesInvoiceDs> SalesReturnPrint(int DocId);
        List<SalesInvoiceDs> StoreRecivePrint(int DocId);
        List<SalesInvoiceDs> StoreOutPrint(int DocId);
        List<SalesInvoiceDs> StoreDeprecatePrint(int DocId);
        List<SalesInvoiceDs> OpenBalancePrint(int DocId);
        List<AddToStoreDs> AddToStorePrint(int DocId);
        List<PurchasesInvoiceDs> PurchasesInvoicePrint(int DocId);
        List<PurchasesInvoiceDs> PurchasesReturnPrint(int DocId);
        List<Store_Count_SettlementDs> StoreSettlementPrint(int DocId);
        List<Store_Count_SettlementDs> StoreCountPrint(int DocId);
        List<DailyEntryRptPrintDs> DailyEntryRptPrint(int DocId);
        List<TransactionRptPrintDs> CashDepositInBankRptPrint(int DocId);
        List<TransactionRptPrintDs> CashWithdrawalFromBankRptPrint(int DocId);
        List<TransactionRptPrintDs> CashRecieveRptPrint(int DocId);
        List<TransactionRptPrintDs> CashExchangeRptPrint(int DocId);
        List<TransactionRptPrintDs> WriteCheckOutRptPrint(int DocId);
        List<TransactionRptPrintDs> CheckOutExchangeRptPrint(int DocId);
        List<TransactionRptPrintDs> RecieveCheckInRptPrint(int DocId);
        List<TransactionRptPrintDs> CheckInExchangeRptPrint(int DocId);
        List<TransactionRptPrintDs> CheckReturnRptPrint(int DocId);



        #region InventoryRports
        List<OpenBalanceReportDs> OpenBalanceReport(OpenBalanceReportFilters Filters);
        List<ItemDataReportDs> ItemDataReport(ItemDataReportFilters Filters);
        List<ItemBalanceQuantityAndValueReportDs> ItemBalanceQuantityAndValueReport(ItemBalanceQuantityAndValueReportFilters Filters);
        List<ItemCartReportDs> ItemCartReport(ItemCartReportFilters Filters);
        List<ItemPurshasAnaysisReportDs> ItemPurshasAnaysisReport(ItemPurshasAnaysisReportFilters Filters);
        List<SupplierBalanceReportDs> SupplierBalanceReport(SupplierBalanceReportFilters Filters);
        List<SupplierBalanceTotalReportDs> SupplierBalanceTotalReport(SupplierBalanceTotalReportFilters Filters);

        List<ItemSalesAnaysisReportDs> ItemSalesAnaysisReport(ItemSalesAnaysisReportFilters Filters);
        List<CustomerBalanceReportDs> CustomerBalanceReport(CustomerBalanceReportFilters Filters);
        List<CustomerBalanceTotalReportDs> CustomerBalanceTotalReport(CustomerBalanceTotalReportFilters Filters);

        List<AlAstazAccountReportDs> AlAstazAccountReport(AlAstazAccountReportFilters Filters);
        List<ReviewBalanceReportDs> ReviewBalanceReport(ReviewBalanceReportFilters Filters);
        List<DailyAccountsDetailsReportDs> DailyAccountsDetailsReport(DailyAccountsDetailsReportFilters Filters);
        List<DailyAccountsTotalReportDs> DailyAccountsTotalReport(DailyAccountsTotalReportFilters Filters);
        List<BankStatementOfAccountReportDs> BankStatementOfAccountReport(BankStatementOfAccountReportFilters Filters);
        List<TreasuryStatementOfAccountReportDs> TreasuryStatementOfAccountReport(TreasuryStatementOfAccountReportFilters Filters);


        public IList<Account> GetChild(int Id, IList<Account> items);
        
        #endregion



        public void SetReportResult(XtraReport rpt, object src, BaseFiltersModel filters);
        public void SetRptParameter(XtraReport rpt, object filter, string ParamterName, object Value = null, bool IgnoreNullFilter = false);
    }
}
