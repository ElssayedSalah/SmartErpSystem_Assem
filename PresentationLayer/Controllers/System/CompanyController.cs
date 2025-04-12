using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models.System;
using BusinessLayer.Services;
using DataAccessLayer.Entities.System;
using Grand.Framework.Kendoui;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static BusinessLayer.Helpers.SharedEnums;

namespace PresentationLayer.Controllers.System
{
    [Authorize]
    //[ValidateAntiForgeryToken]
    public class CompanyController : BaseAdminController
    {
        private readonly IBaseService<Company> _CompanyService;
        private readonly IBaseService<Countries> _CountriesService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly string _currentLanguage;
        private readonly IidentityService _identityService;
        private readonly IHostingEnvironment _Hosting;    
        private readonly IWebHelper _webHelper;

        public CompanyController(IBaseService<Company> CompanyService, IMapper mapper, LocalizationService localizationService, IidentityService IdentityService, IHostingEnvironment hosting, IBaseService<Countries> CountriesService, IWebHelper webHelper)
        {
            _CompanyService = CompanyService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _identityService = IdentityService;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _Hosting = hosting;
            _CountriesService = CountriesService;
            _webHelper = webHelper;

        }

        public void IntializeDropdowens()
        {
            //ViewBag.ClassTypes = PresentationExtensions.ConvertEnumToSelectListItems(typeof(ClassType), _LocalizationService);
            ViewBag.ClassTypes = PresentationExtensions.ConvertEnumToSelectListItems2(typeof(CompanyClassType), _LocalizationService);

            var Countries = _CountriesService.GetAll();
            Countries.Insert(0, new Countries() { Id = 0, NameAr = "إختار", NameEn = "Select" });
            ViewBag.Countries = Countries.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });


        }
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCompanys.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            return View(new CompanyMobel());
        }

        public IActionResult list()
        {
            var Company = _CompanyService.GetAll();
            var CompanyModel = _Mapper.Map<List<CompanyMobel>>(Company);
            var gridModel = new DataSourceResult
            {
                Data = CompanyModel,
                Total = CompanyModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCompanys.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();
            var LastCode = _CompanyService.GetLastCode(o => o.Code);
            var NewCompany = new Company() { Code = LastCode };

            var NewCompanyModel = _Mapper.Map<CompanyMobel>(NewCompany);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Company", CreateNewBtnAction = "Create", BackToListControler = "Company", BackToListAction = "Index", EditBtnControler = "Company", EditBtnAction = "Edit", DeleteBtnControler = "Company", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };

            IntializeDropdowens();
            return View(NewCompanyModel);
        }
        [HttpPost]
        public IActionResult Create(CompanyMobel CompanyModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageCompanys.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    if (!_CompanyService.IsExistRecord(b => b.Code == CompanyModel.Code))
                    {
                        Company Company = _Mapper.Map<Company>(CompanyModel);
                        Company.SetBasicData(SharedEnums.CRUD_OperationType.Create, CurrentUser);
                        string ImagePath = Uploader.UploadImage(CompanyModel.ImageFile, _Hosting);
                        CompanyModel.ImagePath = Company.ImagePath = !string.IsNullOrEmpty(ImagePath) ? ImagePath : Company.ImagePath;


                        _CompanyService.Add(Company);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Company", CreateNewBtnAction = "Create", BackToListControler = "Company", BackToListAction = "Index", EditBtnControler = "Company", EditBtnAction = "Edit", DeleteBtnControler = "Company", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
            IntializeDropdowens();
            return View(CompanyModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCompanys.SystemName, CurrentUser, PermissionActions.Edit))
                return AccessDeniedView();
            Company Company = _CompanyService.GetById(Id);
            CompanyMobel CompanyModel = _Mapper.Map<CompanyMobel>(Company);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Company", CreateNewBtnAction = "Create", BackToListControler = "Company", BackToListAction = "Index", EditBtnControler = "Company", EditBtnAction = "Edit", DeleteBtnControler = "Company", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
           
            IntializeDropdowens();

            return View(CompanyModel);
        }
        [HttpPost]
        public IActionResult Edit(CompanyMobel CompanyModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageCompanys.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    Company Company = _Mapper.Map<Company>(CompanyModel);
                    Company.SetBasicData(SharedEnums.CRUD_OperationType.Update, CurrentUser);
                    string ImagePath = Uploader.UploadImage(CompanyModel.ImageFile, _Hosting);
                    CompanyModel.ImagePath = Company.ImagePath = !string.IsNullOrEmpty(ImagePath) ? ImagePath : Company.ImagePath;
                    _CompanyService.Update(Company);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Company", CreateNewBtnAction = "Create", BackToListControler = "Company", BackToListAction = "Index", EditBtnControler = "Company", EditBtnAction = "Edit", DeleteBtnControler = "Company", DeleteBtnAction = "Delete", RouteId = CompanyModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };

            IntializeDropdowens();

            return View(CompanyModel);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCompanys.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
           
            Company Company = _CompanyService.GetById(Id);
            CompanyMobel CompanyModel = _Mapper.Map<CompanyMobel>(Company);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Company", CreateNewBtnAction = "Create", BackToListControler = "Company", BackToListAction = "Index", EditBtnControler = "Company", EditBtnAction = "Edit", DeleteBtnControler = "Company", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = true, BackToListBtnVisibilty = true, SaveBtnVisibilty = false, DeleteBtnVisibilty = true };

            return View(CompanyModel);
        }
       
        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageCompanys.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

                var msg = _webHelper.IsAllowedDeleteCompany(Id);
                if (msg != "")
                {
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString(msg).Value });
                }

                Company Company = _CompanyService.GetById(Id);
                _CompanyService.Delete(Company);
                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }

        }

    }






}
