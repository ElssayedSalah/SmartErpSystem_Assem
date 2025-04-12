using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models.Identity;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Identity;
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

namespace PresentationLayer.Controllers.Identity
{
    [Authorize]
    public class UserController : BaseAdminController
    {
        private readonly IidentityService _IdentityService;
        private readonly LocalizationService _LocalizationService;
        private readonly IMapper _Mapper;
        private readonly IBaseService<Company> _CompanyService;
        private readonly string _currentLanguage;
        private readonly IWebHelper _webHelper;
        private readonly IBaseService<Branch> _BranchService;


        public UserController(IidentityService IdentityService, LocalizationService localizationService, IMapper mapper, IBaseService<Company> CompanyService, IWebHelper webHelper, IBaseService<Branch> BranchService)
        {
            _IdentityService = IdentityService;
            _Mapper = mapper;
            _LocalizationService = localizationService;
            _CompanyService = CompanyService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _webHelper = webHelper;
            _BranchService = BranchService;


        }
        public void IntializeDropdowens()
        {
            var Companies = _CompanyService.GetAll();
            Companies.Insert(0, new Company() { Id = 0, NameAr = "إختار", NameEn = "Select" });
            ViewBag.Companies = Companies.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var RoleIds = _IdentityService.GetAllRoles();
            RoleIds.Insert(0, new ApplicationRole() { Id = "", NameAr = "إختار", Name = "Select" });
            ViewBag.RoleIds = RoleIds.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.Name });

            ViewBag.Branches = _BranchService.GetAll().Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

        }
        public IActionResult Index()
        {
            if (!_IdentityService.Authorize(StandardPermissionProvider.ManageUsers.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            return View(new UserModel());
        }

        public IActionResult list()
        {
            var user = _IdentityService.GetAllUsers();
            var userModel = _Mapper.Map<List<UserModel>>(user);
            var gridModel = new DataSourceResult
            {
                Data = userModel,
                Total = userModel.Count
            };
            return Json(gridModel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            if (!_IdentityService.Authorize(StandardPermissionProvider.ManageUsers.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();
            var userModel = new UserModel();

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "User", CreateNewBtnAction = "Create", BackToListControler = "User", BackToListAction = "Index", EditBtnControler = "User", EditBtnAction = "Edit", DeleteBtnControler = "User", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            IntializeDropdowens();

            return View(userModel);
        }
        [HttpPost]
        public IActionResult Create(UserModel UserModel)
        {
            try
            {
                if (!_IdentityService.Authorize(StandardPermissionProvider.ManageUsers.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();
                if (UserModel.CompanyId==null || UserModel.CompanyId<=0)
                {
                    ModelState.AddModelError("CompanyId", _LocalizationService.GetLocalizedHtmlString("CompanyRequired"));
                }
                if (ModelState.IsValid)
                {
                    UserModel.SetBasicData(SharedEnums.CRUD_OperationType.Create, CurrentUser);
                    var result=  _IdentityService.AddUser(UserModel).Result;
                    if (result.Succeeded)
                    {
                        result = _IdentityService.AddUserRole(UserModel).Result;
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("SavedSuccessfuly"), NotificationCssType.success.ToString());

                    }
                    else
                    {
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("NotSavedSuccessfuly") + Environment.NewLine + result.Errors.FirstOrDefault().Description, NotificationCssType.danger.ToString());
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "User", CreateNewBtnAction = "Create", BackToListControler = "User", BackToListAction = "Index", EditBtnControler = "User", EditBtnAction = "Edit", DeleteBtnControler = "User", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
            IntializeDropdowens();
            return View(UserModel);
        }


        [HttpGet]
        public IActionResult Edit(string Id)
        {
            if (!_IdentityService.Authorize(StandardPermissionProvider.ManageUsers.SystemName, CurrentUser, PermissionActions.Edit))
                return AccessDeniedView();
            ApplicationUser user = _IdentityService.GetUserById(Id);
            UserModel userModel = _Mapper.Map<UserModel>(user);

            var userRole = _IdentityService.GetUserRoleByUserId(Id).Result;
            if (userRole!=null)
            {
                userModel.RoleId = userRole.Id;
            }

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "User", CreateNewBtnAction = "Create", BackToListControler = "User", BackToListAction = "Index", EditBtnControler = "User", EditBtnAction = "Edit", DeleteBtnControler = "User", DeleteBtnAction = "Delete", RouteId = 0, StringRouteId = userModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            IntializeDropdowens();
            return View(userModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserModel userModel)
        {
            try
            {
                if (!_IdentityService.Authorize(StandardPermissionProvider.ManageUsers.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();
                if (!_IdentityService.Authorize(StandardPermissionProvider.ManageUsers.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();
                if (userModel.CompanyId == null || userModel.CompanyId <= 0)
                {
                    ModelState.AddModelError("CompanyId", _LocalizationService.GetLocalizedHtmlString("CompanyRequired"));
                }
                if (ModelState.IsValid)
                {
                    userModel.SetBasicData(CRUD_OperationType.Update, CurrentUser);
                    var result= _IdentityService.UpdateUser(userModel).Result;
                    if (result.Succeeded) 
                    {
                        result = _IdentityService.AddUserRole(userModel).Result;
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("UpdatedSuccessfuly"), NotificationCssType.success.ToString());
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "User", CreateNewBtnAction = "Create", BackToListControler = "User", BackToListAction = "Index", EditBtnControler = "User", EditBtnAction = "Edit", DeleteBtnControler = "User", DeleteBtnAction = "Delete", RouteId = 0,StringRouteId= userModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            IntializeDropdowens();
            return View(userModel);
        }

        public IActionResult Delete(string Id)
        {
            try
            {
                if (!_IdentityService.Authorize(StandardPermissionProvider.ManageDepartements.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });
                
                

                var result = _IdentityService.DeleteUser(Id).Result;
                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }
        }
       
    }
}
