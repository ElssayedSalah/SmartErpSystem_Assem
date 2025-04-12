using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models;
using BusinessLayer.Models.Charts;
using BusinessLayer.Models.System;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.Purchases;
using DataAccessLayer.Entities.Sales;
using DataAccessLayer.Entities.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static BusinessLayer.Helpers.SharedEnums;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class HomeController : BaseAdminController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IidentityService _IdentityService;
        private readonly IBaseService<Company> _CompanyService;        
        private readonly IBaseService<Store> _StoreService;
        private readonly IBaseService<Suppler> _SupplerService;
        private readonly IBaseService<Customer> _CustomerService;
        private readonly IBaseService<Item> _ItemService;
        private readonly IBaseService<Transaction_InvMaster> _InventoryMasterService;
        private readonly IBaseService<Transaction_InvDetails> _InventoryDetailsService;
        private readonly IBaseService<Treasury> _TreasuryService;
        private readonly IBaseService<Bank> _BankService;
        private readonly IBaseService<DailyEntryDetails> _EntryDetailsService;
        private readonly IBaseService<TreasuryOpenBalance> _TreasuryOpenBalanceService;
        private readonly IBaseService<BankOpenBalance> _BankOpenBalanceService;
        private readonly IBaseService<SupplerOpenBalance> _SupplerOpenBalanceService;
        private readonly IBaseService<CustomerOpenBalance> _CustomerOpenBalanceService;
        private readonly IBaseService<Currency> _CurrencyService;
        private readonly IMapper _Mapper;
        private readonly string _currentLanguage;
      
        public HomeController(ILogger<HomeController> logger, IidentityService IdentityService, IBaseService<Company> CompanyService, IMapper mapper, IBaseService<Store> StoreService, IBaseService<Item> ItemService, IBaseService<Transaction_InvMaster> InventoryMasterService, IBaseService<Transaction_InvDetails> InventoryDetailsService, IBaseService<Suppler> SupplerService, IBaseService<Customer> CustomerService, IBaseService<Treasury> TreasuryService, IBaseService<DailyEntryDetails> EntryDetailsService, IBaseService<Bank> BankService, IBaseService<TreasuryOpenBalance> TreasuryOpenBalanceService, IBaseService<BankOpenBalance> BankOpenBalanceService, IBaseService<SupplerOpenBalance> SupplerOpenBalanceService, IBaseService<CustomerOpenBalance> CustomerOpenBalanceService, IBaseService<Currency> CurrencyService)
        {
            _logger = logger;
            _IdentityService = IdentityService;
            _CompanyService = CompanyService;
            _Mapper = mapper;
            _StoreService = StoreService;
            _ItemService = ItemService;
            _SupplerService = SupplerService;
            _CustomerService = CustomerService;
            _InventoryMasterService = InventoryMasterService;
            _InventoryDetailsService = InventoryDetailsService;
            _TreasuryService = TreasuryService;
            _BankService = BankService;
            _TreasuryOpenBalanceService = TreasuryOpenBalanceService;
            _BankOpenBalanceService = BankOpenBalanceService;
            _SupplerOpenBalanceService = SupplerOpenBalanceService;
            _CustomerOpenBalanceService = CustomerOpenBalanceService;
            _EntryDetailsService = EntryDetailsService;
            _CurrencyService = CurrencyService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //[ValidateAntiForgeryToken]
        public IActionResult Dashpoard()
        {
            return View();
        }
        //[ValidateAntiForgeryToken]
        public IActionResult Dashpoard2()
        {
            if (!_IdentityService.Authorize(StandardPermissionProvider.Dashboard.SystemName, CurrentUser, PermissionActions.List))
            {
                TempData["UnAuthorizeUser"] = "UnAuthorizeUser";
                return RedirectToAction("login", "Account");
            }
            HomeDashboardModel model = new HomeDashboardModel();
            try
            {
                var InventoryTransactions = _InventoryMasterService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId && (x.DocTypeId == (int)DocumentTypes.PurchaseInvoice || x.DocTypeId == (int)DocumentTypes.PurchaseReturn || x.DocTypeId == (int)DocumentTypes.SalesInvoice || x.DocTypeId == (int)DocumentTypes.SalesReturn));
               
                #region Inventory Statestic
                var TransactionsCountTask = Task.Run(() =>
                {
                    var InventoryTrans = InventoryTransactions.Where(x => x.DocTypeId == (int)DocumentTypes.PurchaseInvoice || x.DocTypeId == (int)DocumentTypes.PurchaseReturn || x.DocTypeId == (int)DocumentTypes.SalesInvoice || x.DocTypeId == (int)DocumentTypes.SalesReturn)?.Select(x => x.DocTypeId);
                    if (InventoryTrans != null)
                    {
                        model.PurchasesTransactionCount = InventoryTrans.Count(x => x.HasValue && x.Value == (int)DocumentTypes.PurchaseInvoice);
                        model.PurchasesReturnTransactionCount = InventoryTrans.Count(x => x.HasValue && x.Value == (int)DocumentTypes.PurchaseReturn);
                        model.SalesTransactionCount = InventoryTrans.Count(x => x.HasValue && x.Value == (int)DocumentTypes.SalesInvoice);
                        model.SalesReturnTransactionCount = InventoryTrans.Count(x => x.HasValue && x.Value == (int)DocumentTypes.SalesReturn);
                    }

                });
                #endregion

                var tasks = new List<Task>();
                tasks.Add(TransactionsCountTask);

                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception)
            {
                
            }      

            return View(model);
        }

        //المشتريات والمبيعات علي مستوي الشهور
        public IActionResult GetBarChartDs()
        {
            var InventoryTransactionsMaster = _InventoryMasterService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId && (x.DocTypeId == (int)DocumentTypes.PurchaseInvoice || x.DocTypeId == (int)DocumentTypes.SalesInvoice ));
           
            var monthes = Extensions.GetMonthsNames(_currentLanguage);
            var defultCuruncy = "العملة الإفتراضية";
            var Curuncy = _CurrencyService.GetWithCondetion(x=>x.DefaultCurrency).FirstOrDefault();
            if (Curuncy != null)
            {
                defultCuruncy = _currentLanguage == "ar" ? Curuncy.NameAr : Curuncy.NameEn;
            }
            #region Purchases Chart
            var PurchasesChartDs = new ChartModel();
            PurchasesChartDs.labels = monthes;
            //PurchasesChartDs.label = "ج م";

            var PurchasesValues = InventoryTransactionsMaster
            .Where(x => x.DocTypeId == (int)DocumentTypes.PurchaseInvoice)
            .GroupBy(g => g.DocDate.Month)
            .Select(x => new
            {
                month=x.FirstOrDefault().DocDate.Month,
                data = x.Sum(item => item.InvoiceNet)
            })
            .Select(x =>new { x.data ,x.month}).OrderByDescending(x=>x.month).ToList();
            int index = 11;
            for (int i = 12; i > 0; i--)
            {
                if (PurchasesValues.Any(x=>x.month==i))
                {
                    PurchasesChartDs.data[index] = PurchasesValues.Find(x=>x.month==i).data;
                }
                index--;
            }

            #endregion

            #region Sales Chart      
            var SalesChartDs = new ChartModel();
            SalesChartDs.labels = monthes;
            //SalesChartDs.label = "ج م";
            var SalesValues = InventoryTransactionsMaster
            .Where(x => x.DocTypeId == (int)DocumentTypes.SalesInvoice)
            .GroupBy(g => g.DocDate.Month)
            .Select(x => new
            {
                month = x.FirstOrDefault().DocDate.Month,
                data = x.Sum(item => item.InvoiceNet)
            })
            .Select(x => new { x.data, x.month }).OrderByDescending(x => x.month).ToList();
            int index2 = 11;
            for (int i = 12; i > 0; i--)
            {
                if (SalesValues.Any(x => x.month == i))
                {
                    SalesChartDs.data[index2] = SalesValues.Find(x => x.month == i).data;
                }
                index2--;
            }

            #endregion


            return Json(new { PurchasesChartDs,SalesChartDs , defultCuruncy });
        }

        //حركة المشتريات والمبيعات للاصناف في السنة المالية الحالية
        public IActionResult GetLineChartDs()
        {
            var InventoryTransactions = _InventoryDetailsService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId && (x.DocTypeId == (int)DocumentTypes.PurchaseInvoice || x.DocTypeId == (int)DocumentTypes.SalesInvoice));
            var defultCuruncy = "العملة الإفتراضية";
            var Curuncy = _CurrencyService.GetWithCondetion(x => x.DefaultCurrency).FirstOrDefault();
            if (Curuncy != null)
            {
                defultCuruncy = _currentLanguage == "ar" ? Curuncy.NameAr : Curuncy.NameEn;
            }
            var monthes = Extensions.GetMonthsNames(_currentLanguage);
            var Items = _ItemService.GetAll().OrderBy(x => x.Id);

            #region ItemsPurchasesChart
            var ItemPurchasesChartDs = new LineChartModel();
            ItemPurchasesChartDs.labels = monthes;

            var PurchasesTransactions = InventoryTransactions
            .Where(x => x.DocTypeId == (int)DocumentTypes.PurchaseInvoice);
            int colorIndex = 0;
            foreach (var i in Items)
            {
                var ChartDataset = new LineChartDataset();
                var itemPurchasesInMonth = PurchasesTransactions.GroupBy(x => new { x.ItemId, x.DocDate.Month }).Select(x => new
                {
                    Month = x.FirstOrDefault().DocDate.Month,
                    ItemId = x.FirstOrDefault().ItemId,
                    Total = x.Sum(item => item.Total)
                }).Select(x => new { x.Total, x.ItemId, x.Month }).ToList();

                ChartDataset.label = _currentLanguage == "ar" ? i.NameAr : i.NameEn;
                ChartDataset.borderColor = colorIndex < ColorNames.Length ? ColorNames[colorIndex] : "Red";

                int index = 11;
                for (int r = 12; r > 0; r--)
                {
                    if (itemPurchasesInMonth.Any(x => x.ItemId == i.Id && x.Month == r))
                    {
                        ChartDataset.data[index] = itemPurchasesInMonth.Find(x => x.ItemId == i.Id && x.Month == r).Total.Value;
                    }
                    index--;
                }
                ItemPurchasesChartDs.datasets.Add(ChartDataset);
                colorIndex++;
            }
            #endregion

            #region ItemsSalesChart
            var ItemSalesChartDs = new LineChartModel();
            ItemSalesChartDs.labels = monthes;
            var SalesTransactions = InventoryTransactions
            .Where(x => x.DocTypeId == (int)DocumentTypes.SalesInvoice);
            colorIndex = 0;
            foreach (var i in Items)
            {
                var ChartDataset = new LineChartDataset();
                var itemSalesInMonth = SalesTransactions.GroupBy(x => new { x.ItemId, x.DocDate.Month }).Select(x => new
                {
                    Month = x.FirstOrDefault().DocDate.Month,
                    ItemId = x.FirstOrDefault().ItemId,
                    Total = x.Sum(item => item.Total)
                }).Select(x => new { x.Total, x.ItemId, x.Month }).ToList();

                ChartDataset.label = _currentLanguage == "ar" ? i.NameAr : i.NameEn;
                ChartDataset.borderColor = colorIndex < ColorNames.Length ? ColorNames[colorIndex] : "Red";

                int index = 11;
                for (int r = 12; r > 0; r--)
                {
                    if (itemSalesInMonth.Any(x => x.ItemId == i.Id && x.Month == r))
                    {
                        ChartDataset.data[index] = itemSalesInMonth.Find(x => x.ItemId == i.Id && x.Month == r).Total.Value;
                    }
                    index--;
                }
                ItemSalesChartDs.datasets.Add(ChartDataset);
                colorIndex++;
            }
            #endregion

            return Json(new { ItemPurchasesChartDs, ItemSalesChartDs , defultCuruncy });
        }

        //المشتريات والمبيعات علي مستوي الموردين والعملاء
        public IActionResult GetPieChartDs()
        {
            var InventoryTransactionsMaster = _InventoryMasterService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId && (x.DocTypeId == (int)DocumentTypes.PurchaseInvoice || x.DocTypeId == (int)DocumentTypes.SalesInvoice ));

            #region SupplerPurchasesChart
            var SupplerPurchasesChartDs = new ChartModel();
            var supplers= _SupplerService.GetAll().OrderBy(o=>o.Id);
            //var supplersOpenBalnce =_SupplerOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId);

            SupplerPurchasesChartDs.labels = supplers.Select(x => _currentLanguage=="ar"? x.NameAr:x.NameEn).ToList();
            SupplerPurchasesChartDs.label = "ج م";

            var PurchasesValues = InventoryTransactionsMaster
            .Where(x => x.DocTypeId == (int)DocumentTypes.PurchaseInvoice)
            .GroupBy(g => g.SupplierId)
            .Select(x => new
            {
                suppler = x.FirstOrDefault().SupplierId,
                data = x.Sum(item => item.InvoiceNet)
            })
            .Select(x => new { x.data, x.suppler }).ToList();
            SupplerPurchasesChartDs.data.Clear();
            foreach (var s in supplers)
            {
                //decimal balance = supplersOpenBalnce.Where(x => x.SupplerId == s.Id).Select(x => new { openBalance = x.OpeningBalanceDebit - x.OpeningBalanceCredit }).FirstOrDefault().openBalance; ;
                if (PurchasesValues.Any(x => x.suppler == s.Id))
                {
                    //balance += PurchasesValues.Find(x => x.suppler == s.Id).data;
                    SupplerPurchasesChartDs.data.Add(PurchasesValues.Find(x => x.suppler == s.Id).data);

                }

            }

            #endregion

            #region CustomerSalesChart
            var CustomerSalesDs = new ChartModel();
            var customers =_CustomerService.GetAll().OrderBy(o => o.Id);

            CustomerSalesDs.labels = customers.Select(x => _currentLanguage == "ar" ? x.NameAr : x.NameEn).ToList();
            CustomerSalesDs.label = "ج م";
            var SalesValues = InventoryTransactionsMaster
            .Where(x => x.DocTypeId == (int)DocumentTypes.SalesInvoice)
            .GroupBy(g => g.CustomerId)
            .Select(x => new
            {
                customer = x.FirstOrDefault().CustomerId,
                data = x.Sum(item => item.InvoiceNet)
            })
            .Select(x => new { x.data, x.customer }).ToList();
            CustomerSalesDs.data.Clear();
            foreach (var s in customers)
            {
                if (SalesValues.Any(x => x.customer == s.Id))
                {
                    CustomerSalesDs.data.Add(SalesValues.Find(x => x.customer == s.Id).data);
                }
            }

            #endregion


            return Json(new { SupplerPurchasesChartDs, CustomerSalesDs });
        }
       
        //ارصدة الخزن والبنوك
        public IActionResult GetDonutChartDs()
        {
            var EntryDetailsTransactions = _EntryDetailsService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId );

            #region TreasurysBalanceChart
            var TreasurysBalanceChartDs = new ChartModel();
            var treasurys = _TreasuryService.GetAll().OrderBy(o=>o.Id);
            var treasurysOpenBalnce = _TreasuryOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId);
            TreasurysBalanceChartDs.labels = treasurys.Select(x => _currentLanguage == "ar" ? x.NameAr : x.NameEn).ToList();

            TreasurysBalanceChartDs.data.Clear();
            foreach (var t in treasurys)
            {
               decimal treasuryBalance = 0;
               decimal treasuryBalanceOpenBalance = 0;
               var treasuryOpenBalnce=treasurysOpenBalnce.Where(x => x.TreasuryId == t.Id).Select(x => new { openBalance = x.OpenBalanceDebit - x.OpenBalanceCredit }).FirstOrDefault();

                if (treasuryOpenBalnce!=null)
                {
                    treasuryBalanceOpenBalance= treasuryOpenBalnce.openBalance;
                }

                if (t.AccountId!=null && t.AccountId>0)          
                {
                    var treasuryEntryDetails = EntryDetailsTransactions.Where(x => x.AccountId == t.AccountId).GroupBy(x => x.AccountId).Select(x => new { Balance = x.Sum(s => s.Debit) - x.Sum(s => s.Credit) }).FirstOrDefault();
                    if (treasuryEntryDetails!=null)
                    {
                        treasuryBalance = treasuryEntryDetails.Balance;
                    }
                
                }
                TreasurysBalanceChartDs.data.Add(treasuryBalance + treasuryBalanceOpenBalance);

            }

            #endregion

            #region BanksBalanceChart
            var BanksBalanceChartDs = new ChartModel();
            var Banks = _BankService.GetAll().OrderBy(o => o.Id);
            var BanksOpenBalnce = _BankOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId);
            BanksBalanceChartDs.labels = Banks.Select(x => _currentLanguage == "ar" ? x.NameAr : x.NameEn).ToList();
            BanksBalanceChartDs.label = "ج م";
            BanksBalanceChartDs.data.Clear();
            foreach (var t in Banks)
            {
                decimal BankBalance = 0;
                decimal BankBalanceOpenBalance = 0;
                var bankOpenBalnce = BanksOpenBalnce.Where(x => x.BankId == t.Id).Select(x => new { openBalance = x.OpenBalanceDebit - x.OpenBalanceCredit }).FirstOrDefault();
                if (bankOpenBalnce!=null)
                {
                    BankBalanceOpenBalance= bankOpenBalnce.openBalance;
                }

                if (t.AccountId != null && t.AccountId > 0)
                {
                    var bankEntryDetails = EntryDetailsTransactions.Where(x => x.AccountId == t.AccountId).GroupBy(x => x.AccountId).Select(x => new { Balance = x.Sum(s => s.Debit) - x.Sum(s => s.Credit) }).FirstOrDefault();

                    if (bankEntryDetails!=null)
                    {
                        BankBalance = bankEntryDetails.Balance;
                    }     
                }
                BanksBalanceChartDs.data.Add(BankBalance + BankBalanceOpenBalance);

            }

            #endregion


            return Json(new { TreasurysBalanceChartDs , BanksBalanceChartDs });
        }

       




        public IActionResult GetCompanyData()
        {
            CompanyMobel companyModel = new CompanyMobel() { NameAr="Smart Erp System"};
            var company = _CompanyService.GetAll().FirstOrDefault();
            if (company!=null)
            {
                 companyModel = _Mapper.Map<CompanyMobel>(company);
            }

            return Json(companyModel);
        }
        public IActionResult GetBranchStores(int BranchId)
        {
            try
            {
                var stores = _StoreService.GetAll().Where(x => x.BranchId == BranchId).Select(x => new { Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn, Id = x.Id });
                return Json(stores);
            }
            catch (Exception)
            {
                return Json("erorr");
            }
        }
        public IActionResult GetGroupItems(int GroupId)
        {
            try
            {
                var items = _ItemService.GetAll().Where(x => x.GroupId == GroupId).Select(x => new { Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn, Id = x.Id });
                return Json(items);
            }
            catch (Exception)
            {
                return Json("erorr");
            }
        }


    }
}
