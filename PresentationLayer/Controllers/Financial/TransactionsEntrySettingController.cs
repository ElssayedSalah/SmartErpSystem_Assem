using AutoMapper;
using BusinessLayer.Models.Financial;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.System;
using Grand.Framework.Kendoui;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static BusinessLayer.Helpers.SharedEnums;


namespace PresentationLayer.Controllers.Sales
{

    [Authorize]
    public class TransactionsEntrySettingController : BaseAdminController
    {
        
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly IidentityService _identityService;
        private readonly IWebHelper _webHelper;
        private readonly string _currentLanguage;
        private readonly IBaseService<TransactionsEntrySettingMaster> _TransactionsEntrySettingMasterService;
        private readonly IBaseService<Document> _DocumentService;
        private readonly IBaseService<DailyAccounts_Def> _DailyAccounts_DefService;
        private readonly IBaseService<TransactionsEntrySettingDetails> _TransactionsEntrySettingDetails;


        public TransactionsEntrySettingController( IMapper mapper, LocalizationService localizationService, IidentityService IdentityService,  IWebHelper webHelper, IBaseService<TransactionsEntrySettingMaster> TransactionsEntrySettingMasterService, IBaseService<Document> DocumentService, IBaseService<DailyAccounts_Def> DailyAccounts_DefService, IBaseService<TransactionsEntrySettingDetails> TransactionsEntrySettingDetails)
        {
            _TransactionsEntrySettingMasterService = TransactionsEntrySettingMasterService;
            _DocumentService = DocumentService;
            _DailyAccounts_DefService = DailyAccounts_DefService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _identityService = IdentityService;
            _TransactionsEntrySettingDetails = TransactionsEntrySettingDetails;
            _webHelper = webHelper;
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
        }

        public void IntializeDropdowens()
        {          

            var Documents = _DocumentService.GetAll();
            Documents.Insert(0, new Document() { DocTypeId = 0, NameAr = "اختيار", NameEn = "select" });
            ViewBag.Documents = Documents.Select(x => new { x.DocTypeId, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            var DailyAccounts = _DailyAccounts_DefService.GetAll();
            DailyAccounts.Insert(0, new DailyAccounts_Def() { Id = 0, NameAr = "اختيار يومية", NameEn = "select" });
            ViewBag.DailyAccounts = DailyAccounts.Select(x => new { x.Id, Name = _currentLanguage == "ar" ? x.NameAr : x.NameEn });

            ViewBag.SideTypes = PresentationExtensions.ConvertEnumToSelectListItemsStartFromZeroIndex(typeof(EntrySides), _LocalizationService);

            ViewBag.SideNatural = PresentationExtensions.ConvertEnumToSelectListItemsStartFromZeroIndex(typeof(AccountNatures), _LocalizationService);

        }
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageTransactionsEntrySettinges.SystemName, CurrentUser, PermissionActions.List))
              return AccessDeniedView();

            return View(new TransactionsEntrySettingMasterModel());
        }

        public IActionResult list()
        {
            var TransactionsEntrySetting = _TransactionsEntrySettingMasterService.GetAll();
            var TransactionsEntrySettingModel = _Mapper.Map<List<TransactionsEntrySettingMasterModel>>(TransactionsEntrySetting);

            var documents = _DocumentService.GetAll();
            TransactionsEntrySettingModel = TransactionsEntrySettingModel.Select(x =>
            {
                var docName = "";
                if (documents.Count > 0)
                {
                    docName = _currentLanguage == "ar" ? documents.Where(d => d.DocTypeId == x.DocTypeId).FirstOrDefault().NameAr : documents.Where(d => d.DocTypeId == x.DocTypeId).FirstOrDefault().NameEn;
                }
                var l = x;
                l.DocTypeName = docName;
                return x;
            }).ToList();

            var gridModel = new DataSourceResult
            {
                Data = TransactionsEntrySettingModel,
                Total = TransactionsEntrySettingModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageTransactionsEntrySettinges.SystemName, CurrentUser, PermissionActions.Create))
               return AccessDeniedView();   
         
            IntializeDropdowens();
   
