using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models.Inventory;
using BusinessLayer.Services;
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
    public class BranchController : BaseAdminController
    {
        private readonly IBaseService<Branch> _BranchService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly string _currentLanguage;
        private readonly IidentityService _identityService;
        private readonly IWebHelper _webHelper;

        public BranchController(IBaseService<Branch> BranchService, IMapper mapper, LocalizationService localizationService, IidentityService IdentityService, IWebHelper webHelper)
        {
            _BranchService = BranchService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _identityService = IdentityService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _webHelper = webHelper;

        }
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageBranches.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            return View(new BranchModel());           
        }

        public IActionResult list()
        {
            var branch = _BranchService.GetAll();
            var branchModel = _Mapper.Map<List<BranchModel>>(branch);
            var gridModel = new DataSourceResult
            {
                Data = branchModel,
                Total = branchModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageBranches.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();

            var LastCode = _BranchService.GetLastCode(o=>o.Code) ;
            var NewBranch = new Branch() { Code= LastCode };   

            var NewBranchModel = _Mapper.Map<BranchModel>(NewBranch);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Branch", CreateNewBtnAction = "Create", BackToListControler = "Branch", BackToListAction = "Index", EditBtnControler = "Branch", EditBtnAction = "Edit", DeleteBtnControler = "Branch", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            return View(NewBranchModel);
        }
        [HttpPost]
        public IActionResult Create(BranchModel branchModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageBranches.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();

                if (ModelState.IsValid)
                {
                    if (!_BranchService.IsExistRecord(b=>b.Code== branchModel.Code))
                    {
                        Branch branch = _Mapper.Map<Branch>(branchModel);
                        //var currentUser = _identityService.GetCurrentUser();
                        branch.SetBasicData(SharedEnums.CRUD_OperationType.Create, CurrentUser);
                        _BranchService.Add(branch);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Branch", CreateNewBtnAction = "Create", BackToListControler = "Branch", BackToListAction = "Index", EditBtnControler = "Branch", EditBtnAction = "Edit", DeleteBtnControler = "Branch", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            return View(branchModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageBranches.SystemName, CurrentUser, PermissionActions.Delete))
                return AccessDeniedView();

            Branch branch = _BranchService.GetById(Id);
            BranchModel branchModel = _Mapper.Map<BranchModel>(branch);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Branch", CreateNewBtnAction = "Create", BackToListControler = "Branch", BackToListAction = "Index", EditBtnControler = "Branch", EditBtnAction = "Edit", DeleteBtnControler = "Branch", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };

            return View(branchModel);
        }
        [HttpPost]
        public IActionResult Edit(BranchModel branchModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageBranches.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();

                if (ModelState.IsValid)
                {
                    Branch branch = _Mapper.Map<Branch>(branchModel);
                    //var currentUser = _identityService.GetCurrentUser();
                    branch.SetBasicData(SharedEnums.CRUD_OperationType.Update, CurrentUser);
                    _BranchService.Update(branch);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Branch", CreateNewBtnAction = "Create", BackToListControler = "Branch", BackToListAction = "Index", EditBtnControler = "Branch", EditBtnAction = "Edit", DeleteBtnControler = "Branch", DeleteBtnAction = "Delete", RouteId = branchModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            return View(branchModel);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageBranches.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            Branch branch = _BranchService.GetById(Id);
            BranchModel branchModel = _Mapper.Map<BranchModel>(branch);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Branch", CreateNewBtnAction = "Create", BackToListControler = "Branch", BackToListAction = "Index",EditBtnControler="Branch",EditBtnAction="Edit",DeleteBtnControler= "Branch", DeleteBtnAction= "Delete", RouteId = Id,CreateNewBtnVisibilty=true,EditBtnVisibilty=true, BackToListBtnVisibilty = true,SaveBtnVisibilty=false,DeleteBtnVisibilty=true };

            return View(branchModel);
        }       

        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageBranches.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new {type="faild",msg= _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

                var msg = _webHelper.IsAllowedDeleteBranch(Id);
                if (msg != "")
                {
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString(msg).Value });
                }

                Branch branch = _BranchService.GetById(Id);
               _BranchService.Delete(branch);
               
                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });

               
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }

        }

    }
}
