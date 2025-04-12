using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models.Inventory;
using BusinessLayer.Models.System;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.System;
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
    public class SystemSettingController : BaseAdminController
    {
        private readonly IBaseService<SystemSetting> _SystemSettingService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly IidentityService _identityService;

        public SystemSettingController(IBaseService<SystemSetting> SystemSettingService, IMapper mapper, LocalizationService localizationService, IidentityService IdentityService)
        {
            _SystemSettingService = SystemSettingService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _identityService = IdentityService;

        }
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageSystemSettinges.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            var SystemSetting = _SystemSettingService.GetAll();      

            return View(new SystemSettingModel() { EnableCreate= SystemSetting.Count == 0 ?true:false});           
        }

        public IActionResult list()
        {
            var SystemSetting = _SystemSettingService.GetAll();
            var SystemSettingModel = _Mapper.Map<List<SystemSettingModel>>(SystemSetting);
            var gridModel = new DataSourceResult
            {
                Data = SystemSettingModel,
                Total = SystemSettingModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageSystemSettinges.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();
           
            var NewSystemSetting = new SystemSetting();   

            var NewSystemSettingModel = _Mapper.Map<SystemSettingModel>(NewSystemSetting);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "SystemSetting", CreateNewBtnAction = "Create", BackToListControler = "SystemSetting", BackToListAction = "Index", EditBtnControler = "SystemSetting", EditBtnAction = "Edit", DeleteBtnControler = "SystemSetting", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            ViewBag.InvoiceTypes = PresentationExtensions.ConvertEnumToSelectListItemsStartFromZeroIndex(typeof(ElectronicInvoiceTypes), _LocalizationService);


            return View(NewSystemSettingModel);
        }
        [HttpPost]
        public IActionResult Create(SystemSettingModel SystemSettingModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageSystemSettinges.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();

                if (ModelState.IsValid)
                {
                    SystemSetting SystemSetting = _Mapper.Map<SystemSetting>(SystemSettingModel);
                    _SystemSettingService.Add(SystemSetting);
                    ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("SavedSuccessfuly"), NotificationCssType.success.ToString());

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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "SystemSetting", CreateNewBtnAction = "Create", BackToListControler = "SystemSetting", BackToListAction = "Index", EditBtnControler = "SystemSetting", EditBtnAction = "Edit", DeleteBtnControler = "SystemSetting", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            ViewBag.InvoiceTypes = PresentationExtensions.ConvertEnumToSelectListItemsStartFromZeroIndex(typeof(ElectronicInvoiceTypes), _LocalizationService);

            return View(SystemSettingModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageSystemSettinges.SystemName, CurrentUser, PermissionActions.Delete))
                return AccessDeniedView();

            SystemSetting SystemSetting = _SystemSettingService.GetById(Id);
            SystemSettingModel SystemSettingModel = _Mapper.Map<SystemSettingModel>(SystemSetting);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "SystemSetting", CreateNewBtnAction = "Create", BackToListControler = "SystemSetting", BackToListAction = "Index", EditBtnControler = "SystemSetting", EditBtnAction = "Edit", DeleteBtnControler = "SystemSetting", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = false, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            ViewBag.InvoiceTypes = PresentationExtensions.ConvertEnumToSelectListItemsStartFromZeroIndex(typeof(ElectronicInvoiceTypes), _LocalizationService);

            return View(SystemSettingModel);
        }
        [HttpPost]
        public IActionResult Edit(SystemSettingModel SystemSettingModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageSystemSettinges.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();

                if (ModelState.IsValid)
                {
                    SystemSetting SystemSetting = _Mapper.Map<SystemSetting>(SystemSettingModel);
                   
                    _SystemSettingService.Update(SystemSetting);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "SystemSetting", CreateNewBtnAction = "Create", BackToListControler = "SystemSetting", BackToListAction = "Index", EditBtnControler = "SystemSetting", EditBtnAction = "Edit", DeleteBtnControler = "SystemSetting", DeleteBtnAction = "Delete", RouteId = SystemSettingModel.Id, CreateNewBtnVisibilty = false, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            ViewBag.InvoiceTypes = PresentationExtensions.ConvertEnumToSelectListItemsStartFromZeroIndex(typeof(ElectronicInvoiceTypes), _LocalizationService);

            return View(SystemSettingModel);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageSystemSettinges.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            SystemSetting SystemSetting = _SystemSettingService.GetById(Id);
            SystemSettingModel SystemSettingModel = _Mapper.Map<SystemSettingModel>(SystemSetting);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "SystemSetting", CreateNewBtnAction = "Create", BackToListControler = "SystemSetting", BackToListAction = "Index",EditBtnControler="SystemSetting",EditBtnAction="Edit",DeleteBtnControler= "SystemSetting", DeleteBtnAction= "Delete", RouteId = Id,CreateNewBtnVisibilty=true,EditBtnVisibilty=true, BackToListBtnVisibilty = true,SaveBtnVisibilty=false,DeleteBtnVisibilty=true };

            return View(SystemSettingModel);
        }       

        //public IActionResult Delete(int Id)
        //{
        //    try
        //    {
        //        if (!_identityService.Authorize(StandardPermissionProvider.ManageSystemSettinges.SystemName, CurrentUser, PermissionActions.Delete))
        //            return Json(new {type="faild",msg= _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

        //        var msg = _webHelper.IsAllowedDeleteSystemSetting(Id);
        //        if (msg != "")
        //        {
        //            return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString(msg).Value });
        //        }

        //        SystemSetting SystemSetting = _SystemSettingService.GetById(Id);
        //       _SystemSettingService.Delete(SystemSetting);
               
        //        return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });

               
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
        //    }

        //}

    }
}
