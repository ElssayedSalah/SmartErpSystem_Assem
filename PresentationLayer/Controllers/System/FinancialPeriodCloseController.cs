using AutoMapper;
using BusinessLayer.Models.Financial;
using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.Purchases;
using DataAccessLayer.Entities.Sales;
using DataAccessLayer.Entities.System;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static BusinessLayer.Helpers.SharedEnums;

namespace PresentationLayer.Controllers.System
{
    public class FinancialPeriodCloseController : BaseAdminController
    {
        private readonly IBaseService<FinancialPeriod> _FinancialPeriodService;
        private readonly IInventoryService<Transaction_InvMaster, Transaction_InvDetails> _InventoryMasterService;
        private readonly IBaseService<Transaction_InvDetails> _InventoryDetailsService;
        private readonly IBaseService<Branch> _BranchService;
        private readonly IBaseService<Store> _StoreService;
        private readonly IBaseService<Item> _ItemService;
        private readonly IBaseService<Document> _DocumentService;
        private readonly LocalizationService _LocalizationService;
        private readonly IMapper _Mapper;
        private readonly IBaseService<SystemSetting> _SystemSettingService;
        private readonly IBaseService<TransactionsEntrySettingMaster> _TransactionsEntrySettingService;
        private readonly IBaseService<DefaultAccount> _DefaultAccountService;
        private readonly IFinanceService<DailyEntryMaster, DailyEntryDetails> _DailyEntryService;
        private readonly IBaseService<Currency> _CurrencyService;
        private readonly IBaseService<Customer> _CustomerService;
        private readonly IBaseService<Suppler> _SupplerService;
        private readonly IBaseService<Bank> _BankService;
        private readonly IBaseService<Treasury> _TreasuryService;
        private readonly IBaseService<CustomerOpenBalance> _CustomerOpenBalanceService;
        private readonly IBaseService<SupplerOpenBalance> _SupplerOpenBalanceService;
        private readonly IBaseService<TreasuryOpenBalance> _TreasuryOpenBalanceService;
        private readonly IBaseService<BankOpenBalance> _BankOpenBalanceService;
        private readonly IBaseService<CashTransaction> _CashTransactionService;
        private readonly IBaseService<CheckTransaction> _CheckTransactionService;
        private readonly IBaseService<DailyEntryMaster> _DailyEntryMasterService;
        private readonly IBaseService<DailyEntryDetails> _DailyEntryDetailsService;
        private readonly IBaseService<Account> _AccountService;
        private readonly IBaseService<AccountOpenBalance> _AccountOpenBalanceService;



        private readonly AppHub _AppHub;
        private readonly IWebHelper _WebHelperService;


        public FinancialPeriodCloseController(
            IBaseService<FinancialPeriod> FinancialPeriodService,
            IMapper mapper, 
            LocalizationService localizationService,
            IBaseService<Branch> BranchService, 
            IBaseService<Store> StoreService, 
            IInventoryService<Transaction_InvMaster, Transaction_InvDetails> InventoryMasterService, 
            IBaseService<Item> ItemService, 
            IBaseService<Transaction_InvDetails> InventoryDetailsService, 
            IBaseService<Document> DocumentService,
            IBaseService<SystemSetting> SystemSettingService,
            IBaseService<TransactionsEntrySettingMaster> TransactionsEntrySettingService,
            IBaseService<DefaultAccount> DefaultAccountService,
            IFinanceService<DailyEntryMaster, DailyEntryDetails> DailyEntryService,
            IBaseService<Currency> CurrencyService,
            AppHub AppHub,
            IWebHelper WebHelperService,
            IBaseService<Customer> CustomerService, 
            IBaseService<CustomerOpenBalance> CustomerOpenBalanceService,
            IBaseService<CashTransaction> CashTransactionService,
            IBaseService<CheckTransaction> CheckTransactionService,
            IBaseService<Suppler> SupplerService, 
            IBaseService<SupplerOpenBalance> SupplerOpenBalanceService, 
            IBaseService<Treasury> TreasuryService, 
            IBaseService<TreasuryOpenBalance> TreasuryOpenBalanceService,
            IBaseService<Bank> BankService, 
            IBaseService<BankOpenBalance> BankOpenBalanceService,
            IBaseService<DailyEntryMaster> DailyEntryMasterService,
            IBaseService<DailyEntryDetails> DailyEntryDetailsService,
            IBaseService<Account> AccountService,
            IBaseService<AccountOpenBalance> AccountOpenBalanceService



            )
        {
            _FinancialPeriodService = FinancialPeriodService;
            _LocalizationService = localizationService;
            _Mapper = mapper;
            _BranchService = BranchService;
            _StoreService = StoreService;
            _ItemService = ItemService;
            _DocumentService = DocumentService;
            _InventoryMasterService = InventoryMasterService;
            _InventoryDetailsService = InventoryDetailsService;
            _SystemSettingService = SystemSettingService;
            _TransactionsEntrySettingService = TransactionsEntrySettingService;
            _DefaultAccountService = DefaultAccountService;
            _DailyEntryService = DailyEntryService;
            _CurrencyService = CurrencyService;
            _CustomerService = CustomerService;
            _SupplerService = SupplerService;
            _TreasuryService = TreasuryService;
            _BankService = BankService;
            _TreasuryOpenBalanceService = TreasuryOpenBalanceService;
            _SupplerOpenBalanceService = SupplerOpenBalanceService;
            _CustomerOpenBalanceService = CustomerOpenBalanceService;
            _BankOpenBalanceService = BankOpenBalanceService;
            _CashTransactionService = CashTransactionService;
            _CheckTransactionService = CheckTransactionService;
            _AppHub = AppHub;
            _WebHelperService = WebHelperService;
            _DailyEntryMasterService = DailyEntryMasterService;
            _DailyEntryDetailsService = DailyEntryDetailsService;
            _AccountService = AccountService;
            _AccountOpenBalanceService = AccountOpenBalanceService;

        }
        public IActionResult Index(string msgType ,string msg)
        {
            var FinancialPeriods = _FinancialPeriodService.GetAll();
            var FinancialPeriodsModel = _Mapper.Map<List<FinancialPeriodModel>>(FinancialPeriods);

            if (msg!="")
            {
                if (msgType=="success")
                {
                    ViewData["NotificationMsg"] = Notification.Success(msg, NotificationCssType.success.ToString());
                }
                else if (msgType == "erorr")
                {
                    ViewData["NotificationMsg"] = Notification.Erorr(msg, NotificationCssType.danger.ToString());

                }
           
            }
            
            return View(FinancialPeriodsModel);

        }
       
        [HttpGet]
        public IActionResult TempClose(int id)
        {  
            var FinancialPeriodCloseModel = new FinancialPeriodCloseModel();
            var curuntPeriod = _FinancialPeriodService.GetById(id);
            var nextPeriod = _FinancialPeriodService.GetWithCondetion(x=>x.Year==curuntPeriod.Year+1).FirstOrDefault();
            if (curuntPeriod != null)
            {
                FinancialPeriodCloseModel.CuruntPeriodId = curuntPeriod.Id;
                FinancialPeriodCloseModel.CuruntYear = curuntPeriod.Year;
                FinancialPeriodCloseModel.CuruntDateFrom = curuntPeriod.DateFrom;
                FinancialPeriodCloseModel.CuruntDateTo = curuntPeriod.DateTo;
                FinancialPeriodCloseModel.NextYear = curuntPeriod.Year + 1;
                FinancialPeriodCloseModel.NextDateFrom = new DateTime(curuntPeriod.Year+1, curuntPeriod.DateFrom.Month, curuntPeriod.DateFrom.Day);
                FinancialPeriodCloseModel.NextDateTo = new DateTime(curuntPeriod.Year + 1, curuntPeriod.DateTo.Month, curuntPeriod.DateTo.Day);
                if (nextPeriod!=null)
                {
                    FinancialPeriodCloseModel.AllowClose = true;
                }
                if (curuntPeriod.isClosed)
                {
                    ViewData["NotificationMsg"] = Notification.Erorr(_LocalizationService.GetLocalizedHtmlString("CuruntPeriodIsClosedFinal"), NotificationCssType.danger.ToString());

                }
            }
            else
            {
                ViewData["NotificationMsg"] = Notification.Erorr("FinancialPeriodNotFound", NotificationCssType.danger.ToString());
            }



            return View(FinancialPeriodCloseModel);
        }

       
            
