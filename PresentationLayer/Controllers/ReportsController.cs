using BusinessLayer.Models.Filters;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.Purchases;
using DataAccessLayer.Entities.Sales;
using DataAccessLayer.Entities.System;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helpers;
using Reports;
using Reports.Financial;
using Reports.Inventory;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using static BusinessLayer.Helpers.SharedEnums;

namespace PresentationLayer.Controllers
{
    public class ReportsController : BaseAdminController
    {
        private readonly IBaseService<Item> _ItemService;
        private readonly IBaseService<Branch> _BranchService;
        private readonly IBaseService<Store> _StoreService;
        private readonly IBaseService<Suppler> _SupplerService;
        private readonly IBaseService<Customer> _CustomerService;
        private readonly IBaseService<ItemGroup> _ItemGroupService;
        private readonly IBaseService<OpenBalanceReportView> _OpenBalanceReportViewService;
        private readonly IReportService _ReportService;
        private readonly string _currentLanguage;
        private readonly LocalizationService _LocalizationService;
        private readonly IidentityService _identityService;
        private readonly IBaseService<FinancialPeriod> _FinancialPeriodService;
        private readonly IBaseService<DailyAccounts_Def> _DailyAccounts_DefService;
        private readonly IBaseService<Account> _AccountsService;
        private readonly IBaseService<Document> _DocumentService;
        private readonly IBaseService<Bank> _BankService;
        private readonly IBaseService<Treasury> _TreasuryService;
        //private readonly DateTime FromDate =DateTime.Now;
        //private readonly DateTime ToDate = DateTime.Now;

        public ReportsController(
         IBaseService<Item> ItemService,
         IBaseService<ItemGroup> ItemGroupService,
         IBaseService<Branch> BranchService,
         IBaseService<Store> StoreService,
         IReportService ReportService,
         LocalizationService localizationService,
         IidentityService IdentityService,
         IBaseService<Suppler> SupplerService,
         IBaseService<Customer> CustomerService,
         IBaseService<FinancialPeriod> FinancialPeriodService,
         IBaseService<DailyAccounts_Def> DailyAccounts_DefService,
         IBaseService<OpenBalanceReportView> OpenBalanceReportViewService,
         IBaseService<Account> AccountsService,
         IBaseService<Document> DocumentService,
         IBaseService<Bank> BankService,
         IBaseService<Treasury> TreasuryService

        )
        {

            _ItemService = ItemService;
            _ItemGroupService = ItemGroupService;
            _BranchService = BranchService;
            _StoreService = StoreService;
            _ReportService = ReportService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _LocalizationService = localizationService;
            _OpenBalanceReportViewService = OpenBalanceReportViewService;
            _identityService = IdentityService;
            _SupplerService = SupplerService;
            _CustomerService = CustomerService;
            _FinancialPeriodService = FinancialPeriodService;
            _DailyAccounts_DefService = DailyAccounts_DefService;
            _AccountsService = AccountsService;
            _DocumentService = DocumentService;
            _BankService = BankService;
            _TreasuryService = TreasuryService;

            //var FinancialPeriod = _FinancialPeriodService.GetById(CurrentUser.FinancialPeriodId.Value);
            //if (FinancialPeriod!=null)
            //{
            //    FromDate = FinancialPeriod.DateFrom.Date;
            //    ToDate = FinancialPeriod.DateTo.Date;
            //}


        }

