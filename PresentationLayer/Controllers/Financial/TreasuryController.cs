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
    public class TreasuryController : BaseAdminController
    {
        private readonly IBaseService<Treasury> _TreasuryService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly IidentityService _identityService;
        private readonly IBaseService<Currency> _CurrencyService;
        private readonly IWebHelper _webHelper;
        private readonly IBaseService<TreasuryOpenBalance> _TreasuryOpenBalanceService;
        private readonly string _currentLanguage;
        private readonly IBaseService<Branch> _BranchService;
        private readonly IBaseService<Account> _AccountService;

        public TreasuryController(IBaseService<Treasury> TreasuryService, IMapper mapper, LocalizationService localizationService, IidentityService IdentityService, IBaseService<Currency> CurrencyService, IWebHelper webHelper, IBaseService<TreasuryOpenBalance> TreasuryOpenBalanceService, IBaseService<Branch> BranchService, IBaseService<Account> AccountService)
        {
            _TreasuryService = TreasuryService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _identityService = IdentityService;
            _CurrencyService = CurrencyService;
            _webHelper = webHelper;
            _TreasuryOpenBalanceService = TreasuryOpenBalanceService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _BranchService = BranchService;
            _AccountService = AccountService;

        }

        public void IntializeDropdowens()
        {          

            var Currencies = _CurrencyService.GetAll();
            ViewBag.Currencies = Currencies.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            ViewBag.Branches = _BranchService.GetAll().Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var accounts = _AccountService.GetWithCondetion(x => x.LastLevelInTree);
            accounts.Insert(0, new Account() { Id = 0, NameAr = "إختار", NameEn = "Select" });
            ViewBag.Accounts = accounts.Select(x => new { x.Id, Name = x.Name });
        }
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageTreasuryes.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            return View(new TreasuryModel());
        }

        public IActionResult list()
        {
            var Treasury = _TreasuryService.GetAll();
            var TreasuryModel = _Mapper.Map<List<TreasuryModel>>(Treasury);
            var gridModel = new DataSourceResult
            {
                Data = TreasuryModel,
                Total = TreasuryModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageTreasuryes.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();

            var LastCode = _TreasuryService.GetLastCode(o => o.Code);
            var NewTreasury = new Treasury() { Code = LastCode };

            var NewTreasuryModel = _Mapper.Map<TreasuryModel>(NewTreasury);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Treasury", CreateNewBtnAction = "Create", BackToListControler = "Treasury", BackToListAction = "Index", EditBtnControler = "Treasury", EditBtnAction = "Edit", DeleteBtnControler = "Treasury", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
            IntializeDropdowens();
            return View(NewTreasuryModel);
        }
        [HttpPost]
        public IActionResult Create(TreasuryModel TreasuryModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageTreasuryes.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();

                if (ModelState.IsValid)
                {
                    if (!_TreasuryService.IsExistRecord(b => b.Code == TreasuryModel.Code))
                    {
                        Treasury Treasury = _Mapper.Map<Treasury>(TreasuryModel);
                        Treasury.SetBasicData(CRUD_OperationType.Create, CurrentUser);
                        _TreasuryService.Add(Treasury);

                        TreasuryOpenBalance TreasuryOpenBalance = new TreasuryOpenBalance();
                        TreasuryOpenBalance.TreasuryId = Treasury.Id;
                        TreasuryOpenBalance.FinancialPeriodId = CurrentUser.FinancialPeriodId;
                        TreasuryOpenBalance.CompanyId = CurrentUser.CompanyId;                       
                        TreasuryOpenBalance.EntryNumber = TreasuryModel.EntryNumber;
                        TreasuryOpenBalance.OpenBalanceCredit = TreasuryModel.OpenBalanceCredit;
                        TreasuryOpenBalance.OpenBalanceDebit = TreasuryModel.OpenBalanceDebit;
                        _TreasuryOpenBalanceService.Add(TreasuryOpenBalance);

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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Treasury", CreateNewBtnAction = "Create", BackToListControler = "Treasury", BackToListAction = "Index", EditBtnControler = "Treasury", EditBtnAction = "Edit", DeleteBtnControler = "Treasury", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
            IntializeDropdowens();
            return View(TreasuryModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageTreasuryes.SystemName, CurrentUser, PermissionActions.Delete))
                return AccessDeniedView();

            Treasury Treasury = _TreasuryService.GetById(Id);
            TreasuryModel TreasuryModel = _Mapper.Map<TreasuryModel>(Treasury);

            TreasuryOpenBalance TreasuryOpenBalance = _TreasuryOpenBalanceService.GetWithCondetion(x => x.TreasuryId == Treasury.Id && x.FinancialPeriodId.Value == CurrentUser.FinancialPeriodId.Value && x.CompanyId.Value == CurrentUser.CompanyId.Value).FirstOrDefault();

            if (TreasuryOpenBalance != null)
            {
                TreasuryModel.OpenBalanceDebit = TreasuryOpenBalance.OpenBalanceDebit;
                TreasuryModel.OpenBalanceCredit = TreasuryOpenBalance.OpenBalanceCredit;
            }

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Treasury", CreateNewBtnAction = "Create", BackToListControler = "Treasury", BackToListAction = "Index", EditBtnControler = "Treasury", EditBtnAction = "Edit", DeleteBtnControler = "Treasury", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            IntializeDropdowens();

            return View(TreasuryModel);
        }
        [HttpPost]
        public IActionResult Edit(TreasuryModel TreasuryModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageTreasuryes.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();

                if (ModelState.IsValid)
                {
                    Treasury Treasury = _Mapper.Map<Treasury>(TreasuryModel);
                    //var currentUser = _identityService.GetCurrentUser();
                    Treasury.SetBasicData(SharedEnums.CRUD_OperationType.Update, CurrentUser);
                    _TreasuryService.Update(Treasury);

                    TreasuryOpenBalance TreasuryOpenBalance = _TreasuryOpenBalanceService.GetWithCondetion(x => x.TreasuryId == Treasury.Id && x.FinancialPeriodId.Value == CurrentUser.FinancialPeriodId.Value && x.CompanyId.Value == CurrentUser.CompanyId.Value).FirstOrDefault();
                    if (TreasuryOpenBalance!=null)
                    {
                        TreasuryOpenBalance.OpenBalanceCredit = TreasuryModel.OpenBalanceCredit;
                        TreasuryOpenBalance.OpenBalanceDebit = TreasuryModel.OpenBalanceDebit;
                        _TreasuryOpenBalanceService.Update(TreasuryOpenBalance);
                    }
                    else
                    {
                        TreasuryOpenBalance NewTreasuryOpenBalance = new TreasuryOpenBalance();
                        NewTreasuryOpenBalance.TreasuryId = Treasury.Id;
                        NewTreasuryOpenBalance.FinancialPeriodId = CurrentUser.FinancialPeriodId;
                        NewTreasuryOpenBalance.CompanyId = CurrentUser.CompanyId;
                        NewTreasuryOpenBalance.EntryNumber = TreasuryModel.EntryNumber;
                        NewTreasuryOpenBalance.OpenBalanceCredit = TreasuryModel.OpenBalanceCredit;
                        NewTreasuryOpenBalance.OpenBalanceDebit = TreasuryModel.OpenBalanceDebit;
                        _TreasuryOpenBalanceService.Add(NewTreasuryOpenBalance);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Treasury", CreateNewBtnAction = "Create", BackToListControler = "Treasury", BackToListAction = "Index", EditBtnControler = "Treasury", EditBtnAction = "Edit", DeleteBtnControler = "Treasury", DeleteBtnAction = "Delete", RouteId = TreasuryModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };

            IntializeDropdowens();

            return View(TreasuryModel);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageTreasuryes.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
           
            Treasury Treasury = _TreasuryService.GetById(Id);
            TreasuryModel TreasuryModel = _Mapper.Map<TreasuryModel>(Treasury);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Treasury", CreateNewBtnAction = "Create", BackToListControler = "Treasury", BackToListAction = "Index", EditBtnControler = "Treasury", EditBtnAction = "Edit", DeleteBtnControler = "Treasury", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = true, BackToListBtnVisibilty = true, SaveBtnVisibilty = false, DeleteBtnVisibilty = true };

            return View(TreasuryModel);
        }

        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageTreasuryes.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

                var msg = _webHelper.IsAllowedDeleteTreasury(Id);
                if (msg != "")
                {
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString(msg).Value });
                }

                Treasury Treasury = _TreasuryService.GetById(Id);
                _TreasuryService.Delete(Treasury);

                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });


            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }

        }

    }


}
