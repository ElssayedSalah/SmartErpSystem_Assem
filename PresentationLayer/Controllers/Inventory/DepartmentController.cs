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
using System.Linq;
using System.Threading;
using static BusinessLayer.Helpers.SharedEnums;

namespace SmartErpApp.Controllers.Inventory
{
    [Authorize]
    //[ValidateAntiForgeryToken]
    public class DepartmentController : BaseAdminController
    {
        private readonly IBaseService<Department> _DepartmentService;
        private readonly IBaseService<Branch> _BranchService;
        private readonly IidentityService _identityService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly string _currentLanguage;

        public DepartmentController(IBaseService<Department> DepartmentService, IBaseService<Branch> BranchService, IMapper mapper, LocalizationService localizationService, IidentityService IdentityService)
        {
            _DepartmentService = DepartmentService;
            _BranchService = BranchService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _identityService = IdentityService;

            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
        }
        public void IntializeDropdowens()
        {
            ViewBag.Branches = _BranchService.GetAll().Select(x=>new { x.Id,Name= _currentLanguage =="ar"?x.NameAr:x.NameEn});
        }
        // GET: DepartmentController
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageDepartements.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            return View(new DepartmentModel());
            
        }

        public IActionResult list()
        {
            var Department = _DepartmentService.GetAll();
            var DepartmentModel = _Mapper.Map<List<DepartmentModel>>(Department);
            var gridModel = new DataSourceResult
            {
                Data = DepartmentModel,
                Total = DepartmentModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageDepartements.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();

            var LastCode = _DepartmentService.GetLastCode(o => o.Code);
            var NewDepartment = new Department() { Code = LastCode };

            var NewDepartmentModel = _Mapper.Map<DepartmentModel>(NewDepartment);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Department", CreateNewBtnAction = "Create", BackToListControler = "Department", BackToListAction = "Index", EditBtnControler = "Department", EditBtnAction = "Edit", DeleteBtnControler = "Department", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false ,PrintBtnVisibilty=true};
            IntializeDropdowens();
            return View(NewDepartmentModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentModel DepartmentModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageDepartements.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    if (!_DepartmentService.IsExistRecord(b => b.Code == DepartmentModel.Code))
                    {
                        Department Department = _Mapper.Map<Department>(DepartmentModel);
                        //var currentUser = _identityService.GetCurrentUser();
                        Department.SetBasicData(SharedEnums.CRUD_OperationType.Create, CurrentUser);
                        _DepartmentService.Add(Department);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Department", CreateNewBtnAction = "Create", BackToListControler = "Department", BackToListAction = "Index", EditBtnControler = "Department", EditBtnAction = "Edit", DeleteBtnControler = "Department", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            IntializeDropdowens();

            return View(DepartmentModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageDepartements.SystemName, CurrentUser, PermissionActions.Edit))
                return AccessDeniedView();

            Department Department = _DepartmentService.GetById(Id);
            DepartmentModel DepartmentModel = _Mapper.Map<DepartmentModel>(Department);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Department", CreateNewBtnAction = "Create", BackToListControler = "Department", BackToListAction = "Index", EditBtnControler = "Department", EditBtnAction = "Edit", DeleteBtnControler = "Department", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            IntializeDropdowens();
            return View(DepartmentModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DepartmentModel DepartmentModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageDepartements.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();

                if (ModelState.IsValid)
                {
                    Department Department = _Mapper.Map<Department>(DepartmentModel);
                    //var currentUser = _identityService.GetCurrentUser();
                    Department.SetBasicData(SharedEnums.CRUD_OperationType.Update,CurrentUser);
                    _DepartmentService.Update(Department);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Department", CreateNewBtnAction = "Create", BackToListControler = "Department", BackToListAction = "Index", EditBtnControler = "Department", EditBtnAction = "Edit", DeleteBtnControler = "Department", DeleteBtnAction = "Delete", RouteId = DepartmentModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };

            IntializeDropdowens();

            return View(DepartmentModel);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageDepartements.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            Department Department = _DepartmentService.GetById(Id);
            DepartmentModel DepartmentModel = _Mapper.Map<DepartmentModel>(Department);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Department", CreateNewBtnAction = "Create", BackToListControler = "Department", BackToListAction = "Index", EditBtnControler = "Department", EditBtnAction = "Edit", DeleteBtnControler = "Department", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = true, BackToListBtnVisibilty = true, SaveBtnVisibilty = false, DeleteBtnVisibilty = true };
            if (DepartmentModel.BranchId.HasValue)
            {
                DepartmentModel.BranchName =_currentLanguage=="ar"? _BranchService.GetById(DepartmentModel.BranchId.Value).NameAr : _BranchService.GetById(DepartmentModel.BranchId.Value).NameEn;
            }

            return View(DepartmentModel);
        }
       
        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageDepartements.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

                Department Department = _DepartmentService.GetById(Id);
                _DepartmentService.Delete(Department);
                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }
        }

    }
}
