using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Models.Sales;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.Sales;
using DataAccessLayer.Entities.System;
using Grand.Framework.Kendoui;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using static BusinessLayer.Helpers.SharedEnums;


namespace PresentationLayer.Controllers.Sales
{

    [Authorize]
    public class CustomerController : BaseAdminController
    {
        private readonly IBaseService<Customer> _CustomerService;
        private readonly IMapper _Mapper;
        private readonly LocalizationService _LocalizationService;
        private readonly IidentityService _identityService;
        private readonly IBaseService<Countries> _CountriesService;
        private readonly IWebHelper _webHelper;
        private readonly IBaseService<CustomerOpenBalance> _CustomerOpenBalanceService;
        private readonly IBaseService<DefaultAccount> _DefaultAccountService;
        private readonly IBaseService<Account> _AccountService;
        private readonly IBaseService<SystemSetting> _SystemSettingService;
        private readonly IFinanceService<DailyEntryMaster, DailyEntryDetails> _DailyEntryService;
        private readonly IBaseService<Currency> _CurrencyService;
        public CustomerController(IBaseService<Customer> CustomerService, IMapper mapper, LocalizationService localizationService, IidentityService IdentityService, IBaseService<Countries> CountriesService, IWebHelper webHelper, IBaseService<CustomerOpenBalance> CustomerOpenBalanceService, IBaseService<DefaultAccount> DefaultAccountService, IBaseService<Account> AccountService, IBaseService<SystemSetting> SystemSettingService, IFinanceService<DailyEntryMaster, DailyEntryDetails> DailyEntryService, IBaseService<Currency> CurrencyService)
        {
            _CustomerService = CustomerService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _identityService = IdentityService;
            _CountriesService = CountriesService;
            _webHelper = webHelper;
            _CustomerOpenBalanceService = CustomerOpenBalanceService;
            _DefaultAccountService = DefaultAccountService;
            _AccountService = AccountService;
            _SystemSettingService = SystemSettingService;
            _DailyEntryService = DailyEntryService;
            _CurrencyService = CurrencyService;
        }

        public void IntializeDropdowens()
        {
           
            ViewBag.CustomerClassType = PresentationExtensions.ConvertEnumToSelectListItems2(typeof(CustomerClassType), _LocalizationService);
            ViewBag.IDType = PresentationExtensions.ConvertEnumToSelectListItems2(typeof(IDType), _LocalizationService);

            var Countries = _CountriesService.GetAll();
            Countries.Insert(0, new Countries() { Id = 0, NameAr = "إختار", NameEn = "Select" });
            ViewBag.Countries = Countries.Select(x => new { x.Id, Name =  x.Name});

            var accounts = _AccountService.GetWithCondetion(x => x.LastLevelInTree);
            accounts.Insert(0, new Account() { Id = 0, NameAr = "إختار", NameEn = "Select" });
            ViewBag.Accounts = accounts.Select(x => new { x.Id, Name = x.Name });

        }
        public IActionResult Index()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomeres.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();

            return View(new CustomerModel());
        }

        public IActionResult list()
        {
            var Customer = _CustomerService.GetAll();
            var CustomerModel = _Mapper.Map<List<CustomerModel>>(Customer);
            var gridModel = new DataSourceResult
            {
                Data = CustomerModel,
                Total = CustomerModel.Count
            };
            return Json(gridModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomeres.SystemName, CurrentUser, PermissionActions.Create))
                return AccessDeniedView();

            var LastCode = _CustomerService.GetLastCode(o => o.Code);
            var NewCustomer = new Customer() { Code = LastCode };

