using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models.Inventory;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Inventory;
using Grand.Framework.Kendoui;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Controllers;
using PresentationLayer.Helpers;
using System;
using System.Collections.Generic;
using static BusinessLayer.Helpers.SharedEnums;

namespace SmartErpApp.Controllers.Inventory
{
    [Authorize]
    //[ValidateAntiForgeryToken]
    public class CurrencyController : BaseAdminController
    {
        private readonly IBaseService<Currency> _CurrencyService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly IidentityService _identityService;
        private readonly IWebHelper _webHelper;

        public CurrencyController(IBaseService<Currency> CurrencyService, IMapper mapper, LocalizationService localizationService, IidentityService IdentityService, IWebHelper webHelper)
        {
            _CurrencyService = CurrencyService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _identityService = IdentityService;
            _webHelper = webHelper;

        }
        // GET: CurrencyController
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCurrencys.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            return View(new CurrencyModel());
        
        }      
        public IActionResult list()
        {
            var Currency = _CurrencyService.GetAll();
            var CurrencyModel = _Mapper.Map<List<CurrencyModel>>(Currency);
            var gridModel = new DataSourceResult
            {
                Data = CurrencyModel,
                Total = CurrencyModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCurrencys.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();
            var LastCode = _CurrencyService.GetLastCode(o => o.Code);
            var NewCurrency = new Currency() { Code = LastCode };

            var NewCurrencyModel = _Mapper.Map<CurrencyModel>(NewCurrency);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Currency", CreateNewBtnAction = "Create", BackToListControler = "Currency", BackToListAction = "Index", EditBtnControler = "Currency", EditBtnAction = "Edit", DeleteBtnControler = "Currency", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            return View(NewCurrencyModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CurrencyModel CurrencyModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageCurrencys.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    if (!_CurrencyService.IsExistRecord(b => b.Code == CurrencyModel.Code))
                    {
                        //var currentUser = _identityService.GetCurrentUser();
                        Currency Currency = _Mapper.Map<Currency>(CurrencyModel);
                        Currency.SetBasicData(SharedEnums.CRUD_OperationType.Create, CurrentUser);
                        _CurrencyService.Add(Currency);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Currency", CreateNewBtnAction = "Create", BackToListControler = "Currency", BackToListAction = "Index", EditBtnControler = "Currency", EditBtnAction = "Edit", DeleteBtnControler = "Currency", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            return View(CurrencyModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCurrencys.SystemName, CurrentUser, PermissionActions.Edit))
                return AccessDeniedView();
            Currency Currency = _CurrencyService.GetById(Id);
            CurrencyModel CurrencyModel = _Mapper.Map<CurrencyModel>(Currency);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Currency", CreateNewBtnAction = "Create", BackToListControler = "Currency", BackToListAction = "Index", EditBtnControler = "Currency", EditBtnAction = "Edit", DeleteBtnControler = "Currency", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };

            return View(CurrencyModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CurrencyModel CurrencyModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageCurrencys.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    Currency Currency = _Mapper.Map<Currency>(CurrencyModel);
                    //var currentUser = _identityService.GetCurrentUser();
                    Currency.SetBasicData(SharedEnums.CRUD_OperationType.Update, CurrentUser);
                    _CurrencyService.Update(Currency);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Currency", CreateNewBtnAction = "Create", BackToListControler = "Currency", BackToListAction = "Index", EditBtnControler = "Currency", EditBtnAction = "Edit", DeleteBtnControler = "Currency", DeleteBtnAction = "Delete", RouteId = CurrencyModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            return View(CurrencyModel);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCurrencys.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            Currency Currency = _CurrencyService.GetById(Id);
            CurrencyModel CurrencyModel = _Mapper.Map<CurrencyModel>(Currency);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Currency", CreateNewBtnAction = "Create", BackToListControler = "Currency", BackToListAction = "Index", EditBtnControler = "Currency", EditBtnAction = "Edit", DeleteBtnControler = "Currency", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = true, BackToListBtnVisibilty = true, SaveBtnVisibilty = false, DeleteBtnVisibilty = true };

            return View(CurrencyModel);
        }

        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageCurrencys.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

                var msg = _webHelper.IsAllowedDeleteCurruncy(Id);
                if (msg != "")
                {
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString(msg).Value });
                }

                Currency Currency = _CurrencyService.GetById(Id);
                _CurrencyService.Delete(Currency);
                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }
        }



    }
}
