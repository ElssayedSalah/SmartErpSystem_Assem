using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models.Financial;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.System;
using Grand.Framework.Kendoui;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static BusinessLayer.Helpers.SharedEnums;


namespace PresentationLayer.Controllers.Sales
{

    [Authorize]
    public class BankController : BaseAdminController
    {
        private readonly IBaseService<Bank> _BankService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly IidentityService _identityService;
        private readonly IBaseService<Currency> _CurrencyService;
        private readonly IWebHelper _webHelper;
        private readonly IBaseService<BankOpenBalance> _BankOpenBalanceService;
        private readonly string _currentLanguage;
        private readonly IBaseService<Account> _AccountService;

        public BankController(IBaseService<Bank> BankService, IMapper mapper, LocalizationService localizationService, IidentityService IdentityService, IBaseService<Currency> CurrencyService, IWebHelper webHelper, IBaseService<BankOpenBalance> BankOpenBalanceService, IBaseService<Account> AccountService)
        {
            _BankService = BankService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _identityService = IdentityService;
            _CurrencyService = CurrencyService;
            _webHelper = webHelper;
            _BankOpenBalanceService = BankOpenBalanceService;
            _AccountService = AccountService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
        }

        public void IntializeDropdowens()
        {          

            var Currencies = _CurrencyService.GetAll();
            ViewBag.Currencies = Currencies.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var accounts = _AccountService.GetWithCondetion(x => x.LastLevelInTree);
            accounts.Insert(0, new Account() { Id = 0, NameAr = "إختار", NameEn = "Select" });
            ViewBag.Accounts = accounts.Select(x => new { x.Id, Name = x.Name });
        }
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageBankes.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            return View(new BankModel());
        }

