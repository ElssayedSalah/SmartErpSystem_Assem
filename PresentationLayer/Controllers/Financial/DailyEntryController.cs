using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models.Financial;
using BusinessLayer.Models.Reports.Finance;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.System;
using Grand.Framework.Kendoui;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helpers;
using Reports;
using Reports.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static BusinessLayer.Helpers.SharedEnums;

namespace PresentationLayer.Controllers.Financial
{    
    public class DailyEntryController : BaseAdminController
    {
        private readonly IFinanceService<DailyEntryMaster, DailyEntryDetails> _DailyEntryService;
        private readonly IBaseService<Account> _AccountsService;
        private readonly IBaseService<DailyAccounts_Def> _DailyAccounts_DefService;
        private readonly IBaseService<Document> _DocumentService;
        private readonly IMapper _Mapper;
        private readonly string _currentLanguage;
        private readonly IBaseService<Currency> _CurrencyService;
        private readonly LocalizationService _LocalizationService;
        private readonly IidentityService _identityService;
        private readonly IReportService _ReportService;


        public DailyEntryController(IFinanceService<DailyEntryMaster, DailyEntryDetails> DailyEntryService,IBaseService<Account> AccountsService,  IBaseService<Currency> CurrencyService, IMapper Mapper, LocalizationService localizationService, IidentityService IdentityService, IBaseService<DailyAccounts_Def> DailyAccounts_DefService, IBaseService<Document> DocumentService, IReportService ReportService)
        {
            _DailyEntryService = DailyEntryService;
            _AccountsService = AccountsService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _CurrencyService = CurrencyService;
            _Mapper = Mapper;
            _LocalizationService = localizationService;
            _identityService = IdentityService;
            _DailyAccounts_DefService = DailyAccounts_DefService;
            _DocumentService = DocumentService;
            _ReportService = ReportService;

        }
        public void IntializeDropdowens()
        {
            var allAccounts = _AccountsService.GetAll();
            allAccounts.Add(new Account() { Id = 0, NameAr = "إختار", NameEn = "Select", LastLevelInTree = true });

            var Accounts = allAccounts.Where(x => x.LastLevelInTree).ToList();
            ViewBag.Accounts = Accounts.Select(x => new { Id = x.Id, Name =x.AccountCode + " " + ( _currentLanguage == "ar" ? x.NameAr : x.NameEn) });

            var Currencies = _CurrencyService.GetAll();
            Currencies.Insert(0, new Currency() { Id = 0, NameAr = "اختيار عملة", NameEn = "select" });
            ViewBag.Currencies = Currencies.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });
            
            var Documents = _DocumentService.GetAll();           
            ViewBag.Documents = Documents.Select(x => new { Id= x.DocTypeId, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });
            
