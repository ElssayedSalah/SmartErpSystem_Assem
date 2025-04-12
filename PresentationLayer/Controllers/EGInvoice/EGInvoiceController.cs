using BusinessLayer.EGElectronicInvoice;
using BusinessLayer.Models.EGInvoice;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.Purchases;
using DataAccessLayer.Entities.Sales;
using DataAccessLayer.Entities.System;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static BusinessLayer.Helpers.SharedEnums;

namespace PresentationLayer.Controllers.EGInvoice
{
    public class EGInvoiceController : BaseAdminController
    {    
        private readonly LocalizationService _LocalizationService;
        private readonly string _currentLanguage;
        private readonly IBaseService<Customer> _CustomerService;
        private readonly IBaseService<Suppler> _SupplerService;
        private readonly IBaseService<Company> _CompanyService;
        private readonly IInventoryService<Transaction_InvMaster, Transaction_InvDetails> _InvMasterService;
        private readonly IBaseService<Item> _ItemService;
        private readonly IBaseService<Branch> _BranchService;
        private readonly IBaseService<Unit> _UnitService;
        private readonly IBaseService<Currency> _CurrencyService;
        private readonly IBaseService<Taxes> _TaxesService;
        private readonly IidentityService _IdentityService;

        public EGInvoiceController(LocalizationService localizationService, IBaseService<Customer> CustomerService, IBaseService<Suppler> SupplerService, IBaseService<Company> CompanyService, IInventoryService<Transaction_InvMaster, Transaction_InvDetails> InvMasterService, IBaseService<Item> ItemService, IBaseService<Branch> BranchService, IBaseService<Unit> UnitService, IBaseService<Taxes> TaxesService, IBaseService<Currency> CurrencyService, IidentityService IdentityService)
        {
            _LocalizationService = localizationService;   
            _currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            _CustomerService = CustomerService;
            _SupplerService = SupplerService;
            _CompanyService = CompanyService;
            _InvMasterService = InvMasterService;
            _ItemService = ItemService;
            _BranchService = BranchService;
            _UnitService = UnitService;
            _CurrencyService = CurrencyService;
            _TaxesService = TaxesService;
            _IdentityService = IdentityService;



        }


