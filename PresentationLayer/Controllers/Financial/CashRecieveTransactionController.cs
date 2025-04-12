using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models.Financial;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.Purchases;
using DataAccessLayer.Entities.Sales;
using DataAccessLayer.Entities.System;
using Grand.Framework.Kendoui;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Helpers;
using Reports;
using Reports.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static BusinessLayer.Helpers.SharedEnums;

namespace PresentationLayer.Controllers.Financial
{
    public class CashRecieveTransactionController : BaseAdminController
    {
        private readonly IBaseService<CashTransaction> _CashTransactionService;
        private readonly IFinanceService<DailyEntryMaster, DailyEntryDetails> _DailyEntryService;
        private readonly IBaseService<Account> _AccountsService;
        private readonly IMapper _Mapper;
        private readonly string _currentLanguage;
        private readonly IBaseService<Currency> _CurrencyService;
        private readonly LocalizationService _LocalizationService;
        private readonly IidentityService _identityService;
        private readonly IBaseService<Branch> _BranchService;
        private readonly IBaseService<Suppler> _SupplerService;
        private readonly IBaseService<Customer> _CustomerService;
        private readonly IBaseService<Bank> _BankService;
        private readonly IBaseService<Treasury> _TreasuryService;
        private readonly IBaseService<TransactionsEntrySettingMaster> _TransactionsEntrySettingService;
        private readonly IBaseService<SystemSetting> _SystemSettingService;
        private readonly IBaseService<Document> _DocumentService;
        private readonly IBaseService<TransactionsEntrySettingDetails> _TransactionsEntrySettingDetails;
        private readonly IReportService _ReportService;



        public CashRecieveTransactionController(IBaseService<CashTransaction> CashTransactionService,IFinanceService<DailyEntryMaster, DailyEntryDetails> DailyEntryService, IBaseService<Account> AccountsService, IBaseService<Currency> CurrencyService, IMapper Mapper, LocalizationService localizationService, IidentityService IdentityService, IBaseService<Branch> BranchService, IBaseService<Suppler> SupplerService, IBaseService<Customer> CustomerService, IBaseService<Bank> BankService, IBaseService<Treasury> TreasuryService, IBaseService<TransactionsEntrySettingMaster> TransactionsEntrySettingService, IBaseService<SystemSetting> SystemSettingService, IBaseService<Document> DocumentService, IBaseService<TransactionsEntrySettingDetails> TransactionsEntrySettingDetails, IReportService ReportService)
        {
            _DailyEntryService = DailyEntryService;
            _AccountsService = AccountsService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _CurrencyService = CurrencyService;
            _Mapper = Mapper;
            _LocalizationService = localizationService;
            _identityService = IdentityService;
            _CashTransactionService = CashTransactionService;
            _BranchService = BranchService;
            _SupplerService = SupplerService;
            _CustomerService = CustomerService;
            _BankService = BankService;
            _TreasuryService = TreasuryService;
            _TransactionsEntrySettingService = TransactionsEntrySettingService;
            _SystemSettingService = SystemSettingService;
            _DocumentService = DocumentService;
            _TransactionsEntrySettingDetails = TransactionsEntrySettingDetails;
            _ReportService = ReportService;

        }

        public void IntializeDropdowens()
        {
            var Accounts = _AccountsService.GetAll().Where(x => x.LastLevelInTree).ToList();
            Accounts.Insert(0,new Account() { Id = 0, NameAr = "إختار", NameEn = "Select"});
            ViewBag.Accounts = Accounts.Select(x => new { Id = x.Id, Name = x.AccountCode + "-" + x.Name });

            var Branches = _BranchService.GetAll();
            Branches.Insert(0, new Branch() { Id = 0, NameAr = "اختيار فرع", NameEn = "select" });
            ViewBag.Branches = Branches.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });           

