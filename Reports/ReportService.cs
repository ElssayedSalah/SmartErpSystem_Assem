using BusinessLayer.Models.Filters;
using BusinessLayer.Models.Reports.Finance;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.Purchases;
using DataAccessLayer.Entities.Sales;
using DataAccessLayer.Entities.System;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Hosting;
using PresentationLayer.Helpers;
using Reports;
using Reports.Model;
using Reports.Models.Inventory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using static BusinessLayer.Helpers.SharedEnums;

namespace BusinessLayer.Services
{
    public class ReportService : IReportService
    {
        private readonly string _currentLanguage;
        private readonly IInventoryService<Transaction_InvMaster, Transaction_InvDetails> _InventoryService;
        private readonly IBaseService<Item> _ItemService;
        private readonly IBaseService<Branch> _BranchService;
        private readonly IBaseService<Store> _StoreService;
        private readonly IBaseService<ItemGroup> _ItemGroupService;
        private readonly IBaseService<Unit> _UnitService;
        private readonly IBaseService<Customer> _CustomerService;
        private readonly IBaseService<Suppler> _SupplerService;
        private readonly IBaseService<Currency> _CurrencyService;
        private readonly IBaseService<Company> _CompanyService;
        private readonly IHostingEnvironment _Hosting;
        private readonly LocalizationService _LocalizationService;
        private readonly IRerportsBusinessService _RerportsBusinessService;
        private readonly IBaseService<Transaction_InvMaster> _InventoryMasterService;
        private readonly IBaseService<Document> _DocumentService;
        private readonly IBaseService<CustomerOpenBalance> _CustomerOpenBalance;
        private readonly IBaseService<SupplerOpenBalance> _SupplerOpenBalance;
        private readonly IBaseService<DailyAccounts_Def> _DailyAccounts_DefService;
        private readonly IBaseService<AccountOpenBalance> _AccountOpenBalanceService;
        private readonly IBaseService<Account> _AccountsService;
        private readonly IBaseService<DailyEntryMaster> _DailyEntryMasterService;
        private readonly IBaseService<DailyEntryDetails> _DailyEntryDetailsService;
        private readonly IBaseService<BankOpenBalance> _BankOpenBalanceService;
        private readonly IBaseService<Bank> _BankService;
        private readonly IBaseService<Treasury> _TreasuryService;
        private readonly IBaseService<TreasuryOpenBalance> _TreasuryOpenBalanceService;
        private readonly IFinanceService<DailyEntryMaster, DailyEntryDetails> _DailyEntryService;
        private readonly IBaseService<CashTransaction> _CashTransactionService;
        private readonly IBaseService<CheckTransaction> _CheckTransactionService;


        public ReportService(
             IInventoryService<Transaction_InvMaster, Transaction_InvDetails> InventoryService,
             IBaseService<Transaction_InvMaster> InventoryMasterService,
             IBaseService<Item> ItemService,
             IBaseService<ItemGroup> ItemGroupService,
             IBaseService<Unit> UnitService,
             IBaseService<Branch> BranchService,
             IBaseService<Store> StoreService,
             IBaseService<Customer> CustomerService,
             IBaseService<Suppler> SupplerService,
             IBaseService<Currency> CurrencyService,
             IBaseService<Company> CompanyService,
             IHostingEnvironment hosting,
             LocalizationService localizationService,
             IRerportsBusinessService RerportsBusinessService,
             IBaseService<Document> DocumentService,
             IBaseService<CustomerOpenBalance> CustomerOpenBalance,
             IBaseService<SupplerOpenBalance> SupplerOpenBalance,
             IBaseService<DailyAccounts_Def> DailyAccounts_DefService,
             IBaseService<AccountOpenBalance> AccountOpenBalanceService,
             IBaseService<Account> AccountsService,
             IBaseService<DailyEntryMaster> DailyEntryMasterService,
             IBaseService<DailyEntryDetails> DailyEntryDetailsService,
             IBaseService<BankOpenBalance> BankOpenBalanceService,
             IBaseService<Bank> BankService,
             IBaseService<Treasury> TreasuryService,
             IBaseService<TreasuryOpenBalance> TreasuryOpenBalanceService,
             IFinanceService<DailyEntryMaster, DailyEntryDetails> DailyEntryService,
             IBaseService<CashTransaction> CashTransactionService,
             IBaseService<CheckTransaction> CheckTransactionService

              )
        {
            _InventoryService = InventoryService;
            _ItemService = ItemService;
            _ItemGroupService = ItemGroupService;
            _UnitService = UnitService;
            _BranchService = BranchService;
            _StoreService = StoreService;
            _CustomerService = CustomerService;
            _SupplerService = SupplerService;
            _CurrencyService = CurrencyService;
            _CompanyService = CompanyService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _Hosting = hosting;
            _LocalizationService = localizationService;
            _RerportsBusinessService = RerportsBusinessService;
            _InventoryMasterService = InventoryMasterService;
            _DocumentService = DocumentService;
            _CustomerOpenBalance = CustomerOpenBalance;
            _SupplerOpenBalance = SupplerOpenBalance;
            _DailyAccounts_DefService = DailyAccounts_DefService;
            _AccountOpenBalanceService = AccountOpenBalanceService;
            _AccountsService = AccountsService;
            _DailyEntryMasterService = DailyEntryMasterService;
            _DailyEntryDetailsService = DailyEntryDetailsService;
            _BankOpenBalanceService = BankOpenBalanceService;
            _BankService = BankService;
            _TreasuryService = TreasuryService;
            _TreasuryOpenBalanceService = TreasuryOpenBalanceService;
            _DailyEntryService = DailyEntryService;
            _CashTransactionService = CashTransactionService;
            _CheckTransactionService = CheckTransactionService;

        }

        public string GetCompanyImage(Company company)
        {
            string ImagePath = "";
            string RootPath = Path.Combine(_Hosting.WebRootPath, "Images");
            ImagePath = Path.Combine(RootPath, company.ImagePath);
            return ImagePath;

        }
        #region Inventory Transactions Print

        public List<SalesInvoiceDs> SalesInvoicePrint(int InvId)
        {
            List<SalesInvoiceDs> DataSource = new List<SalesInvoiceDs>();
            SalesInvoiceDs DataSourceItem = new SalesInvoiceDs();
            var master = _InventoryService.GetById(InvId);
            var details = _InventoryService.GetInventoryTransactionDetails(x => x.MasterId == InvId);

            DataSourceItem.DocumentName = _LocalizationService.GetLocalizedHtmlString("SalesInvoice_Transacton").Value;
            DataSourceItem.DocDate = master.DocDate;
            DataSourceItem.DocCode = master.Code;
            DataSourceItem.InvoiceValue = master.InvoiceValue;
            DataSourceItem.TotalAdditions = master.TotalAdditions;
            DataSourceItem.TotalDisounts = master.TotalDisounts;
            DataSourceItem.InvoiceTotal = master.InvoiceTotal;
            DataSourceItem.TaxValue = master.TaxValue;
            DataSourceItem.InvoiceNet = master.InvoiceNet;

            var company = _CompanyService.GetById(master.CompanyId.Value);
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;

            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            var branch = _BranchService.GetById(master.BranchId.Value);
            DataSourceItem.BranchName = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            var store = _StoreService.GetById(master.StoreId.Value);
            DataSourceItem.StoreName = _currentLanguage == "ar" ? store.NameAr : store.NameEn;
            var customer = _CustomerService.GetById(master.CustomerId.Value);
            DataSourceItem.CustomerName = _currentLanguage == "ar" ? customer.NameAr : customer.NameEn;

            foreach (var item in details)
            {
                var d = new TransDetails();
                var invItem = _ItemService.GetById(item.ItemId);
                var itemGroup = _ItemGroupService.GetById(item.GroupId.Value);
                var itemUnit = _UnitService.GetById(item.UnitId.Value);

                d.ItemCode = invItem.Code;
                d.ItemName = _currentLanguage == "ar" ? invItem.NameAr : invItem.NameEn;
                d.GroupName = _currentLanguage == "ar" ? itemGroup.NameAr : itemGroup.NameEn;
                d.UnitName = _currentLanguage == "ar" ? itemUnit.NameAr : itemUnit.NameEn;
                d.Quntity = item.Quntity;
                d.DiscountType = item.DiscountType.HasValue ? _LocalizationService.GetLocalizedHtmlString("DiscountTypes" + "." + ((DiscountTypes)item.DiscountType.Value).ToString()) : "";
                d.DiscountValue = item.DiscountValue.HasValue ? item.DiscountValue.Value : 0;
                d.SalesPrice = item.SalesPrice.Value;
                d.PriceAfterDiscount = item.PriceAfterDiscount.Value;
                DataSourceItem.Details.Add(d);
            }
            DataSource.Add(DataSourceItem);
            return DataSource;


        }

        public List<SalesInvoiceDs> SalesReturnPrint(int DocId)
        {
            List<SalesInvoiceDs> DataSource = new List<SalesInvoiceDs>();
            SalesInvoiceDs DataSourceItem = new SalesInvoiceDs();
            var master = _InventoryService.GetById(DocId);
            var details = _InventoryService.GetInventoryTransactionDetails(x => x.MasterId == DocId);

            DataSourceItem.DocumentName = _LocalizationService.GetLocalizedHtmlString("SalesReturnMenuName").Value;
            DataSourceItem.DocDate = master.DocDate;
            DataSourceItem.DocCode = master.Code;
            DataSourceItem.InvoiceValue = master.InvoiceValue;
            DataSourceItem.TotalAdditions = master.TotalAdditions;
            DataSourceItem.TotalDisounts = master.TotalDisounts;
            DataSourceItem.InvoiceTotal = master.InvoiceTotal;
            DataSourceItem.TaxValue = master.TaxValue;
            DataSourceItem.InvoiceNet = master.InvoiceNet;

            var company = _CompanyService.GetById(master.CompanyId.Value);
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;

            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            var branch = _BranchService.GetById(master.BranchId.Value);
            DataSourceItem.BranchName = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            var store = _StoreService.GetById(master.StoreId.Value);
            DataSourceItem.StoreName = _currentLanguage == "ar" ? store.NameAr : store.NameEn;
            var customer = _CustomerService.GetById(master.CustomerId.Value);
            DataSourceItem.CustomerName = _currentLanguage == "ar" ? customer.NameAr : customer.NameEn;

            foreach (var item in details)
            {
                var d = new TransDetails();
                var invItem = _ItemService.GetById(item.ItemId);
                var itemGroup = _ItemGroupService.GetById(item.GroupId.Value);
                var itemUnit = _UnitService.GetById(item.UnitId.Value);

                d.ItemCode = invItem.Code;
                d.ItemName = _currentLanguage == "ar" ? invItem.NameAr : invItem.NameEn;
                d.GroupName = _currentLanguage == "ar" ? itemGroup.NameAr : itemGroup.NameEn;
                d.UnitName = _currentLanguage == "ar" ? itemUnit.NameAr : itemUnit.NameEn;
                d.Quntity = item.Quntity;
                d.SalesPrice = item.SalesPrice.Value;
                DataSourceItem.Details.Add(d);
            }
            DataSource.Add(DataSourceItem);
            return DataSource;


        }

        public List<SalesInvoiceDs> StoreRecivePrint(int DocId)
        {
            List<SalesInvoiceDs> DataSource = new List<SalesInvoiceDs>();
            SalesInvoiceDs DataSourceItem = new SalesInvoiceDs();
            var master = _InventoryService.GetById(DocId);
            var details = _InventoryService.GetInventoryTransactionDetails(x => x.MasterId == DocId);

            DataSourceItem.DocumentName = _LocalizationService.GetLocalizedHtmlString("StoreRecieveMenuName").Value;
            DataSourceItem.DocDate = master.DocDate;
            DataSourceItem.DocCode = master.Code;

            var company = _CompanyService.GetById(master.CompanyId.Value);
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;

            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            var branch = _BranchService.GetById(master.BranchId.Value);
            DataSourceItem.BranchName = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            var store = _StoreService.GetById(master.StoreId.Value);
            DataSourceItem.StoreName = _currentLanguage == "ar" ? store.NameAr : store.NameEn;
            foreach (var item in details)
            {
                var d = new TransDetails();
                var invItem = _ItemService.GetById(item.ItemId);
                var itemGroup = _ItemGroupService.GetById(item.GroupId.Value);
                var itemUnit = _UnitService.GetById(item.UnitId.Value);

                d.ItemCode = invItem.Code;
                d.ItemName = _currentLanguage == "ar" ? invItem.NameAr : invItem.NameEn;
                d.GroupName = _currentLanguage == "ar" ? itemGroup.NameAr : itemGroup.NameEn;
                d.UnitName = _currentLanguage == "ar" ? itemUnit.NameAr : itemUnit.NameEn;
                d.Quntity = item.Quntity;
                DataSourceItem.Details.Add(d);
            }
            DataSource.Add(DataSourceItem);
            return DataSource;
        }

        public List<SalesInvoiceDs> StoreOutPrint(int DocId)
        {
            List<SalesInvoiceDs> DataSource = new List<SalesInvoiceDs>();
            SalesInvoiceDs DataSourceItem = new SalesInvoiceDs();
            var master = _InventoryService.GetById(DocId);
            var details = _InventoryService.GetInventoryTransactionDetails(x => x.MasterId == DocId);

            DataSourceItem.DocumentName = _LocalizationService.GetLocalizedHtmlString("StoreOutMenuName").Value;
            DataSourceItem.DocDate = master.DocDate;
            DataSourceItem.DocCode = master.Code;
            var company = _CompanyService.GetById(master.CompanyId.Value);
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            DataSourceItem.CompanyLogo = GetCompanyImage(company);
            var branch = _BranchService.GetById(master.BranchId.Value);
            DataSourceItem.BranchName = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            var store = _StoreService.GetById(master.StoreId.Value);
            DataSourceItem.StoreName = _currentLanguage == "ar" ? store.NameAr : store.NameEn;
            foreach (var item in details)
            {
                var d = new TransDetails();
                var invItem = _ItemService.GetById(item.ItemId);
                var itemGroup = _ItemGroupService.GetById(item.GroupId.Value);
                var itemUnit = _UnitService.GetById(item.UnitId.Value);

                d.ItemCode = invItem.Code;
                d.ItemName = _currentLanguage == "ar" ? invItem.NameAr : invItem.NameEn;
                d.GroupName = _currentLanguage == "ar" ? itemGroup.NameAr : itemGroup.NameEn;
                d.UnitName = _currentLanguage == "ar" ? itemUnit.NameAr : itemUnit.NameEn;
                d.Quntity = item.Quntity;
                DataSourceItem.Details.Add(d);
            }
            DataSource.Add(DataSourceItem);
            return DataSource;
        }
        public List<SalesInvoiceDs> StoreDeprecatePrint(int DocId)
        {
            List<SalesInvoiceDs> DataSource = new List<SalesInvoiceDs>();
            SalesInvoiceDs DataSourceItem = new SalesInvoiceDs();
            var master = _InventoryService.GetById(DocId);
            var details = _InventoryService.GetInventoryTransactionDetails(x => x.MasterId == DocId);

            DataSourceItem.DocumentName = _LocalizationService.GetLocalizedHtmlString("StoreDeprecateMenuName").Value;
            DataSourceItem.DocDate = master.DocDate;
            DataSourceItem.DocCode = master.Code;

            var company = _CompanyService.GetById(master.CompanyId.Value);
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            var branch = _BranchService.GetById(master.BranchId.Value);
            DataSourceItem.BranchName = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            var store = _StoreService.GetById(master.StoreId.Value);
            DataSourceItem.StoreName = _currentLanguage == "ar" ? store.NameAr : store.NameEn;
            foreach (var item in details)
            {
                var d = new TransDetails();
                var invItem = _ItemService.GetById(item.ItemId);
                var itemGroup = _ItemGroupService.GetById(item.GroupId.Value);
                var itemUnit = _UnitService.GetById(item.UnitId.Value);

                d.ItemCode = invItem.Code;
                d.ItemName = _currentLanguage == "ar" ? invItem.NameAr : invItem.NameEn;
                d.GroupName = _currentLanguage == "ar" ? itemGroup.NameAr : itemGroup.NameEn;
                d.UnitName = _currentLanguage == "ar" ? itemUnit.NameAr : itemUnit.NameEn;
                d.Quntity = item.Quntity;
                DataSourceItem.Details.Add(d);
            }
            DataSource.Add(DataSourceItem);
            return DataSource;
        }

