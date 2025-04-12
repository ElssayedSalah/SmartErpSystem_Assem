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
    public class FinancialPeriodController : BaseAdminController
    {
        private readonly IBaseService<FinancialPeriod> _FinancialPeriodService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly string _currentLanguage;
        private readonly IidentityService _identityService;
        private readonly IWebHelper _webHelper;

        public FinancialPeriodController(IBaseService<FinancialPeriod> FinancialPeriodService, IMapper mapper, LocalizationService localizationService, IidentityService IdentityService, IWebHelper webHelper)
        {
            _FinancialPeriodService = FinancialPeriodService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _identityService = IdentityService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _webHelper = webHelper;

        }
        // GET: FinancialPeriodController
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageFinancialPeriods.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            return View(new FinancialPeriodModel());
        
        }      
        public IActionResult list()
        {
            var FinancialPeriod = _FinancialPeriodService.GetAll();
            var FinancialPeriodModel = _Mapper.Map<List<FinancialPeriodModel>>(FinancialPeriod);
            var gridModel = new DataSourceResult
            {
                Data = FinancialPeriodModel,
                Total = FinancialPeriodModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageFinancialPeriods.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();
            FinancialPeriodModel FinancialPeriod = new FinancialPeriodModel() 
            {
                Year=DateTime.Now.Year,
                DateFrom= new DateTime(DateTime.Now.Year,1,1),
                DateTo= new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year,12)) 
            };

            var LastFinancialPeriod = _FinancialPeriodService.GetAll()?.LastOrDefault();
            if (LastFinancialPeriod!=null)
            {
                FinancialPeriod.Year = LastFinancialPeriod.Year + 1;
                FinancialPeriod.DateFrom = new DateTime(FinancialPeriod.Year, FinancialPeriod.DateFrom.Month, FinancialPeriod.DateFrom.Day);
                FinancialPeriod.DateTo = new DateTime(FinancialPeriod.Year, FinancialPeriod.DateTo.Month, FinancialPeriod.DateTo.Day);

            }
            FinancialPeriodModel FinancialPeriodModel = _Mapper.Map<FinancialPeriodModel>(FinancialPeriod);


            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "FinancialPeriod", CreateNewBtnAction = "Create", BackToListControler = "FinancialPeriod", BackToListAction = "Index", EditBtnControler = "FinancialPeriod", EditBtnAction = "Edit", DeleteBtnControler = "FinancialPeriod", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            return View(FinancialPeriodModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FinancialPeriodModel FinancialPeriodModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageFinancialPeriods.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    FinancialPeriod FinancialPeriod = _Mapper.Map<FinancialPeriod>(FinancialPeriodModel);
                    _FinancialPeriodService.Add(FinancialPeriod);
                    return RedirectToAction("Edit", new { Id = FinancialPeriod.Id, FromAction = "Create" });
                    //ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("SavedSuccessfuly"), NotificationCssType.success.ToString());

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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "FinancialPeriod", CreateNewBtnAction = "Create", BackToListControler = "FinancialPeriod", BackToListAction = "Index", EditBtnControler = "FinancialPeriod", EditBtnAction = "Edit", DeleteBtnControler = "FinancialPeriod", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            return View(FinancialPeriodModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id, string FromAction)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageFinancialPeriods.SystemName, CurrentUser, PermissionActions.Edit))
                return AccessDeniedView();
            FinancialPeriod FinancialPeriod = _FinancialPeriodService.GetById(Id);
            FinancialPeriodModel FinancialPeriodModel = _Mapper.Map<FinancialPeriodModel>(FinancialPeriod);

            if (FromAction != null && FromAction == "Create")
            {
                ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("SavedSuccessfuly"), NotificationCssType.success.ToString());
            }
            else if (FromAction != null && FromAction == "Edit")
            {
                ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("UpdatedSuccessfuly"), NotificationCssType.success.ToString());
            }


            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "FinancialPeriod", CreateNewBtnAction = "Create", BackToListControler = "FinancialPeriod", BackToListAction = "Index", EditBtnControler = "FinancialPeriod", EditBtnAction = "Edit", DeleteBtnControler = "FinancialPeriod", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };

            return View(FinancialPeriodModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(FinancialPeriodModel FinancialPeriodModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageFinancialPeriods.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    FinancialPeriod FinancialPeriod = _Mapper.Map<FinancialPeriod>(FinancialPeriodModel);
                    _FinancialPeriodService.Update(FinancialPeriod);

                    return RedirectToAction("Edit", new { Id = FinancialPeriod.Id, FromAction = "Edit" });

                    //ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("UpdatedSuccessfuly"), NotificationCssType.success.ToString());

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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "FinancialPeriod", CreateNewBtnAction = "Create", BackToListControler = "FinancialPeriod", BackToListAction = "Index", EditBtnControler = "FinancialPeriod", EditBtnAction = "Edit", DeleteBtnControler = "FinancialPeriod", DeleteBtnAction = "Delete", RouteId = FinancialPeriodModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            return View(FinancialPeriodModel);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageFinancialPeriods.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            FinancialPeriod FinancialPeriod = _FinancialPeriodService.GetById(Id);
            FinancialPeriodModel FinancialPeriodModel = _Mapper.Map<FinancialPeriodModel>(FinancialPeriod);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "FinancialPeriod", CreateNewBtnAction = "Create", BackToListControler = "FinancialPeriod", BackToListAction = "Index", EditBtnControler = "FinancialPeriod", EditBtnAction = "Edit", DeleteBtnControler = "FinancialPeriod", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = true, BackToListBtnVisibilty = true, SaveBtnVisibilty = false, DeleteBtnVisibilty = true };

            return View(FinancialPeriodModel);
        }

        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageFinancialPeriods.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

                var msg = _webHelper.IsAllowedDeleteFinancialPeriod(Id);
                if (msg != "")
                {
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString(msg).Value });
                }


                FinancialPeriod FinancialPeriod = _FinancialPeriodService.GetById(Id);
                _FinancialPeriodService.Delete(FinancialPeriod);
                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }
        }



    }
}