        public IActionResult list()
        {
            var Bank = _BankService.GetAll();
            var BankModel = _Mapper.Map<List<BankModel>>(Bank);
            var gridModel = new DataSourceResult
            {
                Data = BankModel,
                Total = BankModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageBankes.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();

            var LastCode = _BankService.GetLastCode(o => o.Code);
            var NewBank = new Bank() { Code = LastCode };

            var NewBankModel = _Mapper.Map<BankModel>(NewBank);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Bank", CreateNewBtnAction = "Create", BackToListControler = "Bank", BackToListAction = "Index", EditBtnControler = "Bank", EditBtnAction = "Edit", DeleteBtnControler = "Bank", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
            IntializeDropdowens();
            return View(NewBankModel);
        }
        [HttpPost]
        public IActionResult Create(BankModel BankModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageBankes.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();

                if (ModelState.IsValid)
                {
                    if (!_BankService.IsExistRecord(b => b.Code == BankModel.Code))
                    {
                        Bank Bank = _Mapper.Map<Bank>(BankModel);
                        Bank.SetBasicData(CRUD_OperationType.Create, CurrentUser);
                        _BankService.Add(Bank);

                        BankOpenBalance BankOpenBalance = new BankOpenBalance();
                        BankOpenBalance.BankId = Bank.Id;
                        BankOpenBalance.FinancialPeriodId = CurrentUser.FinancialPeriodId;
                        BankOpenBalance.CompanyId = CurrentUser.CompanyId;                       
                        BankOpenBalance.EntryNumber = BankModel.EntryNumber;
                        BankOpenBalance.OpenBalanceCredit = BankModel.OpenBalanceCredit;
                        BankOpenBalance.OpenBalanceDebit = BankModel.OpenBalanceDebit;
                        _BankOpenBalanceService.Add(BankOpenBalance);

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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Bank", CreateNewBtnAction = "Create", BackToListControler = "Bank", BackToListAction = "Index", EditBtnControler = "Bank", EditBtnAction = "Edit", DeleteBtnControler = "Bank", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
            IntializeDropdowens();
            return View(BankModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageBankes.SystemName, CurrentUser, PermissionActions.Delete))
                return AccessDeniedView();

            Bank Bank = _BankService.GetById(Id);
            BankModel BankModel = _Mapper.Map<BankModel>(Bank);

            BankOpenBalance BankOpenBalance = _BankOpenBalanceService.GetWithCondetion(x => x.BankId == Bank.Id && x.FinancialPeriodId.Value == CurrentUser.FinancialPeriodId.Value && x.CompanyId.Value == CurrentUser.CompanyId.Value).FirstOrDefault();

            if (BankOpenBalance != null)
            {
                BankModel.OpenBalanceDebit = BankOpenBalance.OpenBalanceDebit;
                BankModel.OpenBalanceCredit = BankOpenBalance.OpenBalanceCredit;
            }

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Bank", CreateNewBtnAction = "Create", BackToListControler = "Bank", BackToListAction = "Index", EditBtnControler = "Bank", EditBtnAction = "Edit", DeleteBtnControler = "Bank", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            IntializeDropdowens();

            return View(BankModel);
        }
        [HttpPost]
        public IActionResult Edit(BankModel BankModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageBankes.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();

                if (ModelState.IsValid)
                {
                    Bank Bank = _Mapper.Map<Bank>(BankModel);
                    //var currentUser = _identityService.GetCurrentUser();
                    Bank.SetBasicData(SharedEnums.CRUD_OperationType.Update, CurrentUser);
                    _BankService.Update(Bank);

                    BankOpenBalance BankOpenBalance = _BankOpenBalanceService.GetWithCondetion(x => x.BankId == Bank.Id && x.FinancialPeriodId.Value == CurrentUser.FinancialPeriodId.Value && x.CompanyId.Value == CurrentUser.CompanyId.Value).FirstOrDefault();
                    if (BankOpenBalance!=null)
                    {
                        BankOpenBalance.OpenBalanceCredit = BankModel.OpenBalanceCredit;
                        BankOpenBalance.OpenBalanceDebit = BankModel.OpenBalanceDebit;
                        _BankOpenBalanceService.Update(BankOpenBalance);
                    }
                    else
                    {
                        BankOpenBalance NewBankOpenBalance = new BankOpenBalance();
                        NewBankOpenBalance.BankId = Bank.Id;
                        NewBankOpenBalance.FinancialPeriodId = CurrentUser.FinancialPeriodId;
                        NewBankOpenBalance.CompanyId = CurrentUser.CompanyId;
                        NewBankOpenBalance.EntryNumber = BankModel.EntryNumber;
                        NewBankOpenBalance.OpenBalanceCredit = BankModel.OpenBalanceCredit;
                        NewBankOpenBalance.OpenBalanceDebit = BankModel.OpenBalanceDebit;
                        _BankOpenBalanceService.Add(NewBankOpenBalance);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Bank", CreateNewBtnAction = "Create", BackToListControler = "Bank", BackToListAction = "Index", EditBtnControler = "Bank", EditBtnAction = "Edit", DeleteBtnControler = "Bank", DeleteBtnAction = "Delete", RouteId = BankModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };

            IntializeDropdowens();

            return View(BankModel);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageBankes.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
           
            Bank Bank = _BankService.GetById(Id);
            BankModel BankModel = _Mapper.Map<BankModel>(Bank);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Bank", CreateNewBtnAction = "Create", BackToListControler = "Bank", BackToListAction = "Index", EditBtnControler = "Bank", EditBtnAction = "Edit", DeleteBtnControler = "Bank", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = true, BackToListBtnVisibilty = true, SaveBtnVisibilty = false, DeleteBtnVisibilty = true };

            return View(BankModel);
        }

        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageBankes.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

                var msg = _webHelper.IsAllowedDeleteBank(Id);
                if (msg != "")
                {
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString(msg).Value });
                }

                Bank Bank = _BankService.GetById(Id);
                _BankService.Delete(Bank);

                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });


            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }

        }

    }


}
