using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models.Inventory;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.System;
using Grand.Framework.Kendoui;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Controllers;
using PresentationLayer.Helpers;
using Reports;
using Reports.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static BusinessLayer.Helpers.SharedEnums;

namespace SmartErpApp.Controllers.Inventory
{
    [Authorize]
    //[ValidateAntiForgeryToken]
    public class StoreSettlementController : BaseAdminController
    {
        
        private readonly IInventoryService<Transaction_InvMaster, Transaction_InvDetails> _InventoryMasterService;
        private readonly IBaseService<Item> _ItemService;
        private readonly IBaseService<Branch> _BranchService;
        private readonly IBaseService<Store> _StoreService;
        private readonly IBaseService<ItemGroup> _ItemGroupService;
        private readonly IBaseService<Unit> _UnitService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly string _currentLanguage;
        private readonly IidentityService _identityService;
        private readonly IReportService _ReportService;
        private readonly IBaseService<FinancialPeriod> _FinancialPeriod;
        private readonly IBaseService<Document> _Document;
        private readonly IWebHelper _WebHelperService;
        public StoreSettlementController(
             IBaseService<Item> ItemService,
             IBaseService<ItemGroup> ItemGroupService,
             IMapper mapper,
             LocalizationService localizationService,
             IBaseService<Unit> UnitService,
             IBaseService<Branch> BranchService,
             IBaseService<Store> StoreService,            
             IInventoryService<Transaction_InvMaster,
             Transaction_InvDetails> InventoryMasterService,
             IidentityService IdentityService,
             IReportService ReportService,
             IBaseService<FinancialPeriod> FinancialPeriod,
             IBaseService<Document> Document,
             IWebHelper WebHelperService
            )
        {
            
            _ItemService = ItemService;
            _ItemGroupService = ItemGroupService;
            _UnitService = UnitService;
            _BranchService = BranchService;
            _StoreService = StoreService;
            _InventoryMasterService = InventoryMasterService;
            _identityService = IdentityService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _ReportService = ReportService;
            _FinancialPeriod = FinancialPeriod;
            _Document = Document;
            _WebHelperService = WebHelperService;


        }

        public void IntializeDropdowens()
        {
            var Groups = _ItemGroupService.GetAll();
            Groups.Insert(0, new ItemGroup() { Id = 0, NameAr = "", NameEn = "" });
            ViewBag.Groups = Groups.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Units = _UnitService.GetAll();
            Units.Insert(0, new Unit() { Id = 0, NameAr = "", NameEn = "" });
            ViewBag.Units = Units.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Branches = _BranchService.GetAll();
            Branches.Insert(0, new Branch() { Id = 0, NameAr = "", NameEn = "" });
            ViewBag.Branches = Branches.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Stores = _StoreService.GetAll();
            Stores.Insert(0, new Store() { Id = 0, NameAr = "", NameEn = "" });
            ViewBag.Stores = Stores.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var Items = _ItemService.GetAll();
            Items.Insert(0, new Item() { Id = 0, NameAr = "اختيار صنف", NameEn = "select" });
            ViewBag.Itmes = Items.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });            

