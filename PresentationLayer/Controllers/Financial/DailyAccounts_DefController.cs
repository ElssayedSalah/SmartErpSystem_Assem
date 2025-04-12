using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models.Financial;
using BusinessLayer.Models.Inventory;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using Grand.Framework.Kendoui;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Threading;
using static BusinessLayer.Helpers.SharedEnums;

namespace PresentationLayer.Controllers.Inventory
{
    [Authorize]
    public class DailyAccounts_DefController : BaseAdminController
    {
        private readonly IBaseService<DailyAccounts_Def> _DailyAccounts_DefService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly string _currentLanguage;
        private readonly IidentityService _identityService;
        private readonly IWebHelper _webHelper;

        public DailyAccounts_DefController(IBaseService<DailyAccounts_Def> DailyAccounts_DefService, IMapper mapper, LocalizationService localizationService, IidentityService IdentityService, IWebHelper webHelper)
        {
            _DailyAccounts_DefService = DailyAccounts_DefService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _identityService = IdentityService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _webHelper = webHelper;

        }
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageDailyAccounts_Def.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            return View(new DailyAccounts_DefModel());           
        }

        public IActionResult list()
        {
            var DailyAccounts_Def = _DailyAccounts_DefService.GetAll();
            var DailyAccounts_DefModel = _Mapper.Map<List<DailyAccounts_DefModel>>(DailyAccounts_Def);
            var gridModel = new DataSourceResult
            {
                Data = DailyAccounts_DefModel,
                Total = DailyAccounts_DefModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageDailyAccounts_Def.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();

            var LastCode = _DailyAccounts_DefService.GetLastCode(o=>o.Code) ;
            var NewDailyAccounts_Def = new DailyAccounts_Def() { Code= LastCode };   

            var NewDailyAccounts_DefModel = _Mapper.Map<DailyAccounts_DefModel>(NewDailyAccounts_Def);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "DailyAccounts_Def", CreateNewBtnAction = "Create", BackToListControler = "DailyAccounts_Def", BackToListAction = "Index", EditBtnControler = "DailyAccounts_Def", EditBtnAction = "Edit", DeleteBtnControler = "DailyAccounts_Def", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            return View(NewDailyAccounts_DefModel);
        }
        [HttpPost]
        public IActionResult Create(DailyAccounts_DefModel DailyAccounts_DefModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageDailyAccounts_Def.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();

                if (ModelState.IsValid)
                {
                    if (!_DailyAccounts_DefService.IsExistRecord(b=>b.Code== DailyAccounts_DefModel.Code))
                    {
                        DailyAccounts_Def DailyAccounts_Def = _Mapper.Map<DailyAccounts_Def>(DailyAccounts_DefModel);
                        DailyAccounts_Def.SetBasicData(SharedEnums.CRUD_OperationType.Create, CurrentUser);
                        _DailyAccounts_DefService.Add(DailyAccounts_Def);
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
                ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("ErorrDuringSaving") + ex.InnerException , NotificationCssType.danger.ToString());
            }
            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "DailyAccounts_Def", CreateNewBtnAction = "Create", BackToListControler = "DailyAccounts_Def", BackToListAction = "Index", EditBtnControler = "DailyAccounts_Def", EditBtnAction = "Edit", DeleteBtnControler = "DailyAccounts_Def", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            return View(DailyAccounts_DefModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageDailyAccounts_Def.SystemName, CurrentUser, PermissionActions.Delete))
                return AccessDeniedView();

            DailyAccounts_Def DailyAccounts_Def = _DailyAccounts_DefService.GetById(Id);
            DailyAccounts_DefModel DailyAccounts_DefModel = _Mapper.Map<DailyAccounts_DefModel>(DailyAccounts_Def);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "DailyAccounts_Def", CreateNewBtnAction = "Create", BackToListControler = "DailyAccounts_Def", BackToListAction = "Index", EditBtnControler = "DailyAccounts_Def", EditBtnAction = "Edit", DeleteBtnControler = "DailyAccounts_Def", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };

            return View(DailyAccounts_DefModel);
        }
        [HttpPost]
        public IActionResult Edit(DailyAccounts_DefModel DailyAccounts_DefModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageDailyAccounts_Def.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();

                if (ModelState.IsValid)
                {
                    DailyAccounts_Def DailyAccounts_Def = _Mapper.Map<DailyAccounts_Def>(DailyAccounts_DefModel);
                    //var currentUser = _identityService.GetCurrentUser();
                    DailyAccounts_Def.SetBasicData(SharedEnums.CRUD_OperationType.Update, CurrentUser);
                    _DailyAccounts_DefService.Update(DailyAccounts_Def);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "DailyAccounts_Def", CreateNewBtnAction = "Create", BackToListControler = "DailyAccounts_Def", BackToListAction = "Index", EditBtnControler = "DailyAccounts_Def", EditBtnAction = "Edit", DeleteBtnControler = "DailyAccounts_Def", DeleteBtnAction = "Delete", RouteId = DailyAccounts_DefModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            return View(DailyAccounts_DefModel);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageDailyAccounts_Def.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            DailyAccounts_Def DailyAccounts_Def = _DailyAccounts_DefService.GetById(Id);
            DailyAccounts_DefModel DailyAccounts_DefModel = _Mapper.Map<DailyAccounts_DefModel>(DailyAccounts_Def);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "DailyAccounts_Def", CreateNewBtnAction = "Create", BackToListControler = "DailyAccounts_Def", BackToListAction = "Index",EditBtnControler="DailyAccounts_Def",EditBtnAction="Edit",DeleteBtnControler= "DailyAccounts_Def", DeleteBtnAction= "Delete", RouteId = Id,CreateNewBtnVisibilty=true,EditBtnVisibilty=true, BackToListBtnVisibilty = true,SaveBtnVisibilty=false,DeleteBtnVisibilty=true };

            return View(DailyAccounts_DefModel);
        }       

        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageDailyAccounts_Def.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new {type="faild",msg= _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

                var msg = _webHelper.IsAllowedDeleteDailyAccounts_Def(Id);
                if (msg != "")
                {
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString(msg).Value });
                }

                DailyAccounts_Def DailyAccounts_Def = _DailyAccounts_DefService.GetById(Id);
               _DailyAccounts_DefService.Delete(DailyAccounts_Def);
               
                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });

               
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }

        }

    }
}
