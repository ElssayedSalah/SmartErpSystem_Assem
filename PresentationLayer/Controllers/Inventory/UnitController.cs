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
using System.Threading;
using static BusinessLayer.Helpers.SharedEnums;

namespace SmartErpApp.Controllers.Inventory
{
    [Authorize]
    //[ValidateAntiForgeryToken]
    public class UnitController : BaseAdminController
    {
        private readonly IBaseService<Unit> _UnitService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly string _currentLanguage;
        private readonly IidentityService _identityService;
        private readonly IWebHelper _webHelper;

        public UnitController(IBaseService<Unit> UnitService, IMapper mapper, LocalizationService localizationService, IidentityService IdentityService, IWebHelper webHelper)
        {
            _UnitService = UnitService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _identityService = IdentityService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _webHelper = webHelper;
        }
        // GET: UnitController
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageUnits.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            return View(new UnitModel());
        
        }

        public ActionResult Index2()
        {
            try
            {
                var Unit = _UnitService.GetAll();
                var UnitModel = _Mapper.Map<List<UnitModel>>(Unit);
                return View(UnitModel);
            }
            catch (Exception)
            {
                return View();
            }
        }
        public IActionResult list()
        {
            var Unit = _UnitService.GetAll();
            var UnitModel = _Mapper.Map<List<UnitModel>>(Unit);
            var gridModel = new DataSourceResult
            {
                Data = UnitModel,
                Total = UnitModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageUnits.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();
            var LastCode = _UnitService.GetLastCode(o => o.Code);
            var NewUnit = new Unit() { Code = LastCode };

            var NewUnitModel = _Mapper.Map<UnitModel>(NewUnit);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Unit", CreateNewBtnAction = "Create", BackToListControler = "Unit", BackToListAction = "Index", EditBtnControler = "Unit", EditBtnAction = "Edit", DeleteBtnControler = "Unit", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            return View(NewUnitModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UnitModel UnitModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageUnits.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    if (!_UnitService.IsExistRecord(b => b.Code == UnitModel.Code))
                    {
                        //var currentUser = _identityService.GetCurrentUser();
                        Unit Unit = _Mapper.Map<Unit>(UnitModel);
                        Unit.SetBasicData(SharedEnums.CRUD_OperationType.Create, CurrentUser);
                        _UnitService.Add(Unit);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Unit", CreateNewBtnAction = "Create", BackToListControler = "Unit", BackToListAction = "Index", EditBtnControler = "Unit", EditBtnAction = "Edit", DeleteBtnControler = "Unit", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            return View(UnitModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageUnits.SystemName, CurrentUser, PermissionActions.Edit))
                return AccessDeniedView();
            Unit Unit = _UnitService.GetById(Id);
            UnitModel UnitModel = _Mapper.Map<UnitModel>(Unit);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Unit", CreateNewBtnAction = "Create", BackToListControler = "Unit", BackToListAction = "Index", EditBtnControler = "Unit", EditBtnAction = "Edit", DeleteBtnControler = "Unit", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };

            return View(UnitModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UnitModel UnitModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageUnits.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    Unit Unit = _Mapper.Map<Unit>(UnitModel);
                    //var currentUser = _identityService.GetCurrentUser();
                    Unit.SetBasicData(SharedEnums.CRUD_OperationType.Update, CurrentUser);
                    _UnitService.Update(Unit);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Unit", CreateNewBtnAction = "Create", BackToListControler = "Unit", BackToListAction = "Index", EditBtnControler = "Unit", EditBtnAction = "Edit", DeleteBtnControler = "Unit", DeleteBtnAction = "Delete", RouteId = UnitModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            return View(UnitModel);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageUnits.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            Unit Unit = _UnitService.GetById(Id);
            UnitModel UnitModel = _Mapper.Map<UnitModel>(Unit);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Unit", CreateNewBtnAction = "Create", BackToListControler = "Unit", BackToListAction = "Index", EditBtnControler = "Unit", EditBtnAction = "Edit", DeleteBtnControler = "Unit", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = true, BackToListBtnVisibilty = true, SaveBtnVisibilty = false, DeleteBtnVisibilty = true };

            return View(UnitModel);
        }

        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageUnits.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

               var msg= _webHelper.IsAllowedDeleteUnit(Id);
                if (msg!="")
                {
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString(msg).Value });

                }

                Unit Unit = _UnitService.GetById(Id);
                _UnitService.Delete(Unit);
                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }
        }



    }
}