        public  IActionResult TransferItems(int CuruntPeriodId)
        {
            string msgType = "";
            string msg = "";

            try
            {
                var curuntPeriod = _FinancialPeriodService.GetById(CuruntPeriodId);
                if (!curuntPeriod.isClosed)
                {
                    TransactionEntrySetting transactionSettings = new TransactionEntrySetting();
                    //التأكد من الحسابات الافتراضية ومواصفات حركة الرصيد الافتتاحي وإذا كان النظام يسمح بانشاء قيود الحركات
                    msg = ValidateEntrySettings(transactionSettings);
                    if (msg.Length > 0)
                    {
                        return Json(new { msgType = "erorr", msg });
                    }


                    var nextPeriod = _FinancialPeriodService.GetWithCondetion(x => x.Year == curuntPeriod.Year + 1).FirstOrDefault();
                    if (nextPeriod != null)
                    {
                        var Currency = _CurrencyService.GetWithCondetion(x => x.DefaultCurrency).FirstOrDefault();
                        var items = _ItemService.GetWithCondetion(x => x.FinancialPeriodId != null && x.ActivationState.Value && x.FinancialPeriodId <= CurrentUser.FinancialPeriodId);
                        var branches = _BranchService.GetAll();
                        var stores = _StoreService.GetAll();
                        var documents = _DocumentService.GetAll();
                        var branchesStores = (from b in branches
                                              join s in stores on b.Id equals s.BranchId
                                              select new { branchId = b.Id, storeId = s.Id }
                                                  ).ToList();

                        var curuntYearTransMaster = _InventoryMasterService.GetWithCondetion(x => x.FinancialPeriodId == curuntPeriod.Id && x.CompanyId == CurrentUser.CompanyId).ToList();

                        var curuntYearTransDetails = _InventoryDetailsService.GetWithCondetion(x => x.FinancialPeriodId == curuntPeriod.Id && x.CompanyId == CurrentUser.CompanyId).ToList();

                        var curuntYearTrans = (from m in curuntYearTransMaster
                                               join d in curuntYearTransDetails on m.Id equals d.MasterId
                                               join doc in documents on m.DocTypeId equals doc.DocTypeId
                                               where doc.DocSign != 0
                                               select new { m.BranchId, m.StoreId, m.DocTypeId, m.DocDate, doc.DocSign, d.ItemId, d.Quntity }
                                                   ).ToList();

                        var nextYearOpenTransMaster = _InventoryMasterService.GetWithCondetion(x =>x.TransactionType==(int)TransactionTypes.PostedTransaction && x.DocTypeId == (int)DocumentTypes.OpenBalance && x.FinancialPeriodId == nextPeriod.Id && x.CompanyId == CurrentUser.CompanyId).ToList();
                        if (nextYearOpenTransMaster!=null)
                        {
                            foreach (var t in nextYearOpenTransMaster)
                            {
                                var tEntry = _DailyEntryService.GetById(t.EntryId);
                                if (tEntry != null)
                                {
                                    _DailyEntryService.Delete(tEntry);
                                }

                                _InventoryMasterService.Delete(t);
                            }
                       
                        }                      

                        decimal brancheStoreCoast = 0;
                        decimal itemAveragePurchasPrice = 0;

                        int Prersentage = 100/branchesStores.Count;
                        int Step = Prersentage;
                        string branchName = "";
                        string storeName = "";
                        foreach (var branch in branchesStores)
                        {
                            brancheStoreCoast = 0;
                            var LastCode = _InventoryMasterService.GetLastCode(o => o.Code, x => x.FinancialPeriodId == nextPeriod.Id && x.DocTypeId == (int)DocumentTypes.OpenBalance && x.CompanyId == CurrentUser.CompanyId);

                            Transaction_InvMaster openBalanceMaster = new Transaction_InvMaster();
                            openBalanceMaster.BranchId = branch.branchId;
                            openBalanceMaster.StoreId = branch.storeId;
                            openBalanceMaster.DocTypeId = (int)DocumentTypes.OpenBalance;
                            openBalanceMaster.DocDate = nextPeriod.DateFrom;
                            openBalanceMaster.FinancialPeriodId = nextPeriod.Id;
                            openBalanceMaster.CompanyId = CurrentUser.CompanyId;
                            openBalanceMaster.Code = LastCode;
                            openBalanceMaster.CreationDate = DateTime.Now;
                            openBalanceMaster.CreationUserId = CurrentUser.UserName;
                            openBalanceMaster.CurrencyId = Currency != null ? Currency.Id : 0;
                            openBalanceMaster.CurrencyFactor = Currency != null ? Currency.CurrencyChangrRate : 1;
                            openBalanceMaster.TransactionType = (int)TransactionTypes.PostedTransaction;
                            openBalanceMaster.Notes = "تم إنشاء الحركة أليا اثناء إغلاق السنة المالية السابقة";

                            foreach (var item in items)
                            {
                                itemAveragePurchasPrice = 0;
                                var itemOpenQuntity = curuntYearTrans.Where(x => x.ItemId == item.Id && x.BranchId == branch.branchId && x.StoreId == branch.storeId).Sum(s => s.Quntity * s.DocSign);

                                if (itemOpenQuntity != 0)
                                {
                                    itemAveragePurchasPrice = _WebHelperService.GetItemAveragePurchasPrice(curuntPeriod.Id, item.Id, branch.branchId, branch.storeId, curuntPeriod.DateTo);

                                    openBalanceMaster.Transaction_InvDetails.Add(new Transaction_InvDetails()
                                    {
                                        MasterId = openBalanceMaster.Id,
                                        ItemId = item.Id,
                                        GroupId = item.GroupId,
                                        UnitId = item.DefaultUnit,
                                        Quntity = itemOpenQuntity,
                                        PurchasePrice = itemAveragePurchasPrice,
                                        Total = itemAveragePurchasPrice * itemOpenQuntity,
                                        DocDate = openBalanceMaster.DocDate,
                                        FinancialPeriodId = openBalanceMaster.FinancialPeriodId,
                                        CompanyId = openBalanceMaster.CompanyId,
                                        DocTypeId = openBalanceMaster.DocTypeId,

                                    });
                                    brancheStoreCoast += itemAveragePurchasPrice * itemOpenQuntity;

                                }

                            }
                            if (openBalanceMaster.Transaction_InvDetails.Count > 0)
                            {
                                if (transactionSettings.EnableEntryCreation && brancheStoreCoast > 0)
                                {
                                    branchName = branches.Where(x=>x.Id== branch.branchId).FirstOrDefault().NameAr;
                                    storeName = stores.Where(x=>x.Id== branch.storeId).FirstOrDefault().NameAr;
                                    openBalanceMaster.CurrencyId = Currency != null ? Currency.Id : 0;
                                    openBalanceMaster.CurrencyFactor = Currency != null ? Currency.CurrencyChangrRate : 1;
                                    int EntryNumber = 0;
                                    int EntryId = 0;
                                    //add entry here
                                    AddEntry(transactionSettings, openBalanceMaster.Code, openBalanceMaster.CurrencyId, openBalanceMaster.CurrencyFactor, brancheStoreCoast, openBalanceMaster.TaxValue, nextPeriod, branchName, storeName, out EntryNumber, out EntryId);

                                    openBalanceMaster.EntryNumber = EntryNumber;
                                    openBalanceMaster.EntryId = EntryId;

                                }

                                _InventoryMasterService.AddInventoryTransaction(openBalanceMaster);

                            }

                            Step = Step > 100 ? 100 : Step + Prersentage;
                             _AppHub.UpdateProgressBar(Step.ToString());
                            Step += Step;

                        }
                        msgType = "success";
                        msg = _LocalizationService.GetLocalizedHtmlString("ItemsTransferedSuccesflly");
                    }
                    else
                    {
                        msgType = "erorr";
                        msg = _LocalizationService.GetLocalizedHtmlString("FinancialPeriodNotFound");
                    }

                }
                else
                {
                    msgType = "erorr";
                    msg = _LocalizationService.GetLocalizedHtmlString("CuruntPeriodIsClosedFinal");
                }

                  

            }
            catch (Exception ex)
            {
                msgType = "erorr";
                msg = ex.Message;
            }


            return Json(new { msgType, msg });
        }

