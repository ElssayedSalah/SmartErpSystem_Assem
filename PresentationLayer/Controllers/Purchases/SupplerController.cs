using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models.Purchases;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.Purchases;
using DataAccessLayer.Entities.System;
using Grand.Framework.Kendoui;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using static BusinessLayer.Helpers.SharedEnums;


namespace PresentationLayer.Controllers.Sales
{

    [Authorize]
    public class SupplerController : BaseAdminController
    {
        private readonly IBaseService<Suppler> _SupplerService;
        private readonly IBaseService<SupplerOpenBalance> _SupplerOpenBalanceService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly IidentityService _identityService;
        private readonly IBaseService<Countries> _CountriesService;
        private readonly IWebHelper _webHelper;
        private readonly IFinanceService<DailyEntryMaster, DailyEntryDetails> _DailyEntryService;
        private readonly IBaseService<Currency> _CurrencyService;
        private readonly IBaseService<DefaultAccount> _DefaultAccountService;
        private readonly IBaseService<Account> _AccountService;
        private readonly IBaseService<SystemSetting> _SystemSettingService;




        public SupplerController(IBaseService<Suppler> SupplerService, IMapper mapper, LocalizationService localizationService, IidentityService IdentityService, IBaseService<Countries> CountriesService, IWebHelper webHelper, IBaseService<SupplerOpenBalance> SupplerOpenBalanceService, IFinanceService<DailyEntryMaster, DailyEntryDetails> DailyEntryService, IBaseService<Currency> CurrencyService, IBaseService<DefaultAccount> DefaultAccountService, IBaseService<Account> AccountService, IBaseService<SystemSetting> SystemSettingService)
        {
            _SupplerService = SupplerService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _identityService = IdentityService;
            _CountriesService = CountriesService;
            _webHelper = webHelper;
            _SupplerOpenBalanceService = SupplerOpenBalanceService;
            _DailyEntryService = DailyEntryService;
            _CurrencyService = CurrencyService;
            _DefaultAccountService = DefaultAccountService;
            _AccountService = AccountService;
            _SystemSettingService = SystemSettingService;
        }

        public void IntializeDropdowens()
        {
           
            ViewBag.SupplerClassType = PresentationExtensions.ConvertEnumToSelectListItems2(typeof(SupplerClassType), _LocalizationService);
            ViewBag.IDType = PresentationExtensions.ConvertEnumToSelectListItems2(typeof(IDType), _LocalizationService);

            var Countries = _CountriesService.GetAll();
            Countries.Insert(0, new Countries() { Id = 0, NameAr = "إختار", NameEn = "Select" });
            ViewBag.Countries = Countries.Select(x => new { x.Id, Name =  x.Name});

            var accounts = _AccountService.GetWithCondetion(x=>x.LastLevelInTree);
            accounts.Insert(0, new Account() { Id = 0, NameAr = "إختار", NameEn = "Select" });
            ViewBag.Accounts = accounts.Select(x => new { x.Id, Name = x.Name });

        }
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageSuppleres.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            return View(new SupplerModel());
        }