            return View();
        }
        [HttpPost]
        public IActionResult Create(TransactionsEntrySettingMasterModel model)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageTransactionsEntrySettinges.SystemName, CurrentUser, PermissionActions.Create))
                  return AccessDeniedView();
                if (model.FirstSideIds==null ||model.FirstSideIds.FirstOrDefault()==0)
                {
                    ModelState.AddModelError("FirstSideIds", "يجب اختيار الطرف الاول");
                }
                if (model.SecondSideIds==null ||model.SecondSideIds.FirstOrDefault() == 0)
                {
                    ModelState.AddModelError("SecondSideIds", "يجب اختيار الطرف الثاني");
                }

                if (ModelState.IsValid)
                {
                    var oldDocumentSetting = _TransactionsEntrySettingMasterService.GetWithCondetion(x=>x.DocTypeId==model.DocTypeId).FirstOrDefault();
                    if (oldDocumentSetting==null)
                    {
                        var entity = _Mapper.Map<TransactionsEntrySettingMaster>(model);
                         _TransactionsEntrySettingMasterService.Add(entity);                       

                        var FirstSideIds = new List<TransactionsEntrySettingDetails>();
                        foreach (var item in model.FirstSideIds)
                        {
                            FirstSideIds.Add(new TransactionsEntrySettingDetails() { MasterId = entity.Id, SideTypeId = 1, SideId = item });  
                        }
                        _TransactionsEntrySettingDetails.AddRange(FirstSideIds);
                        var SecondSideIds = new List<TransactionsEntrySettingDetails>();
                        foreach (var item in model.SecondSideIds)
                        {
                            SecondSideIds.Add(new TransactionsEntrySettingDetails() { MasterId = entity.Id, SideTypeId = 2, SideId = item });
                        }
                        _TransactionsEntrySettingDetails.AddRange(SecondSideIds);


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

            IntializeDropdowens();
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageTransactionsEntrySettinges.SystemName, CurrentUser, PermissionActions.Delete))
             return AccessDeniedView();

            var DocumentSetting = _TransactionsEntrySettingMasterService.GetWithCondetion(x => x.Id == Id).FirstOrDefault();
            var DocumentSettingModel = _Mapper.Map<TransactionsEntrySettingMasterModel>(DocumentSetting);

            var SideDetails = _TransactionsEntrySettingDetails.GetWithCondetion(x => x.MasterId ==Id).ToList();
            DocumentSettingModel.FirstSideIds = SideDetails.Where(x=>x.SideTypeId==1).Select(x=>x.SideId).ToList();
            DocumentSettingModel.SecondSideIds = SideDetails.Where(x=>x.SideTypeId==2).Select(x=>x.SideId).ToList();

            IntializeDropdowens();

            return View(DocumentSettingModel);
        }
        [HttpPost]
        public IActionResult Edit(TransactionsEntrySettingMasterModel model)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageTransactionsEntrySettinges.SystemName, CurrentUser, PermissionActions.Edit))
                 return AccessDeniedView();

                if (model.FirstSideIds == null || model.FirstSideIds.FirstOrDefault() == 0)
                {
                    ModelState.AddModelError("FirstSideIds", "يجب اختيار الطرف الاول");
                }
                if (model.SecondSideIds == null || model.SecondSideIds.FirstOrDefault() == 0)
                {
                    ModelState.AddModelError("SecondSideIds", "يجب اختيار الطرف الثاني");
                }

                if (ModelState.IsValid)
                {
                    var entity = _Mapper.Map<TransactionsEntrySettingMaster>(model);
                    _TransactionsEntrySettingMasterService.Update(entity);

                    var oldSideDetails = _TransactionsEntrySettingDetails.GetWithCondetion(x=>x.MasterId==model.Id).ToList();
                    _TransactionsEntrySettingDetails.DeleteMany(oldSideDetails);

                    var FirstSideIds = new List<TransactionsEntrySettingDetails>();
                    foreach (var item in model.FirstSideIds)
                    {
                        FirstSideIds.Add(new TransactionsEntrySettingDetails() { MasterId = entity.Id, SideTypeId = 1, SideId = item });
                    }
                    _TransactionsEntrySettingDetails.AddRange(FirstSideIds);
                    var SecondSideIds = new List<TransactionsEntrySettingDetails>();
                    foreach (var item in model.SecondSideIds)
                    {
                        SecondSideIds.Add(new TransactionsEntrySettingDetails() { MasterId = entity.Id, SideTypeId = 2, SideId = item });
                    }
                    _TransactionsEntrySettingDetails.AddRange(SecondSideIds);


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

            IntializeDropdowens();

            return View(model);
        }

        
        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageTransactionsEntrySettinges.SystemName, CurrentUser, PermissionActions.Delete))
                  return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

                var oldSideDetails = _TransactionsEntrySettingDetails.GetWithCondetion(x => x.MasterId == Id).ToList();
                _TransactionsEntrySettingDetails.DeleteMany(oldSideDetails);

                var master = _TransactionsEntrySettingMasterService.GetById(Id);
                _TransactionsEntrySettingMasterService.Delete(master);
                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });
            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }

        }

        

    }


}