        public IActionResult TransferCustomers(int CuruntPeriodId)
        {
            string msgType = "";
            string msg = "";

            try
            {
                var curuntPeriod = _FinancialPeriodService.GetById(CuruntPeriodId);
                if (!curuntPeriod.isClosed)
                {                    
                    var nextPeriod = _FinancialPeriodService.GetWithCondetion(x => x.Year == curuntPeriod.Year + 1).FirstOrDefault();
                    if (nextPeriod != null)
                    {
                        var customers = _CustomerService.GetAll();
                        var customersCuruntOpenBalance = _CustomerOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == curuntPeriod.Id && x.CompanyId == CurrentUser.CompanyId);

                        var customersNextOpenBalance = _CustomerOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == nextPeriod.Id && x.CompanyId == CurrentUser.CompanyId);

                        decimal openBalanceDebit = 0;
                        decimal openBalanceCredit = 0;
                        decimal balanceDebit = 0;
                        decimal balanceCredit = 0;
                        int Prersentage = 100 / customers.Count;
                        int Step = Prersentage;
                        foreach (var customer in customers)
                        {
                           openBalanceDebit = 0;
                           openBalanceCredit = 0;
                           balanceDebit = 0;
                           balanceCredit = 0;
                            var customerOpenBalance = customersCuruntOpenBalance.Where(x=>x.CustomerId==customer.Id).FirstOrDefault();
                            if (customerOpenBalance!=null)
                            {
                                openBalanceDebit = customerOpenBalance.OpeningBalanceDebit;
                                openBalanceCredit = customerOpenBalance.OpeningBalanceCredit;

                            }
                            var curuntBalance = GetCustomerAmount(customer.Id, curuntPeriod.Id);
                            if (curuntBalance!=null)
                            {
                                balanceDebit = curuntBalance.Item1;
                                balanceCredit = curuntBalance.Item2;
                            }
                            balanceDebit += openBalanceDebit;
                            balanceCredit += openBalanceCredit;

                            if (balanceDebit - balanceCredit >= 0)
                            {
                                balanceDebit = balanceDebit - balanceCredit;
                                balanceCredit = 0;
                            }
                            else
                            {
                                balanceCredit = balanceCredit - balanceDebit;
                                balanceDebit = 0;
                            }
                            var customerNextOpenBalance = customersNextOpenBalance.Where(x => x.CustomerId == customer.Id).FirstOrDefault();
                            if (customerNextOpenBalance!=null)
                            {
                                customerNextOpenBalance.OpeningBalanceDebit = balanceDebit;
                                customerNextOpenBalance.OpeningBalanceCredit = balanceCredit;
                                _CustomerOpenBalanceService.Update(customerNextOpenBalance);
                            }
                            else
                            {
                                CustomerOpenBalance openBalance = new CustomerOpenBalance();
                                openBalance.CustomerId = customer.Id;
                                openBalance.FinancialPeriodId = nextPeriod.Id;
                                openBalance.CompanyId = CurrentUser.CompanyId;
                                openBalance.OpeningBalanceDebit = balanceDebit;
                                openBalance.OpeningBalanceCredit = balanceCredit;
                                openBalance.AccountId = customer.AccountId;
                                openBalance.EntryNumber = 0;
                                _CustomerOpenBalanceService.Add(openBalance);
                            }
                           
                            _AppHub.UpdateProgressBar(Step.ToString());
                            Step = Step > 100 ? 100 : Step + Prersentage;
                        }


                        msgType = "success";
                        msg = _LocalizationService.GetLocalizedHtmlString("CustomersTransferedSuccesflly");
                    }
                    else
                    {
                        msgType = "erorr";
                        msg = _LocalizationService.GetLocalizedHtmlString("FinancialPeriodNotFound");
                    }

                }
                else
                {
                    msgType = "erorr";
                    msg = _LocalizationService.GetLocalizedHtmlString("CuruntPeriodIsClosedFinal");
                }



            }
            catch (Exception ex)
            {
                msgType = "erorr";
                msg = ex.Message;
            }