        public void IntializeDropdowens()
        {        

            ViewBag.DocumentTypes = PresentationExtensions.ConvertEnumToSelectListItems(typeof(EGInvoiceDocumentType), _LocalizationService);
            ViewBag.DocumentStates = PresentationExtensions.ConvertEnumToSelectListItems(typeof(EGInvoiceDocumentState), _LocalizationService);


        }
        public IActionResult Index()
        {
            if (!_IdentityService.Authorize(StandardPermissionProvider.ManageEGInvoice.SystemName, CurrentUser, PermissionActions.List) && !_IdentityService.IsSuperUser(CurrentUser).Result)
                return AccessDeniedView();

            IntializeDropdowens();
            var doc = new EGInvoiceModel() {Master=new InvMaster {From=new DateTime(DateTime.Now.Year, DateTime.Now.Month,1) ,To= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 30) } };
            return View(doc);
        }
        [HttpPost]
        public IActionResult Index(EGInvoiceModel model)
        {
            if (!_IdentityService.Authorize(StandardPermissionProvider.ManageEGInvoice.SystemName, CurrentUser, PermissionActions.Edit) && !_IdentityService.IsSuperUser(CurrentUser).Result)
                return AccessDeniedView();

            IntializeDropdowens();

            try
            {                
                     model.InvDetails = new List<InvDetails>();
                
                    var customers = _CustomerService.GetAll();
                    var suppliers = _SupplerService.GetAll();
                    var company = _CompanyService.GetById(CurrentUser.CompanyId.Value);

                    var docs = _InvMasterService.GetWithCondetion(x => (x.DocTypeId == (int)DocumentTypes.SalesInvoice || x.DocTypeId == (int)DocumentTypes.SalesReturn /*|| x.DocTypeId == (int)DocumentTypes.PurchaseInvoice*/ || x.DocTypeId == (int)DocumentTypes.PurchaseReturn) && x.DocDate >= model.Master.From && x.DocDate <= model.Master.To && x.CompanyId == company.Id).ToList();
                    if (model.Master.DocumentType== EGInvoiceDocumentType.Invoice)
                    {
                    //فاتورة بيع او شراء
                        docs = docs.Where(x => x.DocTypeId == (int)DocumentTypes.PurchaseInvoice || x.DocTypeId == (int)DocumentTypes.SalesInvoice).ToList();
                    }
                    if (model.Master.DocumentType == EGInvoiceDocumentType.Debit)
                    {
                    //مرتجع مورد
                     docs = docs.Where(x => x.DocTypeId == (int)DocumentTypes.PurchaseReturn).ToList();

                    }
                   if (model.Master.DocumentType == EGInvoiceDocumentType.Credit)
                    {
                    //مرتجع عميل
                        docs = docs.Where(x => x.DocTypeId == (int)DocumentTypes.SalesReturn).ToList();

                    }

                    if (model.Master.DocumentState == EGInvoiceDocumentState.Valid)
                    {
                        docs = docs.Where(x => x.InvoiceState == "Valid").ToList();
                    }
                    if (model.Master.DocumentState == EGInvoiceDocumentState.Invalid)
                    {
                        docs = docs.Where(x => x.InvoiceState == "Invalid").ToList();
                    }
                    if (model.Master.DocumentState == EGInvoiceDocumentState.Cancelled)
                    {
                        docs = docs.Where(x => x.InvoiceState == "Cancelled").ToList();
                    }
                    if (model.Master.DocumentState == EGInvoiceDocumentState.Rejected)
                    {
                        docs = docs.Where(x => x.InvoiceState == "Rejected").ToList();
                    }
                    
                    if (model.Master.DocumentState == EGInvoiceDocumentState.NotSended)
                    {
                        docs = docs.Where(x => x.UUID == null || x.UUID == "").ToList();
                    }

                  var Details = docs.Select(x =>
                    {
                        InvDetails d = new InvDetails();
                        d.Id = x.Id;
                        d.UUID = x.UUID;
                        d.DocCode = x.Code;
                        d.DocDate = x.DocDate;

                        if (x.DocTypeId == (int)DocumentTypes.SalesInvoice || x.DocTypeId == (int)DocumentTypes.PurchaseInvoice)
                        {
                            d.DocTypeName = "فاتورة بيع";
                            d.DocType = "I";
                        }
                        if (x.DocTypeId == (int)DocumentTypes.PurchaseReturn)
                        {
                            d.DocTypeName = "مرتجع مورد";
                            d.DocType = "D";
                        }
                        if (x.DocTypeId == (int)DocumentTypes.SalesReturn)
                        {
                            d.DocTypeName = "مرتجع عميل";
                            d.DocType = "C";
                        }
                        if (x.UUID == null || x.UUID == "")
                        {
                            d.DocState = "لم يتم الارسال";
                        }
                        else
                        {
                            d.DocState = x.InvoiceState;
                        }
                        d.Issuer = company != null ? company.NameAr : "";
                        if (x.DocTypeId == (int)DocumentTypes.PurchaseReturn )
                        {
                            d.Receiver = suppliers.Where(a => a.Id == x.SupplierId).FirstOrDefault().NameAr;
                        }
                        else  
                        {
                            d.Receiver = customers.Where(a => a.Id == x.CustomerId).FirstOrDefault().NameAr;
                        }

                        //model.InvDetails.Add(d);
                        return d;

                    }).ToList();

                  model.InvDetails.AddRange(Details);
            }
            catch (Exception)
            {
            }
            return View(model);
        }