        #region IntializeDropdowens
        public void IntializeDropdowens()
        {
            var Branches = _BranchService.GetAll();
            Branches.Insert(0, new Branch() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Branches = Branches.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Groups = _ItemGroupService.GetAll();
            Groups.Insert(0, new ItemGroup() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Groups = Groups.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Stores = _StoreService.GetAll();
            Stores.Insert(0, new Store() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Stores = Stores.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Items = _ItemService.GetAll();
            Items.Insert(0, new Item() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Items = Items.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });
        }
        public void IntializeDropdowensForItemCartReport()
        {
            var Branches = _BranchService.GetAll();
            ViewBag.Branches = Branches.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Groups = _ItemGroupService.GetAll();
            ViewBag.Groups = Groups.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Stores = _StoreService.GetAll();
            ViewBag.Stores = Stores.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Items = _ItemService.GetAll();
            ViewBag.Items = Items.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });
        }
        public void IntializeDropdowensItemPurshasAnaysisReport()
        {
            var Suppliers = _SupplerService.GetAll();
            Suppliers.Insert(0, new Suppler() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Suppliers = Suppliers.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Groups = _ItemGroupService.GetAll();
            Groups.Insert(0, new ItemGroup() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Groups = Groups.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });


            var Items = _ItemService.GetAll();
            Items.Insert(0, new Item() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Items = Items.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });
        }
        public void IntializeDropdowensItemSalesAnaysisReport()
        {
            var Customers = _CustomerService.GetAll();
            Customers.Insert(0, new Customer() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Customers = Customers.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Groups = _ItemGroupService.GetAll();
            Groups.Insert(0, new ItemGroup() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Groups = Groups.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });


            var Items = _ItemService.GetAll();
            Items.Insert(0, new Item() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Items = Items.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });
        }
        #endregion


        public IActionResult ReportViewer(string fileName)
        {
            return File(new FileStream(fileName, FileMode.Open, FileAccess.Read), "application/pdf");
        }

        [HttpGet]
        public IActionResult OpenBalanceReport()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageOpenBalanceReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            IntializeDropdowens();
            OpenBalanceReportFilters filters = new OpenBalanceReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId };
            return View(filters);
        }
        [HttpPost]
        public IActionResult OpenBalanceReport(OpenBalanceReportFilters filters)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageOpenBalanceReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();
            OpenBalanceReport rpt = new OpenBalanceReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.OpenBalanceReport(filters);            

            string BranchFilter, StoreFilter, GroupFilter, ItemFilter = "";

            if (_currentLanguage == "ar")
            {
                BranchFilter = (filters.BranchId.HasValue && filters.BranchId.Value > 0) ? _BranchService.GetById(filters.BranchId.Value)?.NameAr : "الكل";
                StoreFilter = (filters.StoreId.HasValue && filters.StoreId.Value > 0) ? _StoreService.GetById(filters.StoreId.Value)?.NameAr : "الكل";
                GroupFilter = (filters.GroupId.HasValue && filters.GroupId.Value > 0) ? _ItemGroupService.GetById(filters.GroupId.Value)?.NameAr : "الكل";
                ItemFilter = (filters.ItemId.HasValue && filters.ItemId.Value > 0) ? _ItemService.GetById(filters.ItemId.Value)?.NameAr : "الكل";
            }
            else
            {
                BranchFilter = (filters.BranchId.HasValue && filters.BranchId.Value > 0) ? _BranchService.GetById(filters.BranchId.Value)?.NameEn : "All";
                StoreFilter = (filters.StoreId.HasValue && filters.StoreId.Value > 0) ? _StoreService.GetById(filters.StoreId.Value)?.NameEn : "All";
                GroupFilter = (filters.GroupId.HasValue && filters.GroupId.Value > 0) ? _ItemGroupService.GetById(filters.GroupId.Value)?.NameEn : "All";
                ItemFilter = (filters.ItemId.HasValue && filters.ItemId.Value > 0) ? _ItemService.GetById(filters.ItemId.Value)?.NameEn : "All";

            }
            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "BranchFilter", BranchFilter != null ? BranchFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "StoreFilter", StoreFilter != null ? StoreFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "GroupFilter", GroupFilter != null ? GroupFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "ItemFilter", ItemFilter != null ? ItemFilter : "");

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);

            IntializeDropdowens();
            return View(filters);
        }

        [HttpGet]
        public IActionResult ItemDataReport()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItemDataReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            IntializeDropdowens();
            ItemDataReportFilters filters = new ItemDataReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId , FromDate= CurrentUser.FinancialPeriodFromDate.Value, ToDate= CurrentUser.FinancialPeriodToDate.Value };
            return View(filters);
        }
        [HttpPost]
        public IActionResult ItemDataReport(ItemDataReportFilters filters)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItemDataReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            ItemDataReport rpt = new ItemDataReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.ItemDataReport(filters);

            string BranchFilter, StoreFilter, GroupFilter, ItemFilter = "";

            if (_currentLanguage == "ar")
            {
                BranchFilter = (filters.BranchId.HasValue && filters.BranchId.Value > 0) ? _BranchService.GetById(filters.BranchId.Value)?.NameAr : "الكل";
                StoreFilter = (filters.StoreId.HasValue && filters.StoreId.Value > 0) ? _StoreService.GetById(filters.StoreId.Value)?.NameAr : "الكل";
                GroupFilter = (filters.GroupId.HasValue && filters.GroupId.Value > 0) ? _ItemGroupService.GetById(filters.GroupId.Value)?.NameAr : "الكل";
                ItemFilter = (filters.ItemId.HasValue && filters.ItemId.Value > 0) ? _ItemService.GetById(filters.ItemId.Value)?.NameAr : "الكل";
            }
            else
            {
                BranchFilter = (filters.BranchId.HasValue && filters.BranchId.Value > 0) ? _BranchService.GetById(filters.BranchId.Value)?.NameEn : "All";
                StoreFilter = (filters.StoreId.HasValue && filters.StoreId.Value > 0) ? _StoreService.GetById(filters.StoreId.Value)?.NameEn : "All";
                GroupFilter = (filters.GroupId.HasValue && filters.GroupId.Value > 0) ? _ItemGroupService.GetById(filters.GroupId.Value)?.NameEn : "All";
                ItemFilter = (filters.ItemId.HasValue && filters.ItemId.Value > 0) ? _ItemService.GetById(filters.ItemId.Value)?.NameEn : "All";

            }
            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "BranchFilter", BranchFilter != null ? BranchFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "StoreFilter", StoreFilter != null ? StoreFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "GroupFilter", GroupFilter != null ? GroupFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "ItemFilter", ItemFilter != null ? ItemFilter : "");

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);

            IntializeDropdowens();
            return View(filters);
        }

        [HttpGet]
        public IActionResult ItemBalanceQuantityAndValueReport()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItemBalanceQuantityAndValueReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            IntializeDropdowens();
            ItemBalanceQuantityAndValueReportFilters filters = new ItemBalanceQuantityAndValueReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId, FromDate = CurrentUser.FinancialPeriodFromDate.Value, ToDate = CurrentUser.FinancialPeriodToDate.Value };
            return View(filters);
        }
        [HttpPost]
        public IActionResult ItemBalanceQuantityAndValueReport(ItemBalanceQuantityAndValueReportFilters filters)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItemBalanceQuantityAndValueReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            ItemBalanceQuantityAndValueReport rpt = new ItemBalanceQuantityAndValueReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.ItemBalanceQuantityAndValueReport(filters);

            string BranchFilter, StoreFilter, GroupFilter, ItemFilter = "";

            if (_currentLanguage == "ar")
            {
                BranchFilter = (filters.BranchId.HasValue && filters.BranchId.Value > 0) ? _BranchService.GetById(filters.BranchId.Value)?.NameAr : "الكل";
                StoreFilter = (filters.StoreId.HasValue && filters.StoreId.Value > 0) ? _StoreService.GetById(filters.StoreId.Value)?.NameAr : "الكل";
                GroupFilter = (filters.GroupId.HasValue && filters.GroupId.Value > 0) ? _ItemGroupService.GetById(filters.GroupId.Value)?.NameAr : "الكل";
                ItemFilter = (filters.ItemId.HasValue && filters.ItemId.Value > 0) ? _ItemService.GetById(filters.ItemId.Value)?.NameAr : "الكل";
            }
            else
            {
                BranchFilter = (filters.BranchId.HasValue && filters.BranchId.Value > 0) ? _BranchService.GetById(filters.BranchId.Value)?.NameEn : "All";
                StoreFilter = (filters.StoreId.HasValue && filters.StoreId.Value > 0) ? _StoreService.GetById(filters.StoreId.Value)?.NameEn : "All";
                GroupFilter = (filters.GroupId.HasValue && filters.GroupId.Value > 0) ? _ItemGroupService.GetById(filters.GroupId.Value)?.NameEn : "All";
                ItemFilter = (filters.ItemId.HasValue && filters.ItemId.Value > 0) ? _ItemService.GetById(filters.ItemId.Value)?.NameEn : "All";

            }
            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "BranchFilter", BranchFilter != null ? BranchFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "StoreFilter", StoreFilter != null ? StoreFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "GroupFilter", GroupFilter != null ? GroupFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "ItemFilter", ItemFilter != null ? ItemFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "ToDateFilter", ItemFilter != null ? filters.ToDate : "");

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);

            IntializeDropdowens();
            return View(filters);
        }

        [HttpGet]
        public IActionResult ItemCartReport()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItemCartReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            IntializeDropdowensForItemCartReport();
            ItemCartReportFilters filters = new ItemCartReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId, FromDate = CurrentUser.FinancialPeriodFromDate.Value, ToDate = CurrentUser.FinancialPeriodToDate.Value };
            return View(filters);
        }
        [HttpPost]
        public IActionResult ItemCartReport(ItemCartReportFilters filters)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItemCartReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            IntializeDropdowensForItemCartReport();
            if (filters.BranchId==null|| filters.BranchId==0)
            {
                ModelState.AddModelError("BranchRequired", "يجب إختيار فرع");
                return View(filters);
            }
            if (filters.StoreId == null || filters.StoreId == 0)
            {
                ModelState.AddModelError("StoreRequired", "يجب إختيار مخزن");
                return View(filters);
            }
            if (filters.ItemId == null || filters.ItemId == 0)
            {
                ModelState.AddModelError("ItemRequired", "يجب إختيار صنف");
                return View(filters);
            }

            ItemCartReport rpt = new ItemCartReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.ItemCartReport(filters);

            string BranchFilter, StoreFilter, GroupFilter, ItemFilter = "";

            if (_currentLanguage == "ar")
            {
                BranchFilter = (filters.BranchId.HasValue && filters.BranchId.Value > 0) ? _BranchService.GetById(filters.BranchId.Value)?.NameAr : "الكل";
                StoreFilter = (filters.StoreId.HasValue && filters.StoreId.Value > 0) ? _StoreService.GetById(filters.StoreId.Value)?.NameAr : "الكل";
                GroupFilter = (filters.GroupId.HasValue && filters.GroupId.Value > 0) ? _ItemGroupService.GetById(filters.GroupId.Value)?.NameAr : "الكل";
                ItemFilter = (filters.ItemId.HasValue && filters.ItemId.Value > 0) ? _ItemService.GetById(filters.ItemId.Value)?.NameAr : "الكل";
            }
            else
            {
                BranchFilter = (filters.BranchId.HasValue && filters.BranchId.Value > 0) ? _BranchService.GetById(filters.BranchId.Value)?.NameEn : "All";
                StoreFilter = (filters.StoreId.HasValue && filters.StoreId.Value > 0) ? _StoreService.GetById(filters.StoreId.Value)?.NameEn : "All";
                GroupFilter = (filters.GroupId.HasValue && filters.GroupId.Value > 0) ? _ItemGroupService.GetById(filters.GroupId.Value)?.NameEn : "All";
                ItemFilter = (filters.ItemId.HasValue && filters.ItemId.Value > 0) ? _ItemService.GetById(filters.ItemId.Value)?.NameEn : "All";

            }
            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "BranchFilter", BranchFilter != null ? BranchFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "StoreFilter", StoreFilter != null ? StoreFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "GroupFilter", GroupFilter != null ? GroupFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "ItemFilter", ItemFilter != null ? ItemFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "FromDateFilter", filters.FromDate);
            _ReportService.SetRptParameter(rpt, filters, "ToDateFilter", filters.ToDate);

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);

            
            return View(filters);
        }

        [HttpGet]
        public IActionResult ItemPurshasAnaysisReport()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItemPurshasAnaysisReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            IntializeDropdowensItemPurshasAnaysisReport();
            ItemPurshasAnaysisReportFilters filters = new ItemPurshasAnaysisReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId, FromDate = CurrentUser.FinancialPeriodFromDate.Value, ToDate = CurrentUser.FinancialPeriodToDate.Value };
            return View(filters);
        }
        [HttpPost]
        public IActionResult ItemPurshasAnaysisReport(ItemPurshasAnaysisReportFilters filters)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItemPurshasAnaysisReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            IntializeDropdowensItemPurshasAnaysisReport();

            ItemPurshasAnaysisReport rpt = new ItemPurshasAnaysisReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.ItemPurshasAnaysisReport(filters);

            string SupplierFilter, GroupFilter, ItemFilter = "";

            if (_currentLanguage == "ar")
            {
                SupplierFilter = (filters.SupplierId.HasValue && filters.SupplierId.Value > 0) ?_SupplerService.GetById(filters.SupplierId.Value)?.NameAr : "الكل";
                GroupFilter = (filters.GroupId.HasValue && filters.GroupId.Value > 0) ? _ItemGroupService.GetById(filters.GroupId.Value)?.NameAr : "الكل";
                ItemFilter = (filters.ItemId.HasValue && filters.ItemId.Value > 0) ? _ItemService.GetById(filters.ItemId.Value)?.NameAr : "الكل";
            }
            else
            {
                SupplierFilter = (filters.SupplierId.HasValue && filters.SupplierId.Value > 0) ? _SupplerService.GetById(filters.SupplierId.Value)?.NameEn : "الكل";
                GroupFilter = (filters.GroupId.HasValue && filters.GroupId.Value > 0) ? _ItemGroupService.GetById(filters.GroupId.Value)?.NameEn : "All";
                ItemFilter = (filters.ItemId.HasValue && filters.ItemId.Value > 0) ? _ItemService.GetById(filters.ItemId.Value)?.NameEn : "All";

            }
            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "SupplierFilter", SupplierFilter != null ? SupplierFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "GroupFilter", GroupFilter != null ? GroupFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "ItemFilter", ItemFilter != null ? ItemFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "FromDateFilter", filters.FromDate);
            _ReportService.SetRptParameter(rpt, filters, "ToDateFilter", filters.ToDate);

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);


            return View(filters);
        }

        [HttpGet]
        public IActionResult SupplierBalanceReport()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageSupplierBalanceReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            var Suppliers = _SupplerService.GetAll();           
            ViewBag.Suppliers = Suppliers.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            SupplierBalanceReportFilters filters = new SupplierBalanceReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId, FromDate = CurrentUser.FinancialPeriodFromDate.Value, ToDate = CurrentUser.FinancialPeriodToDate.Value };
            return View(filters);
        }
        [HttpPost]
        public IActionResult SupplierBalanceReport(SupplierBalanceReportFilters filters)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageSupplierBalanceReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            var Suppliers = _SupplerService.GetAll();
            ViewBag.Suppliers = Suppliers.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            SupplierBalanceReport rpt = new SupplierBalanceReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.SupplierBalanceReport(filters);

            string SupplierFilter="";

            if (_currentLanguage == "ar")
            {
                SupplierFilter = (filters.SupplierId.HasValue && filters.SupplierId.Value > 0) ? _SupplerService.GetById(filters.SupplierId.Value)?.NameAr : "الكل";
            }
            else
            {
                SupplierFilter = (filters.SupplierId.HasValue && filters.SupplierId.Value > 0) ? _SupplerService.GetById(filters.SupplierId.Value)?.NameEn : "الكل";  

            }
            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "SupplierFilter", SupplierFilter != null ? SupplierFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "FromDateFilter", filters.FromDate);
            _ReportService.SetRptParameter(rpt, filters, "ToDateFilter", filters.ToDate);

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);


            return View(filters);
        }

        [HttpGet]
        public IActionResult SupplierBalanceTotalReport()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageSupplierBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            var Branches = _BranchService.GetAll();
            Branches.Insert(0, new Branch() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Branches = Branches.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Suppliers = _SupplerService.GetAll();
            Suppliers.Insert(0, new Suppler() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Suppliers = Suppliers.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            SupplierBalanceTotalReportFilters filters = new SupplierBalanceTotalReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId, FromDate = CurrentUser.FinancialPeriodFromDate.Value, ToDate = CurrentUser.FinancialPeriodToDate.Value };
            return View(filters);
        }
        [HttpPost]
        public IActionResult SupplierBalanceTotalReport(SupplierBalanceTotalReportFilters filters)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageSupplierBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            var Branches = _BranchService.GetAll();
            Branches.Insert(0, new Branch() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Branches = Branches.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Suppliers = _SupplerService.GetAll();
            Suppliers.Insert(0, new Suppler() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Suppliers = Suppliers.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            SupplierBalanceTotalReport rpt = new SupplierBalanceTotalReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.SupplierBalanceTotalReport(filters);

            string SupplierFilter = "";
            string BranchFilter = "";

            if (_currentLanguage == "ar")
            {
                SupplierFilter = (filters.SupplierId.HasValue && filters.SupplierId.Value > 0) ? Suppliers.FirstOrDefault(s=>s.Id== filters.SupplierId.Value).NameAr : "الكل";

                BranchFilter = (filters.BranchId.HasValue && filters.BranchId.Value > 0) ? Branches.FirstOrDefault(b=>b.Id==filters.BranchId.Value).NameAr : "الكل";
            }
            else
            {
                SupplierFilter = (filters.SupplierId.HasValue && filters.SupplierId.Value > 0) ? Suppliers.FirstOrDefault(s => s.Id == filters.SupplierId.Value).NameEn : "الكل";

                BranchFilter = (filters.BranchId.HasValue && filters.BranchId.Value > 0) ? Branches.FirstOrDefault(b => b.Id == filters.BranchId.Value).NameEn : "الكل";

            }
            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "SupplierFilter", SupplierFilter != null ? SupplierFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "BranchFilter", BranchFilter != null ? BranchFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "FromDateFilter", filters.FromDate);
            _ReportService.SetRptParameter(rpt, filters, "ToDateFilter", filters.ToDate);

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);


            return View(filters);
        }

        [HttpGet]
        public IActionResult ItemSalesAnaysisReport()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItemSalesAnaysisReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            IntializeDropdowensItemSalesAnaysisReport();
            ItemSalesAnaysisReportFilters filters = new ItemSalesAnaysisReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId, FromDate = CurrentUser.FinancialPeriodFromDate.Value, ToDate = CurrentUser.FinancialPeriodToDate.Value };
            return View(filters);
        }
        [HttpPost]
        public IActionResult ItemSalesAnaysisReport(ItemSalesAnaysisReportFilters filters)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItemSalesAnaysisReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            IntializeDropdowensItemSalesAnaysisReport();

            ItemSalesAnaysisReport rpt = new ItemSalesAnaysisReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.ItemSalesAnaysisReport(filters);

            string CustomerFilter, GroupFilter, ItemFilter = "";

            if (_currentLanguage == "ar")
            {
                CustomerFilter = (filters.CustomerId.HasValue && filters.CustomerId.Value > 0) ? _SupplerService.GetById(filters.CustomerId.Value)?.NameAr : "الكل";
                GroupFilter = (filters.GroupId.HasValue && filters.GroupId.Value > 0) ? _ItemGroupService.GetById(filters.GroupId.Value)?.NameAr : "الكل";
                ItemFilter = (filters.ItemId.HasValue && filters.ItemId.Value > 0) ? _ItemService.GetById(filters.ItemId.Value)?.NameAr : "الكل";
            }
            else
            {
                CustomerFilter = (filters.CustomerId.HasValue && filters.CustomerId.Value > 0) ? _SupplerService.GetById(filters.CustomerId.Value)?.NameEn : "الكل";
                GroupFilter = (filters.GroupId.HasValue && filters.GroupId.Value > 0) ? _ItemGroupService.GetById(filters.GroupId.Value)?.NameEn : "All";
                ItemFilter = (filters.ItemId.HasValue && filters.ItemId.Value > 0) ? _ItemService.GetById(filters.ItemId.Value)?.NameEn : "All";

            }
            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "CustomerFilter", CustomerFilter != null ? CustomerFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "GroupFilter", GroupFilter != null ? GroupFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "ItemFilter", ItemFilter != null ? ItemFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "FromDateFilter", filters.FromDate);
            _ReportService.SetRptParameter(rpt, filters, "ToDateFilter", filters.ToDate);

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);


            return View(filters);
        }

        [HttpGet]
        public IActionResult CustomerBalanceReport()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();


            var Customers = _CustomerService.GetAll();
            ViewBag.Customers = Customers.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            CustomerBalanceReportFilters filters = new CustomerBalanceReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId, FromDate = CurrentUser.FinancialPeriodFromDate.Value, ToDate = CurrentUser.FinancialPeriodToDate.Value };

            if (CurrentUser != null && CurrentUser.FinancialPeriodId.HasValue)
            {
                var CurruntFinancialPeriod = _FinancialPeriodService.GetById(CurrentUser.FinancialPeriodId.Value);
                if (CurruntFinancialPeriod != null)
                {
                    filters.FromDate = CurruntFinancialPeriod.DateFrom;
                    filters.ToDate = CurruntFinancialPeriod.DateTo;
                }
            }

            return View(filters);
        }
        [HttpPost]
        public IActionResult CustomerBalanceReport(CustomerBalanceReportFilters filters)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();   

            var Customers = _CustomerService.GetAll();
            ViewBag.Customers = Customers.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            CustomerBalanceReport rpt = new CustomerBalanceReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.CustomerBalanceReport(filters);

            string CustomerFilter = "";

            if (_currentLanguage == "ar")
            {
                CustomerFilter = (filters.CustomerId.HasValue && filters.CustomerId.Value > 0) ? _CustomerService.GetById(filters.CustomerId.Value)?.NameAr : "الكل";
            }
            else
            {
                CustomerFilter = (filters.CustomerId.HasValue && filters.CustomerId.Value > 0) ? _CustomerService.GetById(filters.CustomerId.Value)?.NameEn : "الكل";

            }
            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "CustomerFilter", CustomerFilter != null ? CustomerFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "FromDateFilter", filters.FromDate);
            _ReportService.SetRptParameter(rpt, filters, "ToDateFilter", filters.ToDate);

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);


            return View(filters);
        }


        [HttpGet]
        public IActionResult CustomerBalanceTotalReport()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            var Branches = _BranchService.GetAll();
            Branches.Insert(0, new Branch() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Branches = Branches.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Customers = _CustomerService.GetAll();
            Customers.Insert(0, new Customer() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Customers = Customers.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            CustomerBalanceTotalReportFilters filters = new CustomerBalanceTotalReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId, FromDate = CurrentUser.FinancialPeriodFromDate.Value, ToDate = CurrentUser.FinancialPeriodToDate.Value };

            if (CurrentUser != null && CurrentUser.FinancialPeriodId.HasValue)
            {
                var CurruntFinancialPeriod = _FinancialPeriodService.GetById(CurrentUser.FinancialPeriodId.Value);
                if (CurruntFinancialPeriod != null)
                {
                    filters.FromDate = CurruntFinancialPeriod.DateFrom;
                    filters.ToDate = CurruntFinancialPeriod.DateTo;                   

                }
            }
            return View(filters);
        }
        [HttpPost]
        public IActionResult CustomerBalanceTotalReport(CustomerBalanceTotalReportFilters filters)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            var Branches = _BranchService.GetAll();
            Branches.Insert(0, new Branch() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Branches = Branches.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Customers = _CustomerService.GetAll();
            Customers.Insert(0, new Customer() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Customers = Customers.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            CustomerBalanceTotalReport rpt = new CustomerBalanceTotalReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.CustomerBalanceTotalReport(filters);

            string CustomerFilter = "";
            string BranchFilter = "";

            if (_currentLanguage == "ar")
            {
                CustomerFilter = (filters.CustomerId.HasValue && filters.CustomerId.Value > 0) ? Customers.FirstOrDefault(s => s.Id == filters.CustomerId.Value).NameAr : "الكل";

                BranchFilter = (filters.BranchId.HasValue && filters.BranchId.Value > 0) ? Branches.FirstOrDefault(b => b.Id == filters.BranchId.Value).NameAr : "الكل";
            }
            else
            {
                CustomerFilter = (filters.CustomerId.HasValue && filters.CustomerId.Value > 0) ? Customers.FirstOrDefault(s => s.Id == filters.CustomerId.Value).NameEn : "الكل";

                BranchFilter = (filters.BranchId.HasValue && filters.BranchId.Value > 0) ? Branches.FirstOrDefault(b => b.Id == filters.BranchId.Value).NameEn : "الكل";

            }
            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "CustomerFilter", CustomerFilter != null ? CustomerFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "BranchFilter", BranchFilter != null ? BranchFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "FromDateFilter", filters.FromDate);
            _ReportService.SetRptParameter(rpt, filters, "ToDateFilter", filters.ToDate);

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);


            return View(filters);
        }


        #region Finance

        [HttpGet]
        public IActionResult AlAstazAccountReport()
        {
            //تقرير دفتر الاستاذ


            //if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
            //    return AccessDeniedView();

            var DailyAccounts = _DailyAccounts_DefService.GetAll();
            DailyAccounts.Insert(0, new DailyAccounts_Def() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.DailyAccounts = DailyAccounts.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn }).OrderBy(o=>o.Id);

            var allAccounts = _AccountsService.GetWithCondetion(x=>x.LastLevelInTree).ToList();
            allAccounts.Add(new Account() { Id = 0, NameAr = "إختار", NameEn = "Select", LastLevelInTree = true });

            ViewBag.Accounts = allAccounts.Select(x => new { Id = x.Id, Name = x.AccountCode + " " + (_currentLanguage == "ar" ? x.NameAr : x.NameEn) }).OrderBy(o => o.Id);

            var Documents = _DocumentService.GetAll();
            Documents.Add(new Document() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Documents = Documents.Select(x => new { Id = x.DocTypeId, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn }).OrderBy(o => o.Id);

            AlAstazAccountReportFilters filters = new AlAstazAccountReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId, FromDate = CurrentUser.FinancialPeriodFromDate.Value, ToDate = CurrentUser.FinancialPeriodToDate.Value };

            if (CurrentUser != null && CurrentUser.FinancialPeriodId.HasValue)
            {
                var CurruntFinancialPeriod = _FinancialPeriodService.GetById(CurrentUser.FinancialPeriodId.Value);
                if (CurruntFinancialPeriod != null)
                {
                    filters.FromDate = CurruntFinancialPeriod.DateFrom;
                    filters.ToDate = CurruntFinancialPeriod.DateTo;

                }
            }
            return View(filters);
        }
        [HttpPost]
        public IActionResult AlAstazAccountReport(AlAstazAccountReportFilters filters)
        {
            //تقرير دفتر الاستاذ

            //if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
            //    return AccessDeniedView();

            var DailyAccounts = _DailyAccounts_DefService.GetAll();
            DailyAccounts.Insert(0, new DailyAccounts_Def() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.DailyAccounts = DailyAccounts.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn }).OrderBy(o => o.Id);

            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree).ToList();
            allAccounts.Add(new Account() { Id = 0, NameAr = "إختار", NameEn = "Select", LastLevelInTree = true });

            ViewBag.Accounts = allAccounts.Select(x => new { Id = x.Id, Name = x.AccountCode + " " + (_currentLanguage == "ar" ? x.NameAr : x.NameEn) }).OrderBy(o => o.Id);

            var Documents = _DocumentService.GetAll();
            Documents.Add(new Document() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Documents = Documents.Select(x => new { Id = x.DocTypeId, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn }).OrderBy(o => o.Id);

            if (filters.AccountId<=0)
            {
                ModelState.AddModelError("AccountId", "يجب اختيار حساب");
                return View(filters);
            }

            AlAstazAccountReport rpt = new AlAstazAccountReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.AlAstazAccountReport(filters);

            string AccountFilter = "";

            if (_currentLanguage == "ar")
            {
                AccountFilter = (filters.AccountId > 0) ? allAccounts.FirstOrDefault(s => s.Id == filters.AccountId).NameAr : "الكل";
            }
            else
            {
                AccountFilter = ( filters.AccountId > 0) ? allAccounts.FirstOrDefault(s => s.Id == filters.AccountId).NameEn : "الكل";
            }
            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "AccountFilter", AccountFilter != null ? AccountFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "FromDateFilter", filters.FromDate);
            _ReportService.SetRptParameter(rpt, filters, "ToDateFilter", filters.ToDate);

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);


            return View(filters);
        }

        [HttpGet]
        public IActionResult ReviewBalanceReport()
        {
            //تقرير ميزان المراجعة

            //if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
            //    return AccessDeniedView();           

            var allAccounts = _AccountsService.GetAll().ToList();
            allAccounts.Add(new Account() { Id = 0, NameAr = "الكل", NameEn = "ALL", LastLevelInTree = true });

            ViewBag.Accounts = allAccounts.Select(x => new { Id = x.Id, Name = x.AccountCode + " " + (_currentLanguage == "ar" ? x.NameAr : x.NameEn) });


            ReviewBalanceReportFilters filters = new ReviewBalanceReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId, FromDate = CurrentUser.FinancialPeriodFromDate.Value, ToDate = CurrentUser.FinancialPeriodToDate.Value };

            if (CurrentUser != null && CurrentUser.FinancialPeriodId.HasValue)
            {
                var CurruntFinancialPeriod = _FinancialPeriodService.GetById(CurrentUser.FinancialPeriodId.Value);
                if (CurruntFinancialPeriod != null)
                {
                    filters.FromDate = CurruntFinancialPeriod.DateFrom;
                    filters.ToDate = CurruntFinancialPeriod.DateTo;

                }
            }
            return View(filters);
        }

        [HttpPost]
        public IActionResult ReviewBalanceReport(ReviewBalanceReportFilters filters)
        {
            //تقرير ميزان المراجعة

            //if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
            //    return AccessDeniedView();

            var allAccounts = _AccountsService.GetAll().ToList();
            allAccounts.Add(new Account() { Id = 0, NameAr = "الكل", NameEn = "ALL", LastLevelInTree = true });

            ViewBag.Accounts = allAccounts.Select(x => new { Id = x.Id, Name = x.AccountCode + " " + (_currentLanguage == "ar" ? x.NameAr : x.NameEn) });

            ReviewBalanceReport rpt = new ReviewBalanceReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.ReviewBalanceReport(filters);

            string AccountFilter = "";

            if (_currentLanguage == "ar")
            {
                AccountFilter = (filters.AccountId > 0) ? allAccounts.FirstOrDefault(s => s.Id == filters.AccountId).NameAr : "الكل";
            }
            else
            {
                AccountFilter = (filters.AccountId > 0) ? allAccounts.FirstOrDefault(s => s.Id == filters.AccountId).NameEn : "الكل";
            }
            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "AccountFilter", AccountFilter != null ? AccountFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "FromDateFilter", filters.FromDate);
            _ReportService.SetRptParameter(rpt, filters, "ToDateFilter", filters.ToDate);

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);


            return View(filters);
        }


        [HttpGet]
        public IActionResult DailyAccountsDetailsReport()
        {
            //تقرير يومية الحسابات تفصيلي

            //if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
            //    return AccessDeniedView();           

            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree).ToList();
            allAccounts.Add(new Account() { Id = 0, NameAr = "الكل", NameEn = "ALL", LastLevelInTree = true });

            ViewBag.Accounts = allAccounts.Select(x => new { Id = x.Id, Name = x.AccountCode + " " + (_currentLanguage == "ar" ? x.NameAr : x.NameEn) });

            var DailyAccounts = _DailyAccounts_DefService.GetAll();
            DailyAccounts.Insert(0, new DailyAccounts_Def() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.DailyAccounts = DailyAccounts.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Documents = _DocumentService.GetAll();
            Documents.Add(new Document() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Documents = Documents.Select(x => new { Id = x.DocTypeId, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });



            DailyAccountsDetailsReportFilters filters = new DailyAccountsDetailsReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId, FromDate = CurrentUser.FinancialPeriodFromDate.Value, ToDate = CurrentUser.FinancialPeriodToDate.Value };

            if (CurrentUser != null && CurrentUser.FinancialPeriodId.HasValue)
            {
                var CurruntFinancialPeriod = _FinancialPeriodService.GetById(CurrentUser.FinancialPeriodId.Value);
                if (CurruntFinancialPeriod != null)
                {
                    filters.FromDate = CurruntFinancialPeriod.DateFrom;
                    filters.ToDate = CurruntFinancialPeriod.DateTo;

                }
            }
            return View(filters);
        }

        [HttpPost]
        public IActionResult DailyAccountsDetailsReport(DailyAccountsDetailsReportFilters filters)
        {
            //تقرير يومية الحسابات تفصيلي

            //if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
            //    return AccessDeniedView();

            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree).ToList();
            allAccounts.Add(new Account() { Id = 0, NameAr = "الكل", NameEn = "ALL", LastLevelInTree = true });

            ViewBag.Accounts = allAccounts.Select(x => new { Id = x.Id, Name = x.AccountCode + " " + (_currentLanguage == "ar" ? x.NameAr : x.NameEn) });

            var DailyAccounts = _DailyAccounts_DefService.GetAll();
            DailyAccounts.Insert(0, new DailyAccounts_Def() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.DailyAccounts = DailyAccounts.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Documents = _DocumentService.GetAll();
            Documents.Add(new Document() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Documents = Documents.Select(x => new { Id = x.DocTypeId, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });


            //will replaced with DailyAccountsDetailsReport 
            DailyAccountsDetailsReport rpt = new DailyAccountsDetailsReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.DailyAccountsDetailsReport(filters).OrderBy(x=>x.EntryNumber);

            string AccountFilter = "";
            string CoastCenterFilter = "";
            string DailyAccountsFilter = "";
            string DocumentTypeFilter = "";
           

            if (_currentLanguage == "ar")
            {
                AccountFilter = (filters.AccountId > 0) ? allAccounts.FirstOrDefault(s => s.Id == filters.AccountId).NameAr : "الكل";
                DailyAccountsFilter = (filters.DailyAccountsId > 0) ? DailyAccounts.FirstOrDefault(s => s.Id == filters.DailyAccountsId).NameAr : "الكل";
                DocumentTypeFilter = (filters.DocumentTypeId > 0) ? Documents.FirstOrDefault(s => s.Id == filters.DocumentTypeId).NameAr : "الكل";
            }
            else
            {
                AccountFilter = (filters.AccountId > 0) ? allAccounts.FirstOrDefault(s => s.Id == filters.AccountId).NameEn : "All";
                DailyAccountsFilter = (filters.DailyAccountsId > 0) ? DailyAccounts.FirstOrDefault(s => s.Id == filters.DailyAccountsId).NameEn : "All";
                DocumentTypeFilter = (filters.DocumentTypeId > 0) ? Documents.FirstOrDefault(s => s.Id == filters.DocumentTypeId).NameEn : "All";

            }
            filters.ReportName = _LocalizationService.GetLocalizedHtmlString("DailyAccountsDetailsReport").Value;

            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "AccountFilter", AccountFilter != null ? AccountFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "FromDateFilter", filters.FromDate);
            _ReportService.SetRptParameter(rpt, filters, "ToDateFilter", filters.ToDate);
            _ReportService.SetRptParameter(rpt, filters, "CoastCenterFilter", CoastCenterFilter);
            _ReportService.SetRptParameter(rpt, filters, "DailyAccountsFilter", DailyAccountsFilter);
            _ReportService.SetRptParameter(rpt, filters, "DocumentTypeFilter", DocumentTypeFilter);

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);


            return View(filters);
        }

        [HttpGet]
        public IActionResult DailyAccountsTotalReport()
        {
            //تقرير يومية الحسابات اجمالي

            //if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
            //    return AccessDeniedView();           

            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree).ToList();
            allAccounts.Add(new Account() { Id = 0, NameAr = "الكل", NameEn = "ALL", LastLevelInTree = true });

            ViewBag.Accounts = allAccounts.Select(x => new { Id = x.Id, Name = x.AccountCode + " " + (_currentLanguage == "ar" ? x.NameAr : x.NameEn) });

            var DailyAccounts = _DailyAccounts_DefService.GetAll();
            DailyAccounts.Insert(0, new DailyAccounts_Def() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.DailyAccounts = DailyAccounts.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });           

            DailyAccountsTotalReportFilters filters = new DailyAccountsTotalReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId, FromDate = CurrentUser.FinancialPeriodFromDate.Value, ToDate = CurrentUser.FinancialPeriodToDate.Value };

            if (CurrentUser != null && CurrentUser.FinancialPeriodId.HasValue)
            {
                var CurruntFinancialPeriod = _FinancialPeriodService.GetById(CurrentUser.FinancialPeriodId.Value);
                if (CurruntFinancialPeriod != null)
                {
                    filters.FromDate = CurruntFinancialPeriod.DateFrom;
                    filters.ToDate = CurruntFinancialPeriod.DateTo;

                }
            }
            return View(filters);
        }

        [HttpPost]
        public IActionResult DailyAccountsTotalReport(DailyAccountsTotalReportFilters filters)
        {
            //تقرير يومية الحسابات تفصيلي

            //if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
            //    return AccessDeniedView();

            var allAccounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree).ToList();
            allAccounts.Add(new Account() { Id = 0, NameAr = "الكل", NameEn = "ALL", LastLevelInTree = true });

            ViewBag.Accounts = allAccounts.Select(x => new { Id = x.Id, Name = x.AccountCode + " " + (_currentLanguage == "ar" ? x.NameAr : x.NameEn) });

            var DailyAccounts = _DailyAccounts_DefService.GetAll();
            DailyAccounts.Insert(0, new DailyAccounts_Def() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.DailyAccounts = DailyAccounts.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Documents = _DocumentService.GetAll();
            Documents.Add(new Document() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Documents = Documents.Select(x => new { Id = x.DocTypeId, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });


            //will replaced with DailyAccountsDetailsReport 
            DailyAccountsTotalReport rpt = new DailyAccountsTotalReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.DailyAccountsTotalReport(filters).OrderBy(x => x.EntryNumber);

            string AccountFilter = "";
            string CoastCenterFilter = "";
            string DailyAccountsFilter = "";
            if (_currentLanguage == "ar")
            {
                AccountFilter = (filters.AccountId > 0) ? allAccounts.FirstOrDefault(s => s.Id == filters.AccountId).NameAr : "الكل";
                DailyAccountsFilter = (filters.DailyAccountsId > 0) ? DailyAccounts.FirstOrDefault(s => s.Id == filters.DailyAccountsId).NameAr : "الكل";
            }
            else
            {
                AccountFilter = (filters.AccountId > 0) ? allAccounts.FirstOrDefault(s => s.Id == filters.AccountId).NameEn : "All";
                DailyAccountsFilter = (filters.DailyAccountsId > 0) ? DailyAccounts.FirstOrDefault(s => s.Id == filters.DailyAccountsId).NameEn : "All";

            }
            filters.ReportName = _LocalizationService.GetLocalizedHtmlString("DailyAccountsTotalReport").Value;

            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "AccountFilter", AccountFilter != null ? AccountFilter : "");
            _ReportService.SetRptParameter(rpt, filters, "FromDateFilter", filters.FromDate);
            _ReportService.SetRptParameter(rpt, filters, "ToDateFilter", filters.ToDate);
            _ReportService.SetRptParameter(rpt, filters, "CoastCenterFilter", CoastCenterFilter);
            _ReportService.SetRptParameter(rpt, filters, "DailyAccountsFilter", DailyAccountsFilter);

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);


            return View(filters);
        }

        [HttpGet]
        public IActionResult BankStatementOfAccountReport()
        {
            //تقرير كشف حساب بنك

            //if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
            //    return AccessDeniedView();     
           
            var bancks = _BankService.GetAll();
            ViewBag.bancks = bancks.Select(x => new { Id = x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });            

            var Documents = _DocumentService.GetAll();
            Documents.Add(new Document() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Documents = Documents.Select(x => new { Id = x.DocTypeId, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn }).OrderBy(x=>x.Id);

            BankStatementOfAccountReportFilters filters = new BankStatementOfAccountReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId, FromDate = CurrentUser.FinancialPeriodFromDate.Value, ToDate = CurrentUser.FinancialPeriodToDate.Value };

            if (CurrentUser != null && CurrentUser.FinancialPeriodId.HasValue)
            {
                var CurruntFinancialPeriod = _FinancialPeriodService.GetById(CurrentUser.FinancialPeriodId.Value);
                if (CurruntFinancialPeriod != null)
                {
                    filters.FromDate = CurruntFinancialPeriod.DateFrom;
                    filters.ToDate = CurruntFinancialPeriod.DateTo;

                }
            }
            return View(filters);
        }

        [HttpPost]
        public IActionResult BankStatementOfAccountReport(BankStatementOfAccountReportFilters filters)
        {
            //تقرير كشف حساب بنك

            //if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
            //    return AccessDeniedView();    
           
            var bancks = _BankService.GetAll();
            ViewBag.bancks = bancks.Select(x => new { Id = x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Documents = _DocumentService.GetAll();
            Documents.Add(new Document() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Documents = Documents.Select(x => new { Id = x.DocTypeId, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn }).OrderBy(x => x.Id);

            //will replaced with DailyAccountsDetailsReport 
            BankStatementOfAccountReport rpt = new BankStatementOfAccountReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.BankStatementOfAccountReport(filters).OrderBy(x => x.EntryNumber).ToList();

            string BanckFilter = "";
            string DocumentFilter = "";
            if (_currentLanguage == "ar")
            {
                BanckFilter = (filters.BanckId > 0) ? bancks.FirstOrDefault(s => s.Id == filters.BanckId).NameAr : "الكل";
                DocumentFilter = (filters.DocTypeId > 0) ? Documents.FirstOrDefault(s => s.Id == filters.DocTypeId).NameAr : "الكل";
            }
            else
            {
                BanckFilter = (filters.BanckId > 0) ? bancks.FirstOrDefault(s => s.Id == filters.BanckId).NameEn : "All";
                DocumentFilter = (filters.DocTypeId > 0) ? Documents.FirstOrDefault(s => s.Id == filters.DocTypeId).NameEn : "All";

            }
            filters.ReportName = _LocalizationService.GetLocalizedHtmlString("BankStatementOfAccountReport").Value;

            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "BanckFilter", BanckFilter);
            _ReportService.SetRptParameter(rpt, filters, "DocumentFilter", DocumentFilter);
            _ReportService.SetRptParameter(rpt, filters, "FromDateFilter", filters.FromDate);
            _ReportService.SetRptParameter(rpt, filters, "ToDateFilter", filters.ToDate);

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);


            return View(filters);
        }

        [HttpGet]
        public IActionResult TreasuryStatementOfAccountReport()
        {
            //تقرير كشف حساب خزينة

            //if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
            //    return AccessDeniedView();     

            var Treasurys = _TreasuryService.GetAll();
            ViewBag.Treasurys = Treasurys.Select(x => new { Id = x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Documents = _DocumentService.GetAll();
            Documents.Add(new Document() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Documents = Documents.Select(x => new { Id = x.DocTypeId, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn }).OrderBy(x => x.Id);

            TreasuryStatementOfAccountReportFilters filters = new TreasuryStatementOfAccountReportFilters() { FinancialPeriodId = CurrentUser.FinancialPeriodId, CompanyId = CurrentUser.CompanyId, FromDate = CurrentUser.FinancialPeriodFromDate.Value, ToDate = CurrentUser.FinancialPeriodToDate.Value };

            if (CurrentUser != null && CurrentUser.FinancialPeriodId.HasValue)
            {
                var CurruntFinancialPeriod = _FinancialPeriodService.GetById(CurrentUser.FinancialPeriodId.Value);
                if (CurruntFinancialPeriod != null)
                {
                    filters.FromDate = CurruntFinancialPeriod.DateFrom;
                    filters.ToDate = CurruntFinancialPeriod.DateTo;

                }
            }
            return View(filters);
        }

        [HttpPost]
        public IActionResult TreasuryStatementOfAccountReport(TreasuryStatementOfAccountReportFilters filters)
        {
            //تقرير كشف حساب خزينة

            //if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomerBalanceTotalReport.SystemName, CurrentUser, PermissionActions.Report))
            //    return AccessDeniedView();    

            var Treasurys = _TreasuryService.GetAll();
            ViewBag.Treasurys = Treasurys.Select(x => new { Id = x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Documents = _DocumentService.GetAll();
            Documents.Add(new Document() { Id = 0, NameAr = "الكل", NameEn = "All" });
            ViewBag.Documents = Documents.Select(x => new { Id = x.DocTypeId, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn }).OrderBy(x => x.Id);

            //will replaced with DailyAccountsDetailsReport 
            TreasuryStatementOfAccountReport rpt = new TreasuryStatementOfAccountReport(_LocalizationService);
            //get report data source 
            var DataSource = _ReportService.TreasuryStatementOfAccountReport(filters).OrderBy(x => x.EntryNumber).ToList();

            string TreasuryFilter = "";
            string DocumentFilter = "";
            if (_currentLanguage == "ar")
            {
                TreasuryFilter = (filters.TreasuryId > 0) ? Treasurys.FirstOrDefault(s => s.Id == filters.TreasuryId).NameAr : "الكل";
                DocumentFilter = (filters.DocTypeId > 0) ? Documents.FirstOrDefault(s => s.Id == filters.DocTypeId).NameAr : "الكل";
            }
            else
            {
                TreasuryFilter = (filters.TreasuryId > 0) ? Treasurys.FirstOrDefault(s => s.Id == filters.TreasuryId).NameEn : "All";
                DocumentFilter = (filters.DocTypeId > 0) ? Documents.FirstOrDefault(s => s.Id == filters.DocTypeId).NameEn : "All";

            }
            filters.ReportName = _LocalizationService.GetLocalizedHtmlString("TreasuryStatementOfAccountReport").Value;

            //set report parameters
            _ReportService.SetRptParameter(rpt, filters, "TreasuryFilter", TreasuryFilter);
            _ReportService.SetRptParameter(rpt, filters, "DocumentFilter", DocumentFilter);
            _ReportService.SetRptParameter(rpt, filters, "FromDateFilter", filters.FromDate);
            _ReportService.SetRptParameter(rpt, filters, "ToDateFilter", filters.ToDate);

            //set report data source
            _ReportService.SetReportResult(rpt, DataSource, filters);


            return View(filters);
        }


        #endregion















    }
}