        public IActionResult list()
        {
            var Suppler = _SupplerService.GetAll();
            var SupplerModel = _Mapper.Map<List<SupplerModel>>(Suppler);
            var gridModel = new DataSourceResult
            {
                Data = SupplerModel,
                Total = SupplerModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageSuppleres.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();

            var LastCode = _SupplerService.GetLastCode(o => o.Code);
            var NewSuppler = new Suppler() { Code = LastCode };

            var NewSupplerModel = _Mapper.Map<SupplerModel>(NewSuppler);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Suppler", CreateNewBtnAction = "Create", BackToListControler = "Suppler", BackToListAction = "Index", EditBtnControler = "Suppler", EditBtnAction = "Edit", DeleteBtnControler = "Suppler", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
            IntializeDropdowens();
            return View(NewSupplerModel);
        }
        [HttpPost]
        public IActionResult Create(SupplerModel SupplerModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageSuppleres.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();

                if (ModelState.IsValid)
                {
                    var SystemSetting = _SystemSettingService.GetAll().FirstOrDefault();
                    //حساب رأس المال من الحسابات الافتراضية
                    var CapitalAccount = _DefaultAccountService.GetWithCondetion(x => x.AccountNameId == 4).FirstOrDefault();

                    if ((SystemSetting!=null && SystemSetting.EnableOpenEntryCreation) && (SupplerModel.AccountId<=0 || CapitalAccount ==null || CapitalAccount.AccountId<=0))
                    {
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("SupplerAccountAndCapitalAccountRequired"), NotificationCssType.danger.ToString());

                        //set basic buttons visibility
                        ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Suppler", CreateNewBtnAction = "Create", BackToListControler = "Suppler", BackToListAction = "Index", EditBtnControler = "Suppler", EditBtnAction = "Edit", DeleteBtnControler = "Suppler", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
                        IntializeDropdowens();
                        return View(SupplerModel);
                    }

                    if (!_SupplerService.IsExistRecord(b => b.Code == SupplerModel.Code))
                    {

                        Suppler Suppler = _Mapper.Map<Suppler>(SupplerModel);
                        Suppler.SetBasicData(CRUD_OperationType.Create, CurrentUser);
                        _SupplerService.Add(Suppler);
                        SupplerOpenBalance supplerOpenBalance = new SupplerOpenBalance();
                        supplerOpenBalance.SupplerId = Suppler.Id;
                        supplerOpenBalance.FinancialPeriodId = CurrentUser.FinancialPeriodId;
                        supplerOpenBalance.CompanyId = CurrentUser.CompanyId;
                        supplerOpenBalance.AccountId = SupplerModel.AccountId;
                        supplerOpenBalance.EntryNumber = SupplerModel.EntryNumber;
                        supplerOpenBalance.OpeningBalanceCredit = SupplerModel.OpeningBalanceCredit;
                        supplerOpenBalance.OpeningBalanceDebit = SupplerModel.OpeningBalanceDebit;
                        _SupplerOpenBalanceService.Add(supplerOpenBalance);

                        //add open entry for supplier
                        if (SystemSetting != null && SystemSetting.EnableOpenEntryCreation &&  CapitalAccount != null && SupplerModel.AccountId > 0 && CapitalAccount.AccountId > 0)
                        {
                          AddSupplierOpenEntry(Suppler, supplerOpenBalance);
                        }
                            


                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("SavedSuccessfuly"), NotificationCssType.success.ToString());

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
            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Suppler", CreateNewBtnAction = "Create", BackToListControler = "Suppler", BackToListAction = "Index", EditBtnControler = "Suppler", EditBtnAction = "Edit", DeleteBtnControler = "Suppler", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
            IntializeDropdowens();
            return View(SupplerModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageSuppleres.SystemName, CurrentUser, PermissionActions.Delete))
                return AccessDeniedView();

            Suppler Suppler = _SupplerService.GetById(Id);
            SupplerModel SupplerModel = _Mapper.Map<SupplerModel>(Suppler);
            SupplerOpenBalance supplerOpenBalance = _SupplerOpenBalanceService.GetWithCondetion(x => x.SupplerId == Suppler.Id && x.FinancialPeriodId.Value == CurrentUser.FinancialPeriodId.Value && x.CompanyId.Value == CurrentUser.CompanyId.Value).FirstOrDefault();
            if (supplerOpenBalance!=null)
            {
                SupplerModel.OpeningBalanceDebit = supplerOpenBalance.OpeningBalanceDebit;
                SupplerModel.OpeningBalanceCredit = supplerOpenBalance.OpeningBalanceCredit;
            }

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Suppler", CreateNewBtnAction = "Create", BackToListControler = "Suppler", BackToListAction = "Index", EditBtnControler = "Suppler", EditBtnAction = "Edit", DeleteBtnControler = "Suppler", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            IntializeDropdowens();

            return View(SupplerModel);
        }
        [HttpPost]
        public IActionResult Edit(SupplerModel SupplerModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageSuppleres.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();

                if (ModelState.IsValid)
                {
                    var SystemSetting = _SystemSettingService.GetAll().FirstOrDefault();
                    //حساب رأس المال من الحسابات الافتراضية
                    var CapitalAccount = _DefaultAccountService.GetWithCondetion(x => x.AccountNameId == 4).FirstOrDefault();

                    if ((SystemSetting != null && SystemSetting.EnableOpenEntryCreation) && (SupplerModel.AccountId <= 0 || CapitalAccount == null || CapitalAccount.AccountId <= 0))
                    {
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("SupplerAccountAndCapitalAccountRequired"), NotificationCssType.danger.ToString());

                        //set basic buttons visibility
                        ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Suppler", CreateNewBtnAction = "Create", BackToListControler = "Suppler", BackToListAction = "Index", EditBtnControler = "Suppler", EditBtnAction = "Edit", DeleteBtnControler = "Suppler", DeleteBtnAction = "Delete", RouteId = SupplerModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };

                        IntializeDropdowens();

                        return View(SupplerModel);
                    }

                    Suppler Suppler = _Mapper.Map<Suppler>(SupplerModel);
                    //var currentUser = _identityService.GetCurrentUser();
                    Suppler.SetBasicData(SharedEnums.CRUD_OperationType.Update, CurrentUser);
                    _SupplerService.Update(Suppler);

                    SupplerOpenBalance supplerOpenBalance = _SupplerOpenBalanceService.GetWithCondetion(x=>x.SupplerId==Suppler.Id&& x.FinancialPeriodId.Value==CurrentUser.FinancialPeriodId.Value&&x.CompanyId.Value==CurrentUser.CompanyId.Value).FirstOrDefault();
                    if (supplerOpenBalance!=null)
                    {
                        supplerOpenBalance.OpeningBalanceCredit = SupplerModel.OpeningBalanceCredit;
                        supplerOpenBalance.OpeningBalanceDebit = SupplerModel.OpeningBalanceDebit;
                        _SupplerOpenBalanceService.Update(supplerOpenBalance);
                    }
                    else
                    {
                        supplerOpenBalance = new SupplerOpenBalance();
                        supplerOpenBalance.SupplerId = Suppler.Id;
                        supplerOpenBalance.FinancialPeriodId = CurrentUser.FinancialPeriodId;
                        supplerOpenBalance.CompanyId = CurrentUser.CompanyId;
                        supplerOpenBalance.AccountId = SupplerModel.AccountId;
                        supplerOpenBalance.EntryNumber = SupplerModel.EntryNumber;
                        supplerOpenBalance.OpeningBalanceCredit = SupplerModel.OpeningBalanceCredit;
                        supplerOpenBalance.OpeningBalanceDebit = SupplerModel.OpeningBalanceDebit;
                        _SupplerOpenBalanceService.Add(supplerOpenBalance);
                    }

                    //add open entry for supplier
                    if (SystemSetting != null && SystemSetting.EnableOpenEntryCreation && CapitalAccount != null && SupplerModel.AccountId > 0 && CapitalAccount.AccountId > 0)
                    {
                        AddSupplierOpenEntry(Suppler, supplerOpenBalance);
                    }


                    ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("UpdatedSuccessfuly"), NotificationCssType.success.ToString());

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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Suppler", CreateNewBtnAction = "Create", BackToListControler = "Suppler", BackToListAction = "Index", EditBtnControler = "Suppler", EditBtnAction = "Edit", DeleteBtnControler = "Suppler", DeleteBtnAction = "Delete", RouteId = SupplerModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };

