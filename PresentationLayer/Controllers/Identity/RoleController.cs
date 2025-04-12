using AutoMapper;
using BusinessLayer.Models.Identity;
using BusinessLayer.Services;
using Grand.Framework.Kendoui;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using static BusinessLayer.Helpers.SharedEnums;

namespace PresentationLayer.Controllers.Identity
{
    [Authorize]
    public class RoleController : BaseAdminController
    {
        private readonly IidentityService _IdentityService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;

        public RoleController(IidentityService IdentityService, LocalizationService localizationService, IMapper mapper)
        {
            _IdentityService = IdentityService;
            _Mapper = mapper;
            _LocalizationService = localizationService;

        }
        public IActionResult Index()
        {
            if (!_IdentityService.Authorize(StandardPermissionProvider.ManageRoles.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            return View(new RoleModel());
        }

        public IActionResult list()
        {
            var roles = _IdentityService.GetAllRoles();
            var rolesModel = _Mapper.Map<List<RoleModel>>(roles);
            var gridModel = new DataSourceResult
            {
                Data = rolesModel,
                Total = rolesModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_IdentityService.Authorize(StandardPermissionProvider.ManageRoles.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();
            var roleModel = new RoleModel();

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Role", CreateNewBtnAction = "Create", BackToListControler = "Role", BackToListAction = "Index", EditBtnControler = "Role", EditBtnAction = "Edit", DeleteBtnControler = "Role", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };           

            return View(roleModel);
        }

        [HttpPost]
        public IActionResult Create(RoleModel RoleModel)
        {
            try
            {
                if (!_IdentityService.Authorize(StandardPermissionProvider.ManageRoles.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    //UserModel.SetBasicData(SharedEnums.CRUD_OperationType.Create, CurrentUser);
                    var result = _IdentityService.AddRole(RoleModel);
                    if (result.Result.Succeeded)
                    {
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("SavedSuccessfuly"), NotificationCssType.success.ToString());

                    }
                    else
                    {
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("NotSavedSuccessfuly") + Environment.NewLine + result.Result.Errors.FirstOrDefault().Description, NotificationCssType.danger.ToString());
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Role", CreateNewBtnAction = "Create", BackToListControler = "Role", BackToListAction = "Index", EditBtnControler = "Role", EditBtnAction = "Edit", DeleteBtnControler = "Role", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
           
            return View(RoleModel);
        }

        [HttpGet]
        public IActionResult Edit(string Id)
        {
            if (!_IdentityService.Authorize(StandardPermissionProvider.ManageRoles.SystemName, CurrentUser, PermissionActions.Edit))
                return AccessDeniedView();
            IdentityRole rloe = _IdentityService.GetRoleById(Id).Result;
            RoleModel rloeModel = _Mapper.Map<RoleModel>(rloe);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Role", CreateNewBtnAction = "Create", BackToListControler = "Role", BackToListAction = "Index", EditBtnControler = "Role", EditBtnAction = "Edit", DeleteBtnControler = "Role", DeleteBtnAction = "Delete", RouteId = 0,StringRouteId=Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
          
            return View(rloeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoleModel roleModel)
        {
            try
            {
                if (!_IdentityService.Authorize(StandardPermissionProvider.ManageRoles.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    //userModel.SetBasicData(SharedEnums.CRUD_OperationType.Update, CurrentUser);
                    var result = _IdentityService.UpdateRole(roleModel);
                    if (result.Result.Succeeded)
                    {
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("UpdatedSuccessfuly"), NotificationCssType.success.ToString());

                    }
                    else
                    {
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("NotUpdatedSuccessfuly"), NotificationCssType.danger.ToString());
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Role", CreateNewBtnAction = "Create", BackToListControler = "Role", BackToListAction = "Index", EditBtnControler = "Role", EditBtnAction = "Edit", DeleteBtnControler = "Role", DeleteBtnAction = "Delete", RouteId = 0, StringRouteId = roleModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
           
            return View(roleModel);
        }

        public IActionResult Delete(string Id)
        {
            try
            {
                if (!_IdentityService.Authorize(StandardPermissionProvider.ManageRoles.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });
                var result = _IdentityService.DeleteRole(Id).Result;
                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }
        }

    }
}