            var DailyAccounts = _DailyAccounts_DefService.GetAll();
            DailyAccounts.Insert(0, new DailyAccounts_Def() { Id = 0, NameAr = "اختيار يومية", NameEn = "select" });
            ViewBag.DailyAccounts = DailyAccounts.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            ViewBag.EntryTransferState = PresentationExtensions.ConvertEnumToSelectListItemsStartFromZeroIndex(typeof(EntryTransferState), _LocalizationService);
            ViewBag.EntryBalanceState = PresentationExtensions.ConvertEnumToSelectListItemsStartFromZeroIndex(typeof(EntryBalanceState), _LocalizationService);
            ViewBag.EntryCreationMethod = PresentationExtensions.ConvertEnumToSelectListItemsStartFromZeroIndex(typeof(EntryCreationMethod), _LocalizationService);


        }
        public void SetBasicButtonsVisibilityForCreate()
        {

            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "DailyEntry", CreateNewBtnAction = "Create", BackToListControler = "DailyEntry", BackToListAction = "Index", EditBtnControler = "DailyEntry", EditBtnAction = "Edit", DeleteBtnControler = "DailyEntry", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false, PrintBtnVisibilty = false };

        }
        public void SetBasicButtonsVisibilityForEdit(int RouteId)
        {
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "DailyEntry", CreateNewBtnAction = "Create", BackToListControler = "DailyEntry", BackToListAction = "Index", EditBtnControler = "DailyEntry", EditBtnAction = "Edit", DeleteBtnControler = "DailyEntry", DeleteBtnAction = "Delete", RouteId = RouteId, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true, PrintBtnVisibilty = true, PrintBtnControler = "DailyEntry", PrintBtnAction = "Print" };
        }
        public void ValidateModel(DailyEntryMasterModel model)
        {
            if (model != null && model.DailyEntryDetails != null)
            {
                if ( model.CurrencyId <= 0)
                {
                    ModelState.AddModelError("CurrencyId", "يجب إختيار العملة");
                }
                if (model.DailyTypeId <= 0)
                {
                    ModelState.AddModelError("DailyTypeId", "يجب اختيار يومية الحسابات");
                }
                if (CurrentUser.CompanyId == null || CurrentUser.CompanyId <= 0)
                {
                    ModelState.AddModelError("CompanyId", "يجب اختيار شركة للمستخدم الحالي");
                }
                if (CurrentUser.FinancialPeriod == null || CurrentUser.FinancialPeriod <= 0)
                {
                    ModelState.AddModelError("FinancialPeriod", "يجب اختيار سنة مالية للمستخدم الحالي");
                }
                if (model.DailyEntryDetails.Count > 1)
                {
                    foreach (var item in model.DailyEntryDetails.Skip(1))
                    {                        
                        if (item.AccountId <= 0)
                        {
                            ModelState.AddModelError("AccountId", "يجب اختيار الحساب");
                        }
                        if (item.Debit <= 0 && item.Credit<=0)
                        {
                            ModelState.AddModelError("Debit", "يجب ادخال قيمة الحساب مدين أو دائن");
                        }                     

                    }
                }
                else
                {
                    ModelState.AddModelError("EMPTY_Details", "يجب إدخال تفاصيل القيد");
                }


            }
            else
            {
                ModelState.AddModelError("NULL_MODEL", "يجب ادخال جميع الحقول المطلوبه");
            }

        }
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageDailyEntry.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            return View(new DailyEntryMasterModel());
        }
        public IActionResult list()
        {
            var EntryMaster = _DailyEntryService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId  && x.CompanyId == CurrentUser.CompanyId);
            var documents = _DocumentService.GetAll();
            var EntryMasterModel = _Mapper.Map<List<DailyEntryMasterModel>>(EntryMaster);
            EntryMasterModel = EntryMasterModel.Select(x =>
             {
                 var docName = "";
                 if (documents.Count>0)
                 {
                     docName = _currentLanguage == "ar" ? documents.Where(d => d.DocTypeId == x.DocType).FirstOrDefault().NameAr : documents.Where(d => d.DocTypeId == x.DocType).FirstOrDefault().NameEn;
                 }
                 var l = x;
                 l.DocTypeName = docName;
                 return x;
             }).OrderByDescending(x=>x.EntryNumber).ToList();

            var gridModel = new DataSourceResult
            {
                Data = EntryMasterModel,
                Total = EntryMasterModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageDailyEntry.SystemName, CurrentUser, PermissionActions.Create))
               return AccessDeniedView();

            var LastEntryNumber = _DailyEntryService.GetLastEntryNumber(o => o.EntryNumber, x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId );

            var LastDocumentNumber = _DailyEntryService.GetLastCode(o => o.Code, x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId && x.DocType == (int)DocumentTypes.ManualDailyEntry);

            var DefaultCuruncy = _CurrencyService.GetWithCondetion(x=>x.DefaultCurrency).FirstOrDefault();

            var NewMasterModel = new DailyEntryMasterModel() { EntryNumber = LastEntryNumber, TransactionDate = DateTime.Now,DocType=(int) DocumentTypes.ManualDailyEntry ,DocNumber= LastDocumentNumber ,Code= LastDocumentNumber, EntryCreationMethod=(int) EntryCreationMethod.Manual,CurrencyId= DefaultCuruncy.Id,CurrencyFactor= DefaultCuruncy.CurrencyChangrRate};

            //set basic buttons visibility
            SetBasicButtonsVisibilityForCreate();

            IntializeDropdowens();

            return View(NewMasterModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] DailyEntryMasterModel model)
        {
            DailyEntryMaster master = null;
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageDailyEntry.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();

                ValidateModel(model);
                if (ModelState.IsValid)
                {
                    if (model.DailyEntryDetails != null && model.DailyEntryDetails.Count > 0)
                    {
                        var currentUser = CurrentUser;
                        if (!_DailyEntryService.IsExistRecord(b => b.Code == model.Code && b.FinancialPeriodId == currentUser.FinancialPeriodId  && b.CompanyId == CurrentUser.CompanyId && b.DocType == (int)DocumentTypes.ManualDailyEntry))
                        {
                            if (model.TotalDebit == model.TotalCredit)
                            {
                                model.EntryState = 0;
                            }
                            else
                            {
                                model.EntryState = 1;
                            }
                            master = _Mapper.Map<DailyEntryMaster>(model);
                            master.DailyEntryDetails = master.DailyEntryDetails.Skip(1).ToList();
                            master.SetBasicData(CRUD_OperationType.Create, (int)DocumentTypes.ManualDailyEntry, currentUser);
                            foreach (var item in master.DailyEntryDetails)
                            {
                              item.SetBasicData(currentUser, master);
                            }
                            
                            _DailyEntryService.AddEntry(master);

                            ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("SavedSuccessfuly"), NotificationCssType.success.ToString());
                        }
                        else
                        {
                            ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("DuplicateCode"), NotificationCssType.danger.ToString());
                        }
                    }
                    else
                    {
                        model.DailyEntryDetails= new List<DailyEntryDetailsModel>();
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
                return RedirectToAction("Edit", new { Id = master.Id, IsNew = true });
            }
            else
            {
                SetBasicButtonsVisibilityForCreate();
                IntializeDropdowens();
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id, bool IsNew)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageDailyEntry.SystemName, CurrentUser, PermissionActions.Edit))
               return AccessDeniedView();

            DailyEntryMaster master = _DailyEntryService.GetById(Id);
            List<DailyEntryDetails> details = _DailyEntryService.GetEntryDetails(x => x.MasterId == master.Id);
            master.DailyEntryDetails = details.OrderByDescending(x=>x.Debit).ToList();
            master.DailyEntryDetails.Insert(0, new DailyEntryDetails());

            DailyEntryMasterModel masterModel = _Mapper.Map<DailyEntryMasterModel>(master);
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
        public IActionResult Edit(DailyEntryMasterModel model)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageDailyEntry.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();

                ValidateModel(model);
                if (ModelState.IsValid)
                {
                    if (model.DailyEntryDetails != null && model.DailyEntryDetails.Count > 0)
                    {
                        if (model.TotalDebit == model.TotalCredit)
                        {
                            model.EntryState = 0;
                        }
                        else
                        {
                            model.EntryState = 1;
                        }
                        DailyEntryMaster master = _Mapper.Map<DailyEntryMaster>(model);
                        master.DailyEntryDetails = master.DailyEntryDetails.Skip(1).ToList();
                        //get old details to be deleted 
                        List<DailyEntryDetails> oldDetails = _DailyEntryService.GetEntryDetails(x => x.MasterId == master.Id);
                        master.SetBasicData(SharedEnums.CRUD_OperationType.Update, (int)DocumentTypes.ManualDailyEntry, CurrentUser);

                        foreach (var item in master.DailyEntryDetails)
                        {
                            item.SetBasicData(CurrentUser, master);
                        }
                        _DailyEntryService.UpdateEntry(master, oldDetails);
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("UpdatedSuccessfuly"), NotificationCssType.success.ToString());

                    }
                    else
                    {
                        model.DailyEntryDetails = new List<DailyEntryDetailsModel>();
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
            SetBasicButtonsVisibilityForEdit(model.Id);
            IntializeDropdowens();

            return View(model);
        }


        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageDailyEntry.SystemName, CurrentUser, PermissionActions.Delete))
                   return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });
                

                DailyEntryMaster master = _DailyEntryService.GetById(Id);
                _DailyEntryService.Delete(master);
                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }
        }

        public IActionResult GetCurrencyFactor(int CurrencyId)
        {
            decimal factor = 0;
            var curuncy = _CurrencyService.GetById(CurrencyId);
            if (curuncy!=null)
            {
                factor = curuncy.CurrencyChangrRate;
            }

            return Json(factor);
        }
        
        public IActionResult GetAccountData(int AccountId)
        {
            
            var account =_AccountsService.GetById(AccountId); 

            return Json(account);
        }

        public IActionResult Print(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageAddToStore.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            if (Id > 0)
            {
                DailyEntryRptPrint report = new DailyEntryRptPrint(_LocalizationService);
                var DataSource = _ReportService.DailyEntryRptPrint(Id);
                report.DataSource = DataSource;
                var stream = report.GenerateReport("pdf");
                stream.Position = 0;
                return File(stream, "application/pdf", "DailyEntryRptPrint.pdf");
            }
            else
            {
                return View("Error");
            }

        }




    }
}