        public List<SalesInvoiceDs> OpenBalancePrint(int DocId)
        {
            List<SalesInvoiceDs> DataSource = new List<SalesInvoiceDs>();
            SalesInvoiceDs DataSourceItem = new SalesInvoiceDs();
            var master = _InventoryService.GetById(DocId);
            var details = _InventoryService.GetInventoryTransactionDetails(x => x.MasterId == DocId);

            DataSourceItem.DocumentName = _LocalizationService.GetLocalizedHtmlString("OpenBalanceMenuName").Value;
            DataSourceItem.DocDate = master.DocDate;
            DataSourceItem.DocCode = master.Code;

            var company = _CompanyService.GetById(master.CompanyId.Value);
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;

            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            var branch = _BranchService.GetById(master.BranchId.Value);
            DataSourceItem.BranchName = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            var store = _StoreService.GetById(master.StoreId.Value);
            DataSourceItem.StoreName = _currentLanguage == "ar" ? store.NameAr : store.NameEn;
            foreach (var item in details)
            {
                var d = new TransDetails();
                var invItem = _ItemService.GetById(item.ItemId);
                var itemGroup = _ItemGroupService.GetById(item.GroupId.Value);
                var itemUnit = _UnitService.GetById(item.UnitId.Value);

                d.ItemCode = invItem.Code;
                d.ItemName = _currentLanguage == "ar" ? invItem.NameAr : invItem.NameEn;
                d.GroupName = _currentLanguage == "ar" ? itemGroup.NameAr : itemGroup.NameEn;
                d.UnitName = _currentLanguage == "ar" ? itemUnit.NameAr : itemUnit.NameEn;
                d.Quntity = item.Quntity;
                d.SalesPrice = item.SalesPrice.Value;
                DataSourceItem.Details.Add(d);
            }
            DataSource.Add(DataSourceItem);
            return DataSource;
        }
        
        public List<AddToStoreDs> AddToStorePrint(int DocId)
        {
            List<AddToStoreDs> DataSource = new List<AddToStoreDs>();
            AddToStoreDs DataSourceItem = new AddToStoreDs();
            var master = _InventoryService.GetById(DocId);
            var details = _InventoryService.GetInventoryTransactionDetails(x => x.MasterId == DocId);

            DataSourceItem.DocumentName = _LocalizationService.GetLocalizedHtmlString("AddToStore").Value;
            DataSourceItem.DocDate = master.DocDate;
            DataSourceItem.DocCode = master.Code;

            var company = _CompanyService.GetById(master.CompanyId.Value);
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            DataSourceItem.CompanyLogo = GetCompanyImage(company);
            var branch = _BranchService.GetById(master.BranchId.Value);
            DataSourceItem.BranchName = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            var store = _StoreService.GetById(master.StoreId.Value);
            DataSourceItem.StoreName = _currentLanguage == "ar" ? store.NameAr : store.NameEn;

            var supplers =_SupplerService.GetById(master.SupplierId.Value);
            DataSourceItem.SupplierName = _currentLanguage == "ar" ? supplers.NameAr : supplers.NameEn;


            foreach (var item in details)
            {
                var d = new TransDetails();
                var invItem = _ItemService.GetById(item.ItemId);
                var itemGroup = _ItemGroupService.GetById(item.GroupId.Value);
                var itemUnit = _UnitService.GetById(item.UnitId.Value);

                d.ItemCode = invItem.Code;
                d.ItemName = _currentLanguage == "ar" ? invItem.NameAr : invItem.NameEn;
                d.GroupName = _currentLanguage == "ar" ? itemGroup.NameAr : itemGroup.NameEn;
                d.UnitName = _currentLanguage == "ar" ? itemUnit.NameAr : itemUnit.NameEn;
                d.Quntity = item.Quntity;               
                DataSourceItem.Details.Add(d);
            }
            DataSource.Add(DataSourceItem);
            return DataSource;
        }

        public List<PurchasesInvoiceDs> PurchasesInvoicePrint(int InvId)
        {
            List<PurchasesInvoiceDs> DataSource = new List<PurchasesInvoiceDs>();
            PurchasesInvoiceDs DataSourceItem = new PurchasesInvoiceDs();
            var master = _InventoryService.GetById(InvId);
            var details = _InventoryService.GetInventoryTransactionDetails(x => x.MasterId == InvId);

            DataSourceItem.DocumentName = _LocalizationService.GetLocalizedHtmlString("PurchasesInvoice_Transacton").Value;
            DataSourceItem.DocDate = master.DocDate;
            DataSourceItem.DocCode = master.Code;
            DataSourceItem.InvoiceValue = master.InvoiceValue;
            DataSourceItem.TotalAdditions = master.TotalAdditions;
            DataSourceItem.TotalDisounts = master.TotalDisounts;
            DataSourceItem.InvoiceTotal = master.InvoiceTotal;
            DataSourceItem.TaxValue = master.TaxValue;
            DataSourceItem.InvoiceNet = master.InvoiceNet;

            var company = _CompanyService.GetById(master.CompanyId.Value);
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;

            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            var branch = _BranchService.GetById(master.BranchId.Value);
            DataSourceItem.BranchName = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            var store = _StoreService.GetById(master.StoreId.Value);
            DataSourceItem.StoreName = _currentLanguage == "ar" ? store.NameAr : store.NameEn;
            var supplier = _SupplerService.GetById(master.SupplierId.Value);
            DataSourceItem.SupplerName = _currentLanguage == "ar" ? supplier.NameAr : supplier.NameEn;

            foreach (var item in details)
            {
                var d = new TransDetails();
                var invItem = _ItemService.GetById(item.ItemId);
                var itemGroup = _ItemGroupService.GetById(item.GroupId.Value);
                var itemUnit = _UnitService.GetById(item.UnitId.Value);

                d.ItemCode = invItem.Code;
                d.ItemName = _currentLanguage == "ar" ? invItem.NameAr : invItem.NameEn;
                d.GroupName = _currentLanguage == "ar" ? itemGroup.NameAr : itemGroup.NameEn;
                d.UnitName = _currentLanguage == "ar" ? itemUnit.NameAr : itemUnit.NameEn;
                d.Quntity = item.Quntity;
                d.PurchasePrice = item.PurchasePrice.Value;
                d.Total = item.Total.Value;
                DataSourceItem.Details.Add(d);
            }
            DataSource.Add(DataSourceItem);
            return DataSource;


        }

        public List<PurchasesInvoiceDs> PurchasesReturnPrint(int InvId)
        {
            List<PurchasesInvoiceDs> DataSource = new List<PurchasesInvoiceDs>();
            PurchasesInvoiceDs DataSourceItem = new PurchasesInvoiceDs();
            var master = _InventoryService.GetById(InvId);
            var details = _InventoryService.GetInventoryTransactionDetails(x => x.MasterId == InvId);

            DataSourceItem.DocumentName = _LocalizationService.GetLocalizedHtmlString("PurchasesReturnMenuName").Value;
            DataSourceItem.DocDate = master.DocDate;
            DataSourceItem.DocCode = master.Code;
            DataSourceItem.InvoiceValue = master.InvoiceValue;
            DataSourceItem.TotalAdditions = master.TotalAdditions;
            DataSourceItem.TotalDisounts = master.TotalDisounts;
            DataSourceItem.InvoiceTotal = master.InvoiceTotal;
            DataSourceItem.TaxValue = master.TaxValue;
            DataSourceItem.InvoiceNet = master.InvoiceNet;

            var company = _CompanyService.GetById(master.CompanyId.Value);
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;

            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            var branch = _BranchService.GetById(master.BranchId.Value);
            DataSourceItem.BranchName = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            var store = _StoreService.GetById(master.StoreId.Value);
            DataSourceItem.StoreName = _currentLanguage == "ar" ? store.NameAr : store.NameEn;
            var supplier = _SupplerService.GetById(master.SupplierId.Value);
            DataSourceItem.SupplerName = _currentLanguage == "ar" ? supplier.NameAr : supplier.NameEn;

            foreach (var item in details)
            {
                var d = new TransDetails();
                var invItem = _ItemService.GetById(item.ItemId);
                var itemGroup = _ItemGroupService.GetById(item.GroupId.Value);
                var itemUnit = _UnitService.GetById(item.UnitId.Value);

                d.ItemCode = invItem.Code;
                d.ItemName = _currentLanguage == "ar" ? invItem.NameAr : invItem.NameEn;
                d.GroupName = _currentLanguage == "ar" ? itemGroup.NameAr : itemGroup.NameEn;
                d.UnitName = _currentLanguage == "ar" ? itemUnit.NameAr : itemUnit.NameEn;
                d.Quntity = item.Quntity;
                d.PurchasePrice = item.PurchasePrice.Value;
                d.Total = item.Total.Value;
                DataSourceItem.Details.Add(d);
            }
            DataSource.Add(DataSourceItem);
            return DataSource;


        }

        public List<Store_Count_SettlementDs> StoreSettlementPrint(int InvId)
        {
            List<Store_Count_SettlementDs> DataSource = new List<Store_Count_SettlementDs>();
            Store_Count_SettlementDs DataSourceItem = new Store_Count_SettlementDs();
            var master = _InventoryService.GetById(InvId);
            var details = _InventoryService.GetInventoryTransactionDetails(x => x.MasterId == InvId);
            //رقم حركة الجرد
            var StoreCount = _InventoryService.GetById(master.StoreCountId);

            DataSourceItem.DocumentName = _LocalizationService.GetLocalizedHtmlString("StoreSettlementMenuName").Value;
            DataSourceItem.DocDate = master.DocDate;
            DataSourceItem.DocCode = master.Code;
            DataSourceItem.CountDateFrom = master.CountDateFrom.Value;
            DataSourceItem.CountDateTo = master.CountDateTo.Value;
            DataSourceItem.StoreSurplusValue = master.StoreSurplusValue;
            DataSourceItem.StoreDeficitValue = master.StoreDeficitValue;
            DataSourceItem.EntryNumber = master.EntryNumber.HasValue ? master.EntryNumber.Value : 0;
            DataSourceItem.StoreCountNumber = StoreCount != null ? StoreCount.Code : 0;

            var company = _CompanyService.GetById(master.CompanyId.Value);
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            var branch = _BranchService.GetById(master.BranchId.Value);
            DataSourceItem.BranchName = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            var store = _StoreService.GetById(master.StoreId.Value);
            DataSourceItem.StoreName = _currentLanguage == "ar" ? store.NameAr : store.NameEn;

            foreach (var item in details)
            {
                var d = new TransDetails();
                var invItem = _ItemService.GetById(item.ItemId);
                var itemGroup = _ItemGroupService.GetById(item.GroupId.Value);
                var itemUnit = _UnitService.GetById(item.UnitId.Value);

                d.ItemCode = invItem.Code;
                d.ItemName = _currentLanguage == "ar" ? invItem.NameAr : invItem.NameEn;
                d.GroupName = _currentLanguage == "ar" ? itemGroup.NameAr : itemGroup.NameEn;
                d.UnitName = _currentLanguage == "ar" ? itemUnit.NameAr : itemUnit.NameEn;
                d.Quntity = item.Quntity;
                d.ActualQuntity = item.ActualQuntity;
                d.DifferenceQuntity = item.DifferenceQuntity;
                d.PurchasePrice = item.PurchasePrice.Value;
                d.Total = item.Total.Value;
                DataSourceItem.Details.Add(d);
            }
            DataSource.Add(DataSourceItem);
            return DataSource;


        }

        public List<Store_Count_SettlementDs> StoreCountPrint(int InvId)
        {
            List<Store_Count_SettlementDs> DataSource = new List<Store_Count_SettlementDs>();
            Store_Count_SettlementDs DataSourceItem = new Store_Count_SettlementDs();
            var master = _InventoryService.GetById(InvId);
            var details = _InventoryService.GetInventoryTransactionDetails(x => x.MasterId == InvId);
            //رقم حركة الجرد
            var StoreCount = _InventoryService.GetById(master.StoreCountId);

            DataSourceItem.DocumentName = _LocalizationService.GetLocalizedHtmlString("StoreSettlementMenuName").Value;
            DataSourceItem.DocDate = master.DocDate;
            DataSourceItem.DocCode = master.Code;
            DataSourceItem.CountDateFrom = master.CountDateFrom.Value;
            DataSourceItem.CountDateTo = master.CountDateTo.Value;
            DataSourceItem.StoreSurplusValue = master.StoreSurplusValue;
            DataSourceItem.StoreDeficitValue = master.StoreDeficitValue;
            DataSourceItem.EntryNumber = master.EntryNumber.HasValue ? master.EntryNumber.Value : 0;
            DataSourceItem.StoreCountNumber = StoreCount != null ? StoreCount.Code : 0;

            var company = _CompanyService.GetById(master.CompanyId.Value);
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            var branch = _BranchService.GetById(master.BranchId.Value);
            DataSourceItem.BranchName = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            var store = _StoreService.GetById(master.StoreId.Value);
            DataSourceItem.StoreName = _currentLanguage == "ar" ? store.NameAr : store.NameEn;

            foreach (var item in details)
            {
                var d = new TransDetails();
                var invItem = _ItemService.GetById(item.ItemId);
                var itemGroup = _ItemGroupService.GetById(item.GroupId.Value);
                var itemUnit = _UnitService.GetById(item.UnitId.Value);

                d.ItemCode = invItem.Code;
                d.ItemName = _currentLanguage == "ar" ? invItem.NameAr : invItem.NameEn;
                d.GroupName = _currentLanguage == "ar" ? itemGroup.NameAr : itemGroup.NameEn;
                d.UnitName = _currentLanguage == "ar" ? itemUnit.NameAr : itemUnit.NameEn;
                d.Quntity = item.Quntity;
                d.ActualQuntity = item.ActualQuntity;
                d.DifferenceQuntity = item.DifferenceQuntity;
                d.PurchasePrice = item.PurchasePrice.Value;
                d.Total = item.Total.Value;
                DataSourceItem.Details.Add(d);
            }
            DataSource.Add(DataSourceItem);
            return DataSource;


        }




        #endregion

        #region Finance Transactions Print
        public List<DailyEntryRptPrintDs> DailyEntryRptPrint(int DocId)
        {
            List<DailyEntryRptPrintDs> DataSource = new List<DailyEntryRptPrintDs>();
            DailyEntryRptPrintDs DataSourceItem = new DailyEntryRptPrintDs();
            var master = _DailyEntryService.GetById(DocId);
            var details = _DailyEntryService.GetEntryDetails(x => x.MasterId == master.Id);
            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree);
            var Currency = _CurrencyService.GetById(master.CurrencyId);
            var Document = _DocumentService.GetWithCondetion(x=>x.DocTypeId==master.DocType).FirstOrDefault();
            var DailyTypes = _DailyAccounts_DefService.GetById(master.DailyTypeId);
            var company = _CompanyService.GetById(master.CompanyId.Value);

