using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models.Financial;
using BusinessLayer.Models.Inventory;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.System;
using Grand.Framework.Kendoui;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Controllers;
using PresentationLayer.Helpers;
using Reports;
using Reports.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static BusinessLayer.Helpers.SharedEnums;

namespace SmartErpApp.Controllers.Inventory
{
    [Authorize]
    //[ValidateAntiForgeryToken]
    public class OpenBalanceController : BaseAdminController
    {
        private readonly IInventoryService<Transaction_InvMaster, Transaction_InvDetails> _InventoryMasterService;
        private readonly IBaseService<Transaction_InvDetails> _InventoryDetailsService;
        private readonly IBaseService<Item> _ItemService;
        //private readonly IBaseService<ItemOpenBalance> _ItemOpenBalanceService;
        private readonly IBaseService<Branch> _BranchService;
        private readonly IBaseService<Store> _StoreService;
        private readonly IBaseService<ItemGroup> _ItemGroupService;
        private readonly IBaseService<Unit> _UnitService;        
        private readonly IBaseService<SystemSetting> _SystemSettingService;
        private readonly IBaseService<TransactionsEntrySettingMaster> _TransactionsEntrySettingService;
        private readonly IBaseService<DefaultAccount> _DefaultAccountService;
        private readonly IFinanceService<DailyEntryMaster, DailyEntryDetails> _DailyEntryService;
        private readonly IBaseService<Currency> _CurrencyService;
        private readonly IWebHelper _WebHelperService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;       
        private readonly IidentityService _identityService;
        private readonly IReportService _ReportService;
        private readonly string _currentLanguage;


        public OpenBalanceController(
             IBaseService<Item> ItemService,
             //IBaseService<ItemOpenBalance> ItemOpenBalanceService,
             IBaseService<ItemGroup> ItemGroupService,             
             LocalizationService localizationService,
             IBaseService<Unit> UnitService,
             IBaseService<Branch> BranchService,
             IBaseService<Store> StoreService,
             IInventoryService<Transaction_InvMaster,Transaction_InvDetails> InventoryMasterService,             
             IBaseService<Transaction_InvDetails> InventoryDetailsService,
             IBaseService<SystemSetting> SystemSettingService,
             IBaseService<TransactionsEntrySettingMaster> TransactionsEntrySettingService,
             IBaseService<DefaultAccount> DefaultAccountService,
             IFinanceService<DailyEntryMaster, DailyEntryDetails> DailyEntryService,
             IBaseService<Currency> CurrencyService,
             IidentityService IdentityService,
             IReportService ReportService,
             IMapper mapper,
             IWebHelper WebHelperService


            )
        {

            _ItemService = ItemService;
            //_ItemOpenBalanceService = ItemOpenBalanceService;
            _ItemGroupService = ItemGroupService;
            _UnitService = UnitService;
            _BranchService = BranchService;
            _StoreService = StoreService;
            _InventoryMasterService = InventoryMasterService;
            _identityService = IdentityService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _InventoryDetailsService = InventoryDetailsService;
            _ReportService = ReportService;
            _SystemSettingService = SystemSettingService;
            _TransactionsEntrySettingService = TransactionsEntrySettingService;
            _DefaultAccountService = DefaultAccountService;
            _DailyEntryService = DailyEntryService;
            _CurrencyService = CurrencyService;
            _WebHelperService = WebHelperService;

        }