            IntializeDropdowens();

            return View(SupplerModel);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageSuppleres.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
           
            Suppler Suppler = _SupplerService.GetById(Id);
            SupplerModel SupplerModel = _Mapper.Map<SupplerModel>(Suppler);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Suppler", CreateNewBtnAction = "Create", BackToListControler = "Suppler", BackToListAction = "Index", EditBtnControler = "Suppler", EditBtnAction = "Edit", DeleteBtnControler = "Suppler", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = true, BackToListBtnVisibilty = true, SaveBtnVisibilty = false, DeleteBtnVisibilty = true };

            return View(SupplerModel);
        }

        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageSuppleres.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

                var msg = _webHelper.IsAllowedDeleteSuppler(Id);
                if (msg != "")
                {
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString(msg).Value });
                }

                Suppler Suppler = _SupplerService.GetById(Id);
                _SupplerService.Delete(Suppler);

                var supplerEntry = _DailyEntryService.GetById(Suppler.EntryId);
                if (supplerEntry != null)
                {
                    _DailyEntryService.Delete(supplerEntry);
                }

                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });


            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }

        }

        public void AddSupplierOpenEntry(Suppler suppler,SupplerOpenBalance supplerOpenBalance)
        {
            decimal Amount = 0;
            if (suppler.OpeningBalanceDebit > 0)
            {
                Amount = suppler.OpeningBalanceDebit;
            }
            else if (suppler.OpeningBalanceCredit > 0)
            {
                Amount = suppler.OpeningBalanceCredit;

            }

            //حساب رأس المال من الحسابات الافتراضية
            var CapitalAccount = _DefaultAccountService.GetWithCondetion(x => x.AccountNameId == 4).FirstOrDefault();

            if (Amount > 0 && suppler.AccountId>0 && CapitalAccount!=null && CapitalAccount.AccountId>0)
            {
                var oldEntry = _DailyEntryService.GetWithCondetion(x=>x.EntryNumber==suppler.EntryNumber && x.FinancialPeriodId==CurrentUser.FinancialPeriodId && x.CompanyId==CurrentUser.CompanyId && x.DocType== (int)DocumentTypes.SupplierCreation).FirstOrDefault();

                if (oldEntry==null)
                {
                    var LastEntryNumber = _DailyEntryService.GetLastEntryNumber(o => o.EntryNumber, x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId);

                    var LastDocumentNumber = _DailyEntryService.GetLastCode(o => o.Code, x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId && x.DocType == (int)DocumentTypes.SupplierCreation);

                    var DefaultCuruncy = _CurrencyService.GetWithCondetion(x => x.DefaultCurrency).FirstOrDefault();

                    var EntryMaster = new DailyEntryMaster()
                    {
                        EntryNumber = LastEntryNumber,
                        TransactionDate = DateTime.Now,
                        DocNumber = LastDocumentNumber,
                        Code = LastDocumentNumber,
                        EntryCreationMethod = (int)EntryCreationMethod.Automatic,
                        CurrencyId = DefaultCuruncy.Id,
                        CurrencyFactor = DefaultCuruncy.CurrencyChangrRate,
                        DailyTypeId = 0,
                        EntryState = (int)EntryBalanceState.Balanced,
                        EntryType = (int)EntryType.Open,
                        IsTransfered = (int)EntryTransferState.Transfered,
                        TotalCredit = Amount,
                        TotalDebit = Amount,
                        DailyEntryDetails = new List<DailyEntryDetails>()


                    };

                    EntryMaster.SetBasicData(CRUD_OperationType.Create, (int)DocumentTypes.SupplierCreation, CurrentUser);

                    //الطرف الاول للقيد مدين لحساب المورد
                    DailyEntryDetails DailyEntry1 = new DailyEntryDetails()
                    {
                        MasterId = EntryMaster.Id,
                        AccountId = suppler.AccountId,
                        CostCenterId = 0,
                        Debit = Amount,
                        Credit = 0,

                    };
                    DailyEntry1.SetBasicData(CurrentUser, EntryMaster);

                    //الطرف الثاني للقيد داين لحساب رأس المال
                    DailyEntryDetails DailyEntry2 = new DailyEntryDetails()
                    {
                        MasterId = EntryMaster.Id,
                        AccountId = CapitalAccount.AccountId,
                        CostCenterId = 0,
                        Debit = 0,
                        Credit = Amount,

                    };
                    DailyEntry2.SetBasicData(CurrentUser, EntryMaster);

                    EntryMaster.DailyEntryDetails.Add(DailyEntry1);
                    EntryMaster.DailyEntryDetails.Add(DailyEntry2);
                    _DailyEntryService.AddEntry(EntryMaster);

                    suppler.EntryNumber = EntryMaster.EntryNumber;
                    supplerOpenBalance.EntryNumber = EntryMaster.EntryNumber;
                    _SupplerService.Update(suppler);
                    _SupplerOpenBalanceService.Update(supplerOpenBalance);
                }
                else
                {
                    var oldEntryDetails = _DailyEntryService.GetEntryDetails(x=>x.MasterId==oldEntry.Id);
                    oldEntry.SetBasicData(CRUD_OperationType.Update, (int)DocumentTypes.SupplierCreation, CurrentUser);

                    //الطرف الاول للقيد مدين لحساب المورد
                    DailyEntryDetails DailyEntry1 = new DailyEntryDetails()
                    {
                        MasterId = oldEntry.Id,
                        AccountId = suppler.AccountId,
                        CostCenterId = 0,
                        Debit = Amount,
                        Credit = 0,

                    };
                    DailyEntry1.SetBasicData(CurrentUser, oldEntry);

                    //الطرف الثاني للقيد داين لحساب رأس المال
                    DailyEntryDetails DailyEntry2 = new DailyEntryDetails()
                    {
                        MasterId = oldEntry.Id,
                        AccountId = CapitalAccount.AccountId,
                        CostCenterId = 0,
                        Debit = 0,
                        Credit = Amount,

                    };
                    DailyEntry2.SetBasicData(CurrentUser, oldEntry);

                    oldEntry.DailyEntryDetails.Add(DailyEntry1);
                    oldEntry.DailyEntryDetails.Add(DailyEntry2);
                    _DailyEntryService.UpdateEntry(oldEntry, oldEntryDetails);                  
                   

                }


              
            }

        }
    }


}