            return Json(new { msgType, msg });
        }

        public IActionResult TransferSupplers(int CuruntPeriodId)
        {
            string msgType = "";
            string msg = "";

            try
            {
                var curuntPeriod = _FinancialPeriodService.GetById(CuruntPeriodId);
                if (!curuntPeriod.isClosed)
                {
                    var nextPeriod = _FinancialPeriodService.GetWithCondetion(x => x.Year == curuntPeriod.Year + 1).FirstOrDefault();
                    if (nextPeriod != null)
                    {
                        var supplers = _SupplerService.GetAll();
                        var supplersCuruntOpenBalance =_SupplerOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == curuntPeriod.Id && x.CompanyId == CurrentUser.CompanyId);

                        var supplersNextOpenBalance =_SupplerOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == nextPeriod.Id && x.CompanyId == CurrentUser.CompanyId);

                        decimal openBalanceDebit = 0;
                        decimal openBalanceCredit = 0;
                        decimal balanceDebit = 0;
                        decimal balanceCredit = 0;
                        int Prersentage = 100 / supplers.Count;
                        int Step = Prersentage;
                        foreach (var suppler in supplers)
                        {
                            openBalanceDebit = 0;
                            openBalanceCredit = 0;
                            balanceDebit = 0;
                            balanceCredit = 0;
                            var supplerOpenBalance = supplersCuruntOpenBalance.Where(x => x.SupplerId == suppler.Id).FirstOrDefault();
                            if (supplerOpenBalance != null)
                            {
                                openBalanceDebit = supplerOpenBalance.OpeningBalanceDebit;
                                openBalanceCredit = supplerOpenBalance.OpeningBalanceCredit;

                            }
                            var curuntBalance = GetSupplerAmount(suppler.Id, curuntPeriod.Id);
                            if (curuntBalance != null)
                            {
                                balanceDebit = curuntBalance.Item1;
                                balanceCredit = curuntBalance.Item2;
                            }
                            balanceDebit += openBalanceDebit;
                            balanceCredit += openBalanceCredit;

                            if (balanceDebit - balanceCredit >= 0)
                            {
                                balanceDebit = balanceDebit - balanceCredit;
                                balanceCredit = 0;
                            }
                            else
                            {
                                balanceCredit = balanceCredit - balanceDebit;
                                balanceDebit = 0;
                            }
                            var supplerNextOpenBalance = supplersNextOpenBalance.Where(x => x.SupplerId == suppler.Id).FirstOrDefault();
                            if (supplerNextOpenBalance != null)
                            {
                                supplerNextOpenBalance.OpeningBalanceDebit = balanceDebit;
                                supplerNextOpenBalance.OpeningBalanceCredit = balanceCredit;
                                _SupplerOpenBalanceService.Update(supplerNextOpenBalance);
                            }
                            else
                            {
                                SupplerOpenBalance openBalance = new SupplerOpenBalance();
                                openBalance.SupplerId = suppler.Id;
                                openBalance.FinancialPeriodId = nextPeriod.Id;
                                openBalance.CompanyId = CurrentUser.CompanyId;
                                openBalance.OpeningBalanceDebit = balanceDebit;
                                openBalance.OpeningBalanceCredit = balanceCredit;
                                openBalance.AccountId = suppler.AccountId;
                                openBalance.EntryNumber = 0;
                                _SupplerOpenBalanceService.Add(openBalance);
                            }

                            _AppHub.UpdateProgressBar(Step.ToString());
                            Step = Step > 100 ? 100 : Step + Prersentage;
                        }


                        msgType = "success";
                        msg = _LocalizationService.GetLocalizedHtmlString("SupplersTransferedSuccesflly");
                    }
                    else
                    {
                        msgType = "erorr";
                        msg = _LocalizationService.GetLocalizedHtmlString("FinancialPeriodNotFound");
                    }

                }
                else
                {
                    msgType = "erorr";
                    msg = _LocalizationService.GetLocalizedHtmlString("CuruntPeriodIsClosedFinal");
                }



            }
            catch (Exception ex)
            {
                msgType = "erorr";
                msg = ex.Message;
            }


            return Json(new { msgType, msg });
        }

        public IActionResult TransferTreasurys(int CuruntPeriodId)
        {
            string msgType = "";
            string msg = "";

            try
            {
                var curuntPeriod = _FinancialPeriodService.GetById(CuruntPeriodId);
                if (!curuntPeriod.isClosed)
                {
                    var nextPeriod = _FinancialPeriodService.GetWithCondetion(x => x.Year == curuntPeriod.Year + 1).FirstOrDefault();
                    if (nextPeriod != null)
                    {
                        var treasurys = _TreasuryService.GetAll();
                        var treasurysCuruntOpenBalance = _TreasuryOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == curuntPeriod.Id && x.CompanyId == CurrentUser.CompanyId);

                        var treasurysNextOpenBalance = _TreasuryOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == nextPeriod.Id && x.CompanyId == CurrentUser.CompanyId);

                        decimal openBalanceDebit = 0;
                        decimal openBalanceCredit = 0;
                        decimal balanceDebit = 0;
                        decimal balanceCredit = 0;
                        int Prersentage = 100 / treasurys.Count;
                        int Step = Prersentage;
                        foreach (var treasury in treasurys)
                        {
                            openBalanceDebit = 0;
                            openBalanceCredit = 0;
                            balanceDebit = 0;
                            balanceCredit = 0;
                            var treasuryOpenBalance = treasurysCuruntOpenBalance.Where(x => x.TreasuryId == treasury.Id).FirstOrDefault();
                            if (treasuryOpenBalance != null)
                            {
                                openBalanceDebit = treasuryOpenBalance.OpenBalanceDebit;
                                openBalanceCredit = treasuryOpenBalance.OpenBalanceCredit;

                            }
                            var curuntBalance = GetTreasuryAmount(treasury.Id, curuntPeriod.Id);
                            if (curuntBalance != null)
                            {
                                balanceDebit = curuntBalance.Item1;
                                balanceCredit = curuntBalance.Item2;
                            }
                            balanceDebit += openBalanceDebit;
                            balanceCredit += openBalanceCredit;

                            if (balanceDebit - balanceCredit >= 0)
                            {
                                balanceDebit = balanceDebit - balanceCredit;
                                balanceCredit = 0;
                            }
                            else
                            {
                                balanceCredit = balanceCredit - balanceDebit;
                                balanceDebit = 0;
                            }
                            var treasuryNextOpenBalance = treasurysNextOpenBalance.Where(x => x.TreasuryId == treasury.Id).FirstOrDefault();
                            if (treasuryNextOpenBalance != null)
                            {
                                treasuryNextOpenBalance.OpenBalanceDebit = balanceDebit;
                                treasuryNextOpenBalance.OpenBalanceCredit = balanceCredit;
                                _TreasuryOpenBalanceService.Update(treasuryNextOpenBalance);
                            }
                            else
                            {
                                TreasuryOpenBalance openBalance = new TreasuryOpenBalance();
                                openBalance.TreasuryId = treasury.Id;
                                openBalance.FinancialPeriodId = nextPeriod.Id;
                                openBalance.CompanyId = CurrentUser.CompanyId;
                                openBalance.OpenBalanceDebit = balanceDebit;
                                openBalance.OpenBalanceCredit = balanceCredit;
                                openBalance.EntryNumber = 0;
                                _TreasuryOpenBalanceService.Add(openBalance);
                            }

                            _AppHub.UpdateProgressBar(Step.ToString());
                            Step = Step > 100 ? 100 : Step + Prersentage;
                        }


                        msgType = "success";
                        msg = _LocalizationService.GetLocalizedHtmlString("SupplersTransferedSuccesflly");
                    }
                    else
                    {
                        msgType = "erorr";
                        msg = _LocalizationService.GetLocalizedHtmlString("FinancialPeriodNotFound");
                    }

                }
                else
                {
                    msgType = "erorr";
                    msg = _LocalizationService.GetLocalizedHtmlString("CuruntPeriodIsClosedFinal");
                }



            }
            catch (Exception ex)
            {
                msgType = "erorr";
                msg = ex.Message;
            }


            return Json(new { msgType, msg });
        }

        public IActionResult TransferBanks(int CuruntPeriodId)
        {
            string msgType = "";
            string msg = "";

            try
            {
                var curuntPeriod = _FinancialPeriodService.GetById(CuruntPeriodId);
                if (!curuntPeriod.isClosed)
                {
                    var nextPeriod = _FinancialPeriodService.GetWithCondetion(x => x.Year == curuntPeriod.Year + 1).FirstOrDefault();
                    if (nextPeriod != null)
                    {
                        var banks =_BankService.GetAll();
                        var banksCuruntOpenBalance = _BankOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == curuntPeriod.Id && x.CompanyId == CurrentUser.CompanyId);

                        var banksNextOpenBalance = _BankOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == nextPeriod.Id && x.CompanyId == CurrentUser.CompanyId);

                        decimal openBalanceDebit = 0;
                        decimal openBalanceCredit = 0;
                        decimal balanceDebit = 0;
                        decimal balanceCredit = 0;
                        int Prersentage = 100 / banks.Count;
                        int Step = Prersentage;
                        foreach (var bank in banks)
                        {
                            openBalanceDebit = 0;
                            openBalanceCredit = 0;
                            balanceDebit = 0;
                            balanceCredit = 0;
                            var bankOpenBalance = banksCuruntOpenBalance.Where(x => x.BankId == bank.Id).FirstOrDefault();
                            if (bankOpenBalance != null)
                            {
                                openBalanceDebit = bankOpenBalance.OpenBalanceDebit;
                                openBalanceCredit = bankOpenBalance.OpenBalanceCredit;

                            }
                            var curuntBalance = GetBankAmount(bank.Id, curuntPeriod.Id);
                            if (curuntBalance != null)
                            {
                                balanceDebit = curuntBalance.Item1;
                                balanceCredit = curuntBalance.Item2;
                            }
                            balanceDebit += openBalanceDebit;
                            balanceCredit += openBalanceCredit;

                            if (balanceDebit - balanceCredit >= 0)
                            {
                                balanceDebit = balanceDebit - balanceCredit;
                                balanceCredit = 0;
                            }
                            else
                            {
                                balanceCredit = balanceCredit - balanceDebit;
                                balanceDebit = 0;
                            }
                            var bankNextOpenBalance = banksNextOpenBalance.Where(x => x.BankId == bank.Id).FirstOrDefault();
                            if (bankNextOpenBalance != null)
                            {
                                bankNextOpenBalance.OpenBalanceDebit = balanceDebit;
                                bankNextOpenBalance.OpenBalanceCredit = balanceCredit;
                                _BankOpenBalanceService.Update(bankNextOpenBalance);
                            }
                            else
                            {
                                BankOpenBalance openBalance = new BankOpenBalance();
                                openBalance.BankId = bank.Id;
                                openBalance.FinancialPeriodId = nextPeriod.Id;
                                openBalance.CompanyId = CurrentUser.CompanyId;
                                openBalance.OpenBalanceDebit = balanceDebit;
                                openBalance.OpenBalanceCredit = balanceCredit;
                                openBalance.EntryNumber = 0;
                                _BankOpenBalanceService.Add(openBalance);
                            }

                            _AppHub.UpdateProgressBar(Step.ToString());
                            Step = Step > 100 ? 100 : Step + Prersentage;
                        }


                        msgType = "success";
                        msg = _LocalizationService.GetLocalizedHtmlString("SupplersTransferedSuccesflly");
                    }
                    else
                    {
                        msgType = "erorr";
                        msg = _LocalizationService.GetLocalizedHtmlString("FinancialPeriodNotFound");
                    }

                }
                else
                {
                    msgType = "erorr";
                    msg = _LocalizationService.GetLocalizedHtmlString("CuruntPeriodIsClosedFinal");
                }



            }
            catch (Exception ex)
            {
                msgType = "erorr";
                msg = ex.Message;
            }


            return Json(new { msgType, msg });
        }

        public IActionResult TransferAccounts(int CuruntPeriodId)
        {
            string msgType = "";
            string msg = "";

            try
            {
                var curuntPeriod = _FinancialPeriodService.GetById(CuruntPeriodId);
                if (!curuntPeriod.isClosed)
                {
                    var nextPeriod = _FinancialPeriodService.GetWithCondetion(x => x.Year == curuntPeriod.Year + 1).FirstOrDefault();
                    if (nextPeriod != null)
                    {
                        var accounts = _AccountService.GetAll();
                        var AccountsToTransfer = accounts.Where(x=>x.LastLevelInTree && x.PostTo!=(int)PostingAccounts.select && x.PostTo != (int)PostingAccounts.IncomeStatement).ToList();
                        var accountsCuruntOpenBalance = _AccountOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == curuntPeriod.Id && x.CompanyId == CurrentUser.CompanyId);

                        var accountsNextOpenBalance = _AccountOpenBalanceService.GetWithCondetion(x => x.FinancialPeriodId == nextPeriod.Id && x.CompanyId == CurrentUser.CompanyId);

                        decimal openBalanceDebit = 0;
                        decimal openBalanceCredit = 0;
                        decimal balanceDebit = 0;
                        decimal balanceCredit = 0;
                        int Prersentage = 100 / AccountsToTransfer.Count;
                        int Step = Prersentage;
                        foreach (var account in AccountsToTransfer)
                        {
                            openBalanceDebit = 0;
                            openBalanceCredit = 0;
                            balanceDebit = 0;
                            balanceCredit = 0;
                            var accountOpenBalance = accountsCuruntOpenBalance.Where(x => x.AccountId == account.Id).FirstOrDefault();
                            if (accountOpenBalance != null)
                            {
                                openBalanceDebit = accountOpenBalance.OpenBalanceDebit;
                                openBalanceCredit = accountOpenBalance.OpenBalanceCredit;

                            }
                            var curuntBalance = GetAccountAmount(account.Id, curuntPeriod.Id);
                            if (curuntBalance != null)
                            {
                                balanceDebit = curuntBalance.Item1;
                                balanceCredit = curuntBalance.Item2;
                            }
                            balanceDebit += openBalanceDebit;
                            balanceCredit += openBalanceCredit;

                            if (balanceDebit - balanceCredit >= 0)
                            {
                                balanceDebit = balanceDebit - balanceCredit;
                                balanceCredit = 0;
                            }
                            else
                            {
                                balanceCredit = balanceCredit - balanceDebit;
                                balanceDebit = 0;
                            }
                            var accountNextOpenBalance = accountsNextOpenBalance.Where(x => x.AccountId == account.Id).FirstOrDefault();
                            if (accountNextOpenBalance != null)
                            {
                                accountNextOpenBalance.OpenBalanceDebit = balanceDebit;
                                accountNextOpenBalance.OpenBalanceCredit = balanceCredit;
                                _AccountOpenBalanceService.Update(accountNextOpenBalance);
                            }
                            else
                            {
                                AccountOpenBalance openBalance = new AccountOpenBalance();
                                openBalance.AccountId = account.Id;
                                openBalance.FinancialPeriodId = nextPeriod.Id;
                                openBalance.CompanyId = CurrentUser.CompanyId.Value;
                                openBalance.OpenBalanceDebit = balanceDebit;
                                openBalance.OpenBalanceCredit = balanceCredit;
                                _AccountOpenBalanceService.Add(openBalance);
                            }

                            _AppHub.UpdateProgressBar(Step.ToString());
                            Step = Step > 100 ? 100 : Step + Prersentage;
                        }
                        if (Step<100)
                        {
                            _AppHub.UpdateProgressBar((100).ToString());
                            
                        }


                        msgType = "success";
                        msg = _LocalizationService.GetLocalizedHtmlString("AccountsTransferedSuccesflly");
                    }
                    else
                    {
                        msgType = "erorr";
                        msg = _LocalizationService.GetLocalizedHtmlString("FinancialPeriodNotFound");
                    }

                }
                else
                {
                    msgType = "erorr";
                    msg = _LocalizationService.GetLocalizedHtmlString("CuruntPeriodIsClosedFinal");
                }



            }
            catch (Exception ex)
            {
                msgType = "erorr";
                msg = ex.Message;
            }


            return Json(new { msgType, msg });
        }
        public IActionResult FinalClose(int id)
        {           
            try
            {
                if (ModelState.IsValid)
                {
                    var curuntPeriod = _FinancialPeriodService.GetById(id);
                    if (curuntPeriod != null)
                    {
                        curuntPeriod.isClosed = true;
                        curuntPeriod.CloseDate = DateTime.Now;
                        _FinancialPeriodService.Update(curuntPeriod);

                    ViewData["NotificationMsg"] = Notification.Success(_LocalizationService.GetLocalizedHtmlString("CuruntFinancialPeriodClosedSuccesflly"), NotificationCssType.success.ToString());                     

                    }
                    else
                    {
                        ViewData["NotificationMsg"] = Notification.Erorr(_LocalizationService.GetLocalizedHtmlString("FinancialPeriodNotFound"), NotificationCssType.danger.ToString());
                   
                    }
                }

            }
            catch (Exception ex)
            {
                
            }

            return RedirectToAction("TempClose", new { id = id });

        }

        public IActionResult Create(int Id)
        {
            var curuntPeriod = _FinancialPeriodService.GetById(Id);
            if (curuntPeriod != null)
            {
                FinancialPeriod NewPeriod = new FinancialPeriod();
                NewPeriod.Year = curuntPeriod.Year + 1;
                NewPeriod.DateFrom = new DateTime(NewPeriod.Year, curuntPeriod.DateFrom.Month, curuntPeriod.DateFrom.Day);
                NewPeriod.DateTo = new DateTime(NewPeriod.Year, curuntPeriod.DateTo.Month, curuntPeriod.DateTo.Day);


                _FinancialPeriodService.Add(NewPeriod);
            }
            return RedirectToAction("TempClose", new { id = Id });
        }

        /// <summary>
        /// get customer total debit and credit amounts over transactions (item1=debitAmout,item2=creditAmount)
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="financialPeriodId"></param>
        /// <returns></returns>
        public Tuple<decimal,decimal> GetCustomerAmount(int customerId,int financialPeriodId)
        {
            Tuple<decimal, decimal> amounts;
            decimal debitAmount = 0;
            decimal creditAmount = 0;
            
            var InvntoryTransactions = _InventoryMasterService.GetWithCondetion(x=>x.FinancialPeriodId==financialPeriodId && x.CustomerId==customerId && (x.DocTypeId==(int)DocumentTypes.SalesInvoice || x.DocTypeId==(int)DocumentTypes.SalesReturn));
            
            var CashTransactions = _CashTransactionService.GetWithCondetion(x=>x.FinancialPeriodId==financialPeriodId && (x.FirstSideTypeId==(int)EntrySides.Customer && x.FirstSideId == customerId ) || (x.SecondSideTypeId == (int)EntrySides.Customer && x.SecondSideId== customerId) && (x.DocTypeId == (int)DocumentTypes.CashRecieveTransaction));

            var CheckTransactions = _CheckTransactionService.GetWithCondetion(x=>x.FinancialPeriodId==financialPeriodId && (x.FirstSideTypeId == (int)EntrySides.Customer && x.FirstSideId == customerId) || (x.SecondSideTypeId == (int)EntrySides.Customer && x.SecondSideId == customerId) && (x.DocTypeId == (int)DocumentTypes.RecieveCheckInTransaction));
            //get debitAmount from transactions
            debitAmount = InvntoryTransactions.Where(x=>x.DocTypeId== (int)DocumentTypes.SalesInvoice).Sum(s=>s.InvoiceNet * s.CurrencyFactor);
            //debitAmount += CashTransactions.Where(x=>x.DocTypeId== (int)DocumentTypes.CashRecieveTransaction).Sum(s=>s.Amount * s.CurrencyFactor);

            //get creditAmount from transactions
            creditAmount = InvntoryTransactions.Where(x => x.DocTypeId == (int)DocumentTypes.SalesReturn).Sum(s => s.InvoiceNet * s.CurrencyFactor);
            creditAmount += CashTransactions.Where(x => x.DocTypeId == (int)DocumentTypes.CashRecieveTransaction).Sum(s => s.Amount * s.CurrencyFactor);
            creditAmount += CheckTransactions.Where(x => x.DocTypeId == (int)DocumentTypes.RecieveCheckInTransaction).Sum(s => s.Amount * s.CurrencyFactor);

            amounts = new Tuple<decimal, decimal>(debitAmount, creditAmount);
            return amounts;


        }

        /// <summary>
        /// get supplier total debit and credit amounts over transactions (item1=debitAmout,item2=creditAmount)
        /// </summary>
        /// <param name="SupplierId"></param>
        /// <param name="financialPeriodId"></param>
        /// <returns></returns>
        public Tuple<decimal, decimal> GetSupplerAmount(int supplerId, int financialPeriodId)
        {
            Tuple<decimal, decimal> amounts;
            decimal debitAmount = 0;
            decimal creditAmount = 0;

            var InvntoryTransactions = _InventoryMasterService.GetWithCondetion(x => x.FinancialPeriodId == financialPeriodId && x.SupplierId == supplerId && (x.DocTypeId == (int)DocumentTypes.PurchaseInvoice || x.DocTypeId == (int)DocumentTypes.PurchaseReturn));

            var CashTransactions = _CashTransactionService.GetWithCondetion(x => x.FinancialPeriodId == financialPeriodId && (x.FirstSideTypeId == (int)EntrySides.Supplier && x.FirstSideId == supplerId ) || (x.SecondSideTypeId == (int)EntrySides.Supplier && x.SecondSideId == supplerId) && (x.DocTypeId == (int)DocumentTypes.CashExchangeTransaction));

            var CheckTransactions = _CheckTransactionService.GetWithCondetion(x => x.FinancialPeriodId == financialPeriodId && (x.FirstSideTypeId == (int)EntrySides.Supplier && x.FirstSideId == supplerId) || (x.SecondSideTypeId == (int)EntrySides.Supplier && x.SecondSideId == supplerId) && (x.DocTypeId == (int)DocumentTypes.WriteCheckOutTransaction|| x.DocTypeId == (int)DocumentTypes.CheckReturnTransaction));
            //get debitAmount from transactions
            debitAmount = InvntoryTransactions.Where(x => x.DocTypeId == (int)DocumentTypes.PurchaseReturn).Sum(s => s.InvoiceNet * s.CurrencyFactor);
            debitAmount += CashTransactions.Where(x=>x.DocTypeId== (int)DocumentTypes.CashExchangeTransaction).Sum(s=>s.Amount * s.CurrencyFactor);
            debitAmount += CheckTransactions.Where(x => x.DocTypeId == (int)DocumentTypes.WriteCheckOutTransaction).Sum(s => s.Amount * s.CurrencyFactor);

            //get creditAmount from transactions
            creditAmount = InvntoryTransactions.Where(x => x.DocTypeId == (int)DocumentTypes.PurchaseInvoice).Sum(s => s.InvoiceNet * s.CurrencyFactor);
            creditAmount += CashTransactions.Where(x => x.DocTypeId == (int)DocumentTypes.CashRecieveTransaction).Sum(s => s.Amount * s.CurrencyFactor);
            creditAmount += CheckTransactions.Where(x => x.DocTypeId == (int)DocumentTypes.CheckReturnTransaction).Sum(s => s.Amount * s.CurrencyFactor);

            amounts = new Tuple<decimal, decimal>(debitAmount, creditAmount);
            return amounts;


        }

        /// <summary>
        /// get treasury total debit and credit amounts over transactions (item1=debitAmout,item2=creditAmount)
        /// </summary>
        /// <param name="treasuryId"></param>
        /// <param name="financialPeriodId"></param>
        /// <returns></returns>
        public Tuple<decimal, decimal> GetTreasuryAmount(int treasuryId, int financialPeriodId)
        {
            Tuple<decimal, decimal> amounts;
            decimal debitAmount = 0;
            decimal creditAmount = 0;            

            var CashTransactions = _CashTransactionService.GetWithCondetion(x => x.FinancialPeriodId == financialPeriodId && (x.FirstSideTypeId == (int)EntrySides.Treasury && x.FirstSideId == treasuryId ) || (x.SecondSideTypeId == (int)EntrySides.Treasury && x.SecondSideId == treasuryId) && (x.DocTypeId == (int)DocumentTypes.CashExchangeTransaction || x.DocTypeId == (int)DocumentTypes.CashRecieveTransaction ||x.DocTypeId == (int)DocumentTypes.CashDepositInBankTransaction || x.DocTypeId == (int)DocumentTypes.CashWithdrawalFromBankTransaction));
 
            //get debitAmount from transactions
     
            debitAmount += CashTransactions.Where(x => x.DocTypeId == (int)DocumentTypes.CashRecieveTransaction || x.DocTypeId == (int)DocumentTypes.CashWithdrawalFromBankTransaction).Sum(s => s.Amount * s.CurrencyFactor);
 

            //get creditAmount from transactions  
            creditAmount += CashTransactions.Where(x => x.DocTypeId == (int)DocumentTypes.CashExchangeTransaction || x.DocTypeId == (int)DocumentTypes.CashDepositInBankTransaction).Sum(s => s.Amount * s.CurrencyFactor);


            amounts = new Tuple<decimal, decimal>(debitAmount, creditAmount);
            return amounts;


        }

        /// <summary>
        /// get bank total debit and credit amounts over transactions (item1=debitAmout,item2=creditAmount)
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="financialPeriodId"></param>
        /// <returns></returns>
        public Tuple<decimal, decimal> GetBankAmount(int bankId, int financialPeriodId)
        {
            Tuple<decimal, decimal> amounts;
            decimal debitAmount = 0;
            decimal creditAmount = 0;

            var CashTransactions = _CashTransactionService.GetWithCondetion(x => x.FinancialPeriodId == financialPeriodId && (x.FirstSideTypeId == (int)EntrySides.Bank && x.FirstSideId == bankId) || (x.SecondSideTypeId == (int)EntrySides.Bank && x.SecondSideId == bankId) && ( x.DocTypeId == (int)DocumentTypes.CashDepositInBankTransaction || x.DocTypeId == (int)DocumentTypes.CashWithdrawalFromBankTransaction));

            //var CheckTransactions = _CheckTransactionService.GetWithCondetion(x => x.FinancialPeriodId == financialPeriodId && (x.FirstSideTypeId == (int)EntrySides.Bank || x.SecondSideTypeId == (int)EntrySides.Bank) && (x.FirstSideId == bankId || x.SecondSideId == bankId) && (x.DocTypeId == (int)DocumentTypes.CashDepositInBankTransaction));


            //get debitAmount from transactions

            debitAmount += CashTransactions.Where(x => x.DocTypeId == (int)DocumentTypes.CashDepositInBankTransaction).Sum(s => s.Amount * s.CurrencyFactor);


            //get creditAmount from transactions  
            creditAmount += CashTransactions.Where(x => x.DocTypeId == (int)DocumentTypes.CashWithdrawalFromBankTransaction).Sum(s => s.Amount * s.CurrencyFactor);


            amounts = new Tuple<decimal, decimal>(debitAmount, creditAmount);
            return amounts;


        }


        /// <summary>
        /// get account total debit and credit amounts over transactions (item1=debitAmout,item2=creditAmount)
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="financialPeriodId"></param>
        /// <returns></returns>
        public Tuple<decimal, decimal> GetAccountAmount(int accountId, int financialPeriodId)
        {
            Tuple<decimal, decimal> amounts;
            decimal debitAmount = 0;
            decimal creditAmount = 0;

            var MasterTransactions = _DailyEntryMasterService.GetWithCondetion(x => x.FinancialPeriodId == financialPeriodId);
            var DetailsTransactions =_DailyEntryDetailsService.GetWithCondetion(x =>x.AccountId== accountId && x.FinancialPeriodId == financialPeriodId);
            var AllTransactions = (from m in MasterTransactions join d in DetailsTransactions on m.Id equals d.MasterId select new {Debit=d.Debit* m.CurrencyFactor, Credit= d.Credit*m.CurrencyFactor }).ToList();
           
            //get debitAmount from transactions
            debitAmount = AllTransactions.Sum(s => s.Debit);

            //get creditAmount from transactions
            creditAmount = AllTransactions.Sum(s => s.Credit);

            amounts = new Tuple<decimal, decimal>(debitAmount, creditAmount);
            return amounts;


        }


        /// <summary>
        /// التأكد من الحسابات الافتراضية ومواصفات حركة الرصيد الافتتاحي إذا كان النظام يسمح بانشاء قيود الحركات
        /// </summary>
        /// <param name="transactionSettings"></param>
        /// <param name="supper"></param>
        /// <returns></returns>
        public string ValidateEntrySettings(TransactionEntrySetting transactionSettings)
        {
            StringBuilder msg = new StringBuilder();
            var SystemSetting = _SystemSettingService.GetAll().FirstOrDefault();
            if (SystemSetting != null && SystemSetting.EnableOpenEntryCreation)
            {
                transactionSettings.EnableEntryCreation = true;
                var transSettings = _TransactionsEntrySettingService.GetWithCondetion(x => x.DocTypeId == (int)DocumentTypes.OpenBalance).FirstOrDefault();

                if (transSettings == null)
                {
                    msg.AppendLine("يجب تحديد مواصفات حركة الرصيد الإفتتاحي <br>");
                }
                else
                {
                    transactionSettings.DailyTypeId = transSettings.DailyTypeId;
                    transactionSettings.TransferState = transSettings.TransferState;
                    transactionSettings.FirstSideNaturalId = transSettings.FirstSideSideNaturalId;
                    transactionSettings.SecondSideNaturalId = transSettings.SecondSideSideNaturalId;
                    transactionSettings.IsFirstSideTaxble = transSettings.IsFirstSideTaxble;
                    transactionSettings.IsSecondSideTaxble = transSettings.IsSecondSideTaxble;

                }

                var defaultAccounts = _DefaultAccountService.GetAll();
                if (defaultAccounts == null || defaultAccounts.Count == 0)
                {
                    msg.AppendLine("يجب انشاء الحسابات الافتراضية <br>");
                }
                else
                {
                    //الطرف الاول حساب مخزون اول المدة
                    var FSAccount = defaultAccounts.Where(a => a.AccountNameId == 13).FirstOrDefault();
                    if (FSAccount == null || FSAccount.AccountId <= 0)
                    {
                        msg.AppendLine("يجب تحديد حساب افتراضي لحساب مخزون أول المدة <br>");
                    }
                    else
                    {
                        transactionSettings.FirstSideAccountId = FSAccount.AccountId;
                    }

                    //الطرف الثاني حساب راس المال
                    var SSAccount = defaultAccounts.Where(a => a.AccountNameId == 4).FirstOrDefault();
                    if (SSAccount == null || SSAccount.AccountId <= 0)
                    {
                        msg.AppendLine("يجب تحديد حساب افتراضي لحساب رأس المال <br>");
                    }
                    else
                    {
                        transactionSettings.SecondSideAccountId = SSAccount.AccountId;
                    }
                }
            }

            return msg.ToString();
        }

        public void AddEntry(TransactionEntrySetting transactionSettings, int DocNumber, int CuruncyId, decimal CuruncyFactor, decimal Amount, decimal VAT_TaxAmount,FinancialPeriod NextFinancialPeriod,string BranchName, string StoreName, out int EntryNumber, out int EntryId)
        {
            EntryNumber = 0;
            EntryId = 0;
            if (transactionSettings != null && transactionSettings.FirstSideAccountId>0 && transactionSettings.SecondSideAccountId>0)
            {
                var LastEntryNumber = _DailyEntryService.GetLastEntryNumber(o => o.EntryNumber, x => x.FinancialPeriodId == NextFinancialPeriod.Id && x.CompanyId == CurrentUser.CompanyId);

                #region Entry Master

                var EntryMaster = new DailyEntryMaster();
                EntryMaster.EntryNumber = LastEntryNumber;
                EntryMaster.TransactionDate = NextFinancialPeriod.DateFrom;
                EntryMaster.DocNumber = DocNumber;
                EntryMaster.Code = DocNumber;
                EntryMaster.EntryCreationMethod = (int)EntryCreationMethod.Automatic;
                EntryMaster.CurrencyId = CuruncyId;
                EntryMaster.CurrencyFactor = CuruncyFactor;
                EntryMaster.DailyTypeId = transactionSettings.DailyTypeId;
                EntryMaster.DocType = (int)DocumentTypes.OpenBalance;
                EntryMaster.EntryState = (int)EntryBalanceState.Balanced;
                EntryMaster.EntryType = (int)EntryType.Open;
                EntryMaster.CreationDate = DateTime.Now;
                EntryMaster.CreationUserId = CurrentUser?.UserName;
                EntryMaster.CompanyId = CurrentUser?.CompanyId;
                EntryMaster.FinancialPeriodId = NextFinancialPeriod.Id;
                EntryMaster.Notes = $"  تم إنشاء القيد أليا أثناء إغلاق السنة المالية السابقة لفرع  '{BranchName}'   مخزن  '{StoreName}'  ";


                if (transactionSettings.TransferState)
                {
                    EntryMaster.IsTransfered = (int)EntryTransferState.Transfered;
                }
                else
                {
                    EntryMaster.IsTransfered = (int)EntryTransferState.NotTransfered;
                }
                EntryMaster.TotalCredit = 0;
                EntryMaster.TotalDebit = 0;

                #endregion

                #region EntryDetails
                //الطرف الاول لحساب  مخزون اول المدة
                DailyEntryDetails DailyEntry1 = new DailyEntryDetails();

                DailyEntry1.MasterId = EntryMaster.Id;
                DailyEntry1.MasterEntryNumber = EntryMaster.EntryNumber;
                DailyEntry1.AccountId = transactionSettings.FirstSideAccountId;
                DailyEntry1.CostCenterId = 0;
                DailyEntry1.CompanyId = CurrentUser.CompanyId.Value;
                DailyEntry1.FinancialPeriodId = NextFinancialPeriod.Id;
                if (transactionSettings.FirstSideNaturalId == (int)AccountNatures.Debit)
                {
                    DailyEntry1.Debit = Amount;
                    DailyEntry1.Credit = 0;
                }
                else
                {
                    DailyEntry1.Debit = 0;
                    DailyEntry1.Credit = Amount;
                }              
               
                EntryMaster.DailyEntryDetails.Add(DailyEntry1);




                //الطرف الثاني لحساب راس المال
                DailyEntryDetails DailyEntry2 = new DailyEntryDetails();

                DailyEntry2.MasterId = EntryMaster.Id;
                DailyEntry2.MasterEntryNumber = EntryMaster.EntryNumber;
                DailyEntry2.AccountId = transactionSettings.SecondSideAccountId;
                DailyEntry2.CostCenterId = 0;
                DailyEntry2.CompanyId = CurrentUser.CompanyId.Value;
                DailyEntry2.FinancialPeriodId = NextFinancialPeriod.Id;
                if (transactionSettings.SecondSideNaturalId == (int)AccountNatures.Debit)
                {
                    DailyEntry2.Debit = Amount;
                    DailyEntry2.Credit = 0;
                }
                else
                {
                    DailyEntry2.Debit = 0;
                    DailyEntry2.Credit = Amount;
                }
                EntryMaster.DailyEntryDetails.Add(DailyEntry2);


                EntryMaster.TotalDebit = EntryMaster.DailyEntryDetails.Sum(x => x.Debit);
                EntryMaster.TotalCredit = EntryMaster.DailyEntryDetails.Sum(x => x.Credit);

                _DailyEntryService.AddEntry(EntryMaster);

                EntryNumber = EntryMaster.EntryNumber;
                EntryId = EntryMaster.Id;
                #endregion



            }


        }

        public void UpdateEntry(TransactionEntrySetting transactionSettings, int DocNumber, int CuruncyId, decimal CuruncyFactor, decimal Amount, decimal VAT_TaxAmount, int EntryId, FinancialPeriod NextFinancialPeriod)
        {

            if (transactionSettings != null && transactionSettings.FirstSideAccountId > 0 && transactionSettings.SecondSideAccountId > 0)
            {
                var oldEntryMaster = _DailyEntryService.GetById(EntryId);
                List<DailyEntryDetails> EntryDetails = new List<DailyEntryDetails>();
                if (oldEntryMaster != null)
                {
                    var oldEntryDetails = _DailyEntryService.GetEntryDetails(x => x.MasterId == EntryId);
                    #region Entry Master

                    oldEntryMaster.CurrencyId = CuruncyId;
                    oldEntryMaster.CurrencyFactor = CuruncyFactor;
                    oldEntryMaster.DailyTypeId = transactionSettings.DailyTypeId;
                    if (transactionSettings.TransferState)
                    {
                        oldEntryMaster.IsTransfered = (int)EntryTransferState.Transfered;
                    }
                    else
                    {
                        oldEntryMaster.IsTransfered = (int)EntryTransferState.NotTransfered;
                    }

                    #endregion

                    #region EntryDetails
                    //الطرف الاول لحساب مخزون اول المدة
                    DailyEntryDetails DailyEntry1 = new DailyEntryDetails();

                    DailyEntry1.MasterId = oldEntryMaster.Id;
                    DailyEntry1.MasterEntryNumber = oldEntryMaster.EntryNumber;
                    DailyEntry1.AccountId = transactionSettings.FirstSideAccountId;
                    DailyEntry1.CostCenterId = 0;
                    DailyEntry1.CompanyId = CurrentUser.CompanyId.Value;
                    DailyEntry1.FinancialPeriodId = NextFinancialPeriod.Id;
                    if (transactionSettings.FirstSideNaturalId == (int)AccountNatures.Debit)
                    {
                        DailyEntry1.Debit = Amount;
                        DailyEntry1.Credit = 0;
                    }
                    else
                    {
                        DailyEntry1.Debit = 0;
                        DailyEntry1.Credit = Amount;
                    }                   
                    EntryDetails.Add(DailyEntry1);


                    //الطرف الثاني لحساب راس المال
                    DailyEntryDetails DailyEntry2 = new DailyEntryDetails();

                    DailyEntry2.MasterId = oldEntryMaster.Id;
                    DailyEntry2.MasterEntryNumber = oldEntryMaster.EntryNumber;
                    DailyEntry2.AccountId = transactionSettings.SecondSideAccountId;
                    DailyEntry2.CostCenterId = 0;
                    DailyEntry2.CompanyId = CurrentUser.CompanyId.Value;
                    DailyEntry2.FinancialPeriodId = NextFinancialPeriod.Id;
                    if (transactionSettings.SecondSideNaturalId == (int)AccountNatures.Debit)
                    {
                        DailyEntry2.Debit = Amount;
                        DailyEntry2.Credit = 0;
                    }
                    else
                    {
                        DailyEntry2.Debit = 0;
                        DailyEntry2.Credit = Amount;
                    }
                    EntryDetails.Add(DailyEntry2);

                    oldEntryMaster.DailyEntryDetails = EntryDetails;

                    oldEntryMaster.TotalDebit = oldEntryMaster.DailyEntryDetails.Sum(x => x.Debit);
                    oldEntryMaster.TotalCredit = oldEntryMaster.DailyEntryDetails.Sum(x => x.Credit);

                    _DailyEntryService.UpdateEntry(oldEntryMaster, oldEntryDetails);

                    #endregion


                }



            }


        }








       




    }
}
