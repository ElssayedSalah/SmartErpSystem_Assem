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
    public class ItemGroupController : BaseAdminController
    {
        private readonly IBaseService<ItemGroup> _ItemGroupService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly string _currentLanguage;
        private readonly IidentityService _identityService;
        private readonly IWebHelper _webHelper;

        public ItemGroupController(IBaseService<ItemGroup> ItemGroupService, IMapper mapper, LocalizationService localizationService, IidentityService IdentityService, IWebHelper webHelper)
        {
            _ItemGroupService = ItemGroupService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _identityService = IdentityService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _webHelper = webHelper;

        }
        // GET: ItemGroupController
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItemGroups.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            return View(new ItemGroupModel());           
        }

        public IActionResult list()
        {
            var ItemGroup = _ItemGroupService.GetAll();
            var ItemGroupModel = _Mapper.Map<List<ItemGroupModel>>(ItemGroup);
            var gridModel = new DataSourceResult
            {
                Data = ItemGroupModel,
                Total = ItemGroupModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItemGroups.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();
            var LastCode = _ItemGroupService.GetLastCode(o => o.Code);
            var NewItemGroup = new ItemGroup() { Code = LastCode };

            var NewItemGroupModel = _Mapper.Map<ItemGroupModel>(NewItemGroup);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "ItemGroup", CreateNewBtnAction = "Create", BackToListControler = "ItemGroup", BackToListAction = "Index", EditBtnControler = "ItemGroup", EditBtnAction = "Edit", DeleteBtnControler = "ItemGroup", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false, PrintBtnVisibilty = true };

            return View(NewItemGroupModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ItemGroupModel ItemGroupModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageItemGroups.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    if (!_ItemGroupService.IsExistRecord(b => b.Code == ItemGroupModel.Code))
                    {
                        ItemGroup ItemGroup = _Mapper.Map<ItemGroup>(ItemGroupModel);
                        //var currentUser = _identityService.GetCurrentUser();
                        ItemGroup.SetBasicData(SharedEnums.CRUD_OperationType.Create, CurrentUser);
                        _ItemGroupService.Add(ItemGroup);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "ItemGroup", CreateNewBtnAction = "Create", BackToListControler = "ItemGroup", BackToListAction = "Index", EditBtnControler = "ItemGroup", EditBtnAction = "Edit", DeleteBtnControler = "ItemGroup", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            return View(ItemGroupModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItemGroups.SystemName, CurrentUser, PermissionActions.Edit))
                return AccessDeniedView();
            ItemGroup ItemGroup = _ItemGroupService.GetById(Id);
            ItemGroupModel ItemGroupModel = _Mapper.Map<ItemGroupModel>(ItemGroup);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "ItemGroup", CreateNewBtnAction = "Create", BackToListControler = "ItemGroup", BackToListAction = "Index", EditBtnControler = "ItemGroup", EditBtnAction = "Edit", DeleteBtnControler = "ItemGroup", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };

            return View(ItemGroupModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ItemGroupModel ItemGroupModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageItemGroups.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    ItemGroup ItemGroup = _Mapper.Map<ItemGroup>(ItemGroupModel);
                    //var currentUser = _identityService.GetCurrentUser();
                    ItemGroup.SetBasicData(SharedEnums.CRUD_OperationType.Update, CurrentUser);
                    _ItemGroupService.Update(ItemGroup);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "ItemGroup", CreateNewBtnAction = "Create", BackToListControler = "ItemGroup", BackToListAction = "Index", EditBtnControler = "ItemGroup", EditBtnAction = "Edit", DeleteBtnControler = "ItemGroup", DeleteBtnAction = "Delete", RouteId = ItemGroupModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            return View(ItemGroupModel);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItemGroups.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            var msg = _webHelper.IsAllowedDeleteItemGroup(Id);
            if (msg != "")
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString(msg).Value });

            }

            ItemGroup ItemGroup = _ItemGroupService.GetById(Id);
            ItemGroupModel ItemGroupModel = _Mapper.Map<ItemGroupModel>(ItemGroup);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "ItemGroup", CreateNewBtnAction = "Create", BackToListControler = "ItemGroup", BackToListAction = "Index", EditBtnControler = "ItemGroup", EditBtnAction = "Edit", DeleteBtnControler = "ItemGroup", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = true, BackToListBtnVisibilty = true, SaveBtnVisibilty = false, DeleteBtnVisibilty = true };

            return View(ItemGroupModel);
        }

        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageItemGroups.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

                ItemGroup ItemGroup = _ItemGroupService.GetById(Id);
                _ItemGroupService.Delete(ItemGroup);
                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }

        }

    }
}