            var StoreCounts = _InventoryMasterService.GetWithCondetion(x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId.Value && x.CompanyId == CurrentUser.CompanyId.Value && x.DocTypeId == (int)DocumentTypes.StoreCount).Select(x=>new Transaction_InvMaster (){ Id=x.Id,Code=x.Code }).ToList();
            StoreCounts.Insert(0, new Transaction_InvMaster() { Id = 0, Code = 0 });
            ViewBag.StoreCounts = StoreCounts;

        }
        public void SetBasicButtonsVisibilityForCreate()
        {

            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "StoreSettlement", CreateNewBtnAction = "Create", BackToListControler = "StoreSettlement", BackToListAction = "Index", EditBtnControler = "StoreSettlement", EditBtnAction = "Edit", DeleteBtnControler = "StoreSettlement", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false, PrintBtnVisibilty = false };

        }
        public void SetBasicButtonsVisibilityForEdit(int RouteId)
        {
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "StoreSettlement", CreateNewBtnAction = "Create", BackToListControler = "StoreSettlement", BackToListAction = "Index", EditBtnControler = "StoreSettlement", EditBtnAction = "Edit", DeleteBtnControler = "StoreSettlement", DeleteBtnAction = "Delete", RouteId = RouteId, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true, PrintBtnVisibilty = true, PrintBtnControler = "StoreSettlement", PrintBtnAction = "Print" };
        }
        public void ValidateModel(Transaction_InvMasterModel model)
        {
            if (model != null && model.Transaction_InvDetails != null)
            {
                if (model.BranchId == null || model.BranchId <= 0)
                {
                    ModelState.AddModelError("BranchId", "يجب ادخال جميع الحقول المطلوبه");
                }
                if (model.StoreId == null || model.StoreId <= 0)
                {
                    ModelState.AddModelError("StoreId", "يجب اختيار مخزن");
                }
                if (CurrentUser.CompanyId == null || CurrentUser.CompanyId <= 0)
                {
                    ModelState.AddModelError("CompanyId", "يجب اختيار شركة للمستخدم الحالي");
                }
                if (CurrentUser.FinancialPeriod == null || CurrentUser.FinancialPeriod <= 0)
                {
                    ModelState.AddModelError("FinancialPeriod", "يجب اختيار سنة مالية للمستخدم الحالي");
                }
                if (model.Transaction_InvDetails.Count > 1)
                {
                    foreach (var item in model.Transaction_InvDetails.Skip(1))
                    {
                        item.FinancialPeriodId = CurrentUser.FinancialPeriodId;
                        item.CompanyId = CurrentUser.CompanyId;
                        item.DocTypeId = (int)DocumentTypes.StoreSettlement;
                        item.DocDate = model.DocDate;
                        if (item.ItemId <= 0)
                        {
                            ModelState.AddModelError("ItemId", "يجب اختيار صنف");
                            
                        }                       

                    }
                }
                else
                {
                    ModelState.AddModelError("EMPTY_ITEMS", "يجب إضافة صنف علي الأقل");
                }


            }
            else
            {
                ModelState.AddModelError("NULL_MODEL", "يجب ادخال جميع الحقول المطلوبه");
            }

        }

        // GET: ItemController
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageStoreSettlement.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
            return View(new Transaction_InvMasterModel());
        }

        public IActionResult list()
        {
            //var currentUser = _identityService.GetCurrentUser();
            var TrMaster = _InventoryMasterService.GetInventoryTransactions(x=>x.FinancialPeriodId== CurrentUser.FinancialPeriodId&& x.DocTypeId == (int)DocumentTypes.StoreSettlement && x.CompanyId == CurrentUser.CompanyId);
            var TrMasterModel = _Mapper.Map<List<Transaction_InvMasterModel>>(TrMaster);
            var Branches = _BranchService.GetAll();
            var Stores = _StoreService.GetAll();
            foreach (var item in TrMasterModel)
            {
                item.BranchName = Branches.Where(x => x.Id == item.BranchId)?.Select(x => x.NameAr).FirstOrDefault();
                item.StoreName = Stores.Where(x => x.Id == item.StoreId)?.Select(x => x.NameAr).FirstOrDefault();
            }
            var gridModel = new DataSourceResult
            {
                Data = TrMasterModel,
                Total = TrMasterModel.Count
            };
            return Json(gridModel);
        }


        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageStoreSettlement.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView(); 
            
            var LastCode = _InventoryMasterService.GetLastCode(o => o.Code ,x=> x.FinancialPeriodId==CurrentUser.FinancialPeriodId && x.DocTypeId == (int)DocumentTypes.StoreSettlement && x.CompanyId == CurrentUser.CompanyId);

            var NewMasterModel = new Transaction_InvMasterModel() { Code = LastCode, DocDate = DateTime.Now};            

            //set basic buttons visibility
            SetBasicButtonsVisibilityForCreate();

            IntializeDropdowens();

            return View(NewMasterModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Transaction_InvMasterModel TransactionModel)
        {
            Transaction_InvMaster master = null;
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageStoreSettlement.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();

                var ValidateFinancePeriodMsg = _WebHelperService.ValidateFinancePeriod(CurrentUser.FinancialPeriodId.Value, TransactionModel.DocDate);
                if (ValidateFinancePeriodMsg != "")
                {
                    ViewData["NotificationMsg"] = Notification.Erorr(ValidateFinancePeriodMsg, NotificationCssType.danger.ToString());

                    SetBasicButtonsVisibilityForCreate();
                    IntializeDropdowens();
                    return View(TransactionModel);
                }


                ValidateModel(TransactionModel);
                if (ModelState.IsValid)
                {
                    if (TransactionModel.Transaction_InvDetails != null && TransactionModel.Transaction_InvDetails.Count > 0)
                    {
                        var currentUser = CurrentUser;
                        if (!_InventoryMasterService.IsExistRecord(b => b.Code == TransactionModel.Code && b.FinancialPeriodId== currentUser.FinancialPeriodId && b.DocTypeId == (int)DocumentTypes.StoreSettlement && b.CompanyId == CurrentUser.CompanyId))
                        {
                            master = _Mapper.Map<Transaction_InvMaster>(TransactionModel);
                            master.Transaction_InvDetails = master.Transaction_InvDetails.Skip(1).ToList();                           
                            master.SetBasicData(SharedEnums.CRUD_OperationType.Create, (int)DocumentTypes.StoreSettlement, currentUser);

                            _InventoryMasterService.AddInventoryTransaction(master);

                            ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("SavedSuccessfuly"), NotificationCssType.success.ToString());
                        }
                        else
                        {
                            ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("DuplicateCode"), NotificationCssType.danger.ToString());
                        }
                    }
                    else
                    {
                        TransactionModel.Transaction_InvDetails = new List<Transaction_InvDetailsModel>();
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("NotItemsSelected"), NotificationCssType.danger.ToString());

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

            if (master != null && master.Id > 0)
            {
                return RedirectToAction("Edit", new { Id = master.Id, IsNew = true });
            }
            else
            {
                SetBasicButtonsVisibilityForCreate();
                IntializeDropdowens();
                return View(TransactionModel);
            } 
        }

        [HttpGet]
        public IActionResult Edit(int Id,bool IsNew)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageStoreSettlement.SystemName, CurrentUser, PermissionActions.Edit))
                return AccessDeniedView();
            Transaction_InvMaster master = _InventoryMasterService.GetById(Id);

            //رقم حركة الجرد
            var StoreCount = _InventoryMasterService.GetById(master.StoreCountId);           

            List<Transaction_InvDetails> details = _InventoryMasterService.GetInventoryTransactionDetails(x => x.MasterId == master.Id);
            master.Transaction_InvDetails = details;
            master.Transaction_InvDetails.Insert(0, new Transaction_InvDetails());

            Transaction_InvMasterModel masterModel = _Mapper.Map<Transaction_InvMasterModel>(master);
            masterModel.StoreCountNumber = StoreCount != null ? StoreCount.Code : 0;

            if (CurrentUser != null && CurrentUser.FinancialPeriodId.HasValue)
            {
                var FinancialPeriod = _FinancialPeriod.GetById(CurrentUser.FinancialPeriodId.Value);
                masterModel.CountDateFrom = FinancialPeriod.DateFrom;
                masterModel.CountDateTo = FinancialPeriod.DateTo;

            }
            //to show save message if the request came from create action
            if (IsNew)
            {
                ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("SavedSuccessfuly"), NotificationCssType.success.ToString());
            }
            //set basic buttons visibility
            SetBasicButtonsVisibilityForEdit(Id);
            IntializeDropdowens();
            return View(masterModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Transaction_InvMasterModel TransactionModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageStoreSettlement.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();

                var ValidateFinancePeriodMsg = _WebHelperService.ValidateFinancePeriod(CurrentUser.FinancialPeriodId.Value, TransactionModel.DocDate);
                if (ValidateFinancePeriodMsg != "")
                {
                    ViewData["NotificationMsg"] = Notification.Erorr(ValidateFinancePeriodMsg, NotificationCssType.danger.ToString());

                    SetBasicButtonsVisibilityForEdit(TransactionModel.Id);
                    IntializeDropdowens();
                    return View(TransactionModel);
                }

                ValidateModel(TransactionModel);
                if (ModelState.IsValid)
                {
                    if (TransactionModel.Transaction_InvDetails != null && TransactionModel.Transaction_InvDetails.Count > 0)
                    {
                        Transaction_InvMaster master = _Mapper.Map<Transaction_InvMaster>(TransactionModel);
                        master.Transaction_InvDetails = master.Transaction_InvDetails.Skip(1).ToList();
                        //get old details to be deleted 
                        List<Transaction_InvDetails> oldDetails = _InventoryMasterService.GetInventoryTransactionDetails(x => x.MasterId == master.Id);
                        master.SetBasicData(SharedEnums.CRUD_OperationType.Update, (int)DocumentTypes.StoreSettlement, CurrentUser);

                        _InventoryMasterService.UpdateInventoryTransaction(master, oldDetails);
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("UpdatedSuccessfuly"), NotificationCssType.success.ToString());

                    }
                    else
                    {
                        TransactionModel.Transaction_InvDetails = new List<Transaction_InvDetailsModel>();
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("NotItemsSelected"), NotificationCssType.danger.ToString());

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
            SetBasicButtonsVisibilityForEdit(TransactionModel.Id);
            IntializeDropdowens();

            return View(TransactionModel);
        }

        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageStoreSettlement.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });
                Transaction_InvMaster master = _InventoryMasterService.GetById(Id);

                var ValidateFinancePeriodMsg = _WebHelperService.ValidateFinancePeriod(CurrentUser.FinancialPeriodId.Value, master.DocDate);
                if (ValidateFinancePeriodMsg != "")
                {
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("CuruntPeriodIsClosedFinal").Value });
                }

                _InventoryMasterService.Delete(master);
                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }
        } 
        
        public IActionResult GetBranchStores(int BranchId)
        {
            try
            {
                var stores =_StoreService.GetAll().Where(x=>x.BranchId==BranchId).Select(x=>new {Name= _currentLanguage == "ar" ? x.NameAr : x.NameEn,Id=x.Id });               
                return Json(stores);
            }
            catch (Exception)
            {
                return Json("erorr");
            }
        }

        public IActionResult GetStoreCount(int StoreCountId)
        {
            try
            {
                List<Transaction_InvDetails> detail = new List<Transaction_InvDetails>();
                var master = _InventoryMasterService.GetById(StoreCountId);
                var details = _InventoryMasterService.GetInventoryTransactionDetails(x=>x.MasterId== StoreCountId);
                detail = details.Select(x =>
                 {
                     Transaction_InvDetails d = new Transaction_InvDetails();
                     d.ItemId = x.ItemId;
                     d.GroupId = x.GroupId.Value;
                     d.UnitId = x.UnitId.Value;
                     d.Quntity = x.Quntity;
                     d.ActualQuntity = x.ActualQuntity;
                     d.DifferenceQuntity = x.DifferenceQuntity;
                     d.PurchasePrice = x.PurchasePrice;
                     d.Total = x.Total;                    
                     return d;
                 }).ToList();
                master.Transaction_InvDetails = detail;
                return Json(new { master });
            }
            catch (Exception)
            {
                return Json("erorr");
            }
        }



        public IActionResult Print(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageStoreSettlement.SystemName, CurrentUser, PermissionActions.Report))
                return AccessDeniedView();

            if (Id > 0)
            {
                StoreSettlementRptPrint report = new StoreSettlementRptPrint(_LocalizationService);
                var DataSource = _ReportService.StoreSettlementPrint(Id);
                report.DataSource = DataSource;
                var stream = report.GenerateReport("pdf");
                stream.Position = 0;
                return File(stream, "application/pdf", "StoreSettlementRpt.pdf");
            }
            else
            {
                return View("Error");
            }

        }
    }
}