        public IActionResult SendEInvoice(int Id,string DocType)
        {
            if (Id > 0)
            {
                //الفاتورة المصرية
                if (EGInvoiceCredentials.E_InvoiceType == 1)
                {
                    if (CurrentUser != null && CurrentUser.CompanyId != null)
                    {
                        Transaction_InvMaster master = _InvMasterService.GetById(Id);
                        List<Transaction_InvDetails> details = _InvMasterService.GetInventoryTransactionDetails(x => x.MasterId == master.Id);
                        if (master != null && master.Transaction_InvDetails != null && master.Transaction_InvDetails.Count() > 0)
                        {
                            var company = _CompanyService.GetById(CurrentUser.CompanyId.Value);
                            if (company != null && company.ActivateEInvoice)
                            {
                                string ValidateItemMsg = "";
                                string ValidateUnitMsg = "";
                                var branch = _BranchService.GetById(master.BranchId.Value);
                                var customer = master.CustomerId.HasValue? _CustomerService.GetById(master.CustomerId.Value):null;
                                var suppler = master.SupplierId.HasValue? _SupplerService.GetById(master.SupplierId.Value):null;
                                var curuncy = _CurrencyService.GetById(master.CurrencyId);
                                var items = _ItemService.GetAll();
                                var units = _UnitService.GetAll();

                                var ValidateCustomerMsg = EInvoiceHelper.ValidateCustomerForEInvoice(customer);
                                var ValidatesupplerMsg = EInvoiceHelper.ValidateSuplierForEInvoice(suppler);
                                var ValidateCuruncyMsg = EInvoiceHelper.ValidateCuruncyForEInvoice(curuncy);
                                var ValidateCompanyMsg = EInvoiceHelper.ValidateCompanyForEInvoice(company);

                                if (ValidateCustomerMsg != string.Empty && (DocType=="I" || DocType == "C"))
                                {
                                    return Json(new { type = "faild", msg = ValidateCustomerMsg });
                                }
                                if (ValidatesupplerMsg != string.Empty && DocType == "D")
                                {
                                    return Json(new { type = "faild", msg = ValidatesupplerMsg });
                                }
                                if (ValidateCuruncyMsg != string.Empty)
                                {
                                    return Json(new { type = "faild", msg = ValidateCuruncyMsg });
                                }
                                if (ValidateCompanyMsg != string.Empty)
                                {
                                    return Json(new { type = "faild", msg = ValidateCompanyMsg });
                                }


                                foreach (var i in master.Transaction_InvDetails)
                                {
                                    var item = items.Where(x => x.Id == i.ItemId).FirstOrDefault();
                                    ValidateItemMsg += EInvoiceHelper.ValidateItemForEInvoice(item);

                                    var unit = units.Where(x => x.Id == i.UnitId).FirstOrDefault();
                                    ValidateUnitMsg += EInvoiceHelper.ValidateUnitForEInvoice(unit);
                                }
                                if (ValidateItemMsg != string.Empty)
                                {
                                    return Json(new { type = "faild", msg = ValidateItemMsg });
                                }
                                if (ValidateUnitMsg != string.Empty)
                                {
                                    return Json(new { type = "faild", msg = ValidateUnitMsg });
                                }

                                var Tax = _TaxesService.GetWithCondetion(x => x.TaxId == 1).FirstOrDefault();
                                if (Tax != null)
                                {
                                    if (EInvoiceHelper._AccessToken == null)
                                    {
                                        EInvoiceHelper.FetchToken();

                                    }
                                    if (EInvoiceHelper._AccessToken != null)
                                    {
                                        //انشاء الفاتورة الالكترونية
                                        var Document = EInvoiceHelper.CreateEInvoiceDocument(master, company, branch, items, units, curuncy, Tax, DocType, customer, suppler);
                                        //ارسال الفاتورة
                                        Task<SubmissionDocumentResult> Result = EInvoiceHelper.SendInvoiceToApi(Document);

                                        if (Result != null && Result.Result.SubmissionId != null && Result.Result.AcceptedDocuments != null && Result.Result.AcceptedDocuments.Count > 0)
                                        {

                                            Task<DocumentDetailsResult> docState = EInvoiceHelper.GetInvoiceDetailsFromApi(Result.Result.AcceptedDocuments.FirstOrDefault().UUID);//حالة الفاتورة
                                            master.UUID = Result.Result.AcceptedDocuments.FirstOrDefault().UUID;
                                            master.InvoiceState = docState.Result.Status;

                                            _InvMasterService.Update(master);

                                            return Json(new { type = "success", msg = $"تم إرسال الفاتورة ورقم التعريف { master.UUID }" });
                                        }
                                        else
                                        {
                                            return Json(new { type = "faild", msg = "حدث خطأ أثناء إرسال الفاتورة" });
                                        }

                                    }
                                    else
                                    {
                                        return Json(new { type = "faild", msg = "حدث خطأ أثناء الاتصال بالخادم" });
                                    }
                                }
                                else
                                {
                                    return Json(new { type = "faild", msg = "يجب ادخال إعدادات الضريبة" });
                                }
                            }
                            else
                            {
                                return Json(new { type = "faild", msg = "يجب تفعيل الفاتورة الالكترونية من شاشة الشركة" });
                            }
                        }
                        else
                        {
                            return Json(new { type = "faild", msg = "يجب ادخال تفاصيل الفاتورة" });
                        }
                    }
                    else
                    {
                        return Json(new { type = "faild", msg = "Erorr" });
                    }
                }
                //الفاتورة السعودية
                else if (EGInvoiceCredentials.E_InvoiceType == 2)
                {
                    //الفاتورة السعودية







                    return Json(new { type = "faild", msg = "Erorr" });
                }
                else
                {
                    return Json(new { type = "faild", msg = "يجب إختيار نوع الفاتورة الالكترونية من شاشة إعدادات النظام " });
                }
            }
            else
            {
                return Json(new { type = "faild", msg = "Erorr" });
            }
        }