            DataSourceItem.ReportName = _LocalizationService.GetLocalizedHtmlString("DailyEntry").Value;  
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            DataSourceItem.EntryNumber = master.EntryNumber;
            DataSourceItem.DocumentDate= master.TransactionDate;
            DataSourceItem.DocNumber = master.Code;
            DataSourceItem.Currency = _currentLanguage == "ar" ? Currency.NameAr: Currency.NameEn;
            DataSourceItem.CurrencyFactor = master.CurrencyFactor;
            DataSourceItem.DocumentName = _currentLanguage == "ar" ? Document.NameAr:Document.NameEn;
            DataSourceItem.DailyType = _currentLanguage == "ar" ? DailyTypes.NameAr: DailyTypes.NameEn;

            DataSourceItem.BalanceState = _LocalizationService.GetLocalizedHtmlString("EntryBalanceState." + Enum.GetValues(typeof(EntryBalanceState)).GetValue(master.EntryState).ToString());
            DataSourceItem.TransferState = _LocalizationService.GetLocalizedHtmlString("EntryTransferState." + Enum.GetValues(typeof(EntryTransferState)).GetValue(master.IsTransfered).ToString());
            DataSourceItem.EntryCreationMethod = _LocalizationService.GetLocalizedHtmlString("EntryCreationMethod." + Enum.GetValues(typeof(EntryCreationMethod)).GetValue(master.EntryCreationMethod).ToString());

            foreach (var item in details)
            {
                var d = new DailyEntryRptPrintDetailsDs();
                var account = allAccounts.Where(x=>x.Id==item.AccountId).FirstOrDefault();
                if (account!=null)
                {
                    d.AccountCode = account.AccountCode;
                    d.AccountName = account.Name;
                }
                //if (coastCenter != null)
                //{
                //    d.CostCenterCode = account.AccountCode;
                //    d.CostCenterName = account.Name;
                //}

                d.Debit = item.Debit;
                d.Credit = item.Credit;              
                DataSourceItem.Details.Add(d);
            }
            DataSource.Add(DataSourceItem);
            return DataSource;
        }
        public List<TransactionRptPrintDs> CashDepositInBankRptPrint(int DocId)
        {
            List<TransactionRptPrintDs> DataSource = new List<TransactionRptPrintDs>();
            TransactionRptPrintDs DataSourceItem = new TransactionRptPrintDs();
            var master = _CashTransactionService.GetById(DocId);
            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree);
            var Currency = _CurrencyService.GetById(master.CurrencyId);
            var company = _CompanyService.GetById(master.CompanyId.Value);
            var branch = _BranchService.GetById(master.BranchId);

            DataSourceItem.ReportName = _LocalizationService.GetLocalizedHtmlString("CashDepositInBankTransaction").Value;  
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            DataSourceItem.EntryNumber = master.EntryNumber;
            DataSourceItem.DocumentDate= master.DocDate;
            DataSourceItem.DocNumber = master.Code;
            DataSourceItem.CurrencyFactor = master.CurrencyFactor;
            DataSourceItem.Amount = master.Amount;
            DataSourceItem.DiscountAmount = master.DiscountAmount;
            DataSourceItem.Notes = master.Notes;
            DataSourceItem.Currency = _currentLanguage == "ar" ? Currency.NameAr: Currency.NameEn;
            DataSourceItem.Branch = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;