            var Currencies = _CurrencyService.GetAll();
            Currencies.Insert(0, new Currency() { Id = 0, NameAr = "اختيار عملة", NameEn = "select" });
            ViewBag.Currencies = Currencies.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            //ViewBag.SideTypes = PresentationExtensions.ConvertEnumToSelectListItemsStartFromZeroIndex(typeof(EntrySides), _LocalizationService);


            var transSettings = _TransactionsEntrySettingService.GetWithCondetion(x => x.DocTypeId == (int)DocumentTypes.CashRecieveTransaction).FirstOrDefault();

            if (transSettings!=null)
            {
                var transSettingsDetails = _TransactionsEntrySettingDetails.GetWithCondetion(x => x.MasterId == transSettings.Id).ToList();
                var FirstSide = transSettingsDetails.Where(x => x.SideTypeId == 1).Select(x => x.SideId).ToList();
                var SecondSide = transSettingsDetails.Where(x => x.SideTypeId == 2).Select(x => x.SideId).ToList();


                ViewBag.SideTypes1 = PresentationExtensions.ConvertEnumToSelectListItemsStartFromZeroIndex(typeof(EntrySides), _LocalizationService).Where(x => FirstSide.Contains(int.Parse(x.Value)));

                ViewBag.SideTypes2 = PresentationExtensions.ConvertEnumToSelectListItemsStartFromZeroIndex(typeof(EntrySides), _LocalizationService).Where(x => SecondSide.Contains(int.Parse(x.Value)));
            }
            else
            {
                ViewBag.SideTypes1 = new List<SelectListItem>();
                ViewBag.SideTypes2 = new List<SelectListItem>();
            }
           





        }

       
        public void ValidateModel(CashTransactionModel model)
        {
            if (model != null)
            {

                if (model.BranchId <= 0)
                {
                    ModelState.AddModelError("BranchId", "يجب اختيار فرع");
                }  
                if (model.FirstSideTypeId <= 0)
                {
                    ModelState.AddModelError("FirstSideTypeId", "يجب اختيار نوع الطرف الاول");
                }
                if (model.FirstSideId <= 0)
                {
                    ModelState.Remove("FirstSideId");
                    ModelState.AddModelError("FirstSideId", "يجب اختيار الطرف الاول");
                }
                if (model.FirstSideAccountId <= 0)
                {
                    ModelState.AddModelError("FirstSideAccountId", "يجب اختيار حساب الطرف الاول");
                }
                if (model.SecondSideTypeId <= 0)
                {
                    ModelState.AddModelError("SecondSideTypeId", "يجب اختيار نوع الطرف الثاني");
                }
                if (model.SecondSideId <= 0)
                {
                    ModelState.Remove("SecondSideId");
                    ModelState.AddModelError("SecondSideId", "يجب اختيار الطرف الثاني");
                }
                if (model.SecondSideAccountId <= 0)
                {
                    ModelState.AddModelError("SecondSideAccountId", "يجب اختيار حساب الطرف الثاني");
                }
                if (model.Amount <= 0)
                {
                    ModelState.AddModelError("Amount", "يجب إدخال المبلغ ");
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

            }
            else
            {
                ModelState.AddModelError("NULL_MODEL", "يجب ادخال جميع الحقول المطلوبه");
            }

        }

        public void SetBasicButtonsVisibilityForCreate()
        {

            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "CashRecieveTransaction", CreateNewBtnAction = "Create", BackToListControler = "CashRecieveTransaction", BackToListAction = "Index", EditBtnControler = "CashRecieveTransaction", EditBtnAction = "Edit", DeleteBtnControler = "CashRecieveTransaction", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false, PrintBtnVisibilty = false };

        }

        public void SetBasicButtonsVisibilityForEdit(int RouteId)
        {
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "CashRecieveTransaction", CreateNewBtnAction = "Create", BackToListControler = "CashRecieveTransaction", BackToListAction = "Index", EditBtnControler = "CashRecieveTransaction", EditBtnAction = "Edit", DeleteBtnControler = "CashRecieveTransaction", DeleteBtnAction = "Delete", RouteId = RouteId, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true, PrintBtnVisibilty = true, PrintBtnControler = "CashRecieveTransaction", PrintBtnAction = "Print"};
        }
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCashRecieveTransaction.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            return View(new CashTransactionModel());
        }
        public IActionResult list()
        {
            var transactions = _CashTransactionService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId && x.DocTypeId== (int)DocumentTypes.CashRecieveTransaction);
            var branches =_BranchService.GetAll();
            var transactionsModel = _Mapper.Map<List<CashTransactionModel>>(transactions);

            transactionsModel = transactionsModel.Select(x =>
            {
                var BranchName = "";
                if (branches.Count > 0)
                {
                    BranchName = _currentLanguage == "ar" ? branches.Where(d => d.Id == x.BranchId).FirstOrDefault().NameAr : branches.Where(d => d.Id == x.BranchId).FirstOrDefault().NameEn;
                }
                var l = x;
                l.BranchName = BranchName;
                return x;
            }).ToList();



            var gridModel = new DataSourceResult
            {
                Data = transactionsModel,
                Total = transactionsModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCashRecieveTransaction.SystemName, CurrentUser, PermissionActions.Create))
               return AccessDeniedView();

            var LastCode = _CashTransactionService.GetLastCode(o => o.Code, x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.DocTypeId == (int)DocumentTypes.CashRecieveTransaction && x.CompanyId == CurrentUser.CompanyId);
            var DefaultCurrency = _CurrencyService.GetWithCondetion(x => x.DefaultCurrency).FirstOrDefault();

           

            var Model = new CashTransactionModel() { Code = LastCode, DocDate = DateTime.Now, CurrencyId = DefaultCurrency != null ? DefaultCurrency.Id : 0 };
           
            //set basic buttons visibility
            SetBasicButtonsVisibilityForCreate();
            IntializeDropdowens();

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] CashTransactionModel model)
        {
            CashTransaction master = null;
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageCashRecieveTransaction.SystemName, CurrentUser, PermissionActions.Create))
                   return AccessDeniedView();

                ValidateModel(model);

                if (ModelState.IsValid)
                {
                    TransactionEntrySetting transactionSettings = new TransactionEntrySetting();
                   
                    //التأكد من الحسابات الافتراضية ومواصفات حركة المبيعات إذا كان النظام يسمح بانشاء قيود الحركات
                    var msg = ValidateEntrySettings(transactionSettings, model);

                    if (msg.Length > 0)
                    {
                        ViewData["NotificationMsg"] = Notification.Success(msg, NotificationCssType.danger.ToString());

                        SetBasicButtonsVisibilityForCreate();
                        IntializeDropdowens();
                        return View(model);
                    }

                    var currentUser = CurrentUser;
                    if (!_CashTransactionService.IsExistRecord(b => b.Code == model.Code && b.FinancialPeriodId == currentUser.FinancialPeriodId && b.DocTypeId == (int)DocumentTypes.CashRecieveTransaction && b.CompanyId == CurrentUser.CompanyId))
                    {
                        master = _Mapper.Map<CashTransaction>(model);
                        master.SetBasicData(CRUD_OperationType.Create, (int)DocumentTypes.CashRecieveTransaction, currentUser);
                        var SystemSetting = _SystemSettingService.GetAll().FirstOrDefault();
                        if (SystemSetting!=null && SystemSetting.EnableTransactionsEntryCreation)
                        {
                            int EntryNumber = 0;
                            int EntryId = 0;
                            //add transaction entry
                            AddEntry(transactionSettings, master.Code, master.CurrencyId, master.CurrencyFactor, master.Amount, master.DiscountAmount, master.DiscountAccountId, out EntryNumber, out EntryId);

                            master.EntryNumber = EntryNumber;
                            master.EntryId = EntryId;
                        }                        

                        //save invoice Transaction
                        _CashTransactionService.Add(master);

                        return RedirectToAction("Edit", new { Id = master.Id, FromAction = "Create" });
                    }
                    else
                    {
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("DuplicateCode"), NotificationCssType.danger.ToString());
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
                return RedirectToAction("Edit", new { Id = master.Id, Action = "Create", ShowMessege = true });
            }
            else
            {
                SetBasicButtonsVisibilityForCreate();
                IntializeDropdowens();
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult Edit(int Id, string FromAction)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCashRecieveTransaction.SystemName, CurrentUser, PermissionActions.Edit))
             return AccessDeniedView();
            CashTransaction master = _CashTransactionService.GetById(Id);

            CashTransactionModel masterModel = _Mapper.Map<CashTransactionModel>(master);
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
        public IActionResult Edit(CashTransactionModel model)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageCashRecieveTransaction.SystemName, CurrentUser, PermissionActions.Edit))
                  return AccessDeniedView();

                ValidateModel(model);

                if (ModelState.IsValid)
                {
                    TransactionEntrySetting transactionSettings = new TransactionEntrySetting();
                    //التأكد من الحسابات الافتراضية ومواصفات حركة المبيعات إذا كان النظام يسمح بانشاء قيود الحركات
                    var msg = ValidateEntrySettings(transactionSettings,model);

                    if (msg.Length > 0)
                    {
                        ViewData["NotificationMsg"] = Notification.Success(msg, NotificationCssType.danger.ToString());

                        SetBasicButtonsVisibilityForEdit(model.Id);
                        IntializeDropdowens();
                        return View(model);
                    }

                    CashTransaction master = _Mapper.Map<CashTransaction>(model);
                    master.SetBasicData(SharedEnums.CRUD_OperationType.Update, (int)DocumentTypes.CashRecieveTransaction, CurrentUser); var SystemSetting = _SystemSettingService.GetAll().FirstOrDefault();

                    if (SystemSetting != null && SystemSetting.EnableTransactionsEntryCreation)
                    {
                        //تعديل قيد الحركة اذا كان موجود
                        if (master.EntryId > 0 && master.EntryNumber > 0)
                        {
                            UpdateEntry(transactionSettings, master.Code, master.CurrencyId, master.CurrencyFactor, master.Amount, master.DiscountAmount, master.DiscountAccountId, master.EntryId);
                        }
                        else
                        {
                            int EntryNumber = 0;
                            int EntryId = 0;
                            //اضافة قيد للحركة اذا كان غير موجود
                            AddEntry(transactionSettings, master.Code, master.CurrencyId, master.CurrencyFactor, master.Amount, master.DiscountAmount, master.DiscountAccountId, out EntryNumber, out EntryId);

                            master.EntryNumber = EntryNumber;
                            master.EntryId = EntryId;
                        }
                    }


                    _CashTransactionService.Update(master);

                    return RedirectToAction("Edit", new { Id = master.Id, FromAction = "Edit" });
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
                if (!_identityService.Authorize(StandardPermissionProvider.ManageCashRecieveTransaction.SystemName, CurrentUser, PermissionActions.Delete))
                  return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

                CashTransaction master = _CashTransactionService.GetById(Id);
                _CashTransactionService.Delete(master);

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
        public IActionResult Print(int Id)
        {
            if (Id > 0)
            {
                CashRecieveRptPrint report = new CashRecieveRptPrint(_LocalizationService);
                var DataSource = _ReportService.CashRecieveRptPrint(Id);
                report.DataSource = DataSource;
                var stream = report.GenerateReport("pdf");
                stream.Position = 0;
                return File(stream, "application/pdf", "CashRecieveRptPrint.pdf");
            }
            else
            {
                return View("Error");
            }
        }

        public IActionResult GetCurrencyFactor(int CurrencyId)
        {
            decimal factor = 0;
            var curuncy = _CurrencyService.GetById(CurrencyId);
            if (curuncy != null)
            {
                factor = curuncy.CurrencyChangrRate;
            }

            return Json(factor);
        }
        public IActionResult GetTransactionSide(int SideTypeId)
        {
            if (SideTypeId==(int)EntrySides.Account)
            {
                var accounts = _AccountsService.GetWithCondetion(x => x.LastLevelInTree).Select(a=>new { Name=a.AccountCode +"-"+ a.Name,a.Id }).ToList();
                return Json(accounts);
            }
            else if (SideTypeId == (int)EntrySides.Supplier)
            {
                var supplers = _SupplerService.GetAll().Select(x=>new {Name= _currentLanguage == "ar" ? x.NameAr : x.NameEn, Id=x.Id }).ToList();
                return Json(supplers);
            }
            else if (SideTypeId == (int)EntrySides.Customer)
            {
                var customers = _CustomerService.GetAll().Select(x => new { Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn, Id = x.Id }).ToList();
                return Json(customers);
            }
            else if (SideTypeId == (int)EntrySides.Bank)
            {
                var banks = _BankService.GetAll().Select(x => new { Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn, Id = x.Id }).ToList();
                return Json(banks);
            }
            else if (SideTypeId == (int)EntrySides.Treasury)
            {
                var treasurys = _TreasuryService.GetAll().Select(x => new { Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn, Id = x.Id }).ToList();
                return Json(treasurys);
            }
            else
            {
                return Json("");
            }

            
        }

        public IActionResult GetTransactionSideAccount(int SideTypeId, int SideId)
        {
            if (SideTypeId == (int)EntrySides.Account)
            {
                var accountId = _AccountsService.GetWithCondetion(x => x.LastLevelInTree && x.Id== SideId).Select(a => a.Id).FirstOrDefault();
                return Json(accountId);
            }
            else if (SideTypeId == (int)EntrySides.Supplier)
            {
                var accountId = _SupplerService.GetWithCondetion(x=>x.Id== SideId).Select(a => a.AccountId).FirstOrDefault();
                return Json(accountId);
            }
            else if (SideTypeId == (int)EntrySides.Customer)
            {     
                var accountId = _CustomerService.GetWithCondetion(x => x.Id == SideId).Select(a => a.AccountId).FirstOrDefault();
                return Json(accountId);
            }
            else if (SideTypeId == (int)EntrySides.Bank)
            {  
                var accountId = _BankService.GetWithCondetion(x => x.Id == SideId).Select(a => a.AccountId).FirstOrDefault();
                return Json(accountId);

            }
            else if (SideTypeId == (int)EntrySides.Treasury)
            {                
                var accountId = _TreasuryService.GetWithCondetion(x => x.Id == SideId).Select(a => a.AccountId).FirstOrDefault();
                return Json(accountId);
            }
            else
            {
                return Json(0);
            }


        }

        public string ValidateEntrySettings(TransactionEntrySetting transactionSettings, CashTransactionModel model)
        {
            StringBuilder msg = new StringBuilder();
            var SystemSetting = _SystemSettingService.GetAll().FirstOrDefault();
            if (SystemSetting != null && SystemSetting.EnableTransactionsEntryCreation)
            {
                //transactionSettings = new TransactionEntrySetting();
                var transSettings = _TransactionsEntrySettingService.GetWithCondetion(x => x.DocTypeId == (int)DocumentTypes.CashRecieveTransaction).FirstOrDefault();

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

                    transactionSettings.FirstSideAccountId = model.FirstSideAccountId;
                    transactionSettings.SecondSideAccountId = model.SecondSideAccountId;
                }              

            }

            return msg.ToString();
        }
        public void AddEntry(TransactionEntrySetting transactionSettings, int DocNumber, int CuruncyId, decimal CuruncyFactor, decimal Amount, decimal DiscountAmount,int DiscountAccountId,out int EntryNumber, out int EntryId)
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
                EntryMaster.DocType = (int)DocumentTypes.CashRecieveTransaction;
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



                EntryMaster.SetBasicData(CRUD_OperationType.Create, (int)DocumentTypes.CashRecieveTransaction, CurrentUser);
                #endregion

                #region EntryDetails
                //الطرف الاول
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




                //الطرف الثاني  
                DailyEntryDetails DailyEntry2 = new DailyEntryDetails();

                DailyEntry2.MasterId = EntryMaster.Id;
                DailyEntry2.AccountId = transactionSettings.SecondSideAccountId;
                DailyEntry2.CostCenterId = 0;
                if (transactionSettings.SecondSideNaturalId == (int)AccountNatures.Debit)
                {
                    DailyEntry2.Debit = Amount - DiscountAmount;
                    DailyEntry2.Credit = 0;
                }
                else
                {
                    DailyEntry2.Debit = 0;
                    DailyEntry2.Credit = Amount - DiscountAmount;
                }
                DailyEntry2.SetBasicData(CurrentUser, EntryMaster);
                EntryMaster.DailyEntryDetails.Add(DailyEntry2);

                if (DiscountAmount>0)
                {
                     
                    DailyEntryDetails DailyEntry3 = new DailyEntryDetails();

                    DailyEntry3.MasterId = EntryMaster.Id;
                    DailyEntry3.AccountId = DiscountAccountId;
                    DailyEntry3.CostCenterId = 0;
                    if (transactionSettings.SecondSideNaturalId == (int)AccountNatures.Debit)
                    {
                        DailyEntry3.Debit =DiscountAmount;
                        DailyEntry3.Credit = 0;
                    }
                    else
                    {
                        DailyEntry3.Debit = 0;
                        DailyEntry3.Credit = DiscountAmount;
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


        public void UpdateEntry(TransactionEntrySetting transactionSettings, int DocNumber, int CuruncyId, decimal CuruncyFactor, decimal Amount, decimal DiscountAmount, int DiscountAccountId, int EntryId)
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

                    oldEntryMaster.SetBasicData(CRUD_OperationType.Update, (int)DocumentTypes.CashRecieveTransaction, CurrentUser);
                    #endregion

                    #region EntryDetails

                    //الطرف الاول
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
                    EntryDetails.Add(DailyEntry1);

                    //الطرف الثاني  
                    DailyEntryDetails DailyEntry2 = new DailyEntryDetails();

                    DailyEntry2.MasterId = oldEntryMaster.Id;
                    DailyEntry2.AccountId = transactionSettings.SecondSideAccountId;
                    DailyEntry2.CostCenterId = 0;
                    if (transactionSettings.SecondSideNaturalId == (int)AccountNatures.Debit)
                    {
                        DailyEntry2.Debit = Amount - DiscountAmount;
                        DailyEntry2.Credit = 0;
                    }
                    else
                    {
                        DailyEntry2.Debit = 0;
                        DailyEntry2.Credit = Amount - DiscountAmount;
                    }
                    DailyEntry2.SetBasicData(CurrentUser, oldEntryMaster);
                    EntryDetails.Add(DailyEntry2);

                    if (DiscountAmount > 0)
                    {

                        DailyEntryDetails DailyEntry3 = new DailyEntryDetails();

                        DailyEntry3.MasterId = oldEntryMaster.Id;
                        DailyEntry3.AccountId = DiscountAccountId;
                        DailyEntry3.CostCenterId = 0;
                        if (transactionSettings.SecondSideNaturalId == (int)AccountNatures.Debit)
                        {
                            DailyEntry3.Debit = DiscountAmount;
                            DailyEntry3.Credit = 0;
                        }
                        else
                        {
                            DailyEntry3.Debit = 0;
                            DailyEntry3.Credit = DiscountAmount;
                        }
                        DailyEntry3.SetBasicData(CurrentUser, oldEntryMaster);
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