        public IActionResult CancelEInvoice(int Id)
        {
            if (Id > 0)
            {
                //الفاتورة المصرية
                if (EGInvoiceCredentials.E_InvoiceType == 1)
                {
                    if (CurrentUser != null && CurrentUser.CompanyId != null)
                    {
                        Transaction_InvMaster master = _InvMasterService.GetById(Id);
                        if (master != null)
                        {
                            var company = _CompanyService.GetById(CurrentUser.CompanyId.Value);
                            if (company != null && company.ActivateEInvoice)
                            {

                                if (EInvoiceHelper._AccessToken == null)
                                {
                                    EInvoiceHelper.FetchToken();
                                }
                                if (EInvoiceHelper._AccessToken != null)
                                {
                                    if (master.UUID != null && master.UUID != "")
                                    {
                                        if (EInvoiceHelper.CancelInvoiceToApi(master.UUID, "بيانات الفاتورة خاطئة").Result)
                                        {
                                            master.InvoiceState = "Cancelled";
                                            _InvMasterService.Update(master);

                                            return Json(new { type = "success", msg = $"تم إلغاء الفاتورة  بنجاح { master.UUID }" });

                                        }
                                        else
                                        {
                                           return Json(new { type = "faild", msg = "حدث خطأ أثناء الاتصال بالخادم" });

                                        }
                                    }
                                    else
                                    {
                                        return Json(new { type = "faild", msg = "حدث خطأ أثناء الاتصال بالخادم" });
                                    }


                                }
                                else
                                {
                                    return Json(new { type = "faild", msg = "حدث خطأ أثناء الاتصال بالخادم" });
                                }
                            }
                            else
                            {
                                return Json(new { type = "faild", msg = "يجب تفعيل الفاتورة الالكترونية من شاشة الشركة" });
                            }
                        }
                        else
                        {
                            return Json(new { type = "faild", msg = "يجب ادخال تفاصيل الفاتورة" });
                        }
                    }
                    else
                    {
                        return Json(new { type = "faild", msg = "Erorr" });
                    }
                }
                //الفاتورة السعودية
                else if (EGInvoiceCredentials.E_InvoiceType == 2)
                {
                    //الفاتورة السعودية







                    return Json(new { type = "faild", msg = "Erorr" });
                }
                else
                {
                    return Json(new { type = "faild", msg = "يجب إختيار نوع الفاتورة الالكترونية من شاشة إعدادات النظام " });
                }
            }
            else
            {
                return Json(new { type = "faild", msg = "Erorr" });
            }
        }


        public IActionResult RefreshEInvoice(int Id)
        {
            if (Id > 0)
            {
                //الفاتورة المصرية
                if (EGInvoiceCredentials.E_InvoiceType == 1)
                {
                    if (CurrentUser != null && CurrentUser.CompanyId != null)
                    {
                        Transaction_InvMaster master = _InvMasterService.GetById(Id);
                        if (master != null)
                        {
                            var company = _CompanyService.GetById(CurrentUser.CompanyId.Value);
                            if (company != null && company.ActivateEInvoice)
                            {
                                if (EInvoiceHelper._AccessToken == null)
                                {
                                    EInvoiceHelper.FetchToken();
                                }
                                if (EInvoiceHelper._AccessToken != null)
                                {
                                    if (master.UUID != null && master.UUID != "")
                                    {
                                        Task<DocumentDetailsResult> docState = EInvoiceHelper.GetInvoiceDetailsFromApi(master.UUID);//حالة الفاتورة
                                        master.InvoiceState = docState.Result.Status;
                                        _InvMasterService.Update(master);
                                        return Json(new { type = "success", msg =docState.Result.Status});
                                    }
                                    else
                                    {
                                        return Json(new { type = "faild", msg = "حدث خطأ أثناء الاتصال بالخادم" });
                                    }
                                }
                                else
                                {
                                    return Json(new { type = "faild", msg = "حدث خطأ أثناء الاتصال بالخادم" });
                                }
                            }
                            else
                            {
                                return Json(new { type = "faild", msg = "يجب تفعيل الفاتورة الالكترونية من شاشة الشركة" });
                            }
                        }
                        else
                        {
                            return Json(new { type = "faild", msg = "يجب ادخال تفاصيل الفاتورة" });
                        }
                    }
                    else
                    {
                        return Json(new { type = "faild", msg = "Erorr" });
                    }
                }
                else
                {
                    return Json(new { type = "faild", msg = "يجب إختيار نوع الفاتورة الالكترونية من شاشة إعدادات النظام " });

                }
            }
            else
            {
                return Json(new { type = "faild", msg = "Erorr" });
            }
        }





    }
}