            var NewCustomerModel = _Mapper.Map<CustomerModel>(NewCustomer);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Customer", CreateNewBtnAction = "Create", BackToListControler = "Customer", BackToListAction = "Index", EditBtnControler = "Customer", EditBtnAction = "Edit", DeleteBtnControler = "Customer", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
            IntializeDropdowens();
            return View(NewCustomerModel);
        }
        [HttpPost]
        public IActionResult Create(CustomerModel CustomerModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomeres.SystemName, CurrentUser, PermissionActions.Create))
                    return AccessDeniedView();

                if (ModelState.IsValid)
                {
                    var SystemSetting = _SystemSettingService.GetAll().FirstOrDefault();
                    //حساب رأس المال من الحسابات الافتراضية
                    var CapitalAccount = _DefaultAccountService.GetWithCondetion(x => x.AccountNameId == 4).FirstOrDefault();

                    if ((SystemSetting != null && SystemSetting.EnableOpenEntryCreation) && (CustomerModel.AccountId <= 0 || CapitalAccount == null || CapitalAccount.AccountId <= 0))
                    {
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("CustomerAccountAndCapitalAccountRequired"), NotificationCssType.danger.ToString());

                        //set basic buttons visibility
                        ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Customer", CreateNewBtnAction = "Create", BackToListControler = "Customer", BackToListAction = "Index", EditBtnControler = "Customer", EditBtnAction = "Edit", DeleteBtnControler = "Customer", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
                        IntializeDropdowens();
                        return View(CustomerModel);
                    }

                    if (!_CustomerService.IsExistRecord(b => b.Code == CustomerModel.Code))
                    {
                        Customer Customer = _Mapper.Map<Customer>(CustomerModel);
                        Customer.SetBasicData(CRUD_OperationType.Create, CurrentUser);
                        _CustomerService.Add(Customer);

                        CustomerOpenBalance customerOpenBalance = new CustomerOpenBalance();
                        customerOpenBalance.CustomerId = Customer.Id;
                        customerOpenBalance.FinancialPeriodId = CurrentUser.FinancialPeriodId;
                        customerOpenBalance.CompanyId = CurrentUser.CompanyId;
                        customerOpenBalance.AccountId = CustomerModel.AccountId;
                        customerOpenBalance.EntryNumber = CustomerModel.EntryNumber;
                        customerOpenBalance.OpeningBalanceCredit = CustomerModel.OpeningBalanceCredit;
                        customerOpenBalance.OpeningBalanceDebit = CustomerModel.OpeningBalanceDebit;
                        _CustomerOpenBalanceService.Add(customerOpenBalance);

                        //add open entry for customer
                        if (SystemSetting != null && SystemSetting.EnableOpenEntryCreation && CapitalAccount != null && CustomerModel.AccountId > 0 && CapitalAccount.AccountId > 0)
                        {
                            AddCustomerOpenEntry(Customer, customerOpenBalance);
                        }

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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Customer", CreateNewBtnAction = "Create", BackToListControler = "Customer", BackToListAction = "Index", EditBtnControler = "Customer", EditBtnAction = "Edit", DeleteBtnControler = "Customer", DeleteBtnAction = "Delete", RouteId = 0, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = false };
            IntializeDropdowens();
            return View(CustomerModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomeres.SystemName, CurrentUser, PermissionActions.Delete))
                return AccessDeniedView();

            Customer Customer = _CustomerService.GetById(Id);
            CustomerModel CustomerModel = _Mapper.Map<CustomerModel>(Customer);

            CustomerOpenBalance customerOpenBalance = _CustomerOpenBalanceService.GetWithCondetion(x => x.CustomerId == Customer.Id && x.FinancialPeriodId.Value == CurrentUser.FinancialPeriodId.Value && x.CompanyId.Value == CurrentUser.CompanyId.Value).FirstOrDefault();

            if (customerOpenBalance != null)
            {
                CustomerModel.OpeningBalanceDebit = customerOpenBalance.OpeningBalanceDebit;
                CustomerModel.OpeningBalanceCredit = customerOpenBalance.OpeningBalanceCredit;
            }

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Customer", CreateNewBtnAction = "Create", BackToListControler = "Customer", BackToListAction = "Index", EditBtnControler = "Customer", EditBtnAction = "Edit", DeleteBtnControler = "Customer", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
            IntializeDropdowens();

            return View(CustomerModel);
        }
        [HttpPost]
        public IActionResult Edit(CustomerModel CustomerModel)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomeres.SystemName, CurrentUser, PermissionActions.Edit))
                    return AccessDeniedView();

                if (ModelState.IsValid)
                {
                    var SystemSetting = _SystemSettingService.GetAll().FirstOrDefault();
                    //حساب رأس المال من الحسابات الافتراضية
                    var CapitalAccount = _DefaultAccountService.GetWithCondetion(x => x.AccountNameId == 4).FirstOrDefault();

                    if ((SystemSetting != null && SystemSetting.EnableOpenEntryCreation) && (CustomerModel.AccountId <= 0 || CapitalAccount == null || CapitalAccount.AccountId <= 0))
                    {
                        ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("CustomerAccountAndCapitalAccountRequired"), NotificationCssType.danger.ToString());

                        //set basic buttons visibility
                        ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Customer", CreateNewBtnAction = "Create", BackToListControler = "Customer", BackToListAction = "Index", EditBtnControler = "Customer", EditBtnAction = "Edit", DeleteBtnControler = "Customer", DeleteBtnAction = "Delete", RouteId = CustomerModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };
                        IntializeDropdowens();
                        return View(CustomerModel);
                    }

                    Customer Customer = _Mapper.Map<Customer>(CustomerModel);
                    //var currentUser = _identityService.GetCurrentUser();
                    Customer.SetBasicData(SharedEnums.CRUD_OperationType.Update, CurrentUser);
                    _CustomerService.Update(Customer);

                    CustomerOpenBalance customerOpenBalance = _CustomerOpenBalanceService.GetWithCondetion(x => x.CustomerId == Customer.Id && x.FinancialPeriodId.Value == CurrentUser.FinancialPeriodId.Value && x.CompanyId.Value == CurrentUser.CompanyId.Value).FirstOrDefault();
                    if (customerOpenBalance!=null)
                    {
                        customerOpenBalance.OpeningBalanceCredit = CustomerModel.OpeningBalanceCredit;
                        customerOpenBalance.OpeningBalanceDebit = CustomerModel.OpeningBalanceDebit;
                        _CustomerOpenBalanceService.Update(customerOpenBalance);
                    }
                    else
                    {
                        customerOpenBalance = new CustomerOpenBalance();
                        customerOpenBalance.CustomerId = Customer.Id;
                        customerOpenBalance.FinancialPeriodId = CurrentUser.FinancialPeriodId;
                        customerOpenBalance.CompanyId = CurrentUser.CompanyId;
                        customerOpenBalance.AccountId = CustomerModel.AccountId;
                        customerOpenBalance.EntryNumber = CustomerModel.EntryNumber;
                        customerOpenBalance.OpeningBalanceCredit = CustomerModel.OpeningBalanceCredit;
                        customerOpenBalance.OpeningBalanceDebit = CustomerModel.OpeningBalanceDebit;
                        _CustomerOpenBalanceService.Add(customerOpenBalance);
                    }

                    //add open entry for supplier
                    if (SystemSetting != null && SystemSetting.EnableOpenEntryCreation && CapitalAccount != null && CustomerModel.AccountId > 0 && CapitalAccount.AccountId > 0)
                    {
                        AddCustomerOpenEntry(Customer, customerOpenBalance);
                    }


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
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Customer", CreateNewBtnAction = "Create", BackToListControler = "Customer", BackToListAction = "Index", EditBtnControler = "Customer", EditBtnAction = "Edit", DeleteBtnControler = "Customer", DeleteBtnAction = "Delete", RouteId = CustomerModel.Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = false, BackToListBtnVisibilty = true, SaveBtnVisibilty = true, DeleteBtnVisibilty = true };

            IntializeDropdowens();

            return View(CustomerModel);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomeres.SystemName, CurrentUser, PermissionActions.List))
                return AccessDeniedView();
           
            Customer Customer = _CustomerService.GetById(Id);
            CustomerModel CustomerModel = _Mapper.Map<CustomerModel>(Customer);

            //set basic buttons visibility
            ViewData["BasicButtons"] = new BasicButtons() { CreateNewBtnControler = "Customer", CreateNewBtnAction = "Create", BackToListControler = "Customer", BackToListAction = "Index", EditBtnControler = "Customer", EditBtnAction = "Edit", DeleteBtnControler = "Customer", DeleteBtnAction = "Delete", RouteId = Id, CreateNewBtnVisibilty = true, EditBtnVisibilty = true, BackToListBtnVisibilty = true, SaveBtnVisibilty = false, DeleteBtnVisibilty = true };

            return View(CustomerModel);
        }

        public IActionResult Delete(int Id)
        {
            try
            {
                if (!_identityService.Authorize(StandardPermissionProvider.ManageCustomeres.SystemName, CurrentUser, PermissionActions.Delete))
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("Permission.AccessDenied").Value });

                var msg = _webHelper.IsAllowedDeleteCustomer(Id);
                if (msg != "")
                {
                    return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString(msg).Value });
                }

                Customer Customer = _CustomerService.GetById(Id);
                _CustomerService.Delete(Customer);

                var customerEntry = _DailyEntryService.GetById(Customer.EntryId);
                if (customerEntry != null)
                {
                    _DailyEntryService.Delete(customerEntry);
                }

                return Json(new { type = "success", msg = _LocalizationService.GetLocalizedHtmlString("DeletedSuccessfuly").Value });


            }
            catch (Exception)
            {
                return Json(new { type = "faild", msg = _LocalizationService.GetLocalizedHtmlString("shared.delete.erorr").Value });
            }

        }

        public void AddCustomerOpenEntry(Customer customer, CustomerOpenBalance customerOpenBalance)
        {
            decimal Amount = 0;
            if (customer.OpeningBalanceDebit > 0)
            {
                Amount = customer.OpeningBalanceDebit;
            }
            else if (customer.OpeningBalanceCredit > 0)
            {
                Amount = customer.OpeningBalanceCredit;

            }

            //حساب رأس المال من الحسابات الافتراضية
            var CapitalAccount = _DefaultAccountService.GetWithCondetion(x => x.AccountNameId == 4).FirstOrDefault();

            if (Amount > 0 && customer.AccountId > 0 && CapitalAccount != null && CapitalAccount.AccountId > 0)
            {
                var oldEntry = _DailyEntryService.GetWithCondetion(x => x.EntryNumber == customer.EntryNumber && x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId && x.DocType == (int)DocumentTypes.CustomerCreation).FirstOrDefault();

                if (oldEntry == null)
                {
                    var LastEntryNumber = _DailyEntryService.GetLastEntryNumber(o => o.EntryNumber, x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId);

                    var LastDocumentNumber = _DailyEntryService.GetLastCode(o => o.Code, x => x.FinancialPeriodId == CurrentUser.FinancialPeriodId && x.CompanyId == CurrentUser.CompanyId && x.DocType == (int)DocumentTypes.CustomerCreation);

                    var DefaultCuruncy = _CurrencyService.GetWithCondetion(x => x.DefaultCurrency).FirstOrDefault();

                    var EntryMaster = new DailyEntryMaster()
                    {
                        EntryNumber = LastEntryNumber,
                        TransactionDate = DateTime.Now,
                        DocNumber = LastDocumentNumber,
                        Code = LastDocumentNumber,
                        EntryCreationMethod = (int)EntryCreationMethod.Automatic,
                        CurrencyId = DefaultCuruncy.Id,
                        CurrencyFactor = DefaultCuruncy.CurrencyChangrRate,
                        DailyTypeId = 0,
                        EntryState = (int)EntryBalanceState.Balanced,
                        EntryType = (int)EntryType.Open,
                        IsTransfered = (int)EntryTransferState.Transfered,
                        TotalCredit = Amount,
                        TotalDebit = Amount,
                        DailyEntryDetails = new List<DailyEntryDetails>()


                    };

                    EntryMaster.SetBasicData(CRUD_OperationType.Create, (int)DocumentTypes.CustomerCreation, CurrentUser);

                    //الطرف الاول للقيد مدين لحساب العميل
                    DailyEntryDetails DailyEntry1 = new DailyEntryDetails()
                    {
                        MasterId = EntryMaster.Id,
                        AccountId = customer.AccountId,
                        CostCenterId = 0,
                        Debit = Amount,
                        Credit = 0,

                    };
                    DailyEntry1.SetBasicData(CurrentUser, EntryMaster);

                    //الطرف الثاني للقيد داين لحساب رأس المال
                    DailyEntryDetails DailyEntry2 = new DailyEntryDetails()
                    {
                        MasterId = EntryMaster.Id,
                        AccountId = CapitalAccount.AccountId,
                        CostCenterId = 0,
                        Debit = 0,
                        Credit = Amount,

                    };
                    DailyEntry2.SetBasicData(CurrentUser, EntryMaster);

                    EntryMaster.DailyEntryDetails.Add(DailyEntry1);
                    EntryMaster.DailyEntryDetails.Add(DailyEntry2);
                    _DailyEntryService.AddEntry(EntryMaster);

                    customer.EntryNumber = EntryMaster.EntryNumber;
                    customer.EntryNumber = EntryMaster.EntryNumber;
                   _CustomerService.Update(customer);
                   _CustomerOpenBalanceService.Update(customerOpenBalance);
                }
                else
                {
                    var oldEntryDetails = _DailyEntryService.GetEntryDetails(x => x.MasterId == oldEntry.Id);
                    oldEntry.SetBasicData(CRUD_OperationType.Update, (int)DocumentTypes.CustomerCreation, CurrentUser);

                    //الطرف الاول للقيد مدين لحساب العميل
                    DailyEntryDetails DailyEntry1 = new DailyEntryDetails()
                    {
                        MasterId = oldEntry.Id,
                        AccountId = customer.AccountId,
                        CostCenterId = 0,
                        Debit = Amount,
                        Credit = 0,

                    };
                    DailyEntry1.SetBasicData(CurrentUser, oldEntry);

                    //الطرف الثاني للقيد داين لحساب رأس المال
                    DailyEntryDetails DailyEntry2 = new DailyEntryDetails()
                    {
                        MasterId = oldEntry.Id,
                        AccountId = CapitalAccount.AccountId,
                        CostCenterId = 0,
                        Debit = 0,
                        Credit = Amount,

                    };
                    DailyEntry2.SetBasicData(CurrentUser, oldEntry);

                    oldEntry.DailyEntryDetails.Add(DailyEntry1);
                    oldEntry.DailyEntryDetails.Add(DailyEntry2);
                    _DailyEntryService.UpdateEntry(oldEntry, oldEntryDetails);


                }



            }

        }

    }


}
