using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models.Financial;
using BusinessLayer.Models.Inventory;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.Sales;
using DataAccessLayer.Entities.System;
using DataAccessLayer.Repositories;
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
    public class SalesInvoiceController : BaseAdminController
    {
        
        private readonly IInventoryService<Transaction_InvMaster, Transaction_InvDetails> _InventoryMasterService;
        private readonly IBaseService<Item> _ItemService;
        private readonly IBaseService<Branch> _BranchService;
        private readonly IBaseService<Store> _StoreService;
        private readonly IBaseService<ItemGroup> _ItemGroupService;
        private readonly IBaseService<Unit> _UnitService;
        private readonly IBaseService<Customer> _CustomerService;
        private readonly IBaseService<Currency> _CurrencyService;
        private readonly IBaseService<Company> _CompanyService;
        private readonly IBaseService<Taxes> _TaxesService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly string _currentLanguage;
        private readonly IidentityService _identityService;
        private readonly IReportService _ReportService;
        private readonly IBaseService<SystemSetting> _SystemSettingService;
        private readonly IBaseService<DefaultAccount> _DefaultAccountService;
        private readonly IBaseService<TransactionsEntrySettingMaster> _TransactionsEntrySettingService;
        private readonly IFinanceService<DailyEntryMaster, DailyEntryDetails> _DailyEntryService;
        private readonly IHelperRepository _HelperRepository;
        private readonly IBaseService<FinancialPeriod> _FinancialPeriodService;
        private readonly IWebHelper _WebHelperService;

        public SalesInvoiceController(
            IBaseService<Item> ItemService,
            IBaseService<ItemGroup> ItemGroupService,
            IMapper mapper, 
             LocalizationService localizationService,
             IBaseService<Unit> UnitService,
             IBaseService<Branch> BranchService,
             IBaseService<Store> StoreService,            
             IInventoryService<Transaction_InvMaster,
             Transaction_InvDetails> InventoryMasterService,
             IidentityService IdentityService,
             IBaseService<Customer> CustomerService,
             IBaseService<Currency> CurrencyService,
             IBaseService<Company> CompanyService,
             IReportService ReportService,
             IBaseService<Taxes> TaxesService,
             IBaseService<SystemSetting> SystemSettingService,
             IBaseService<DefaultAccount> DefaultAccountService,
             IBaseService<TransactionsEntrySettingMaster> TransactionsEntrySettingService,
             IFinanceService<DailyEntryMaster,
             DailyEntryDetails> DailyEntryService, 
             IHelperRepository HelperRepository,
             IBaseService<FinancialPeriod> FinancialPeriodService,
             IWebHelper WebHelperService

            )
        {
            
            _ItemService = ItemService;
            _ItemGroupService = ItemGroupService;
            _UnitService = UnitService;
            _BranchService = BranchService;
            _StoreService = StoreService;
            _InventoryMasterService = InventoryMasterService;
            _identityService = IdentityService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _CustomerService = CustomerService;
            _CurrencyService = CurrencyService;
            _CompanyService = CompanyService;
            _ReportService = ReportService;
            _TaxesService = TaxesService;
            _SystemSettingService = SystemSettingService;
            _DefaultAccountService = DefaultAccountService;
            _TransactionsEntrySettingService = TransactionsEntrySettingService;
            _DailyEntryService = DailyEntryService;
            _HelperRepository = HelperRepository;
            _FinancialPeriodService = FinancialPeriodService;
            _WebHelperService = WebHelperService;


        }
        public void SetBasicButtonsVisibilityForCreate()
        {

            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "SalesInvoice", CreateNewBtnAction = "Create", BackToListControler = "SalesInvoice", BackToListAction = "Index", EditBtnControler = "SalesInvoice", EditBtnAction = "Edit", DeleteBtnControler = "SalesInvoice", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false, PrintBtnVisibilty = false };

        }
        public void SetBasicButtonsVisibilityForEdit(int RouteId)
        {
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "SalesInvoice", CreateNewBtnAction = "Create", BackToListControler = "SalesInvoice", BackToListAction = "Index", EditBtnControler = "SalesInvoice", EditBtnAction = "Edit", DeleteBtnControler = "SalesInvoice", DeleteBtnAction = "Delete", RouteId = RouteId, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true, PrintBtnVisibilty = true, PrintBtnControler = "SalesInvoice", PrintBtnAction = "Print",SendEInvoiceBtnControler= "SalesInvoice",SendEInvoiceBtnAction= "SendEInvoice",SendEInvoiceBtnVisibilty=true,DocumentType="I" };
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

            var Items = _ItemService.GetWithCondetion(x => x.FinancialPeriodId != null && x.ActivationState.Value && x.FinancialPeriodId <= CurrentUser.FinancialPeriodId);
            Items.Insert(0, new Item() { Id = 0, NameAr = "اختيار صنف", NameEn = "select" });
            ViewBag.Itmes = Items.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Customers = _CustomerService.GetAll();
            Customers.Insert(0, new Customer() { Id = 0, NameAr = "اختيار عميل", NameEn = "select" });
            ViewBag.Customers = Customers.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Currencies = _CurrencyService.GetAll();
            Currencies.Insert(0, new Currency() { Id = 0, NameAr = "اختيار عملة", NameEn = "select" });
            ViewBag.Currencies = Currencies.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            ViewBag.DiscountTypes = PresentationExtensions.ConvertEnumToSelectListItems(typeof(DiscountTypes), _LocalizationService);


        }
        public void ValidateModel(Transaction_InvMasterModel model)
        {
            if (model != null && model.Transaction_InvDetails != null)
            {
                
                if (model.BranchId == null || model.BranchId <= 0)
                {
                    ModelState.AddModelError("BranchId", "يجب اختيار فرع");
                }
                if (model.StoreId == null || model.StoreId <= 0)
                {
                    ModelState.AddModelError("StoreId", "يجب اختيار مخزن");
                }
                if (model.CustomerId == null || model.CustomerId <= 0)
                {
                    ModelState.AddModelError("CustomerId", "يجب اختيار عميل");
                }
                if (model.CurrencyId <= 0)
                {
                    ModelState.AddModelError("CurrencyId", "يجب اختيار عملة");
                }
                if (CurrentUser.CompanyId == null || CurrentUser.CompanyId <= 0)
                {
                    ModelState.AddModelError("CompanyId", "يجب اختيار شركة للمستخدم الحالي");
                }
                if (CurrentUser.FinancialPeriod == null || CurrentUser.FinancialPeriod <= 0)
                {
                    ModelState.AddModelError("FinancialPeriod", "يجب اختيار سنة مالية للمستخدم الحالي");
                }
                var FinancialPeriod = _FinancialPeriodService.GetById(CurrentUser.FinancialPeriodId.Value);

                if (model.Transaction_InvDetails.Count > 1)
                {
                    foreach (var item in model.Transaction_InvDetails.Skip(1))
                    {
                        item.FinancialPeriodId = CurrentUser.FinancialPeriodId;
                        item.CompanyId = CurrentUser.CompanyId;
                        item.DocTypeId = (int)DocumentTypes.SalesInvoice;
                        item.DocDate = model.DocDate;
                        if (item.ItemId <= 0)
                        {
                            ModelState.AddModelError("ItemId", "يجب اختيار صنف");
                        }
                        if (item.Quntity <= 0)
                        {
                            ModelState.AddModelError("Quntity", "يجب ادخال الكمية");
                        }
                        if (item.SalesPrice <= 0)
                        {
                            ModelState.AddModelError("SalesPrice", "يجب ادخال سعر البيع");
                        }

                        var rowItem = _ItemService.GetById(item.ItemId);
                        if (rowItem!=null && !rowItem.AllowNegativeOut)
                        {
                            var ItemCurrentBalance = _HelperRepository.GetItemBalance(item.ItemId, model.BranchId.Value, model.StoreId.Value, CurrentUser.FinancialPeriodId.Value, CurrentUser.CompanyId.Value, FinancialPeriod.DateFrom, model.DocDate, model.Id);
                            if (item.Quntity > ItemCurrentBalance)
                            {
                                ModelState.AddModelError("ItemCurrentBalance", $"كمية الصنف  {rowItem.NameAr} غير كافية");
                            }
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
            if (!_identityService.Authorize(StandardPermissionProvider.ManageSalesInvoice.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            return View(new Transaction_InvMasterModel());
        }

        public IActionResult list()
        {
            var TrMaster = _InventoryMasterService.GetInventoryTransactions(x=>x.FinancialPeriodId== CurrentUser.FinancialPeriodId && x.DocTypeId== (int)DocumentTypes.SalesInvoice && x.CompanyId== CurrentUser.CompanyId);
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
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageSalesInvoice.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();
            
            var LastCode = _InventoryMasterService.GetLastCode(o => o.Code ,x=> x.FinancialPeriodId==CurrentUser.FinancialPeriodId && x.DocTypeId == (int)DocumentTypes.SalesInvoice && x.CompanyId == CurrentUser.CompanyId);
            var DefaultCurrency = _CurrencyService.GetWithCondetion(x=>x.DefaultCurrency).FirstOrDefault();

            var NewMasterModel = new Transaction_InvMasterModel() { Code = LastCode, DocDate = DateTime.Now, DueDate = DateTime.Now,CurrencyId= DefaultCurrency!=null? DefaultCurrency.Id:0 };

            if (CurrentUser.CompanyId.HasValue)
            {
                var Tax = _TaxesService.GetWithCondetion(x => x.TaxId == 1).FirstOrDefault();
                var Company = _CompanyService.GetById(CurrentUser.CompanyId.Value);
                if (Tax != null && Company != null && Company.Activate_VAT_Tax)
                {
                    NewMasterModel.VAT_Rate = Tax.Rate;
                }
            }

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
                if (!_identityService.Authorize(StandardPermissionProvider.ManageSalesInvoice.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();

                var ValidateFinancePeriodMsg = _WebHelperService.ValidateFinancePeriod(CurrentUser.FinancialPeriodId.Value, TransactionModel.DocDate);
                if (ValidateFinancePeriodMsg != "")
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
                    var customer = _CustomerService.GetById(TransactionModel.CustomerId.Value);
                    //التأكد من الحسابات الافتراضية ومواصفات حركة المبيعات إذا كان النظام يسمح بانشاء قيود الحركات
                    var msg = ValidateEntrySettings(transactionSettings, customer);

                    if (msg.Length > 0)
                    {
                        ViewData["NotificationMsg"] = Notification.Success(msg, NotificationCssType.danger.ToString());

                        SetBasicButtonsVisibilityForCreate();
                        IntializeDropdowens();
                        return View(TransactionModel);
                    }

                    if (TransactionModel.Transaction_InvDetails != null && TransactionModel.Transaction_InvDetails.Count > 0)
                    {
                        var currentUser = CurrentUser;
                        if (!_InventoryMasterService.IsExistRecord(b => b.Code == TransactionModel.Code && b.FinancialPeriodId== currentUser.FinancialPeriodId && b.DocTypeId == (int)DocumentTypes.SalesInvoice && b.CompanyId == CurrentUser.CompanyId))
                        {
                            master = _Mapper.Map<Transaction_InvMaster>(TransactionModel);
                            master.Transaction_InvDetails = master.Transaction_InvDetails.Skip(1).ToList();                           
                            master.SetBasicData(CRUD_OperationType.Create, (int)DocumentTypes.SalesInvoice, currentUser);

                            var Currency = _CurrencyService.GetById(master.CurrencyId);
                            master.CurrencyFactor = Currency.CurrencyChangrRate;
                            if (transactionSettings.EnableEntryCreation)
                            {
                                int EntryNumber = 0;
                                int EntryId = 0;
                                //add invoice entry
                                AddEntry(transactionSettings, master.Code, master.CurrencyId, master.CurrencyFactor, master.InvoiceTotal, master.InvoiceNet, master.TaxValue, out EntryNumber, out EntryId);

                                master.EntryNumber = EntryNumber;
                                master.EntryId = EntryId;
                            }                    

                            //save invoice Transaction
                            _InventoryMasterService.AddInventoryTransaction(master);

                            return RedirectToAction("Edit", new { Id = master.Id, FromAction = "Create" });
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
                return RedirectToAction("Edit", new { Id = master.Id, Action = "Create" , ShowMessege =true});
            }
            else
            {
                SetBasicButtonsVisibilityForCreate();
                IntializeDropdowens();
                return View(TransactionModel);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id,string FromAction)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageSalesInvoice.SystemName, CurrentUser, PermissionActions.Edit))
                return AccessDeniedView();
            Transaction_InvMaster master = _InventoryMasterService.GetById(Id);
            List<Transaction_InvDetails> details = _InventoryMasterService.GetInventoryTransactionDetails(x => x.MasterId == master.Id);
            master.Transaction_InvDetails = details;
            master.Transaction_InvDetails.Insert(0, new Transaction_InvDetails());

            Transaction_InvMasterModel masterModel = _Mapper.Map<Transaction_InvMasterModel>(master);
            //to show save message if the request came from create action
            if (FromAction != null && FromAction == "Create")
            {
                ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("SavedSuccessfuly"), NotificationCssType.success.ToString());
            }
            else if (FromAction != null && FromAction == "Edit")
            {
                ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("UpdatedSuccessfuly"), NotificationCssType.success.ToString());
            }
            //set basic buttons visibility
            SetBasicButtonsVisibilityForEdit(Id);
            IntializeDropdowens();
            return View(masterModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Transaction_InvMasterModel TransactionModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageSalesInvoice.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();

                var ValidateFinancePeriodMsg = _WebHelperService.ValidateFinancePeriod(CurrentUser.FinancialPeriodId.Value, TransactionModel.DocDate);
                if (ValidateFinancePeriodMsg != "")
                {
                    ViewData["NotificationMsg"] = Notification.Erorr(ValidateFinancePeriodMsg, NotificationCssType.danger.ToString());

                    SetBasicButtonsVisibilityForEdit(TransactionModel.Id);
                    IntializeDropdowens();
                    return View(TransactionModel);
                }


                ValidateModel(TransactionModel);
                if (ModelState.IsValid)
                {
                    TransactionEntrySetting transactionSettings = new TransactionEntrySetting();
                    var customer = _CustomerService.GetById(TransactionModel.CustomerId.Value);
                    //التأكد من الحسابات الافتراضية ومواصفات حركة المبيعات إذا كان النظام يسمح بانشاء قيود الحركات
                    var msg = ValidateEntrySettings(transactionSettings, customer);

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
                        master.SetBasicData(SharedEnums.CRUD_OperationType.Update, (int)DocumentTypes.SalesInvoice, CurrentUser);

                        var Currency = _CurrencyService.GetById(master.CurrencyId);
                        master.CurrencyFactor = Currency.CurrencyChangrRate;
                        if (transactionSettings.EnableEntryCreation)
                        {
                            //تعديل قيد الحركة اذا كان موجود
                            if (master.EntryId > 0 && master.EntryNumber > 0)
                            {
                                UpdateEntry(transactionSettings, master.Code, master.CurrencyId, master.CurrencyFactor, master.InvoiceTotal, master.InvoiceNet, master.TaxValue, master.EntryId);
                            }
                            else
                            {
                                int EntryNumber = 0;
                                int EntryId = 0;
                                //اضافة قيد للحركة اذا كان غير موجود
                                AddEntry(transactionSettings, master.Code, master.CurrencyId, master.CurrencyFactor, master.InvoiceTotal, master.InvoiceNet, master.TaxValue, out EntryNumber, out EntryId);

                                master.EntryNumber = EntryNumber;
                                master.EntryId = EntryId;
                            }
                        }
              

                        _InventoryMasterService.UpdateInventoryTransaction(master, oldDetails);

                        return RedirectToAction("Edit", new { Id = master.Id, FromAction = "Edit" });

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

        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageSalesInvoice.SystemName, CurrentUser, PermissionActions.Delete))
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
            if (Id>0)
            {
                SalesInvoiceRptPrint report = new SalesInvoiceRptPrint(_LocalizationService);
                var DataSource = _ReportService.SalesInvoicePrint(Id);
                report.DataSource = DataSource;
                var stream = report.GenerateReport("pdf");
                stream.Position = 0;
                return File(stream, "application/pdf", "SalesInvoiceRpt.pdf");
            }
            else
            {
                return View("Error");
            }            
        }

        /// <summary>
        /// التأكد من الحسابات الافتراضية ومواصفات حركة المشتريات إذا كان النظام يسمح بانشاء قيود الحركات
        /// </summary>
        /// <param name="transactionSettings"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public string ValidateEntrySettings(TransactionEntrySetting transactionSettings, Customer customer)
        {
            StringBuilder msg = new StringBuilder();
            var SystemSetting = _SystemSettingService.GetAll().FirstOrDefault();
            if (SystemSetting != null && SystemSetting.EnableTransactionsEntryCreation)
            {
                transactionSettings.EnableEntryCreation = true;
                var transSettings = _TransactionsEntrySettingService.GetWithCondetion(x => x.DocTypeId == (int)DocumentTypes.SalesInvoice).FirstOrDefault();

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
                    //الطرف الثاني حساب المبيعات
                    var FSAccount = defaultAccounts.Where(a => a.AccountNameId == 67).FirstOrDefault();
                    if (FSAccount == null || FSAccount.AccountId <= 0)
                    {
                        msg.AppendLine("يجب تحديد حساب افتراضي لحساب المبيعات <br>");
                    }
                    else
                    {
                        transactionSettings.SecondSideAccountId = FSAccount.AccountId;
                    }
                }

                if (customer == null || customer.AccountId <= 0)
                {
                    msg.AppendLine("يجب تحديد حساب العميل <br>");
                }
                else
                {
                    transactionSettings.FirstSideAccountId = customer.AccountId;
                }

                var companySetting = _CompanyService.GetById(CurrentUser.CompanyId.Value);
                if (companySetting != null && companySetting.Activate_VAT_Tax)
                {
                    var taxAcount = defaultAccounts.Where(a => a.AccountNameId == 35).FirstOrDefault();
                    if (taxAcount == null || taxAcount.AccountId <= 0)
                    {
                        msg.AppendLine("يجب تحديد حساب افتراضي لحساب ضريبة القيمة المضافة <br>");

                    }
                    else
                    {
                        transactionSettings.VAT_TaxAccountId = taxAcount.AccountId;

                    }
                }

            }

            return msg.ToString();
        }

        public void AddEntry(TransactionEntrySetting transactionSettings, int DocNumber, int CuruncyId, decimal CuruncyFactor, decimal FirstSideAmount, decimal SecondSideAmount, decimal VAT_TaxAmount, out int EntryNumber, out int EntryId)
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
                EntryMaster.DocType = (int)DocumentTypes.SalesInvoice;
                EntryMaster.EntryState = (int)EntryBalanceState.Balanced;
                EntryMaster.EntryType = (int)EntryType.Transaction;
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



                EntryMaster.SetBasicData(CRUD_OperationType.Create, (int)DocumentTypes.SalesInvoice, CurrentUser);
                #endregion

                #region EntryDetails
                //الطرف الاول لحساب العميل
                DailyEntryDetails DailyEntry1 = new DailyEntryDetails();

                DailyEntry1.MasterId = EntryMaster.Id;
                DailyEntry1.AccountId = transactionSettings.FirstSideAccountId;
                DailyEntry1.CostCenterId = 0;
                if (transactionSettings.FirstSideNaturalId == (int)AccountNatures.Debit)
                {
                    DailyEntry1.Debit = FirstSideAmount;
                    DailyEntry1.Credit = 0;
                }
                else
                {
                    DailyEntry1.Debit = 0;
                    DailyEntry1.Credit = FirstSideAmount;
                }
                DailyEntry1.SetBasicData(CurrentUser, EntryMaster);
                EntryMaster.DailyEntryDetails.Add(DailyEntry1);




                //الطرف الثاني لحساب المبيعات
                DailyEntryDetails DailyEntry2 = new DailyEntryDetails();

                DailyEntry2.MasterId = EntryMaster.Id;
                DailyEntry2.AccountId = transactionSettings.SecondSideAccountId;
                DailyEntry2.CostCenterId = 0;
                if (transactionSettings.SecondSideNaturalId == (int)AccountNatures.Debit)
                {
                    DailyEntry2.Debit = SecondSideAmount;
                    DailyEntry2.Credit = 0;
                }
                else
                {
                    DailyEntry2.Debit = 0;
                    DailyEntry2.Credit = SecondSideAmount;
                }
                DailyEntry2.SetBasicData(CurrentUser, EntryMaster);
                EntryMaster.DailyEntryDetails.Add(DailyEntry2);





                //الطرف الثالث لحساب ضريبة القيمة المضافة
                if (transactionSettings.IsFirstSideTaxble && VAT_TaxAmount > 0 && transactionSettings.VAT_TaxAccountId > 0)
                {
                    DailyEntryDetails DailyEntry3 = new DailyEntryDetails();

                    DailyEntry3.MasterId = EntryMaster.Id;
                    DailyEntry3.AccountId = transactionSettings.VAT_TaxAccountId;
                    DailyEntry3.CostCenterId = 0;
                    if (transactionSettings.FirstSideNaturalId == (int)AccountNatures.Debit)
                    {
                        DailyEntry3.Debit = VAT_TaxAmount;
                        DailyEntry3.Credit = 0;
                    }
                    else
                    {
                        DailyEntry3.Debit = 0;
                        DailyEntry3.Credit = VAT_TaxAmount;
                    }
                    DailyEntry3.SetBasicData(CurrentUser, EntryMaster);
                    EntryMaster.DailyEntryDetails.Add(DailyEntry3);

                }




                if (transactionSettings.IsSecondSideTaxble && VAT_TaxAmount > 0 && transactionSettings.VAT_TaxAccountId > 0)
                {
                    DailyEntryDetails DailyEntry3 = new DailyEntryDetails();

                    DailyEntry3.MasterId = EntryMaster.Id;
                    DailyEntry3.AccountId = transactionSettings.VAT_TaxAccountId;
                    DailyEntry3.CostCenterId = 0;
                    if (transactionSettings.SecondSideNaturalId == (int)AccountNatures.Debit)
                    {
                        DailyEntry3.Debit = VAT_TaxAmount;
                        DailyEntry3.Credit = 0;
                    }
                    else
                    {
                        DailyEntry3.Debit = 0;
                        DailyEntry3.Credit = VAT_TaxAmount;
                    }
                    DailyEntry3.SetBasicData(CurrentUser, EntryMaster);
                    EntryMaster.DailyEntryDetails.Add(DailyEntry3);

                }

                EntryMaster.TotalDebit = EntryMaster.DailyEntryDetails.Sum(x => x.Debit);
                EntryMaster.TotalCredit = EntryMaster.DailyEntryDetails.Sum(x => x.Credit);

                _DailyEntryService.AddEntry(EntryMaster);

                EntryNumber = EntryMaster.EntryNumber;
                EntryId = EntryMaster.Id;
                #endregion



            }


        }


        public void UpdateEntry(TransactionEntrySetting transactionSettings, int DocNumber, int CuruncyId, decimal CuruncyFactor, decimal FirstSideAmount, decimal SecondSideAmount, decimal VAT_TaxAmount, int EntryId)
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

                    oldEntryMaster.SetBasicData(CRUD_OperationType.Update, (int)DocumentTypes.SalesInvoice, CurrentUser);
                    #endregion

                    #region EntryDetails
                    //الطرف الاول لحساب المبيعات
                    DailyEntryDetails DailyEntry1 = new DailyEntryDetails();

                    DailyEntry1.MasterId = oldEntryMaster.Id;
                    DailyEntry1.AccountId = transactionSettings.FirstSideAccountId;
                    DailyEntry1.CostCenterId = 0;
                    if (transactionSettings.FirstSideNaturalId == (int)AccountNatures.Debit)
                    {
                        DailyEntry1.Debit = FirstSideAmount;
                        DailyEntry1.Credit = 0;
                    }
                    else
                    {
                        DailyEntry1.Debit = 0;
                        DailyEntry1.Credit = FirstSideAmount;
                    }
                    DailyEntry1.SetBasicData(CurrentUser, oldEntryMaster);
                    //oldEntryMaster.DailyEntryDetails.Add(DailyEntry1);
                    EntryDetails.Add(DailyEntry1);




                    //الطرف الثاني لحساب العميل
                    DailyEntryDetails DailyEntry2 = new DailyEntryDetails();

                    DailyEntry2.MasterId = oldEntryMaster.Id;
                    DailyEntry2.AccountId = transactionSettings.SecondSideAccountId;
                    DailyEntry2.CostCenterId = 0;
                    if (transactionSettings.SecondSideNaturalId == (int)AccountNatures.Debit)
                    {
                        DailyEntry2.Debit = SecondSideAmount;
                        DailyEntry2.Credit = 0;
                    }
                    else
                    {
                        DailyEntry2.Debit = 0;
                        DailyEntry2.Credit = SecondSideAmount;
                    }
                    DailyEntry2.SetBasicData(CurrentUser, oldEntryMaster);
                    //oldEntryMaster.DailyEntryDetails.Add(DailyEntry2);
                    EntryDetails.Add(DailyEntry2);





                    //الطرف الثالث لحساب ضريبة القيمة المضافة
                    if (transactionSettings.IsFirstSideTaxble && VAT_TaxAmount > 0 && transactionSettings.VAT_TaxAccountId > 0)
                    {
                        DailyEntryDetails DailyEntry3 = new DailyEntryDetails();

                        DailyEntry3.MasterId = oldEntryMaster.Id;
                        DailyEntry3.AccountId = transactionSettings.VAT_TaxAccountId;
                        DailyEntry3.CostCenterId = 0;
                        if (transactionSettings.FirstSideNaturalId == (int)AccountNatures.Debit)
                        {
                            DailyEntry3.Debit = VAT_TaxAmount;
                            DailyEntry3.Credit = 0;
                        }
                        else
                        {
                            DailyEntry3.Debit = 0;
                            DailyEntry3.Credit = VAT_TaxAmount;
                        }
                        DailyEntry3.SetBasicData(CurrentUser, oldEntryMaster);
                        //oldEntryMaster.DailyEntryDetails.Add(DailyEntry3);
                        EntryDetails.Add(DailyEntry3);

                    }




                    if (transactionSettings.IsSecondSideTaxble && VAT_TaxAmount > 0 && transactionSettings.VAT_TaxAccountId > 0)
                    {
                        DailyEntryDetails DailyEntry3 = new DailyEntryDetails();

                        DailyEntry3.MasterId = oldEntryMaster.Id;
                        DailyEntry3.AccountId = transactionSettings.VAT_TaxAccountId;
                        DailyEntry3.CostCenterId = 0;
                        if (transactionSettings.SecondSideNaturalId == (int)AccountNatures.Debit)
                        {
                            DailyEntry3.Debit = VAT_TaxAmount;
                            DailyEntry3.Credit = 0;
                        }
                        else
                        {
                            DailyEntry3.Debit = 0;
                            DailyEntry3.Credit = VAT_TaxAmount;
                        }
                        DailyEntry3.SetBasicData(CurrentUser, oldEntryMaster);
                        //oldEntryMaster.DailyEntryDetails.Add(DailyEntry3);
                        EntryDetails.Add(DailyEntry3);

                    }
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