            //بيانات الطرف الاول
            string FirstSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.FirstSideTypeId).ToString());
            string FirstSideName = GetTransactionSideName(master.FirstSideTypeId, master.FirstSideId);
            string FirstSideAccount = allAccounts.Where(x => x.Id == master.FirstSideAccountId).FirstOrDefault().Name;

            DataSourceItem.FirstSideType = FirstSideType;
            DataSourceItem.FirstSideName = FirstSideName;
            DataSourceItem.FirstSideAccount =FirstSideAccount;

            //بيانات الطرف الثاني
            string SecondSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.SecondSideTypeId).ToString());
            string SecondSideName = GetTransactionSideName(master.SecondSideTypeId, master.SecondSideId);
            string SecondSideAccount = allAccounts.Where(x=>x.Id==master.SecondSideAccountId).FirstOrDefault().Name;

            DataSourceItem.SecondSideType = SecondSideType;
            DataSourceItem.SecondSideName = SecondSideName;
            DataSourceItem.SecondSideAccount = SecondSideAccount;

            var DiscountAccount = allAccounts.Where(x => x.Id == master.DiscountAccountId).FirstOrDefault();
            if (DiscountAccount!=null)
            {
                DataSourceItem.DiscountAccount = DiscountAccount.Name;
            }     
            DataSource.Add(DataSourceItem);
            return DataSource;
        }
        public List<TransactionRptPrintDs> CashWithdrawalFromBankRptPrint(int DocId)
        {
            List<TransactionRptPrintDs> DataSource = new List<TransactionRptPrintDs>();
            TransactionRptPrintDs DataSourceItem = new TransactionRptPrintDs();
            var master = _CashTransactionService.GetById(DocId);
            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree);
            var Currency = _CurrencyService.GetById(master.CurrencyId);
            var company = _CompanyService.GetById(master.CompanyId.Value);
            var branch = _BranchService.GetById(master.BranchId);

            DataSourceItem.ReportName = _LocalizationService.GetLocalizedHtmlString("CashWithdrawalFromBankTransaction").Value;  
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            DataSourceItem.EntryNumber = master.EntryNumber;
            DataSourceItem.DocumentDate= master.DocDate;
            DataSourceItem.DocNumber = master.Code;
            DataSourceItem.CurrencyFactor = master.CurrencyFactor;
            DataSourceItem.Amount = master.Amount;
            DataSourceItem.DiscountAmount = master.DiscountAmount;
            DataSourceItem.Notes = master.Notes;
            DataSourceItem.Currency = _currentLanguage == "ar" ? Currency.NameAr: Currency.NameEn;
            DataSourceItem.Branch = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;

            //بيانات الطرف الاول
            string FirstSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.FirstSideTypeId).ToString());
            string FirstSideName = GetTransactionSideName(master.FirstSideTypeId, master.FirstSideId);
            string FirstSideAccount = allAccounts.Where(x => x.Id == master.FirstSideAccountId).FirstOrDefault().Name;

            DataSourceItem.FirstSideType = FirstSideType;
            DataSourceItem.FirstSideName = FirstSideName;
            DataSourceItem.FirstSideAccount =FirstSideAccount;

            //بيانات الطرف الثاني
            string SecondSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.SecondSideTypeId).ToString());
            string SecondSideName = GetTransactionSideName(master.SecondSideTypeId, master.SecondSideId);
            string SecondSideAccount = allAccounts.Where(x=>x.Id==master.SecondSideAccountId).FirstOrDefault().Name;

            DataSourceItem.SecondSideType = SecondSideType;
            DataSourceItem.SecondSideName = SecondSideName;
            DataSourceItem.SecondSideAccount = SecondSideAccount;

            var DiscountAccount = allAccounts.Where(x => x.Id == master.DiscountAccountId).FirstOrDefault();
            if (DiscountAccount!=null)
            {
                DataSourceItem.DiscountAccount = DiscountAccount.Name;
            }     
            DataSource.Add(DataSourceItem);
            return DataSource;
        }
         public List<TransactionRptPrintDs> CashRecieveRptPrint(int DocId)
        {
            List<TransactionRptPrintDs> DataSource = new List<TransactionRptPrintDs>();
            TransactionRptPrintDs DataSourceItem = new TransactionRptPrintDs();
            var master = _CashTransactionService.GetById(DocId);
            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree);
            var Currency = _CurrencyService.GetById(master.CurrencyId);
            var company = _CompanyService.GetById(master.CompanyId.Value);
            var branch = _BranchService.GetById(master.BranchId);

            DataSourceItem.ReportName = _LocalizationService.GetLocalizedHtmlString("CashRecieveTransaction").Value;  
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            DataSourceItem.EntryNumber = master.EntryNumber;
            DataSourceItem.DocumentDate= master.DocDate;
            DataSourceItem.DocNumber = master.Code;
            DataSourceItem.CurrencyFactor = master.CurrencyFactor;
            DataSourceItem.Amount = master.Amount;
            DataSourceItem.DiscountAmount = master.DiscountAmount;
            DataSourceItem.Notes = master.Notes;
            DataSourceItem.Currency = _currentLanguage == "ar" ? Currency.NameAr: Currency.NameEn;
            DataSourceItem.Branch = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;

            //بيانات الطرف الاول
            string FirstSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.FirstSideTypeId).ToString());
            string FirstSideName = GetTransactionSideName(master.FirstSideTypeId, master.FirstSideId);
            string FirstSideAccount = allAccounts.Where(x => x.Id == master.FirstSideAccountId).FirstOrDefault().Name;

            DataSourceItem.FirstSideType = FirstSideType;
            DataSourceItem.FirstSideName = FirstSideName;
            DataSourceItem.FirstSideAccount =FirstSideAccount;

            //بيانات الطرف الثاني
            string SecondSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.SecondSideTypeId).ToString());
            string SecondSideName = GetTransactionSideName(master.SecondSideTypeId, master.SecondSideId);
            string SecondSideAccount = allAccounts.Where(x=>x.Id==master.SecondSideAccountId).FirstOrDefault().Name;

            DataSourceItem.SecondSideType = SecondSideType;
            DataSourceItem.SecondSideName = SecondSideName;
            DataSourceItem.SecondSideAccount = SecondSideAccount;

            var DiscountAccount = allAccounts.Where(x => x.Id == master.DiscountAccountId).FirstOrDefault();
            if (DiscountAccount!=null)
            {
                DataSourceItem.DiscountAccount = DiscountAccount.Name;
            }     
            DataSource.Add(DataSourceItem);
            return DataSource;
        }
        public List<TransactionRptPrintDs> CashExchangeRptPrint(int DocId)
        {
            List<TransactionRptPrintDs> DataSource = new List<TransactionRptPrintDs>();
            TransactionRptPrintDs DataSourceItem = new TransactionRptPrintDs();
            var master = _CashTransactionService.GetById(DocId);
            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree);
            var Currency = _CurrencyService.GetById(master.CurrencyId);
            var company = _CompanyService.GetById(master.CompanyId.Value);
            var branch = _BranchService.GetById(master.BranchId);

            DataSourceItem.ReportName = _LocalizationService.GetLocalizedHtmlString("CashExchangeTransaction").Value;  
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            DataSourceItem.EntryNumber = master.EntryNumber;
            DataSourceItem.DocumentDate= master.DocDate;
            DataSourceItem.DocNumber = master.Code;
            DataSourceItem.CurrencyFactor = master.CurrencyFactor;
            DataSourceItem.Amount = master.Amount;
            DataSourceItem.DiscountAmount = master.DiscountAmount;
            DataSourceItem.Notes = master.Notes;
            DataSourceItem.Currency = _currentLanguage == "ar" ? Currency.NameAr: Currency.NameEn;
            DataSourceItem.Branch = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;

            //بيانات الطرف الاول
            string FirstSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.FirstSideTypeId).ToString());
            string FirstSideName = GetTransactionSideName(master.FirstSideTypeId, master.FirstSideId);
            string FirstSideAccount = allAccounts.Where(x => x.Id == master.FirstSideAccountId).FirstOrDefault().Name;

            DataSourceItem.FirstSideType = FirstSideType;
            DataSourceItem.FirstSideName = FirstSideName;
            DataSourceItem.FirstSideAccount =FirstSideAccount;

            //بيانات الطرف الثاني
            string SecondSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.SecondSideTypeId).ToString());
            string SecondSideName = GetTransactionSideName(master.SecondSideTypeId, master.SecondSideId);
            string SecondSideAccount = allAccounts.Where(x=>x.Id==master.SecondSideAccountId).FirstOrDefault().Name;

            DataSourceItem.SecondSideType = SecondSideType;
            DataSourceItem.SecondSideName = SecondSideName;
            DataSourceItem.SecondSideAccount = SecondSideAccount;

            var DiscountAccount = allAccounts.Where(x => x.Id == master.DiscountAccountId).FirstOrDefault();
            if (DiscountAccount!=null)
            {
                DataSourceItem.DiscountAccount = DiscountAccount.Name;
            }     
            DataSource.Add(DataSourceItem);
            return DataSource;
        }
        public List<TransactionRptPrintDs> WriteCheckOutRptPrint(int DocId)
        {
            List<TransactionRptPrintDs> DataSource = new List<TransactionRptPrintDs>();
            TransactionRptPrintDs DataSourceItem = new TransactionRptPrintDs();
            var master = _CheckTransactionService.GetById(DocId);
            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree);
            var Currency = _CurrencyService.GetById(master.CurrencyId);
            var company = _CompanyService.GetById(master.CompanyId.Value);
            var branch = _BranchService.GetById(master.BranchId);
            var bank =_BankService.GetById(master.BankId);

            DataSourceItem.ReportName = _LocalizationService.GetLocalizedHtmlString("WriteCheckOutTransaction").Value;  
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            DataSourceItem.EntryNumber = master.EntryNumber;
            DataSourceItem.DueDate= master.DueDate;
            DataSourceItem.DocumentDate= master.DocDate;
            DataSourceItem.DocNumber = master.Code;
            DataSourceItem.CurrencyFactor = master.CurrencyFactor;
            DataSourceItem.Amount = master.Amount;
            DataSourceItem.DiscountAmount = master.DiscountAmount;
            DataSourceItem.CheckNumber = master.CheckNumber;
            DataSourceItem.Notes = master.Notes;
            DataSourceItem.Currency = _currentLanguage == "ar" ? Currency.NameAr: Currency.NameEn;
            DataSourceItem.Branch = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            DataSourceItem.Bank = _currentLanguage == "ar" ? bank.NameAr : bank.NameEn;

            //بيانات الطرف الاول
            string FirstSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.FirstSideTypeId).ToString());
            string FirstSideName = GetTransactionSideName(master.FirstSideTypeId, master.FirstSideId);
            string FirstSideAccount = allAccounts.Where(x => x.Id == master.FirstSideAccountId).FirstOrDefault().Name;

            DataSourceItem.FirstSideType = FirstSideType;
            DataSourceItem.FirstSideName = FirstSideName;
            DataSourceItem.FirstSideAccount =FirstSideAccount;

            //بيانات الطرف الثاني
            string SecondSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.SecondSideTypeId).ToString());
            string SecondSideName = GetTransactionSideName(master.SecondSideTypeId, master.SecondSideId);
            string SecondSideAccount = allAccounts.Where(x=>x.Id==master.SecondSideAccountId).FirstOrDefault().Name;

            DataSourceItem.SecondSideType = SecondSideType;
            DataSourceItem.SecondSideName = SecondSideName;
            DataSourceItem.SecondSideAccount = SecondSideAccount;

            var DiscountAccount = allAccounts.Where(x => x.Id == master.DiscountAccountId).FirstOrDefault();
            if (DiscountAccount!=null)
            {
                DataSourceItem.DiscountAccount = DiscountAccount.Name;
            }     
            DataSource.Add(DataSourceItem);
            return DataSource;
        }
        public List<TransactionRptPrintDs> CheckOutExchangeRptPrint(int DocId)
        {
            List<TransactionRptPrintDs> DataSource = new List<TransactionRptPrintDs>();
            TransactionRptPrintDs DataSourceItem = new TransactionRptPrintDs();
            var master = _CheckTransactionService.GetById(DocId);
            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree);
            var Currency = _CurrencyService.GetById(master.CurrencyId);
            var company = _CompanyService.GetById(master.CompanyId.Value);
            var branch = _BranchService.GetById(master.BranchId);
            var bank =_BankService.GetById(master.BankId);

            DataSourceItem.ReportName = _LocalizationService.GetLocalizedHtmlString("CheckOutExchangeTransaction").Value;  
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            DataSourceItem.EntryNumber = master.EntryNumber;
            DataSourceItem.DueDate= master.DueDate;
            DataSourceItem.DocumentDate= master.DocDate;
            DataSourceItem.DocNumber = master.Code;
            DataSourceItem.CurrencyFactor = master.CurrencyFactor;
            DataSourceItem.Amount = master.Amount;
            DataSourceItem.DiscountAmount = master.DiscountAmount;
            DataSourceItem.CheckNumber = master.CheckNumber;
            DataSourceItem.Notes = master.Notes;
            DataSourceItem.Currency = _currentLanguage == "ar" ? Currency.NameAr: Currency.NameEn;
            DataSourceItem.Branch = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            DataSourceItem.Bank = _currentLanguage == "ar" ? bank.NameAr : bank.NameEn;

            //بيانات الطرف الاول
            string FirstSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.FirstSideTypeId).ToString());
            string FirstSideName = GetTransactionSideName(master.FirstSideTypeId, master.FirstSideId);
            string FirstSideAccount = allAccounts.Where(x => x.Id == master.FirstSideAccountId).FirstOrDefault().Name;

            DataSourceItem.FirstSideType = FirstSideType;
            DataSourceItem.FirstSideName = FirstSideName;
            DataSourceItem.FirstSideAccount =FirstSideAccount;

            //بيانات الطرف الثاني
            string SecondSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.SecondSideTypeId).ToString());
            string SecondSideName = GetTransactionSideName(master.SecondSideTypeId, master.SecondSideId);
            string SecondSideAccount = allAccounts.Where(x=>x.Id==master.SecondSideAccountId).FirstOrDefault().Name;

            DataSourceItem.SecondSideType = SecondSideType;
            DataSourceItem.SecondSideName = SecondSideName;
            DataSourceItem.SecondSideAccount = SecondSideAccount;

            var DiscountAccount = allAccounts.Where(x => x.Id == master.DiscountAccountId).FirstOrDefault();
            if (DiscountAccount!=null)
            {
                DataSourceItem.DiscountAccount = DiscountAccount.Name;
            }     
            DataSource.Add(DataSourceItem);
            return DataSource;
        }
       
         public List<TransactionRptPrintDs> RecieveCheckInRptPrint(int DocId)
        {
            List<TransactionRptPrintDs> DataSource = new List<TransactionRptPrintDs>();
            TransactionRptPrintDs DataSourceItem = new TransactionRptPrintDs();
            var master = _CheckTransactionService.GetById(DocId);
            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree);
            var Currency = _CurrencyService.GetById(master.CurrencyId);
            var company = _CompanyService.GetById(master.CompanyId.Value);
            var branch = _BranchService.GetById(master.BranchId);
            var bank =_BankService.GetById(master.BankId);

            DataSourceItem.ReportName = _LocalizationService.GetLocalizedHtmlString("RecieveCheckInTransaction").Value;  
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            DataSourceItem.EntryNumber = master.EntryNumber;
            DataSourceItem.DueDate= master.DueDate;
            DataSourceItem.DocumentDate= master.DocDate;
            DataSourceItem.DocNumber = master.Code;
            DataSourceItem.CurrencyFactor = master.CurrencyFactor;
            DataSourceItem.Amount = master.Amount;
            DataSourceItem.DiscountAmount = master.DiscountAmount;
            DataSourceItem.CheckNumber = master.CheckNumber;
            DataSourceItem.Notes = master.Notes;
            DataSourceItem.Currency = _currentLanguage == "ar" ? Currency.NameAr: Currency.NameEn;
            DataSourceItem.Branch = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            DataSourceItem.Bank = _currentLanguage == "ar" ? bank.NameAr : bank.NameEn;

            //بيانات الطرف الاول
            string FirstSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.FirstSideTypeId).ToString());
            string FirstSideName = GetTransactionSideName(master.FirstSideTypeId, master.FirstSideId);
            string FirstSideAccount = allAccounts.Where(x => x.Id == master.FirstSideAccountId).FirstOrDefault().Name;

            DataSourceItem.FirstSideType = FirstSideType;
            DataSourceItem.FirstSideName = FirstSideName;
            DataSourceItem.FirstSideAccount =FirstSideAccount;

            //بيانات الطرف الثاني
            string SecondSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.SecondSideTypeId).ToString());
            string SecondSideName = GetTransactionSideName(master.SecondSideTypeId, master.SecondSideId);
            string SecondSideAccount = allAccounts.Where(x=>x.Id==master.SecondSideAccountId).FirstOrDefault().Name;

            DataSourceItem.SecondSideType = SecondSideType;
            DataSourceItem.SecondSideName = SecondSideName;
            DataSourceItem.SecondSideAccount = SecondSideAccount;

            var DiscountAccount = allAccounts.Where(x => x.Id == master.DiscountAccountId).FirstOrDefault();
            if (DiscountAccount!=null)
            {
                DataSourceItem.DiscountAccount = DiscountAccount.Name;
            }     
            DataSource.Add(DataSourceItem);
            return DataSource;
        }
        public List<TransactionRptPrintDs> CheckInExchangeRptPrint(int DocId)
        {
            List<TransactionRptPrintDs> DataSource = new List<TransactionRptPrintDs>();
            TransactionRptPrintDs DataSourceItem = new TransactionRptPrintDs();
            var master = _CheckTransactionService.GetById(DocId);
            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree);
            var Currency = _CurrencyService.GetById(master.CurrencyId);
            var company = _CompanyService.GetById(master.CompanyId.Value);
            var branch = _BranchService.GetById(master.BranchId);
            var bank =_BankService.GetById(master.BankId);

            DataSourceItem.ReportName = _LocalizationService.GetLocalizedHtmlString("CheckInExchangeTransaction").Value;  
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            DataSourceItem.EntryNumber = master.EntryNumber;
            DataSourceItem.DueDate= master.DueDate;
            DataSourceItem.DocumentDate= master.DocDate;
            DataSourceItem.DocNumber = master.Code;
            DataSourceItem.CurrencyFactor = master.CurrencyFactor;
            DataSourceItem.Amount = master.Amount;
            DataSourceItem.DiscountAmount = master.DiscountAmount;
            DataSourceItem.CheckNumber = master.CheckNumber;
            DataSourceItem.Notes = master.Notes;
            DataSourceItem.Currency = _currentLanguage == "ar" ? Currency.NameAr: Currency.NameEn;
            DataSourceItem.Branch = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            DataSourceItem.Bank = _currentLanguage == "ar" ? bank.NameAr : bank.NameEn;

            //بيانات الطرف الاول
            string FirstSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.FirstSideTypeId).ToString());
            string FirstSideName = GetTransactionSideName(master.FirstSideTypeId, master.FirstSideId);
            string FirstSideAccount = allAccounts.Where(x => x.Id == master.FirstSideAccountId).FirstOrDefault().Name;

            DataSourceItem.FirstSideType = FirstSideType;
            DataSourceItem.FirstSideName = FirstSideName;
            DataSourceItem.FirstSideAccount =FirstSideAccount;

            //بيانات الطرف الثاني
            string SecondSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.SecondSideTypeId).ToString());
            string SecondSideName = GetTransactionSideName(master.SecondSideTypeId, master.SecondSideId);
            string SecondSideAccount = allAccounts.Where(x=>x.Id==master.SecondSideAccountId).FirstOrDefault().Name;

            DataSourceItem.SecondSideType = SecondSideType;
            DataSourceItem.SecondSideName = SecondSideName;
            DataSourceItem.SecondSideAccount = SecondSideAccount;

            var DiscountAccount = allAccounts.Where(x => x.Id == master.DiscountAccountId).FirstOrDefault();
            if (DiscountAccount!=null)
            {
                DataSourceItem.DiscountAccount = DiscountAccount.Name;
            }     
            DataSource.Add(DataSourceItem);
            return DataSource;
        }
         public List<TransactionRptPrintDs> CheckReturnRptPrint(int DocId)
        {
            List<TransactionRptPrintDs> DataSource = new List<TransactionRptPrintDs>();
            TransactionRptPrintDs DataSourceItem = new TransactionRptPrintDs();
            var master = _CheckTransactionService.GetById(DocId);
            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree);
            var Currency = _CurrencyService.GetById(master.CurrencyId);
            var company = _CompanyService.GetById(master.CompanyId.Value);
            var branch = _BranchService.GetById(master.BranchId);
            var bank =_BankService.GetById(master.BankId);

            DataSourceItem.ReportName = _LocalizationService.GetLocalizedHtmlString("CheckReturnTransaction").Value;  
            DataSourceItem.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            DataSourceItem.CompanyLogo = GetCompanyImage(company);

            DataSourceItem.EntryNumber = master.EntryNumber;
            DataSourceItem.DueDate= master.DueDate;
            DataSourceItem.DocumentDate= master.DocDate;
            DataSourceItem.DocNumber = master.Code;
            DataSourceItem.CurrencyFactor = master.CurrencyFactor;
            DataSourceItem.Amount = master.Amount;
            DataSourceItem.DiscountAmount = master.DiscountAmount;
            DataSourceItem.CheckNumber = master.CheckNumber;
            DataSourceItem.Notes = master.Notes;
            DataSourceItem.Currency = _currentLanguage == "ar" ? Currency.NameAr: Currency.NameEn;
            DataSourceItem.Branch = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
            DataSourceItem.Bank = _currentLanguage == "ar" ? bank.NameAr : bank.NameEn;

            //بيانات الطرف الاول
            string FirstSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.FirstSideTypeId).ToString());
            string FirstSideName = GetTransactionSideName(master.FirstSideTypeId, master.FirstSideId);
            string FirstSideAccount = allAccounts.Where(x => x.Id == master.FirstSideAccountId).FirstOrDefault().Name;

            DataSourceItem.FirstSideType = FirstSideType;
            DataSourceItem.FirstSideName = FirstSideName;
            DataSourceItem.FirstSideAccount =FirstSideAccount;

            //بيانات الطرف الثاني
            string SecondSideType = _LocalizationService.GetLocalizedHtmlString("EntrySides." + Enum.GetValues(typeof(EntrySides)).GetValue(master.SecondSideTypeId).ToString());
            string SecondSideName = GetTransactionSideName(master.SecondSideTypeId, master.SecondSideId);
            string SecondSideAccount = allAccounts.Where(x=>x.Id==master.SecondSideAccountId).FirstOrDefault().Name;

            DataSourceItem.SecondSideType = SecondSideType;
            DataSourceItem.SecondSideName = SecondSideName;
            DataSourceItem.SecondSideAccount = SecondSideAccount;

            var DiscountAccount = allAccounts.Where(x => x.Id == master.DiscountAccountId).FirstOrDefault();
            if (DiscountAccount!=null)
            {
                DataSourceItem.DiscountAccount = DiscountAccount.Name;
            }     
            DataSource.Add(DataSourceItem);
            return DataSource;
        }
       
        
        
        #endregion


        #region InventoryRports
        public List<OpenBalanceReportDs> OpenBalanceReport(OpenBalanceReportFilters Filters)
        {
            List<OpenBalanceReportDs> DataSource = new List<OpenBalanceReportDs>();

            var result = _RerportsBusinessService.GetOpenBalanceReportView(x => x.FinancialPeriodId == Filters.FinancialPeriodId);
            if (result != null)
            {
                if (Filters.BranchId != null && Filters.BranchId > 0)
                {
                    result = result.Where(x => x.BranchId == Filters.BranchId).ToList();
                }

                if (Filters.StoreId != null && Filters.StoreId > 0)
                {
                    result = result.Where(x => x.StoreId == Filters.StoreId).ToList();
                }
                if (Filters.GroupId != null && Filters.GroupId > 0)
                {
                    result = result.Where(x => x.GroupId == Filters.GroupId).ToList();
                }

                if (Filters.ItemId != null && Filters.ItemId > 0)
                {
                    result = result.Where(x => x.ItemId == Filters.ItemId.Value).ToList();
                }

                var company = _CompanyService.GetById(Filters.CompanyId.Value);

                Filters.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
                Filters.CompanyLogo = GetCompanyImage(company);
                Filters.ReportName = _LocalizationService.GetLocalizedHtmlString("OpenBalanceReport").Value;

                var Items = _ItemService.GetAll();
                var itemGroups = _ItemGroupService.GetAll();
                var itemUnits = _UnitService.GetAll();
                var stores = _StoreService.GetAll();
                var branches = _BranchService.GetAll();

                DataSource = result.GroupBy(g => new { g.BranchId, g.StoreId, g.GroupId, g.ItemId, g.UnitId }).Select(x =>
                         {
                             OpenBalanceReportDs DataSourceItem = new OpenBalanceReportDs();
                             var item = Items.Where(a => a.Id == x.FirstOrDefault().ItemId)?.FirstOrDefault();
                             if (item != null)
                             {
                                 DataSourceItem.ItemCode = item.Code;
                                 DataSourceItem.ItemName = _currentLanguage == "ar" ? item.NameAr : item.NameEn;
                                 DataSourceItem.StoreName = _currentLanguage == "ar" ? stores.Where(s => s.Id == x.FirstOrDefault().StoreId)?.FirstOrDefault()?.NameAr : stores.Where(s => s.Id == x.FirstOrDefault().StoreId)?.FirstOrDefault()?.NameEn;
                                 DataSourceItem.UnitName = _currentLanguage == "ar" ? itemUnits.Where(u => u.Id == x.FirstOrDefault().UnitId)?.FirstOrDefault()?.NameAr : itemUnits.Where(u => u.Id == x.FirstOrDefault().UnitId)?.FirstOrDefault()?.NameEn;
                                 DataSourceItem.GroupName = _currentLanguage == "ar" ? itemGroups.Where(g => g.Id == item.GroupId)?.FirstOrDefault()?.NameAr : itemGroups.Where(g => g.Id == item.GroupId)?.FirstOrDefault()?.NameEn;
                                 DataSourceItem.Quntity = x.Sum(s => s.Quntity);

                                 DataSourceItem.ItemId = item.Id;
                                 DataSourceItem.GroupId = item.GroupId;
                             }

                             return DataSourceItem;

                         }).ToList();

            }

            return DataSource;

        }

        public List<ItemDataReportDs> ItemDataReport(ItemDataReportFilters Filters)
        {
            List<ItemDataReportDs> DataSource = new List<ItemDataReportDs>();

            var result = _RerportsBusinessService.GetItemDataReportView(x => x.FinancialPeriodId == Filters.FinancialPeriodId);
            if (result != null)
            {
                if (Filters.BranchId != null && Filters.BranchId > 0)
                {
                    result = result.Where(x => x.BranchId == Filters.BranchId).ToList();
                }

                if (Filters.StoreId != null && Filters.StoreId > 0)
                {
                    result = result.Where(x => x.StoreId == Filters.StoreId).ToList();
                }
                if (Filters.GroupId != null && Filters.GroupId > 0)
                {
                    result = result.Where(x => x.GroupId == Filters.GroupId).ToList();
                }

                if (Filters.ItemId != null && Filters.ItemId > 0)
                {
                    result = result.Where(x => x.ItemId == Filters.ItemId.Value).ToList();
                }

                var company = _CompanyService.GetById(Filters.CompanyId.Value);

                Filters.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
                Filters.CompanyLogo = GetCompanyImage(company);
                Filters.ReportName = _LocalizationService.GetLocalizedHtmlString("ItemDataReport").Value;

                var Items = _ItemService.GetAll();
                var itemGroups = _ItemGroupService.GetAll();
                var itemUnits = _UnitService.GetAll();
                var stores = _StoreService.GetAll();
                var branches = _BranchService.GetAll();

                DataSource = result.GroupBy(g => new { g.BranchId, g.StoreId, g.GroupId, g.ItemId, g.UnitId }).Select(x =>
                         {
                             ItemDataReportDs DataSourceItem = new ItemDataReportDs();
                             var item = Items.Where(a => a.Id == x.FirstOrDefault().ItemId)?.FirstOrDefault();
                             if (item != null)
                             {
                                 DataSourceItem.ItemCode = item.Code;
                                 DataSourceItem.ItemName = _currentLanguage == "ar" ? x.FirstOrDefault().ItemNameAr : x.FirstOrDefault().ItemNameEn;
                                 DataSourceItem.StoreName = _currentLanguage == "ar" ? x.FirstOrDefault().StoreNameAr : x.FirstOrDefault().StoreNameEn;
                                 DataSourceItem.UnitName = _currentLanguage == "ar" ? x.FirstOrDefault().UnitNameAr : x.FirstOrDefault().UnitNameEn;
                                 DataSourceItem.GroupName = _currentLanguage == "ar" ? x.FirstOrDefault().ItemGroupNameAr : x.FirstOrDefault().ItemGroupNameEn;
                                 DataSourceItem.BranchName = _currentLanguage == "ar" ? x.FirstOrDefault().BrancheNameAr : x.FirstOrDefault().BrancheNameEn;
                                 DataSourceItem.Quntity = x.Sum(s => s.SignedQuntity);

                             }

                             return DataSourceItem;

                         }).ToList();

            }

            return DataSource;

        }

        public List<ItemBalanceQuantityAndValueReportDs> ItemBalanceQuantityAndValueReport(ItemBalanceQuantityAndValueReportFilters Filters)
        {
            List<ItemBalanceQuantityAndValueReportDs> DataSource = new List<ItemBalanceQuantityAndValueReportDs>();

            var result = _RerportsBusinessService.GetItemBalanceQuantityAndValueReportView(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate <= Filters.ToDate);
            if (result != null)
            {
                if (Filters.BranchId != null && Filters.BranchId > 0)
                {
                    result = result.Where(x => x.BranchId == Filters.BranchId).ToList();
                }

                if (Filters.StoreId != null && Filters.StoreId > 0)
                {
                    result = result.Where(x => x.StoreId == Filters.StoreId).ToList();
                }
                if (Filters.GroupId != null && Filters.GroupId > 0)
                {
                    result = result.Where(x => x.GroupId == Filters.GroupId).ToList();
                }

                if (Filters.ItemId != null && Filters.ItemId > 0)
                {
                    result = result.Where(x => x.ItemId == Filters.ItemId.Value).ToList();
                }

                var company = _CompanyService.GetById(Filters.CompanyId.Value);

                Filters.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
                Filters.CompanyLogo = GetCompanyImage(company);
                Filters.ReportName = _LocalizationService.GetLocalizedHtmlString("ItemBalanceQuantityAndValueReport").Value;

                var Items = _ItemService.GetAll();

                DataSource = result.GroupBy(g => new { g.BranchId, g.StoreId, g.GroupId, g.ItemId, g.UnitId }).Select(x =>
                {
                    ItemBalanceQuantityAndValueReportDs DataSourceItem = new ItemBalanceQuantityAndValueReportDs();
                    var item = Items.Where(a => a.Id == x.FirstOrDefault().ItemId)?.FirstOrDefault();
                    if (item != null)
                    {
                        DataSourceItem.ItemCode = item.Code;
                        DataSourceItem.ItemName = _currentLanguage == "ar" ? x.FirstOrDefault().ItemNameAr : x.FirstOrDefault().ItemNameEn;
                        DataSourceItem.StoreName = _currentLanguage == "ar" ? x.FirstOrDefault().StoreNameAr : x.FirstOrDefault().StoreNameEn;
                        DataSourceItem.UnitName = _currentLanguage == "ar" ? x.FirstOrDefault().UnitNameAr : x.FirstOrDefault().UnitNameEn;
                        DataSourceItem.GroupName = _currentLanguage == "ar" ? x.FirstOrDefault().ItemGroupNameAr : x.FirstOrDefault().ItemGroupNameEn;
                        DataSourceItem.BranchName = _currentLanguage == "ar" ? x.FirstOrDefault().BrancheNameAr : x.FirstOrDefault().BrancheNameEn;
                        DataSourceItem.Quntity = x.Sum(s => s.SignedQuntity);
                        DataSourceItem.AvgPurchasePrice = x.Sum(s => s.PurchasePrice) / DataSourceItem.Quntity;
                        DataSourceItem.Coast = DataSourceItem.Quntity * DataSourceItem.AvgPurchasePrice;
                    }

                    return DataSourceItem;

                }).ToList();

            }

            return DataSource;

        }

        public List<ItemCartReportDs> ItemCartReport(ItemCartReportFilters Filters)
        {
            List<ItemCartReportDs> DataSource = new List<ItemCartReportDs>();


            var result = _RerportsBusinessService.GetItemCartReportView(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate.Date >= Filters.FromDate.Date && x.DocDate.Date <= Filters.ToDate.Date);
            if (result != null)
            {
                if (Filters.BranchId != null && Filters.BranchId > 0)
                {
                    result = result.Where(x => x.BranchId == Filters.BranchId).ToList();
                }

                if (Filters.StoreId != null && Filters.StoreId > 0)
                {
                    result = result.Where(x => x.StoreId == Filters.StoreId).ToList();
                }
                if (Filters.GroupId != null && Filters.GroupId > 0)
                {
                    result = result.Where(x => x.GroupId == Filters.GroupId).ToList();
                }

                if (Filters.ItemId != null && Filters.ItemId > 0)
                {
                    result = result.Where(x => x.ItemId == Filters.ItemId.Value).ToList();
                }

                var company = _CompanyService.GetById(Filters.CompanyId.Value);

                Filters.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
                Filters.CompanyLogo = GetCompanyImage(company);
                Filters.ReportName = _LocalizationService.GetLocalizedHtmlString("ItemCartReport").Value;

                var Items = _ItemService.GetAll();

                DataSource = result.OrderBy(d => d.DocDate).Select(x =>
                  {
                      ItemCartReportDs DataSourceItem = new ItemCartReportDs();
                      var item = Items.Where(a => a.Id == x.ItemId)?.FirstOrDefault();

                      var TotalCurruntQuntity = result.Where(t => t.ItemId == x.ItemId && t.DocDate >= Filters.FromDate.AddHours(-12) && t.DocDate <= x.DocDate && (t.DocTypeId == (int)DocumentTypes.PurchaseInvoice || t.DocTypeId == (int)DocumentTypes.OpenBalance)).Sum(s => s.Quntity);

                      var TotalCurrentPrice = result.Where(t => t.ItemId == x.ItemId && t.DocDate >= Filters.FromDate.AddHours(-12) && t.DocDate <= x.DocDate && (t.DocTypeId == (int)DocumentTypes.PurchaseInvoice || t.DocTypeId == (int)DocumentTypes.OpenBalance)).Sum(s => s.PurchasePrice);



                      decimal AvgPurchasePrice = TotalCurruntQuntity > 0 ? TotalCurrentPrice / TotalCurruntQuntity : 0;
                      if (item != null)
                      {
                          DataSourceItem.DocName = _currentLanguage == "ar" ? x.DocNameAr : x.DocNameEn;
                          DataSourceItem.UnitName = _currentLanguage == "ar" ? x.UnitNameAr : x.UnitNameEn;
                          DataSourceItem.DocNumber = x.DocNumber;
                          DataSourceItem.DocDate = x.DocDate;
                          DataSourceItem.Quntity = x.Quntity;
                          DataSourceItem.InQuntity = x.DocSign == 1 ? x.Quntity : 0;
                          DataSourceItem.OutQuntity = x.DocSign == -1 ? x.Quntity : 0;
                          DataSourceItem.PurchasePrice = x.PurchasePrice;
                          DataSourceItem.CurrentQuntity = result.Where(r => r.DocDate >= Filters.FromDate.AddHours(-12) && r.DocDate <= x.DocDate).Sum(s => s.SignedQuntity);
                          if (x.DocTypeId == (int)DocumentTypes.OpenBalance)
                          {
                              DataSourceItem.AvgPurchasePrice = x.PurchasePrice;
                          }
                          else
                          {
                              DataSourceItem.AvgPurchasePrice = AvgPurchasePrice;
                          }

                          DataSourceItem.DocValue = DataSourceItem.PurchasePrice > 0 ? DataSourceItem.Quntity * DataSourceItem.PurchasePrice : DataSourceItem.Quntity * DataSourceItem.AvgPurchasePrice;
                          DataSourceItem.BalanceValue = DataSourceItem.CurrentQuntity * DataSourceItem.AvgPurchasePrice;



                      }

                      return DataSourceItem;

                  }).ToList();

            }

            return DataSource;

        }

        public List<ItemPurshasAnaysisReportDs> ItemPurshasAnaysisReport(ItemPurshasAnaysisReportFilters Filters)
        {
            List<ItemPurshasAnaysisReportDs> DataSource = new List<ItemPurshasAnaysisReportDs>();


            var result = _RerportsBusinessService.GetItemPurshasAnaysisReportView(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate.Date >= Filters.FromDate.Date && x.DocDate.Date <= Filters.ToDate.Date);
            if (result != null)
            {
                if (Filters.SupplierId != null && Filters.SupplierId > 0)
                {
                    result = result.Where(x => x.SupplierId == Filters.SupplierId).ToList();
                }
                if (Filters.GroupId != null && Filters.GroupId > 0)
                {
                    result = result.Where(x => x.GroupId == Filters.GroupId).ToList();
                }

                if (Filters.ItemId != null && Filters.ItemId > 0)
                {
                    result = result.Where(x => x.ItemId == Filters.ItemId.Value).ToList();
                }

                var company = _CompanyService.GetById(Filters.CompanyId.Value);

                Filters.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
                Filters.CompanyLogo = GetCompanyImage(company);
                Filters.ReportName = _LocalizationService.GetLocalizedHtmlString("ItemPurshasAnaysisReport").Value;

                decimal PurshasPriceAvg = 0;
                decimal PurshasReturnPriceAvg = 0;


                DataSource = result.GroupBy(g => new { g.BranchId, g.ItemId, g.SupplierId }).Select(x =>
                {
                    ItemPurshasAnaysisReportDs DataSourceItem = new ItemPurshasAnaysisReportDs();
                    DataSourceItem.ItemName = _currentLanguage == "ar" ? x.FirstOrDefault().ItemNameAr : x.FirstOrDefault().ItemNameEn;
                    DataSourceItem.SupplierName = _currentLanguage == "ar" ? x.FirstOrDefault().SupplierNameAr : x.FirstOrDefault().SupplierNameEn;
                    DataSourceItem.BranchName = _currentLanguage == "ar" ? x.FirstOrDefault().BrancheNameAr : x.FirstOrDefault().BrancheNameEn;

                    DataSourceItem.PurshasQuntity = x.Where(d => d.DocTypeId == (int)DocumentTypes.PurchaseInvoice).Sum(q => q.Quntity);
                    DataSourceItem.PurshasReturnQuntity = x.Where(d => d.DocTypeId == (int)DocumentTypes.PurchaseReturn).Sum(q => q.Quntity);
                    DataSourceItem.PurshasNetQuntity = DataSourceItem.PurshasQuntity - DataSourceItem.PurshasReturnQuntity;

                    PurshasPriceAvg = x.Where(d => d.DocTypeId == (int)DocumentTypes.PurchaseInvoice).Count() > 0 ? x.Where(d => d.DocTypeId == (int)DocumentTypes.PurchaseInvoice).Average(p => p.PurchasePrice) : 0;
                    DataSourceItem.PurshasValue = PurshasPriceAvg > 0 ? x.Where(d => d.DocTypeId == (int)DocumentTypes.PurchaseInvoice).Sum(q => q.Quntity) * PurshasPriceAvg : 0;

                    PurshasReturnPriceAvg = x.Where(d => d.DocTypeId == (int)DocumentTypes.PurchaseReturn).Count() > 0 ? x.Where(d => d.DocTypeId == (int)DocumentTypes.PurchaseReturn).Average(p => p.PurchasePrice) : 0;
                    DataSourceItem.PurshasReturnValue = PurshasReturnPriceAvg > 0 ? x.Where(d => d.DocTypeId == (int)DocumentTypes.PurchaseReturn).Sum(q => q.Quntity) * PurshasReturnPriceAvg : 0;

                    DataSourceItem.PurshasNetValue = DataSourceItem.PurshasValue - DataSourceItem.PurshasReturnValue;

                    return DataSourceItem;

                }).ToList();

            }

            return DataSource;

        }

        public List<SupplierBalanceReportDs> SupplierBalanceReport(SupplierBalanceReportFilters Filters)
        {
            List<SupplierBalanceReportDs> DataSource = new List<SupplierBalanceReportDs>();
            var Supplier = _SupplerService.GetById(Filters.SupplierId.Value);
            var result = _InventoryMasterService.GetWithCondetion(x => x.SupplierId == Filters.SupplierId && x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate.Date >= Filters.FromDate.Date && x.DocDate.Date <= Filters.ToDate.Date && x.DocTypeId == (int)DocumentTypes.PurchaseInvoice || x.DocTypeId == (int)DocumentTypes.PurchaseReturn);

            var CashTransactions = _CashTransactionService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate.Date >= Filters.FromDate.Date && x.DocDate.Date <= Filters.ToDate.Date && (x.FirstSideTypeId == (int)EntrySides.Supplier && x.FirstSideId == Filters.SupplierId) || (x.SecondSideTypeId == (int)EntrySides.Supplier && x.SecondSideId == Filters.SupplierId) && (x.DocTypeId == (int)DocumentTypes.CashExchangeTransaction));

            var CheckTransactions = _CheckTransactionService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate.Date >= Filters.FromDate.Date && x.DocDate.Date <= Filters.ToDate.Date && (x.FirstSideTypeId == (int)EntrySides.Supplier && x.FirstSideId == Filters.SupplierId) || (x.SecondSideTypeId == (int)EntrySides.Supplier && x.SecondSideId == Filters.SupplierId) && (x.DocTypeId == (int)DocumentTypes.WriteCheckOutTransaction || x.DocTypeId == (int)DocumentTypes.CheckReturnTransaction));

            if (CashTransactions != null)
            {
                result.AddRange(CashTransactions.Select(x => new Transaction_InvMaster() { BranchId = x.BranchId, DocDate = x.DocDate, DocTypeId = x.DocTypeId, Code = x.Code, InvoiceNet = x.Amount, CurrencyFactor = x.CurrencyFactor, Notes = x.Notes }));
            }
            if (CheckTransactions != null)
            {
                result.AddRange(CheckTransactions.Select(x => new Transaction_InvMaster() { BranchId = x.BranchId, DocDate = x.DocDate, DocTypeId = x.DocTypeId, Code = x.Code, InvoiceNet = x.Amount, CurrencyFactor = x.CurrencyFactor, Notes = x.Notes }));
            }

            var branchs = _BranchService.GetAll();
            var documents = _DocumentService.GetAll();
            var company = _CompanyService.GetById(Filters.CompanyId.Value);

            Filters.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            Filters.CompanyLogo = GetCompanyImage(company);
            Filters.ReportName = _LocalizationService.GetLocalizedHtmlString("SupplierBalanceReport").Value;

            if (result != null && Supplier != null)
            {
                var SupplerOpenBalance = _SupplerOpenBalance.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId.Value && x.SupplerId == Filters.SupplierId.Value)?.FirstOrDefault();

                decimal openDebit = SupplerOpenBalance != null ? SupplerOpenBalance.OpeningBalanceDebit : 0;
                decimal openCredit = SupplerOpenBalance != null ? SupplerOpenBalance.OpeningBalanceCredit : 0;

                decimal balanceDebit = openDebit;
                decimal balanceCredit = openCredit;
                decimal balance = 0;
                DataSource = result.OrderBy(o=>o.DocDate).Select(x =>
                {
                    SupplierBalanceReportDs DataSourceItem = new SupplierBalanceReportDs();

                    if (Supplier != null)
                    {
                        var branch = branchs.Where(b => b.Id == x.BranchId).FirstOrDefault();
                        DataSourceItem.SupplierName = _currentLanguage == "ar" ? Supplier.NameAr : Supplier.NameEn;
                        DataSourceItem.BranchName = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
                        DataSourceItem.DocDate = x.DocDate;
                        DataSourceItem.DocNumber = x.Code;
                        DataSourceItem.DocName = _currentLanguage == "ar" ? documents.FirstOrDefault(b => b.DocTypeId == x.DocTypeId).NameAr : documents.FirstOrDefault(b => b.DocTypeId == x.DocTypeId).NameEn;
                        DataSourceItem.Notes = x.Notes;
                        if (x.DocTypeId == (int)DocumentTypes.PurchaseReturn || x.DocTypeId == (int)DocumentTypes.CashExchangeTransaction || x.DocTypeId == (int)DocumentTypes.WriteCheckOutTransaction)
                        {
                            DataSourceItem.AmountDebit = x.InvoiceNet * x.CurrencyFactor;
                            balanceDebit += x.InvoiceNet * x.CurrencyFactor; 
                        }
                        if (x.DocTypeId == (int)DocumentTypes.PurchaseInvoice || x.DocTypeId == (int)DocumentTypes.CashRecieveTransaction|| x.DocTypeId == (int)DocumentTypes.CheckReturnTransaction)
                        {
                            DataSourceItem.AmountCredit = x.InvoiceNet * x.CurrencyFactor;
                            balanceCredit += x.InvoiceNet * x.CurrencyFactor; 
                        }
                        balance = balanceDebit - balanceCredit;
                        if (balanceDebit > balanceCredit)
                        {
                            DataSourceItem.BalanceDebit = Math.Abs(balance);
                        }
                        else if (balanceDebit < balanceCredit)
                        {
                            DataSourceItem.BalanceCredit = Math.Abs(balance);

                        }
                    }
                    return DataSourceItem;

                }).ToList();

                DataSource.Insert(0, new SupplierBalanceReportDs() { DocName = _currentLanguage == "ar" ? "رصيد إفتتاحي" : "Open Balance", AmountDebit = openDebit, AmountCredit = openCredit, BalanceDebit = openDebit, BalanceCredit = openCredit });

            }

            return DataSource;

        }

        public List<SupplierBalanceTotalReportDs> SupplierBalanceTotalReport(SupplierBalanceTotalReportFilters Filters)
        {
            List<SupplierBalanceTotalReportDs> DataSource = new List<SupplierBalanceTotalReportDs>();
            var Suppliers = _SupplerService.GetAll();
            var SupplersOpenBalance = _SupplerOpenBalance.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId.Value);
            var result = _InventoryMasterService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate.Date >= Filters.FromDate.Date && x.DocDate.Date <= Filters.ToDate.Date && x.DocTypeId == (int)DocumentTypes.PurchaseInvoice || x.DocTypeId == (int)DocumentTypes.PurchaseReturn);

            var CashTransactions = _CashTransactionService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate.Date >= Filters.FromDate.Date && x.DocDate.Date <= Filters.ToDate.Date && (x.FirstSideTypeId == (int)EntrySides.Supplier || x.SecondSideTypeId == (int)EntrySides.Supplier) && (x.DocTypeId == (int)DocumentTypes.CashExchangeTransaction));

            var CheckTransactions = _CheckTransactionService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate.Date >= Filters.FromDate.Date && x.DocDate.Date <= Filters.ToDate.Date && (x.FirstSideTypeId == (int)EntrySides.Supplier || x.SecondSideTypeId == (int)EntrySides.Supplier) && (x.DocTypeId == (int)DocumentTypes.WriteCheckOutTransaction || x.DocTypeId == (int)DocumentTypes.CheckReturnTransaction));

            if (CashTransactions != null)
            {
                result.AddRange(CashTransactions.Select(x => new Transaction_InvMaster() {SupplierId= x.FirstSideTypeId == (int)EntrySides.Supplier ? x.FirstSideId:x.SecondSideId, BranchId = x.BranchId, DocDate = x.DocDate, DocTypeId = x.DocTypeId, Code = x.Code, InvoiceNet = x.Amount, CurrencyFactor = x.CurrencyFactor, Notes = x.Notes }));
            }
            if (CheckTransactions != null)
            {
                result.AddRange(CheckTransactions.Select(x => new Transaction_InvMaster() { SupplierId = x.FirstSideTypeId == (int)EntrySides.Supplier ? x.FirstSideId : x.SecondSideId, BranchId = x.BranchId, DocDate = x.DocDate, DocTypeId = x.DocTypeId, Code = x.Code, InvoiceNet = x.Amount, CurrencyFactor = x.CurrencyFactor, Notes = x.Notes }));
            }

            if (Filters.SupplierId != null && Filters.SupplierId > 0)
            {
                Suppliers = Suppliers.Where(b => b.Id == Filters.SupplierId).ToList();
            }

            if (Filters.BranchId != null && Filters.BranchId > 0)
            {
                result = result.Where(b => b.BranchId == Filters.BranchId).ToList();
            }

            var company = _CompanyService.GetById(Filters.CompanyId.Value);

            Filters.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            Filters.CompanyLogo = GetCompanyImage(company);
            Filters.ReportName = _LocalizationService.GetLocalizedHtmlString("SupplierBalanceTotalReport").Value;

            if (result != null && Suppliers != null)
            {
                DataSource = Suppliers.Select(x =>
                {
                    SupplierBalanceTotalReportDs DataSourceItem = new SupplierBalanceTotalReportDs();
                    var SupplerOpenBalance = SupplersOpenBalance.Where(c => c.SupplerId == x.Id)?.FirstOrDefault();

                    decimal balance = 0;
                    DataSourceItem.SupplierName = _currentLanguage == "ar" ? x.NameAr : x.NameEn;
                    DataSourceItem.SupplierCode = x.Code;
                    DataSourceItem.OpenBalanceDebit = SupplerOpenBalance != null ? SupplerOpenBalance.OpeningBalanceDebit : 0;
                    DataSourceItem.OpenBalanceCredit = SupplerOpenBalance != null ? SupplerOpenBalance.OpeningBalanceCredit : 0;
                    DataSourceItem.AmountDebit = result.Where(r => r.SupplierId == x.Id && (r.DocTypeId == (int)DocumentTypes.PurchaseReturn || r.DocTypeId == (int)DocumentTypes.CashExchangeTransaction || r.DocTypeId == (int)DocumentTypes.WriteCheckOutTransaction)).Sum(s => s.InvoiceNet * s.CurrencyFactor);
                    DataSourceItem.AmountCredit = result.Where(r => r.SupplierId == x.Id && (r.DocTypeId == (int)DocumentTypes.PurchaseInvoice || r.DocTypeId == (int)DocumentTypes.CashRecieveTransaction || r.DocTypeId == (int)DocumentTypes.CheckReturnTransaction)).Sum(s => s.InvoiceNet * s.CurrencyFactor);

                    balance = (DataSourceItem.AmountDebit + DataSourceItem.OpenBalanceDebit) - (DataSourceItem.AmountCredit + DataSourceItem.OpenBalanceCredit);
                    if (balance > 0)
                    {
                        DataSourceItem.BalanceDebit = Math.Abs(balance);
                    }
                    else if (balance < 0)
                    {
                        DataSourceItem.BalanceCredit = Math.Abs(balance);
                    }
                    else
                    {
                        DataSourceItem.BalanceDebit = 0;
                        DataSourceItem.BalanceCredit = 0;
                    }

                    return DataSourceItem;

                }).ToList();

            }

            return DataSource;

        }

        public List<ItemSalesAnaysisReportDs> ItemSalesAnaysisReport(ItemSalesAnaysisReportFilters Filters)
        {
            List<ItemSalesAnaysisReportDs> DataSource = new List<ItemSalesAnaysisReportDs>();


            var result = _RerportsBusinessService.GetItemSalesAnaysisReportView(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate.Date >= Filters.FromDate.Date && x.DocDate.Date <= Filters.ToDate.Date);
            if (result != null)
            {
                if (Filters.CustomerId != null && Filters.CustomerId > 0)
                {
                    result = result.Where(x => x.CustomerId == Filters.CustomerId).ToList();
                }
                if (Filters.GroupId != null && Filters.GroupId > 0)
                {
                    result = result.Where(x => x.GroupId == Filters.GroupId).ToList();
                }

                if (Filters.ItemId != null && Filters.ItemId > 0)
                {
                    result = result.Where(x => x.ItemId == Filters.ItemId.Value).ToList();
                }

                var company = _CompanyService.GetById(Filters.CompanyId.Value);

                Filters.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
                Filters.CompanyLogo = GetCompanyImage(company);
                Filters.ReportName = _LocalizationService.GetLocalizedHtmlString("ItemSalesAnaysisReport").Value;

                decimal SalesPriceAvg = 0;
                decimal SalesReturnPriceAvg = 0;


                DataSource = result.GroupBy(g => new { g.BranchId, g.ItemId, g.CustomerId }).Select(x =>
                {
                    ItemSalesAnaysisReportDs DataSourceItem = new ItemSalesAnaysisReportDs();
                    DataSourceItem.ItemName = _currentLanguage == "ar" ? x.FirstOrDefault().ItemNameAr : x.FirstOrDefault().ItemNameEn;
                    DataSourceItem.CustomerName = _currentLanguage == "ar" ? x.FirstOrDefault().CustomerNameAr : x.FirstOrDefault().CustomerNameEn;
                    DataSourceItem.BranchName = _currentLanguage == "ar" ? x.FirstOrDefault().BrancheNameAr : x.FirstOrDefault().BrancheNameEn;

                    DataSourceItem.SalesQuntity = x.Where(d => d.DocTypeId == (int)DocumentTypes.SalesInvoice).Sum(q => q.Quntity);
                    DataSourceItem.SalesReturnQuntity = x.Where(d => d.DocTypeId == (int)DocumentTypes.SalesReturn).Sum(q => q.Quntity);
                    DataSourceItem.SalesNetQuntity = DataSourceItem.SalesQuntity - DataSourceItem.SalesReturnQuntity;

                    SalesPriceAvg = x.Where(d => d.DocTypeId == (int)DocumentTypes.SalesInvoice).Count() > 0 ? x.Where(d => d.DocTypeId == (int)DocumentTypes.SalesInvoice).Average(p => p.SalesPrice) : 0;
                    DataSourceItem.SalesValue = SalesPriceAvg > 0 ? x.Where(d => d.DocTypeId == (int)DocumentTypes.SalesInvoice).Sum(q => q.Quntity) * SalesPriceAvg : 0;

                    SalesReturnPriceAvg = x.Where(d => d.DocTypeId == (int)DocumentTypes.SalesReturn).Count() > 0 ? x.Where(d => d.DocTypeId == (int)DocumentTypes.SalesReturn).Average(p => p.SalesPrice) : 0;
                    DataSourceItem.SalesReturnValue = SalesReturnPriceAvg > 0 ? x.Where(d => d.DocTypeId == (int)DocumentTypes.SalesReturn).Sum(q => q.Quntity) * SalesReturnPriceAvg : 0;

                    DataSourceItem.SalesNetValue = DataSourceItem.SalesValue - DataSourceItem.SalesReturnValue;

                    return DataSourceItem;

                }).ToList();

            }

            return DataSource;

        }

        public List<CustomerBalanceReportDs> CustomerBalanceReport(CustomerBalanceReportFilters Filters)
        {
            List<CustomerBalanceReportDs> DataSource = new List<CustomerBalanceReportDs>();

            var Customer = _CustomerService.GetById(Filters.CustomerId.Value);
            var result = _InventoryMasterService.GetWithCondetion(x => x.CustomerId == Filters.CustomerId && x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate.Date >= Filters.FromDate.Date && x.DocDate.Date <= Filters.ToDate.Date && x.DocTypeId == (int)DocumentTypes.SalesInvoice || x.DocTypeId == (int)DocumentTypes.SalesReturn);

            var CashTransactions = _CashTransactionService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate.Date >= Filters.FromDate.Date && x.DocDate.Date <= Filters.ToDate.Date && (x.FirstSideTypeId == (int)EntrySides.Customer && x.FirstSideId == Filters.CustomerId ) || (x.SecondSideTypeId == (int)EntrySides.Customer && x.SecondSideId == Filters.CustomerId) && (x.DocTypeId == (int)DocumentTypes.CashRecieveTransaction));

            var CheckTransactions = _CheckTransactionService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate.Date >= Filters.FromDate.Date && x.DocDate.Date <= Filters.ToDate.Date && (x.FirstSideTypeId == (int)EntrySides.Customer && x.FirstSideId == Filters.CustomerId) || (x.SecondSideTypeId == (int)EntrySides.Customer && x.SecondSideId == Filters.CustomerId) && (x.DocTypeId == (int)DocumentTypes.RecieveCheckInTransaction));

            if (CashTransactions!=null)
            {
                result.AddRange(CashTransactions.Select(x=>new Transaction_InvMaster() {BranchId=x.BranchId,DocDate=x.DocDate,DocTypeId=x.DocTypeId,Code=x.Code,InvoiceNet=x.Amount, CurrencyFactor=x.CurrencyFactor,Notes=x.Notes }));
            }
            if (CheckTransactions != null)
            {
                result.AddRange(CheckTransactions.Select(x => new Transaction_InvMaster() { BranchId = x.BranchId, DocDate = x.DocDate, DocTypeId = x.DocTypeId, Code = x.Code, InvoiceNet = x.Amount, CurrencyFactor = x.CurrencyFactor, Notes = x.Notes }));
            }


            var branchs = _BranchService.GetAll();
            var documents = _DocumentService.GetAll();
            var company = _CompanyService.GetById(Filters.CompanyId.Value);

            Filters.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            Filters.CompanyLogo = GetCompanyImage(company);
            Filters.ReportName = _LocalizationService.GetLocalizedHtmlString("CustomerBalanceReport").Value;

            if (result != null && Customer != null)
            {
                var CustomerOpenBalance = _CustomerOpenBalance.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId.Value && x.CustomerId == Filters.CustomerId.Value)?.FirstOrDefault();

                decimal OpenBalanceDebit = CustomerOpenBalance != null ? CustomerOpenBalance.OpeningBalanceDebit : 0;
                decimal OpenBalanceCredit = CustomerOpenBalance != null ? CustomerOpenBalance.OpeningBalanceCredit : 0;

                decimal balanceDebit = OpenBalanceDebit;
                decimal balanceCredit = OpenBalanceCredit;
                decimal balance = 0;
                DataSource = result.OrderBy(o=>o.DocDate).Select(x =>
                {
                    CustomerBalanceReportDs DataSourceItem = new CustomerBalanceReportDs();

                    if (Customer != null)
                    {

                        var branch = branchs.Where(b => b.Id == x.BranchId).FirstOrDefault();
                        DataSourceItem.CustomerName = _currentLanguage == "ar" ? Customer.NameAr : Customer.NameEn;
                        DataSourceItem.BranchName = _currentLanguage == "ar" ? branch.NameAr : branch.NameEn;
                        DataSourceItem.DocDate = x.DocDate;
                        DataSourceItem.DocNumber = x.Code;
                        DataSourceItem.DocName = _currentLanguage == "ar" ? documents.FirstOrDefault(b => b.DocTypeId == x.DocTypeId).NameAr : documents.FirstOrDefault(b => b.DocTypeId == x.DocTypeId).NameEn;
                        DataSourceItem.Notes = x.Notes;
                        if (x.DocTypeId == (int)DocumentTypes.SalesReturn || x.DocTypeId == (int)DocumentTypes.CashRecieveTransaction || x.DocTypeId == (int)DocumentTypes.RecieveCheckInTransaction)
                        {
                            DataSourceItem.AmountCredit = x.InvoiceNet * x.CurrencyFactor;
                            balanceCredit += x.InvoiceNet * x.CurrencyFactor;
                        }
                        if (x.DocTypeId == (int)DocumentTypes.SalesInvoice)
                        {
                            DataSourceItem.AmountDebit = x.InvoiceNet * x.CurrencyFactor;
                            balanceDebit += x.InvoiceNet * x.CurrencyFactor;
                        }
                        balance = balanceDebit - balanceCredit;
                        if (balanceDebit > balanceCredit)
                        {
                            DataSourceItem.BalanceDebit = Math.Abs(balance);
                        }
                        else if (balanceDebit < balanceCredit)
                        {
                            DataSourceItem.BalanceCredit = Math.Abs(balance);

                        }
                    }
                    return DataSourceItem;

                }).ToList();

                DataSource.Insert(0, new CustomerBalanceReportDs() { DocName = _currentLanguage == "ar" ? "رصيد إفتتاحي" : "Open Balance", AmountDebit = OpenBalanceDebit, AmountCredit = OpenBalanceCredit, BalanceDebit = OpenBalanceDebit, BalanceCredit = OpenBalanceCredit });

            }

            return DataSource;

        }
        public List<CustomerBalanceTotalReportDs> CustomerBalanceTotalReport(CustomerBalanceTotalReportFilters Filters)
        {
            List<CustomerBalanceTotalReportDs> DataSource = new List<CustomerBalanceTotalReportDs>();
            var Customers = _CustomerService.GetAll();
            var CustomersOpenBalance = _CustomerOpenBalance.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId.Value);

            var result = _InventoryMasterService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate.Date >= Filters.FromDate.Date && x.DocDate.Date <= Filters.ToDate.Date && x.DocTypeId == (int)DocumentTypes.SalesInvoice || x.DocTypeId == (int)DocumentTypes.SalesReturn);

            var CashTransactions = _CashTransactionService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate.Date >= Filters.FromDate.Date && x.DocDate.Date <= Filters.ToDate.Date && (x.FirstSideTypeId == (int)EntrySides.Customer || x.SecondSideTypeId == (int)EntrySides.Customer)  && (x.DocTypeId == (int)DocumentTypes.CashRecieveTransaction));

            var CheckTransactions = _CheckTransactionService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.DocDate.Date >= Filters.FromDate.Date && x.DocDate.Date <= Filters.ToDate.Date && (x.FirstSideTypeId == (int)EntrySides.Customer || x.SecondSideTypeId == (int)EntrySides.Customer) && (x.DocTypeId == (int)DocumentTypes.RecieveCheckInTransaction));

            if (CashTransactions != null)
            {
                result.AddRange(CashTransactions.Select(x => new Transaction_InvMaster() {CustomerId=x.FirstSideTypeId== (int)EntrySides.Customer? x.FirstSideId:x.SecondSideId, BranchId = x.BranchId, DocDate = x.DocDate, DocTypeId = x.DocTypeId, Code = x.Code, InvoiceNet = x.Amount, CurrencyFactor = x.CurrencyFactor, Notes = x.Notes }));
            }
            if (CheckTransactions != null)
            {
                result.AddRange(CheckTransactions.Select(x => new Transaction_InvMaster() { CustomerId = x.FirstSideTypeId == (int)EntrySides.Customer? x.FirstSideId : x.SecondSideId, BranchId = x.BranchId, DocDate = x.DocDate, DocTypeId = x.DocTypeId, Code = x.Code, InvoiceNet = x.Amount, CurrencyFactor = x.CurrencyFactor, Notes = x.Notes }));
            }

            if (Filters.CustomerId != null && Filters.CustomerId > 0)
            {
                Customers = Customers.Where(b => b.Id == Filters.CustomerId).ToList();
            }

            if (Filters.BranchId != null && Filters.BranchId > 0)
            {
                result = result.Where(b => b.BranchId == Filters.BranchId).ToList();
            }

            var company = _CompanyService.GetById(Filters.CompanyId.Value);

            Filters.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            Filters.CompanyLogo = GetCompanyImage(company);
            Filters.ReportName = _LocalizationService.GetLocalizedHtmlString("CustomerBalanceTotalReport").Value;

            if (result != null && Customers != null)
            {
                DataSource = Customers.Select(x =>
                {
                    CustomerBalanceTotalReportDs DataSourceItem = new CustomerBalanceTotalReportDs();

                    var CustomerOpenBalance = CustomersOpenBalance.Where(c => c.CustomerId == x.Id)?.FirstOrDefault();

                    decimal balance = 0;
                    DataSourceItem.CustomerName = _currentLanguage == "ar" ? x.NameAr : x.NameEn;
                    DataSourceItem.CustomerCode = x.Code;
                    DataSourceItem.OpenBalanceDebit = CustomerOpenBalance != null ? CustomerOpenBalance.OpeningBalanceDebit : 0;
                    DataSourceItem.OpenBalanceCredit = CustomerOpenBalance != null ? CustomerOpenBalance.OpeningBalanceCredit : 0;
                    DataSourceItem.AmountDebit = result.Where(r => r.CustomerId == x.Id && r.DocTypeId == (int)DocumentTypes.SalesInvoice).Sum(s => s.InvoiceNet*s.CurrencyFactor);
                    DataSourceItem.AmountCredit = result.Where(r => r.CustomerId == x.Id && (r.DocTypeId == (int)DocumentTypes.SalesReturn || r.DocTypeId == (int)DocumentTypes.CashRecieveTransaction || r.DocTypeId == (int)DocumentTypes.RecieveCheckInTransaction)).Sum(s => s.InvoiceNet * s.CurrencyFactor);

                    balance = (DataSourceItem.AmountDebit + DataSourceItem.OpenBalanceDebit) - (DataSourceItem.AmountCredit + DataSourceItem.OpenBalanceCredit);
                    if (balance > 0)
                    {
                        DataSourceItem.BalanceDebit = Math.Abs(balance);
                    }
                    else if (balance < 0)
                    {
                        DataSourceItem.BalanceCredit = Math.Abs(balance);
                    }
                    else
                    {
                        DataSourceItem.BalanceDebit = 0;
                        DataSourceItem.BalanceCredit = 0;
                    }

                    return DataSourceItem;

                }).ToList();

            }

            return DataSource;

        }

        public List<AlAstazAccountReportDs> AlAstazAccountReport(AlAstazAccountReportFilters Filters)
        {
            List<AlAstazAccountReportDs> DataSource = new List<AlAstazAccountReportDs>();

            var result = _RerportsBusinessService.GetAlAstazAccountReportView(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.TransactionDate >= Filters.FromDate && x.CompanyId == Filters.CompanyId && x.TransactionDate <= Filters.ToDate).OrderBy(x => x.EntryNumber).ToList();
            var openBalance = _AccountOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId).FirstOrDefault();

            if (result != null)
            {
                if (Filters.AccountId > 0)
                {
                    result = result.Where(x => x.AccountId == Filters.AccountId).ToList();
                }
                if (Filters.DocumentTypeId > 0)
                {
                    result = result.Where(x => x.DocType == Filters.DocumentTypeId).ToList();
                }
                if (Filters.DailyAccountsId > 0)
                {
                    result = result.Where(x => x.DailyTypeId == Filters.DailyAccountsId).ToList();
                }

                if (Filters.EntryNumber > 0)
                {
                    result = result.Where(x => x.EntryNumber == Filters.EntryNumber).ToList();
                }
                if (openBalance != null)
                {
                    result.Insert(0, new AlAstazAccountReportView() { EntryNumber = 0, Debit = openBalance.OpenBalanceDebit, Credit = openBalance.OpenBalanceCredit, DocType = 1, BalanceDebit = openBalance.OpenBalanceDebit, BalanceCredit = openBalance.OpenBalanceCredit });
                }
                var company = _CompanyService.GetById(Filters.CompanyId.Value);

                Filters.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
                Filters.CompanyLogo = GetCompanyImage(company);
                Filters.ReportName = _LocalizationService.GetLocalizedHtmlString("AlAstazAccountReport").Value;

                var DailyAccounts = _DailyAccounts_DefService.GetAll();
                var Documents = _DocumentService.GetAll();
                string DocumentName = "";
                string DailyTypeName = "";
                int i = 0;
                decimal curruntBalance = 0;

                DataSource = result.Select(x =>
                {
                    curruntBalance = 0;
                    var document = Documents.Where(d => d.DocTypeId == x.DocType).FirstOrDefault();
                    var DailyType = DailyAccounts.Where(d => d.Id == x.DailyTypeId).FirstOrDefault();
                    AlAstazAccountReportDs DataSourceItem = new AlAstazAccountReportDs();
                    DataSourceItem.EntryNumber = x.EntryNumber;
                    DataSourceItem.DocNumber = x.DocNumber;
                    DataSourceItem.TransactionDate = x.TransactionDate;
                    DataSourceItem.Debit = x.Debit;
                    DataSourceItem.Credit = x.Credit;
                    if (document != null)
                    {
                        DocumentName = _currentLanguage == "ar" ? document.NameAr : document.NameEn;
                    }
                    if (DailyType != null)
                    {
                        DailyTypeName = _currentLanguage == "ar" ? DailyType.NameAr : DailyType.NameEn;
                    }
                    DataSourceItem.DocumentName = DocumentName;
                    DataSourceItem.DailyTypeName = DailyTypeName;

                    if (i == 0)
                    {
                        curruntBalance = x.Debit - x.Credit;
                    }
                    else
                    {
                        curruntBalance = (result[i - 1].BalanceDebit + x.Debit) - (result[i - 1].BalanceCredit + x.Credit);
                    }

                    if (curruntBalance > 0)
                    {
                        DataSourceItem.BalanceDebit = Math.Abs(curruntBalance);
                        DataSourceItem.BalanceCredit = 0;

                        x.BalanceDebit = Math.Abs(curruntBalance);
                        x.BalanceCredit = 0;

                    }
                    else if (curruntBalance < 0)
                    {
                        DataSourceItem.BalanceDebit = 0;
                        DataSourceItem.BalanceCredit = Math.Abs(curruntBalance);

                        x.BalanceDebit = 0;
                        x.BalanceCredit = Math.Abs(curruntBalance);
                    }
                    else
                    {
                        DataSourceItem.BalanceDebit = 0;
                        DataSourceItem.BalanceCredit = 0;
                    }

                    i++;
                    return DataSourceItem;
                }).ToList();
            }
            return DataSource;
        }

        public List<ReviewBalanceReportDs> ReviewBalanceReport(ReviewBalanceReportFilters Filters)
        {
            List<ReviewBalanceReportDs> DataSource = new List<ReviewBalanceReportDs>();

            var allAccounts = _AccountsService.GetAll();
            if (Filters.AccountId>0)
            {
                allAccounts = GetChild(Filters.AccountId, allAccounts).ToList();
            }
            var AccountsIds = allAccounts.Select(x => x.Id).ToList();

            var master = _DailyEntryMasterService.GetWithCondetion(x=>x.FinancialPeriodId== Filters.FinancialPeriodId && x.CompanyId==Filters.CompanyId && x.TransactionDate.Date>=Filters.FromDate.Date && x.TransactionDate.Date <= Filters.ToDate.Date);

            var details = _DailyEntryDetailsService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId && AccountsIds.Contains(x.AccountId));

            var transactions= (from m in master join d in details on m.Id equals d.MasterId                               
                               select new { Date = m.TransactionDate.Date, AccountId = d.AccountId, Debit = d.Debit, Credit = d.Credit, FinancialPeriodId = m.FinancialPeriodId }
                  ).ToList();

            var openBalance = _AccountOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId).ToList();

            DataSource = allAccounts.Select(x =>
            {
                ReviewBalanceReportDs lst = new ReviewBalanceReportDs();

                lst.MainAccountName = x.Level == 1 ? x.Name : "";
                lst.AccountId = x.Id;
                lst.AccountCode = x.AccountCode;
                lst.AccountName = x.Name;

                var accountOpenBalance = openBalance.Where(a => a.AccountId == x.Id).FirstOrDefault();

                lst.OpenBalanceDebit = accountOpenBalance != null ? accountOpenBalance.OpenBalanceDebit : 0;
                lst.OpenBalanceCredit = accountOpenBalance != null ? accountOpenBalance.OpenBalanceCredit : 0;

           
                var BalanceInDebit = transactions.Where(d => d.Date.Date >= Filters.FromDate.Date && d.AccountId == x.Id).Sum(s => s.Debit);
                lst.BalanceInDebit = BalanceInDebit;

                var BalanceInCredit = transactions.Where(d => d.Date.Date >= Filters.FromDate.Date && d.AccountId == x.Id).Sum(s => s.Credit);
                lst.BalanceInCredit = BalanceInCredit;

                var BalanceBeforDebit = transactions.Where(d => d.Date.Date < Filters.FromDate.Date && d.AccountId == x.Id).Sum(s => s.Debit);
                lst.BalanceBeforDebit = BalanceBeforDebit;

                var BalanceBeforCredit = transactions.Where(d => d.Date.Date < Filters.FromDate.Date && d.AccountId == x.Id).Sum(s => s.Credit);

                lst.BalanceBeforCredit = BalanceBeforCredit;

                lst.BalanceTotalDebit = lst.BalanceBeforDebit + lst.BalanceInDebit + lst.OpenBalanceDebit;
                lst.BalanceTotalCredit = lst.BalanceBeforCredit + lst.BalanceInCredit + lst.OpenBalanceCredit;

                lst.ParentId = x.ParentId.HasValue ? x.ParentId.Value : -1;
                lst.IsParent = allAccounts.Any(t => t.ParentId == x.Id);
                lst.Level = x.Level ;
                lst.IsLastLevel = x.LastLevelInTree ;

                return lst;

            }).ToList();



            DataSource = DataSource.OrderByDescending(x => x.AccountId).Select(x =>
            {
                var lst = x;
                if (x.IsParent)
                {
                    lst.BalanceBeforDebit = Math.Round(DataSource.Where(t => t.ParentId == x.AccountId).Sum(t => t.BalanceBeforDebit), 2);
                    lst.BalanceBeforCredit = Math.Round(DataSource.Where(t => t.ParentId == x.AccountId).Sum(t => t.BalanceBeforCredit), 2);
                    lst.BalanceInDebit = Math.Round(DataSource.Where(t => t.ParentId == x.AccountId).Sum(t => t.BalanceInDebit), 2);
                    lst.BalanceInCredit = Math.Round(DataSource.Where(t => t.ParentId == x.AccountId).Sum(t => t.BalanceInCredit), 2);
                    lst.BalanceTotalDebit = Math.Round(DataSource.Where(t => t.ParentId == x.AccountId).Sum(t => t.BalanceTotalDebit), 2);
                    lst.BalanceTotalCredit = Math.Round(DataSource.Where(t => t.ParentId == x.AccountId).Sum(t => t.BalanceTotalCredit), 2);
                    lst.OpenBalanceDebit = Math.Round(DataSource.Where(t => t.ParentId == x.AccountId).Sum(t => t.OpenBalanceDebit), 2);
                    lst.OpenBalanceCredit = Math.Round(DataSource.Where(t => t.ParentId == x.AccountId).Sum(t => t.OpenBalanceCredit), 2);

                    if (lst.BalanceTotalDebit > lst.BalanceTotalCredit)
                    {
                        lst.TotalDebit = Math.Round(lst.BalanceTotalDebit - lst.BalanceTotalCredit, 2);
                        lst.TotalCredit = 0;
                    }
                    if (lst.BalanceTotalDebit < lst.BalanceTotalCredit)
                    {
                        lst.TotalDebit = 0;
                        lst.TotalCredit = Math.Round(lst.BalanceTotalCredit - lst.BalanceTotalDebit, 2);
                    }
                    if (lst.BalanceTotalDebit == lst.BalanceTotalCredit)
                    {
                        lst.TotalDebit = 0;
                        lst.TotalCredit = 0;
                    }

                }
                return lst;

            }).OrderBy(x => x.AccountId).ToList();


            if (Filters.HideZeroBalance)
            {
                DataSource = DataSource.Where(x=>x.TotalDebit>0 || x.TotalCredit>0).ToList();
            }

            var company = _CompanyService.GetById(Filters.CompanyId.Value);

            Filters.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            Filters.CompanyLogo = GetCompanyImage(company);
            Filters.ReportName = _LocalizationService.GetLocalizedHtmlString("ReviewBalanceReport").Value;   

            return DataSource;


        }

       
        public List<DailyAccountsDetailsReportDs> DailyAccountsDetailsReport(DailyAccountsDetailsReportFilters Filters)
        {
            List<DailyAccountsDetailsReportDs> DataSource = new List<DailyAccountsDetailsReportDs>();

            var allAccounts = _AccountsService.GetWithCondetion(x=>x.LastLevelInTree).ToList();

            var Documents = _DocumentService.GetAll().ToList();

            var master = _DailyEntryMasterService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId && x.TransactionDate.Date >= Filters.FromDate.Date && x.TransactionDate.Date <= Filters.ToDate.Date);

            var details = _DailyEntryDetailsService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId);

            if (Filters.AccountId>0)
            {
                details = details.Where(x => x.AccountId== Filters.AccountId).ToList();
            }
            if (Filters.DocumentTypeId > 0)
            {
                master = master.Where(x => x.DocType == Filters.DocumentTypeId).ToList();
            }
            if (Filters.DailyAccountsId > 0)
            {
                master = master.Where(x => x.DailyTypeId == Filters.DailyAccountsId).ToList();
            }           

            DataSource = (from m in master
                                join d in details on m.Id equals d.MasterId
                                join a in allAccounts on d.AccountId equals a.Id
                                join doc in Documents on m.DocType equals doc.DocTypeId
                                select new DailyAccountsDetailsReportDs()
                                {
                                    EntryNumber=m.EntryNumber,
                                    DocumentNumber=m.DocNumber,
                                    DocumentName= _currentLanguage == "ar" ? doc.NameAr:doc.NameEn,
                                    AccountName=a.Name,
                                    CoastCenterName="",
                                    Debit=d.Debit,
                                    Credit=d.Credit,
                                    Notes=m.Notes
                                }
                  ).ToList();

            return DataSource;
        }

        public List<DailyAccountsTotalReportDs> DailyAccountsTotalReport(DailyAccountsTotalReportFilters Filters)
        {
            List<DailyAccountsTotalReportDs> DataSource = new List<DailyAccountsTotalReportDs>();

            var openBalance = _AccountOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId).ToList();

            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree).ToList();

            var master = _DailyEntryMasterService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId && x.TransactionDate.Date >= Filters.FromDate.Date && x.TransactionDate.Date <= Filters.ToDate.Date);

            var details = _DailyEntryDetailsService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId);

            if (Filters.AccountId > 0)
            {
                details = details.Where(x => x.AccountId == Filters.AccountId).ToList();
            }
            if (Filters.DailyAccountsId > 0)
            {
                master = master.Where(x => x.DailyTypeId == Filters.DailyAccountsId).ToList();
            }

            DataSource = (from m in master
                          join d in details on m.Id equals d.MasterId
                          join a in allAccounts on d.AccountId equals a.Id                        
                          select new DailyAccountsTotalReportDs()
                          {
                              AccountId = d.AccountId,    
                              AccountName = a.Name,                            
                              Debit = d.Debit,
                              Credit = d.Credit,
                              Notes = m.Notes
                          }
                  ).ToList();

            DataSource = DataSource.GroupBy(g=>g.AccountId).Select(x => 
            {
                var d = x.FirstOrDefault();
                var accountOpenBalance = openBalance.Where(a => a.AccountId == d.AccountId).FirstOrDefault();
                d.OpenBalanceDebit = accountOpenBalance != null ? accountOpenBalance.OpenBalanceDebit : 0;
                d.OpenBalanceCredit = accountOpenBalance != null ? accountOpenBalance.OpenBalanceCredit : 0;
                d.Debit = x.Sum(s=>s.Debit);
                d.Credit = x.Sum(s=>s.Credit);
                d.BalanceDebit = d.OpenBalanceDebit + d.Debit;
                d.BalanceCredit = d.OpenBalanceCredit + d.Credit;
                return d;
            }).ToList();

            return DataSource;
        }
        public List<BankStatementOfAccountReportDs> BankStatementOfAccountReport(BankStatementOfAccountReportFilters Filters)
        {
            List<BankStatementOfAccountReportDs> DataSource = new List<BankStatementOfAccountReportDs>();

            var bank = _BankService.GetWithCondetion(x=>x.Id==Filters.BanckId).Select(b=>new { BanckId=b.Id, AccountId=b.AccountId }).FirstOrDefault();
            var bankOpenBalance= _BankOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId && x.BankId==Filters.BanckId).FirstOrDefault();

            var master = _DailyEntryMasterService.GetWithCondetion(x =>x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId && x.TransactionDate.Date >= Filters.FromDate.Date && x.TransactionDate.Date <= Filters.ToDate.Date);

            if (Filters.DocTypeId > 0)
            {
                master = master.Where(x => x.DocType == Filters.DocTypeId).ToList();
            }

            var details = _DailyEntryDetailsService.GetWithCondetion(x =>x.AccountId== bank.AccountId && x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId);           
            var Documents = _DocumentService.GetAll();

            DataSource = (from m in master
                          join d in details on m.Id equals d.MasterId
                          join doc in Documents on m.DocType equals doc.DocTypeId
                          select new BankStatementOfAccountReportDs()
                          {
                              EntryNumber=m.EntryNumber,
                              DocumentName= _currentLanguage == "ar" ? doc.NameAr : doc.NameEn,
                              DocumentNumber=m.DocNumber,
                              TransactionDate=m.TransactionDate,                               
                              Debit = d.Debit,
                              Credit = d.Credit,
                              Notes = m.Notes
                          }
                  ).OrderBy(o=>o.EntryNumber).ToList();
            DataSource.Insert(0, new BankStatementOfAccountReportDs() {EntryNumber=0, DocumentName = _currentLanguage == "ar" ? "الرصيد الافتتاحي" : "Open Balance",Debit= bankOpenBalance != null ? bankOpenBalance.OpenBalanceDebit : 0,Credit= bankOpenBalance != null ? bankOpenBalance.OpenBalanceCredit : 0, BalanceDebit = bankOpenBalance != null ? bankOpenBalance.OpenBalanceDebit : 0 ,BalanceCredit= bankOpenBalance != null ? bankOpenBalance.OpenBalanceCredit : 0 });

            int i = 0;
            decimal curruntBalance = 0;

            DataSource = DataSource.Select(x =>
            {
                curruntBalance = 0;                
                BankStatementOfAccountReportDs DataSourceItem = new BankStatementOfAccountReportDs();
                DataSourceItem.EntryNumber = x.EntryNumber;
                DataSourceItem.DocumentName = x.DocumentName;
                DataSourceItem.DocumentNumber = x.DocumentNumber;
                DataSourceItem.TransactionDate = x.TransactionDate;
                DataSourceItem.Debit = x.Debit;
                DataSourceItem.Credit = x.Credit;               
                DataSourceItem.Notes = x.Notes;               
              
                if (i == 0)
                {
                    curruntBalance = x.Debit - x.Credit;
                }
                else
                {
                    curruntBalance = (DataSource[i - 1].BalanceDebit + x.Debit) - (DataSource[i - 1].BalanceCredit + x.Credit);
                }

                if (curruntBalance > 0)
                {
                    DataSourceItem.BalanceDebit = Math.Abs(curruntBalance);
                    DataSourceItem.BalanceCredit = 0;

                    x.BalanceDebit = Math.Abs(curruntBalance);
                    x.BalanceCredit = 0;

                }
                else if (curruntBalance < 0)
                {
                    DataSourceItem.BalanceDebit = 0;
                    DataSourceItem.BalanceCredit = Math.Abs(curruntBalance);

                    x.BalanceDebit = 0;
                    x.BalanceCredit = Math.Abs(curruntBalance);
                }
                else
                {
                    DataSourceItem.BalanceDebit = 0;
                    DataSourceItem.BalanceCredit = 0;
                }

                i++;
                return DataSourceItem;
            }).ToList();
            return DataSource;
        }
       
         public List<TreasuryStatementOfAccountReportDs> TreasuryStatementOfAccountReport(TreasuryStatementOfAccountReportFilters Filters)
        {
            List<TreasuryStatementOfAccountReportDs> DataSource = new List<TreasuryStatementOfAccountReportDs>();

            var Treasury = _TreasuryService.GetWithCondetion(x=>x.Id==Filters.TreasuryId).Select(b=>new { TreasuryId = b.Id, AccountId=b.AccountId }).FirstOrDefault();
            var TreasuryOpenBalance = _TreasuryOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId && x.TreasuryId==Filters.TreasuryId).FirstOrDefault();

            var master = _DailyEntryMasterService.GetWithCondetion(x =>x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId && x.TransactionDate.Date >= Filters.FromDate.Date && x.TransactionDate.Date <= Filters.ToDate.Date);

            if (Filters.DocTypeId > 0)
            {
                master = master.Where(x => x.DocType == Filters.DocTypeId).ToList();
            }

            var details = _DailyEntryDetailsService.GetWithCondetion(x =>x.AccountId== Treasury.AccountId && x.FinancialPeriodId == Filters.FinancialPeriodId && x.CompanyId == Filters.CompanyId);           
            var Documents = _DocumentService.GetAll();

            DataSource = (from m in master
                          join d in details on m.Id equals d.MasterId
                          join doc in Documents on m.DocType equals doc.DocTypeId
                          select new TreasuryStatementOfAccountReportDs()
                          {
                              EntryNumber=m.EntryNumber,
                              DocumentName= _currentLanguage == "ar" ? doc.NameAr : doc.NameEn,
                              DocumentNumber=m.DocNumber,
                              TransactionDate=m.TransactionDate,                               
                              Debit = d.Debit,
                              Credit = d.Credit,
                              Notes = m.Notes
                          }
                  ).OrderBy(o=>o.EntryNumber).ToList();
            DataSource.Insert(0, new TreasuryStatementOfAccountReportDs() {EntryNumber=0, DocumentName = _currentLanguage == "ar" ? "الرصيد الافتتاحي" : "Open Balance",Debit= TreasuryOpenBalance != null ? TreasuryOpenBalance.OpenBalanceDebit : 0,Credit= TreasuryOpenBalance != null ? TreasuryOpenBalance.OpenBalanceCredit : 0, BalanceDebit = TreasuryOpenBalance != null ? TreasuryOpenBalance.OpenBalanceDebit : 0 ,BalanceCredit= TreasuryOpenBalance != null ? TreasuryOpenBalance.OpenBalanceCredit : 0 });

            int i = 0;
            decimal curruntBalance = 0;

            DataSource = DataSource.Select(x =>
            {
                curruntBalance = 0;
                TreasuryStatementOfAccountReportDs DataSourceItem = new TreasuryStatementOfAccountReportDs();
                DataSourceItem.EntryNumber = x.EntryNumber;
                DataSourceItem.DocumentName = x.DocumentName;
                DataSourceItem.DocumentNumber = x.DocumentNumber;
                DataSourceItem.TransactionDate = x.TransactionDate;
                DataSourceItem.Debit = x.Debit;
                DataSourceItem.Credit = x.Credit;               
                DataSourceItem.Notes = x.Notes;               
              
                if (i == 0)
                {
                    curruntBalance = x.Debit - x.Credit;
                }
                else
                {
                    curruntBalance = (DataSource[i - 1].BalanceDebit + x.Debit) - (DataSource[i - 1].BalanceCredit + x.Credit);
                }

                if (curruntBalance > 0)
                {
                    DataSourceItem.BalanceDebit = Math.Abs(curruntBalance);
                    DataSourceItem.BalanceCredit = 0;

                    x.BalanceDebit = Math.Abs(curruntBalance);
                    x.BalanceCredit = 0;

                }
                else if (curruntBalance < 0)
                {
                    DataSourceItem.BalanceDebit = 0;
                    DataSourceItem.BalanceCredit = Math.Abs(curruntBalance);

                    x.BalanceDebit = 0;
                    x.BalanceCredit = Math.Abs(curruntBalance);
                }
                else
                {
                    DataSourceItem.BalanceDebit = 0;
                    DataSourceItem.BalanceCredit = 0;
                }

                i++;
                return DataSourceItem;
            }).ToList();
            return DataSource;
        }
       
        
        
        
        
        
        
        #endregion

       
        
        
        
        public IList<Account> GetChild(int Id, IList<Account> items)
        {
            var childs = items
                   .Where(x => x.ParentId == Id || x.Id == Id)
                   .Union(items.Where(x => x.ParentId == Id)
                   .SelectMany(y => GetChild(y.Id, items)));

            return childs.ToList();
        }

        public IList<ReviewBalanceReportDs> GetChild2(int Id, IList<ReviewBalanceReportDs> items)
        {
            var childs = items
                   .Where(x => x.ParentId == Id || x.AccountId == Id)
                   .Union(items.Where(x => x.ParentId == Id)
                   .SelectMany(y => GetChild2(y.AccountId, items)));

            return childs.ToList();
        }

        public string GetTransactionSideName(int SideTypeId, int SideId)
        {
            string SideName = "";
            if (SideTypeId == (int)EntrySides.Account)
            {

                var account = _AccountsService.GetWithCondetion(x => x.LastLevelInTree && x.Id == SideId).Select(a => new { Name = a.Name, }).FirstOrDefault();
                if (account != null)
                {
                    SideName = account.Name;
                }
            }
            else if (SideTypeId == (int)EntrySides.Supplier)
            {
                var suppler = _SupplerService.GetWithCondetion(x => x.Id == SideId).Select(x => new { Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn }).FirstOrDefault();

                if (suppler != null)
                {
                    SideName = suppler.Name;
                }
            }
            else if (SideTypeId == (int)EntrySides.Customer)
            {
                var customer = _CustomerService.GetWithCondetion(x => x.Id == SideId).Select(x => new { Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn }).FirstOrDefault();

                if (customer != null)
                {
                    SideName = customer.Name;
                }

            }
            else if (SideTypeId == (int)EntrySides.Bank)
            {
                var bank = _BankService.GetWithCondetion(x => x.Id == SideId).Select(x => new { Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn }).FirstOrDefault();

                if (bank != null)
                {
                    SideName = bank.Name;
                }
            }
            else if (SideTypeId == (int)EntrySides.Treasury)
            {
                var treasury = _TreasuryService.GetWithCondetion(x => x.Id == SideId).Select(x => new { Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn }).FirstOrDefault();

                if (treasury != null)
                {
                    SideName = treasury.Name;
                }
            }
            return SideName;
        }

        public void SetReportResult(XtraReport rpt, object src, BaseFiltersModel filters)
        {
            SetRptHeader(filters);
            var model = new ReportResultModel();
            filters.reportResultModel = model;
            rpt.DataSource = src;
            model.FilePath = rpt.GeneratePdfByPath(filters);
            model.Success = true;
            model.DataSource = src;
        }

        public void SetRptParameter(XtraReport rpt, object filter, string ParamterName, object Value = null, bool IgnoreNullFilter = false)
        {
            if (rpt.Parameters[ParamterName] != null)
                if (filter != null || IgnoreNullFilter)
                    rpt.Parameters[ParamterName].Value = Value ?? filter;
        }

         public void SetRptHeader(BaseFiltersModel Filters)
        {
            var company = _CompanyService.GetById(Filters.CompanyId.Value);
            Filters.CompanyName = _currentLanguage == "ar" ? company.NameAr : company.NameEn;
            Filters.CompanyLogo = GetCompanyImage(company);
            Filters.ReportName = Filters.ReportName;
        }





    }
}