        public void IntializeDropdowens()
        {
            var Groups = _ItemGroupService.GetAll();
            Groups.Insert(0, new ItemGroup() { Id = 0, NameAr = "", NameEn = "" });
            ViewBag.Groups = Groups.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Units = _UnitService.GetAll();
            Units.Insert(0, new Unit() { Id = 0, NameAr = "", NameEn = "" });
            ViewBag.Units = Units.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            ViewBag.Branches = _BranchService.GetAll().Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });
            ViewBag.Stores = _StoreService.GetAll().Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Items = _ItemService.GetWithCondetion(x=>x.FinancialPeriodId!=null&& x.ActivationState.Value && x.FinancialPeriodId<=CurrentUser.FinancialPeriodId);
            Items.Insert(0, new Item() { Id = 0, NameAr = "اختيار صنف", NameEn = "select" });
            ViewBag.Itmes = Items.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

           
        }
        public void SetBasicButtonsVisibilityForCreate()
        {

            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "OpenBalance", CreateNewBtnAction = "Create", BackToListControler = "OpenBalance", BackToListAction = "Index", EditBtnControler = "OpenBalance", EditBtnAction = "Edit", DeleteBtnControler = "OpenBalance", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false, PrintBtnVisibilty = false };

        }
        public void SetBasicButtonsVisibilityForEdit(int RouteId)
        {
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "OpenBalance", CreateNewBtnAction = "Create", BackToListControler = "OpenBalance", BackToListAction = "Index", EditBtnControler = "OpenBalance", EditBtnAction = "Edit", DeleteBtnControler = "OpenBalance", DeleteBtnAction = "Delete", RouteId = RouteId, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true, PrintBtnVisibilty = true, PrintBtnControler = "OpenBalance", PrintBtnAction = "Print" };
        }
        public void ValidateModel(Transaction_InvMasterModel model)
        {
            if (model != null && model.Transaction_InvDetails != null)
            {
                if (model.BranchId == null || model.BranchId <= 0)
                {
                    ModelState.AddModelError("BranchId", "يجب ادخال جميع الحقول المطلوبه");
                }
                if (model.StoreId == null || model.StoreId <= 0)
                {
                    ModelState.AddModelError("StoreId", "يجب اختيار مخزن");
                }
                if (CurrentUser.CompanyId == null || CurrentUser.CompanyId <= 0)
                {
                    ModelState.AddModelError("CompanyId", "يجب اختيار شركة للمستخدم الحالي");
                }
                if (CurrentUser.FinancialPeriod == null || CurrentUser.FinancialPeriod <= 0)
                {
                    ModelState.AddModelError("FinancialPeriod", "يجب اختيار سنة مالية للمستخدم الحالي");
                }
                if (model.Transaction_InvDetails.Count > 1)
                {
                   var oldOpenBalanceMaster  = _InventoryMasterService.GetWithCondetion(x =>x.Code!=model.Code&& x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.DocTypeId == (int)DocumentTypes.OpenBalance && x.CompanyId == CurrentUser.CompanyId && x.BranchId == model.BranchId && x.StoreId == model.StoreId).Select(x=>x.Id).ToList();
                   var oldDetails = _InventoryDetailsService.GetWithCondetion(x => oldOpenBalanceMaster.Contains(x.MasterId)).ToList();
                    var items = _ItemService.GetAll();
                    foreach (var item in model.Transaction_InvDetails.Skip(1))
                    {
                        item.FinancialPeriodId = CurrentUser.FinancialPeriodId;
                        item.CompanyId = CurrentUser.CompanyId;
                        item.DocTypeId = (int)DocumentTypes.OpenBalance;
                        item.DocDate = model.DocDate;
                        if (item.ItemId <= 0)
                        {
                            ModelState.AddModelError("ItemId", "يجب اختيار صنف");
                        }
                        if (item.Quntity <= 0)
                        {
                            ModelState.AddModelError("Quntity", "يجب ادخال الكمية");
                        }
                        if (oldDetails.Where(x=>x.ItemId== item.ItemId).FirstOrDefault() !=null)
                        {
                            var itemData =_Mapper.Map<ItemModel>(items.Where(x => x.Id == item.ItemId).FirstOrDefault()) ;
                            ModelState.AddModelError("ItemHasOpenBalanceBefore", $"الصنف {itemData.Name}  مسجل له حركة رصيد إفتتاحي لنفس الفرع والمخزن من قبل");
                        }

                    }
                }
                else
                {
                    ModelState.AddModelError("EMPTY_ITEMS", "يجب إضافة صنف علي الأقل");
                }

            }
            else
            {
                ModelState.AddModelError("NULL_MODEL", "يجب ادخال جميع الحقول المطلوبه");
            }

        }

        // GET: ItemController
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageOpenBalance.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            
            return View(new Transaction_InvMasterModel());
        }

        public IActionResult list()
        {
            var TrMaster = _InventoryMasterService.GetInventoryTransactions(x=>x.FinancialPeriodId== CurrentUser.FinancialPeriodId&& x.DocTypeId == (int)DocumentTypes.OpenBalance && x.CompanyId == CurrentUser.CompanyId);
            var TrMasterModel = _Mapper.Map<List<Transaction_InvMasterModel>>(TrMaster);
            var Branches = _BranchService.GetAll();
            var Stores = _StoreService.GetAll();
            foreach (var item in TrMasterModel)
            {
                item.BranchName = Branches.Where(x => x.Id == item.BranchId)?.Select(x => x.NameAr).FirstOrDefault();
                item.StoreName = Stores.Where(x => x.Id == item.StoreId)?.Select(x => x.NameAr).FirstOrDefault();
            }
            var gridModel = new DataSourceResult
            {
                Data = TrMasterModel,
                Total = TrMasterModel.Count
            };
            return Json(gridModel);
        }


        [HttpGet]
        //[TypeFilter(typeof(ValidateFinancePeriodStateFilter))]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageOpenBalance.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView(); 
            
            var LastCode = _InventoryMasterService.GetLastCode(o => o.Code ,x=> x.FinancialPeriodId==CurrentUser.FinancialPeriodId && x.DocTypeId == (int)DocumentTypes.OpenBalance && x.CompanyId == CurrentUser.CompanyId);
            var NewMasterModel = new Transaction_InvMasterModel() { Code = LastCode, DocDate = DateTime.Now};

            //set basic buttons visibility
            SetBasicButtonsVisibilityForCreate();

            IntializeDropdowens();

            return View(NewMasterModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Transaction_InvMasterModel TransactionModel)
        {
            Transaction_InvMaster master = null;
            try
            {
                var currentUser = CurrentUser;
                if (!_identityService.Authorize(StandardPermissionProvider.ManageOpenBalance.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();

                var ValidateFinancePeriodMsg = _WebHelperService.ValidateFinancePeriod(CurrentUser.FinancialPeriodId.Value, TransactionModel.DocDate);
                if (ValidateFinancePeriodMsg!="")
                {
                    ViewData["NotificationMsg"] = Notification.Erorr(ValidateFinancePeriodMsg, NotificationCssType.danger.ToString());

                    SetBasicButtonsVisibilityForCreate();
                    IntializeDropdowens();
                    return View(TransactionModel);
                }

                ValidateModel(TransactionModel);
                if (ModelState.IsValid)
                {
                    TransactionEntrySetting transactionSettings = new TransactionEntrySetting();
                    //التأكد من الحسابات الافتراضية ومواصفات حركة الرصيد الافتتاحي وإذا كان النظام يسمح بانشاء قيود الحركات
                    var msg = ValidateEntrySettings(transactionSettings);
                    if (msg.Length > 0)
                    {
                        ViewData["NotificationMsg"] = Notification.Success(msg, NotificationCssType.danger.ToString());

                        SetBasicButtonsVisibilityForCreate();
                        IntializeDropdowens();
                        return View(TransactionModel);
                    }

                    if (TransactionModel.Transaction_InvDetails != null && TransactionModel.Transaction_InvDetails.Count > 0)
                    {
                        if (!_InventoryMasterService.IsExistRecord(b => b.Code == TransactionModel.Code && b.FinancialPeriodId == currentUser.FinancialPeriodId && b.DocTypeId == (int)DocumentTypes.OpenBalance && b.CompanyId == CurrentUser.CompanyId))
                        {
                            master = _Mapper.Map<Transaction_InvMaster>(TransactionModel);
                            master.Transaction_InvDetails = master.Transaction_InvDetails.Skip(1).ToList();
                            master.SetBasicData(CRUD_OperationType.Create, (int)DocumentTypes.OpenBalance, currentUser);
                            master.TransactionType = (int)TransactionTypes.CurruntTransaction;

                            var Currency = _CurrencyService.GetWithCondetion(x => x.DefaultCurrency).FirstOrDefault();
                            master.CurrencyId = Currency != null ? Currency.Id : 0;
                            master.CurrencyFactor = Currency != null ? Currency.CurrencyChangrRate : 1;                           

                            if (transactionSettings.EnableEntryCreation)
                            {
                                //add invoice entry
                                int EntryNumber = 0;
                                int EntryId = 0;
                                decimal TotalCoast = master.Transaction_InvDetails.Sum(s => s.Quntity * s.PurchasePrice.Value);
                                if (TotalCoast > 0)
                                {
                                    AddEntry(transactionSettings, master.Code, master.CurrencyId, master.CurrencyFactor, TotalCoast, master.TaxValue, out EntryNumber, out EntryId);

                                    master.EntryNumber = EntryNumber;
                                    master.EntryId = EntryId;
                                }
                            }                                

                            _InventoryMasterService.AddInventoryTransaction(master);

                            ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("SavedSuccessfuly"), NotificationCssType.success.ToString());
                        }
                        else
                        {
                            ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("DuplicateCode"), NotificationCssType.danger.ToString());
                        }
                    }
                    else
                    {
                        TransactionModel.Transaction_InvDetails = new List<Transaction_InvDetailsModel>();
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("NotItemsSelected"), NotificationCssType.danger.ToString());

                    }









                    //سوف يتم انشاء حركة رصيد افتتاحي واحدة لكل فرع ومخزن علي مستوي السنة المالية لذلك سوف يتم تعديل الاصناف القديمة في الحركة واضافة الاصناف الجديدة لنفس الحركة
                    //if (TransactionModel.Transaction_InvDetails != null && TransactionModel.Transaction_InvDetails.Count > 0)
                    //{                        
                    //    if (!_InventoryMasterService.IsExistRecord(b => b.Code == TransactionModel.Code && b.FinancialPeriodId == currentUser.FinancialPeriodId && b.DocTypeId == (int)DocumentTypes.OpenBalance && b.CompanyId == CurrentUser.CompanyId))
                    //    {
                    //        var oldOpenBalanceMaster = master = _InventoryMasterService.GetWithCondetion(x => x.FinancialPeriodId == currentUser.FinancialPeriodId && x.DocTypeId == (int)DocumentTypes.OpenBalance && x.CompanyId == CurrentUser.CompanyId && x.BranchId == TransactionModel.BranchId && x.StoreId == TransactionModel.StoreId).FirstOrDefault();
                    //        //var oldOpenBalance = _ItemOpenBalanceService.GetWithCondetion(x=>x.FinancialPeriodId== currentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId);
                    //        if (oldOpenBalanceMaster != null)
                    //        {
                    //            decimal TotalCoast = 0;
                    //            foreach (var item in TransactionModel.Transaction_InvDetails.Skip(1))
                    //            {
                    //                var oldDetails = _InventoryDetailsService.GetWithCondetion(x => x.MasterId == oldOpenBalanceMaster.Id && x.ItemId == item.ItemId && x.FinancialPeriodId == currentUser.FinancialPeriodId && x.DocTypeId == (int)DocumentTypes.OpenBalance && x.CompanyId == CurrentUser.CompanyId).FirstOrDefault();
                    //                if (oldDetails != null)
                    //                {
                    //                    oldDetails.Quntity = item.Quntity.Value;
                    //                    _InventoryDetailsService.Update(oldDetails);
                    //                    TotalCoast += item.Quntity.Value * item.PurchasePrice.Value;

                    //                }
                    //                else
                    //                {
                    //                    var newItem = _Mapper.Map<Transaction_InvDetails>(item);
                    //                    newItem.MasterId = oldOpenBalanceMaster.Id;
                    //                    _InventoryDetailsService.Add(newItem);
                    //                    TotalCoast += newItem.Quntity * newItem.PurchasePrice.Value;

                    //                }

                    //                //var ItemOldOpenBalance = oldOpenBalance.Where(x => x.ItemId == item.ItemId && x.BranchId == master.BranchId.Value && x.StoreId == master.StoreId.Value).FirstOrDefault();
                    //                //if (ItemOldOpenBalance != null)
                    //                //{
                    //                //     ItemOldOpenBalance.CurruntBalance = item.Quntity.Value;
                    //                //    _ItemOpenBalanceService.Update(ItemOldOpenBalance);
                    //                //}
                    //                //else
                    //                //{
                    //                //    ItemOpenBalance OpenBalance = new ItemOpenBalance()
                    //                //    {
                    //                //        ItemId = item.ItemId,
                    //                //        BranchId = master.BranchId.Value,
                    //                //        StoreId = master.StoreId.Value,
                    //                //        FinancialPeriodId = master.FinancialPeriodId.Value,
                    //                //        CompanyId = master.CompanyId.Value,
                    //                //        CurruntBalance = item.Quntity.Value,

                    //                //    };

                    //                //    _ItemOpenBalanceService.Add(OpenBalance);
                    //                //}


                    //            }

                    //            if (TotalCoast>0)
                    //            {
                    //                //تعديل قيد الحركة اذا كان موجود
                    //                if (master.EntryId > 0 && master.EntryNumber > 0)
                    //                {
                    //                    decimal updatedDetailsCoast = _InventoryDetailsService.GetWithCondetion(x=>x.MasterId==oldOpenBalanceMaster.Id).Sum(s=>s.Quntity * s.PurchasePrice.Value);

                    //                    UpdateEntry(transactionSettings, master.Code, master.CurrencyId, master.CurrencyFactor, updatedDetailsCoast, master.TaxValue, master.EntryId);
                    //                }
                    //                else
                    //                {
                    //                    decimal updatedDetailsCoast = _InventoryDetailsService.GetWithCondetion(x => x.MasterId == oldOpenBalanceMaster.Id).Sum(s => s.Quntity * s.PurchasePrice.Value);
                    //                    int EntryNumber = 0;
                    //                    int EntryId = 0;
                    //                    //اضافة قيد للحركة اذا كان غير موجود
                    //                    AddEntry(transactionSettings, master.Code, master.CurrencyId, master.CurrencyFactor, updatedDetailsCoast,  master.TaxValue, out EntryNumber, out EntryId);

                    //                    master.EntryNumber = EntryNumber;
                    //                    master.EntryId = EntryId;
                    //                }
                    //            }                 

                    //        }
                    //        else
                    //        {
                    //            master = _Mapper.Map<Transaction_InvMaster>(TransactionModel);
                    //            master.Transaction_InvDetails = master.Transaction_InvDetails.Skip(1).ToList();
                    //            master.SetBasicData(CRUD_OperationType.Create, (int)DocumentTypes.OpenBalance, currentUser);

                    //            var Currency = _CurrencyService.GetWithCondetion(x=>x.DefaultCurrency).FirstOrDefault();
                    //            master.CurrencyId = Currency!=null? Currency.Id:0;
                    //            master.CurrencyFactor = Currency!=null? Currency.CurrencyChangrRate:1;
                    //            int EntryNumber = 0;
                    //            int EntryId = 0;
                    //            decimal TotalCoast = master.Transaction_InvDetails.Sum(s=>s.Quntity*s.PurchasePrice.Value);
                    //            //add invoice entry
                    //            if (TotalCoast>0)
                    //            {
                    //                AddEntry(transactionSettings, master.Code, master.CurrencyId, master.CurrencyFactor, TotalCoast, master.TaxValue, out EntryNumber, out EntryId);

                    //                master.EntryNumber = EntryNumber;
                    //                master.EntryId = EntryId;
                    //            }                     

                    //            _InventoryMasterService.AddInventoryTransaction(master);

                    //        }

                    //        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("SavedSuccessfuly"), NotificationCssType.success.ToString());
                    //    }
                    //    else
                    //    {
                    //        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("DuplicateCode"), NotificationCssType.danger.ToString());
                    //    }
                    //}
                    //else
                    //{
                    //    TransactionModel.Transaction_InvDetails = new List<Transaction_InvDetailsModel>();
                    //    ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("NotItemsSelected"), NotificationCssType.danger.ToString());

                    //}

                }
                else
                {
                    ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("NotSavedSuccessfuly"), NotificationCssType.danger.ToString());
                }
            }
            catch (Exception ex)
            {
                ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("ErorrDuringSaving") + ex.InnerException, NotificationCssType.danger.ToString());
            }

            if (master != null && master.Id > 0)
            {
                return RedirectToAction("Edit", new { Id = master.Id, IsNew = true });
            }
            else
            {
                SetBasicButtonsVisibilityForCreate();
                IntializeDropdowens();
                return View(TransactionModel);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id,bool IsNew)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageOpenBalance.SystemName, CurrentUser, PermissionActions.Edit))
                return AccessDeniedView();
            Transaction_InvMaster master = _InventoryMasterService.GetById(Id);
            List<Transaction_InvDetails> details = _InventoryMasterService.GetInventoryTransactionDetails(x => x.MasterId == master.Id);
            master.Transaction_InvDetails = details;
            master.Transaction_InvDetails.Insert(0, new Transaction_InvDetails());

            Transaction_InvMasterModel masterModel = _Mapper.Map<Transaction_InvMasterModel>(master);
            //to show save message if the request came from create action
            if (IsNew)
            {
                ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("SavedSuccessfuly"), NotificationCssType.success.ToString());
            }
            //set basic buttons visibility
            SetBasicButtonsVisibilityForEdit(Id);
            IntializeDropdowens();
            return View(masterModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[TypeFilter(typeof(ValidateFinancePeriodStateFilter))]
        public IActionResult Edit(Transaction_InvMasterModel TransactionModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageOpenBalance.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();

                var ValidateFinancePeriodMsg = _WebHelperService.ValidateFinancePeriod(CurrentUser.FinancialPeriodId.Value, TransactionModel.DocDate);
                if (ValidateFinancePeriodMsg != "")
                {
                    ViewData["NotificationMsg"] = Notification.Erorr(_LocalizationService.GetLocalizedHtmlString("CuruntPeriodIsClosedFinal"), NotificationCssType.danger.ToString());

                    SetBasicButtonsVisibilityForEdit(TransactionModel.Id);
                    IntializeDropdowens();
                    return View(TransactionModel);
                }

                ValidateModel(TransactionModel);
                if (ModelState.IsValid)
                {
                    TransactionEntrySetting transactionSettings = new TransactionEntrySetting();
                    //التأكد من الحسابات الافتراضية ومواصفات حركة الرصيد الافتتاحي إذا كان النظام يسمح بانشاء قيود الحركات
                    var msg = ValidateEntrySettings(transactionSettings);

                    if (msg.Length > 0)
                    {
                        ViewData["NotificationMsg"] = Notification.Success(msg, NotificationCssType.danger.ToString());

                        SetBasicButtonsVisibilityForEdit(TransactionModel.Id);
                        IntializeDropdowens();
                        return View(TransactionModel);
                    }

                    if (TransactionModel.Transaction_InvDetails != null && TransactionModel.Transaction_InvDetails.Count > 0)
                    {
                        Transaction_InvMaster master = _Mapper.Map<Transaction_InvMaster>(TransactionModel);
                        master.Transaction_InvDetails = master.Transaction_InvDetails.Skip(1).ToList();
                        //get old details to be deleted 
                        List<Transaction_InvDetails> oldDetails = _InventoryMasterService.GetInventoryTransactionDetails(x => x.MasterId == master.Id);
                        master.SetBasicData(CRUD_OperationType.Update, (int)DocumentTypes.OpenBalance, CurrentUser);

                        var Currency = _CurrencyService.GetWithCondetion(x => x.DefaultCurrency).FirstOrDefault();
                        master.CurrencyId = Currency != null ? Currency.Id : 0;
                        master.CurrencyFactor = Currency != null ? Currency.CurrencyChangrRate : 1;
                        master.TransactionType = (int)TransactionTypes.CurruntTransaction;                        

                        if (transactionSettings.EnableEntryCreation)
                        {
                            decimal TotalCoast = master.Transaction_InvDetails.Sum(s => s.Quntity * s.PurchasePrice.Value);
                            if (TotalCoast > 0)
                            {
                                //تعديل قيد الحركة اذا كان موجود
                                if (master.EntryId > 0 && master.EntryNumber > 0)
                                {
                                    UpdateEntry(transactionSettings, master.Code, master.CurrencyId, master.CurrencyFactor, TotalCoast, master.TaxValue, master.EntryId);
                                }
                                else
                                {
                                    int EntryNumber = 0;
                                    int EntryId = 0;
                                    //اضافة قيد للحركة اذا كان غير موجود
                                    AddEntry(transactionSettings, master.Code, master.CurrencyId, master.CurrencyFactor, TotalCoast, master.TaxValue, out EntryNumber, out EntryId);

                                    master.EntryNumber = EntryNumber;
                                    master.EntryId = EntryId;
                                }
                            }
                        
                        }
                       

                        _InventoryMasterService.UpdateInventoryTransaction(master, oldDetails);
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("UpdatedSuccessfuly"), NotificationCssType.success.ToString());

                    }
                    else
                    {
                        TransactionModel.Transaction_InvDetails = new List<Transaction_InvDetailsModel>();
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("NotItemsSelected"), NotificationCssType.danger.ToString());

                    }
                }
                else
                {
                    ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("NotUpdatedSuccessfuly"), NotificationCssType.danger.ToString());
                }
            }
            catch (Exception ex)
            {
                ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("ErorrDuringUpdating") + ex.InnerException, NotificationCssType.danger.ToString());
            }

            //set basic buttons visibility
            SetBasicButtonsVisibilityForEdit(TransactionModel.Id);
            IntializeDropdowens();

            return View(TransactionModel);
        }

        //[TypeFilter(typeof(ValidateFinancePeriodStateFilter))]
        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageOpenBalance.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });
                Transaction_InvMaster master = _InventoryMasterService.GetById(Id);

                var ValidateFinancePeriodMsg = _WebHelperService.ValidateFinancePeriod(CurrentUser.FinancialPeriodId.Value, master.DocDate);
                if (ValidateFinancePeriodMsg != "")
                {               
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("CuruntPeriodIsClosedFinal").Value });
                }

                _InventoryMasterService.Delete(master);

                var EntryMaster = _DailyEntryService.GetById(master.EntryId);
                if (EntryMaster != null)
                {
                    _DailyEntryService.Delete(EntryMaster);
                }

                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }
        } 
        
        public IActionResult GetBranchStores(int BranchId)
        {
            try
            {
                var stores =_StoreService.GetAll().Where(x=>x.BranchId==BranchId).Select(x=>new {Name= _currentLanguage == "ar" ? x.NameAr : x.NameEn,Id=x.Id });               
                return Json(stores);
            }
            catch (Exception)
            {
                return Json("erorr");
            }
        }

        public IActionResult Print(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageOpenBalance.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            if (Id > 0)
            {
                //ReportsHelper.InitializeHostingEnvironment(_Hosting);
                OpenBalanceRptPrint report = new OpenBalanceRptPrint(_LocalizationService);
                var DataSource = _ReportService.OpenBalancePrint(Id);
                report.DataSource = DataSource;
                var stream = report.GenerateReport("pdf");
                stream.Position = 0;
                return File(stream, "application/pdf", "OpenBalanceRpt.pdf");
            }
            else
            {
                return View("Error");
            }

        }

        /// <summary>
        /// التأكد من الحسابات الافتراضية ومواصفات حركة الرصيد الافتتاحي إذا كان النظام يسمح بانشاء قيود الحركات
        /// </summary>
        /// <param name="transactionSettings"></param>
        /// <param name="supper"></param>
        /// <returns></returns>
        public string ValidateEntrySettings(TransactionEntrySetting transactionSettings)
        {
            StringBuilder msg = new StringBuilder();
            var SystemSetting = _SystemSettingService.GetAll().FirstOrDefault();
            if (SystemSetting != null && SystemSetting.EnableTransactionsEntryCreation)
            {
                transactionSettings.EnableEntryCreation = true;
                var transSettings = _TransactionsEntrySettingService.GetWithCondetion(x => x.DocTypeId == (int)DocumentTypes.OpenBalance).FirstOrDefault();

                if (transSettings == null)
                {
                    msg.AppendLine("يجب تحديد مواصفات الحركة <br>");
                }
                else
                {
                    transactionSettings.DailyTypeId = transSettings.DailyTypeId;
                    transactionSettings.TransferState = transSettings.TransferState;
                    transactionSettings.FirstSideNaturalId = transSettings.FirstSideSideNaturalId;
                    transactionSettings.SecondSideNaturalId = transSettings.SecondSideSideNaturalId;
                    transactionSettings.IsFirstSideTaxble = transSettings.IsFirstSideTaxble;
                    transactionSettings.IsSecondSideTaxble = transSettings.IsSecondSideTaxble;

                }

                var defaultAccounts = _DefaultAccountService.GetAll();
                if (defaultAccounts == null || defaultAccounts.Count == 0)
                {
                    msg.AppendLine("يجب انشاء الحسابات الافتراضية <br>");
                }
                else
                {
                    //الطرف الاول حساب مخزون اول المدة
                    var FSAccount = defaultAccounts.Where(a => a.AccountNameId == 13).FirstOrDefault();
                    if (FSAccount == null || FSAccount.AccountId <= 0)
                    {
                        msg.AppendLine("يجب تحديد حساب افتراضي لحساب مخزون أول المدة <br>");
                    }
                    else
                    {
                        transactionSettings.FirstSideAccountId = FSAccount.AccountId;
                    }

                    //الطرف الثاني حساب راس المال
                     var SSAccount = defaultAccounts.Where(a => a.AccountNameId == 4).FirstOrDefault();
                    if (SSAccount == null || SSAccount.AccountId <= 0)
                    {
                        msg.AppendLine("يجب تحديد حساب افتراضي لحساب رأس المال <br>");
                    }
                    else
                    {
                        transactionSettings.SecondSideAccountId = SSAccount.AccountId;
                    }
                }                
            }

            return msg.ToString();
        }

        public void AddEntry(TransactionEntrySetting transactionSettings, int DocNumber, int CuruncyId, decimal CuruncyFactor, decimal Amount, decimal VAT_TaxAmount, out int EntryNumber, out int EntryId)
        {
            EntryNumber = 0;
            EntryId = 0;
            if (transactionSettings != null)
            {
                var LastEntryNumber = _DailyEntryService.GetLastEntryNumber(o => o.EntryNumber, x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId);

                #region Entry Master

                var EntryMaster = new DailyEntryMaster();
                EntryMaster.EntryNumber = LastEntryNumber;
                EntryMaster.TransactionDate = DateTime.Now;
                EntryMaster.DocNumber = DocNumber;
                EntryMaster.Code = DocNumber;
                EntryMaster.EntryCreationMethod = (int)EntryCreationMethod.Automatic;
                EntryMaster.CurrencyId = CuruncyId;
                EntryMaster.CurrencyFactor = CuruncyFactor;
                EntryMaster.DailyTypeId = transactionSettings.DailyTypeId;
                EntryMaster.DocType = (int)DocumentTypes.OpenBalance;
                EntryMaster.EntryState = (int)EntryBalanceState.Balanced;
                EntryMaster.EntryType = (int)EntryType.Transaction;
                //EntryMaster.Notes = "";
                if (transactionSettings.TransferState)
                {
                    EntryMaster.IsTransfered = (int)EntryTransferState.Transfered;
                }
                else
                {
                    EntryMaster.IsTransfered = (int)EntryTransferState.NotTransfered;
                }
                EntryMaster.TotalCredit = 0;
                EntryMaster.TotalDebit = 0;



                EntryMaster.SetBasicData(CRUD_OperationType.Create, (int)DocumentTypes.OpenBalance, CurrentUser);
                #endregion

                #region EntryDetails
                //الطرف الاول لحساب  مخزون اول المدة
                DailyEntryDetails DailyEntry1 = new DailyEntryDetails();

                DailyEntry1.MasterId = EntryMaster.Id;
                DailyEntry1.AccountId = transactionSettings.FirstSideAccountId;
                DailyEntry1.CostCenterId = 0;
                if (transactionSettings.FirstSideNaturalId == (int)AccountNatures.Debit)
                {
                    DailyEntry1.Debit = Amount;
                    DailyEntry1.Credit = 0;
                }
                else
                {
                    DailyEntry1.Debit = 0;
                    DailyEntry1.Credit = Amount;
                }
                DailyEntry1.SetBasicData(CurrentUser, EntryMaster);
                EntryMaster.DailyEntryDetails.Add(DailyEntry1);




                //الطرف الثاني لحساب راس المال
                DailyEntryDetails DailyEntry2 = new DailyEntryDetails();

                DailyEntry2.MasterId = EntryMaster.Id;
                DailyEntry2.AccountId = transactionSettings.SecondSideAccountId;
                DailyEntry2.CostCenterId = 0;
                if (transactionSettings.SecondSideNaturalId == (int)AccountNatures.Debit)
                {
                    DailyEntry2.Debit = Amount;
                    DailyEntry2.Credit = 0;
                }
                else
                {
                    DailyEntry2.Debit = 0;
                    DailyEntry2.Credit = Amount;
                }
                DailyEntry2.SetBasicData(CurrentUser, EntryMaster);
                EntryMaster.DailyEntryDetails.Add(DailyEntry2);


                EntryMaster.TotalDebit = EntryMaster.DailyEntryDetails.Sum(x => x.Debit);
                EntryMaster.TotalCredit = EntryMaster.DailyEntryDetails.Sum(x => x.Credit);

                _DailyEntryService.AddEntry(EntryMaster);

                EntryNumber = EntryMaster.EntryNumber;
                EntryId = EntryMaster.Id;
                #endregion



            }


        }

        public void UpdateEntry(TransactionEntrySetting transactionSettings, int DocNumber, int CuruncyId, decimal CuruncyFactor, decimal Amount, decimal VAT_TaxAmount, int EntryId)
        {

            if (transactionSettings != null)
            {
                var oldEntryMaster = _DailyEntryService.GetById(EntryId);
                List<DailyEntryDetails> EntryDetails = new List<DailyEntryDetails>();
                if (oldEntryMaster != null)
                {
                    var oldEntryDetails = _DailyEntryService.GetEntryDetails(x => x.MasterId == EntryId);
                    #region Entry Master

                    oldEntryMaster.CurrencyId = CuruncyId;
                    oldEntryMaster.CurrencyFactor = CuruncyFactor;
                    oldEntryMaster.DailyTypeId = transactionSettings.DailyTypeId;
                    if (transactionSettings.TransferState)
                    {
                        oldEntryMaster.IsTransfered = (int)EntryTransferState.Transfered;
                    }
                    else
                    {
                        oldEntryMaster.IsTransfered = (int)EntryTransferState.NotTransfered;
                    }

                    oldEntryMaster.SetBasicData(CRUD_OperationType.Update, (int)DocumentTypes.OpenBalance, CurrentUser);
                    #endregion

                    #region EntryDetails
                    //الطرف الاول لحساب مخزون اول المدة
                    DailyEntryDetails DailyEntry1 = new DailyEntryDetails();

                    DailyEntry1.MasterId = oldEntryMaster.Id;
                    DailyEntry1.AccountId = transactionSettings.FirstSideAccountId;
                    DailyEntry1.CostCenterId = 0;
                    if (transactionSettings.FirstSideNaturalId == (int)AccountNatures.Debit)
                    {
                        DailyEntry1.Debit = Amount;
                        DailyEntry1.Credit = 0;
                    }
                    else
                    {
                        DailyEntry1.Debit = 0;
                        DailyEntry1.Credit = Amount;
                    }
                    DailyEntry1.SetBasicData(CurrentUser, oldEntryMaster);
                    //oldEntryMaster.DailyEntryDetails.Add(DailyEntry1);
                    EntryDetails.Add(DailyEntry1);




                    //الطرف الثاني لحساب راس المال
                    DailyEntryDetails DailyEntry2 = new DailyEntryDetails();

                    DailyEntry2.MasterId = oldEntryMaster.Id;
                    DailyEntry2.AccountId = transactionSettings.SecondSideAccountId;
                    DailyEntry2.CostCenterId = 0;
                    if (transactionSettings.SecondSideNaturalId == (int)AccountNatures.Debit)
                    {
                        DailyEntry2.Debit = Amount;
                        DailyEntry2.Credit = 0;
                    }
                    else
                    {
                        DailyEntry2.Debit = 0;
                        DailyEntry2.Credit = Amount;
                    }
                    DailyEntry2.SetBasicData(CurrentUser, oldEntryMaster);
                    EntryDetails.Add(DailyEntry2);

                    oldEntryMaster.DailyEntryDetails = EntryDetails;

                    oldEntryMaster.TotalDebit = oldEntryMaster.DailyEntryDetails.Sum(x => x.Debit);
                    oldEntryMaster.TotalCredit = oldEntryMaster.DailyEntryDetails.Sum(x => x.Credit);

                    _DailyEntryService.UpdateEntry(oldEntryMaster, oldEntryDetails);

                    #endregion


                }



            }


        }



    }
}
