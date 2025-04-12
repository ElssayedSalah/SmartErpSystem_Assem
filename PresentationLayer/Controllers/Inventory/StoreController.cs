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
    public class StoreController : BaseAdminController
    {
        private readonly IBaseService<Store> _StoreService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly string _currentLanguage;
        private readonly IBaseService<Branch> _BranchService;
        private readonly IidentityService _identityService;
        private readonly IWebHelper _webHelper;

        public StoreController(IBaseService<Store> StoreService, IBaseService<Branch> BranchService, IMapper mapper, LocalizationService localizationService, IidentityService IdentityService, IWebHelper webHelper)
        {
            _StoreService = StoreService;
            _BranchService = BranchService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _identityService = IdentityService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _webHelper = webHelper;

        }
        public void IntializeDropdowens()
        {
            ViewBag.Branches = _BranchService.GetAll().Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });
        }
        // GET: StoreController
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageStores.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            return View(new StoreModel());
            
        }

        public IActionResult list()
        {


            var Store = _StoreService.GetAll();
            var StoreModel = _Mapper.Map<List<StoreModel>>(Store);

            var gridModel = new DataSourceResult
            {
                Data = StoreModel,
                Total = StoreModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageStores.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();
            var LastCode = _StoreService.GetLastCode(o => o.Code);
            var NewStore = new Store() { Code = LastCode };

            var NewStoreModel = _Mapper.Map<StoreModel>(NewStore);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Store", CreateNewBtnAction = "Create", BackToListControler = "Store", BackToListAction = "Index", EditBtnControler = "Store", EditBtnAction = "Edit", DeleteBtnControler = "Store", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
            IntializeDropdowens();
            return View(NewStoreModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StoreModel StoreModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!_identityService.Authorize(StandardPermissionProvider.ManageStores.SystemName, CurrentUser, PermissionActions.Create))
                        return AccessDeniedView();
                    if (!_StoreService.IsExistRecord(b => b.Code == StoreModel.Code))
                    {
                        Store Store = _Mapper.Map<Store>(StoreModel);
                        //var currentUser = _identityService.GetCurrentUser();
                        Store.SetBasicData(SharedEnums.CRUD_OperationType.Create,CurrentUser);
                        _StoreService.Add(Store);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Store", CreateNewBtnAction = "Create", BackToListControler = "Store", BackToListAction = "Index", EditBtnControler = "Store", EditBtnAction = "Edit", DeleteBtnControler = "Store", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
            IntializeDropdowens();
            return View(StoreModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageStores.SystemName, CurrentUser, PermissionActions.Edit))
                return AccessDeniedView();
            Store Store = _StoreService.GetById(Id);
            StoreModel StoreModel = _Mapper.Map<StoreModel>(Store);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Store", CreateNewBtnAction = "Create", BackToListControler = "Store", BackToListAction = "Index", EditBtnControler = "Store", EditBtnAction = "Edit", DeleteBtnControler = "Store", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            IntializeDropdowens();
            return View(StoreModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StoreModel StoreModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageStores.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    Store Store = _Mapper.Map<Store>(StoreModel);
                    //var currentUser = _identityService.GetCurrentUser();
                    Store.SetBasicData(SharedEnums.CRUD_OperationType.Update,CurrentUser);
                    _StoreService.Update(Store);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Store", CreateNewBtnAction = "Create", BackToListControler = "Store", BackToListAction = "Index", EditBtnControler = "Store", EditBtnAction = "Edit", DeleteBtnControler = "Store", DeleteBtnAction = "Delete", RouteId = StoreModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            IntializeDropdowens();

            return View(StoreModel);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageStores.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            Store Store = _StoreService.GetById(Id);
            StoreModel StoreModel = _Mapper.Map<StoreModel>(Store);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Store", CreateNewBtnAction = "Create", BackToListControler = "Store", BackToListAction = "Index", EditBtnControler = "Store", EditBtnAction = "Edit", DeleteBtnControler = "Store", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = true, BackToListBtnVisibilty = true, SaveBtnVisibilty = false, DeleteBtnVisibilty = true };

            return View(StoreModel);
        }

        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageStores.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });
               
                var msg = _webHelper.IsAllowedDeleteStore(Id);
                if (msg != "")
                {
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString(msg).Value });
                }

                Store Store = _StoreService.GetById(Id);
                _StoreService.Delete(Store);
                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }

        }

    }
}
