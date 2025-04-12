using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models.Inventory;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.System;
using Grand.Framework.Kendoui;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class ItemController : BaseAdminController
    {
        private readonly IBaseService<Item> _ItemService;
        private readonly IBaseService<ItemGroup> _ItemGroupService;
        private readonly IBaseService<Unit> _UnitService;
        private readonly IBaseService<Company> _CompanyService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly IHostingEnvironment _Hosting;
        private readonly string _currentLanguage;
        private readonly IidentityService _identityService;
        private readonly IWebHelper _webHelper;

        public ItemController(
            IBaseService<Item> ItemService,
            IBaseService<ItemGroup> ItemGroupService,
            IMapper mapper, LocalizationService localizationService,
             IBaseService<Unit> UnitService,
             IHostingEnvironment hosting,
             IidentityService IdentityService,
             IWebHelper webHelper,
             IBaseService<Company> CompanyService
            )
        {
            _ItemService = ItemService;
            _ItemGroupService = ItemGroupService;
            _UnitService = UnitService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _Hosting = hosting;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _identityService = IdentityService;
            _webHelper = webHelper;
            _CompanyService = CompanyService;

        }

        public void IntializeDropdowens()
        {
            ViewBag.Groups = _ItemGroupService.GetAll().Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });
            ViewBag.Units = _UnitService.GetAll().Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });
            ViewBag.TaxAuthorityTypes = PresentationExtensions.ConvertEnumToSelectListItems2(typeof(TaxAuthorityTypes), _LocalizationService);
            ViewBag.ItemTypes = PresentationExtensions.ConvertEnumToSelectListItems(typeof(ItemTypes), _LocalizationService);

        }
        // GET: ItemController
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItems.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            return View(new ItemModel());           
        }

        public IActionResult list()
        {
            var Item = _ItemService.GetAll();
            var ItemModel = _Mapper.Map<List<ItemModel>>(Item);
            foreach (var item in ItemModel)
            {
                item.UnitName = _currentLanguage == "ar" ? _UnitService.GetById(item.DefaultUnit)?.NameAr : _UnitService.GetById(item.DefaultUnit)?.NameEn;
                item.GroupName = _currentLanguage == "ar" ? _ItemGroupService.GetById(item.GroupId)?.NameAr : _ItemGroupService.GetById(item.GroupId)?.NameEn;
                item.TypeName = PresentationExtensions.ConvertEnumToSelectListItems(typeof(ItemTypes), _LocalizationService).Where(x => x.Value == item.TypeId.ToString()).Select(x => x.Text).FirstOrDefault();

            }
            var gridModel = new DataSourceResult
            {
                Data = ItemModel,
                Total = ItemModel.Count
            };
            return Json(gridModel);
        }


        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItems.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();

            var LastCode = _ItemService.GetLastCode(o => o.Code);
            var NewItem = new Item() { Code = LastCode };

            var NewItemModel = _Mapper.Map<ItemModel>(NewItem);

            if (CurrentUser!=null && CurrentUser.CompanyId.HasValue)
            {
                var company = _CompanyService.GetById(CurrentUser.CompanyId.Value);
                NewItemModel.TaxAuthorityCode = $"EG-{company.TaxAuthorityRegestrationNumber}-{NewItemModel.Code}";
            }

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Item", CreateNewBtnAction = "Create", BackToListControler = "Item", BackToListAction = "Index", EditBtnControler = "Item", EditBtnAction = "Edit", DeleteBtnControler = "Item", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false, PrintBtnVisibilty = true };

            IntializeDropdowens();

            return View(NewItemModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ItemModel ItemModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageItems.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    if (!_ItemService.IsExistRecord(b => b.Code == ItemModel.Code))
                    {
                        Item Item = _Mapper.Map<Item>(ItemModel);
                        //var currentUser = _identityService.GetCurrentUser();
                        Item.SetBasicData(CRUD_OperationType.Create, CurrentUser);
                        Item.FinancialPeriodId = CurrentUser.FinancialPeriodId.Value;
                        string ImagePath = Uploader.UploadImage(ItemModel.ItemImageFile, _Hosting);
                        ItemModel.ImagePath= Item.ImagePath = !string.IsNullOrEmpty(ImagePath) ? ImagePath : Item.ImagePath;
                        _ItemService.Add(Item);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Item", CreateNewBtnAction = "Create", BackToListControler = "Item", BackToListAction = "Index", EditBtnControler = "Item", EditBtnAction = "Edit", DeleteBtnControler = "Item", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
            IntializeDropdowens();
            return View(ItemModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItems.SystemName, CurrentUser, PermissionActions.Edit))
                return AccessDeniedView();
            Item Item = _ItemService.GetById(Id);
            ItemModel ItemModel = _Mapper.Map<ItemModel>(Item);

            if (CurrentUser != null && CurrentUser.CompanyId.HasValue)
            {
                var company = _CompanyService.GetById(CurrentUser.CompanyId.Value);
                ItemModel.TaxAuthorityRegestrationNumber = company.TaxAuthorityRegestrationNumber;
            }

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Item", CreateNewBtnAction = "Create", BackToListControler = "Item", BackToListAction = "Index", EditBtnControler = "Item", EditBtnAction = "Edit", DeleteBtnControler = "Item", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            IntializeDropdowens();
            return View(ItemModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ItemModel ItemModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageItems.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();
                if (ModelState.IsValid)
                {
                    Item Item = _Mapper.Map<Item>(ItemModel);
                    //var currentUser = _identityService.GetCurrentUser();
                    Item.SetBasicData(CRUD_OperationType.Update, CurrentUser);
                    string ImagePath = Uploader.UploadImage(ItemModel.ItemImageFile, _Hosting);
                    Item.ImagePath = ItemModel.ImagePath = !string.IsNullOrEmpty(ImagePath) ? ImagePath : Item.ImagePath;
                    _ItemService.Update(Item);
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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Item", CreateNewBtnAction = "Create", BackToListControler = "Item", BackToListAction = "Index", EditBtnControler = "Item", EditBtnAction = "Edit", DeleteBtnControler = "Item", DeleteBtnAction = "Delete", RouteId = ItemModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };

            IntializeDropdowens();

            return View(ItemModel);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageItems.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            Item Item = _ItemService.GetById(Id);
            ItemModel ItemModel = _Mapper.Map<ItemModel>(Item);


            ItemModel.UnitName = _currentLanguage == "ar" ? _UnitService.GetById(ItemModel.DefaultUnit)?.NameAr : _UnitService.GetById(ItemModel.DefaultUnit)?.NameEn;
            ItemModel.GroupName = _currentLanguage == "ar" ? _ItemGroupService.GetById(ItemModel.GroupId)?.NameAr : _ItemGroupService.GetById(ItemModel.GroupId)?.NameEn;
            ItemModel.TypeName = PresentationExtensions.ConvertEnumToSelectListItems(typeof(ItemTypes), _LocalizationService).Where(x => x.Value == ItemModel.TypeId.ToString()).Select(x => x.Text).FirstOrDefault();

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Item", CreateNewBtnAction = "Create", BackToListControler = "Item", BackToListAction = "Index", EditBtnControler = "Item", EditBtnAction = "Edit", DeleteBtnControler = "Item", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = true, BackToListBtnVisibilty = true, SaveBtnVisibilty = false, DeleteBtnVisibilty = true };


            return View(ItemModel);
        }
       
        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageItems.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

                var msg = _webHelper.IsAllowedDeleteItem(Id);
                if (msg != "")
                {
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString(msg).Value });

                }

                Item Item = _ItemService.GetById(Id);
                _ItemService.Delete(Item);
                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }
        }

        public IActionResult GetItemData(int ItemId)
        {
            var ItemGroup = _ItemService.GetById(ItemId);
            if (ItemGroup is not null)
            {
                var data = new { ItemGroup.GroupId, ItemGroup.DefaultUnit, ItemGroup.SalesPrice,ItemGroup.PurchasePrice };
                return new JsonResult(data);
            }
            else
            {
                return new JsonResult(new{ GroupId=0, DefaultUnit=0 , SalesPrice=0, PurchasePrice=0});
            }

        }




    }
}
