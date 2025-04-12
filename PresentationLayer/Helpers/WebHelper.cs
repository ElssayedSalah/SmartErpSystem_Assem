using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.Purchases;
using DataAccessLayer.Entities.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static BusinessLayer.Helpers.SharedEnums;

namespace PresentationLayer.Helpers
{
    public partial class WebHelper : IWebHelper
    {
        #region Fields  

        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseService<Item> _ItemService;
        private readonly IBaseService<Store> _StoreService;
        private readonly IBaseService<Transaction_InvDetails> _TransDetailsService;
        private readonly IBaseService<Transaction_InvMaster> _TransMasterService;
        private readonly IidentityService _IdentityService;
        private readonly IBaseService<Customer> _CustomerService;
        private readonly IBaseService<CustomerOpenBalance> _CustomerOpenBalanceService;
        private readonly IBaseService<SupplerOpenBalance> _SupplerOpenBalanceService;
        private readonly IBaseService<Suppler> _SupplerService;
        private readonly IBaseService<BankOpenBalance> _BankOpenBalanceService;
        private readonly IBaseService<Bank> _BankService;
        private readonly IBaseService<TreasuryOpenBalance> _TreasuryOpenBalanceService;
        private readonly IBaseService<Treasury> _TreasuryService;
        private readonly IFinanceService<DailyEntryMaster, DailyEntryDetails> _DailyEntryService;
        private readonly IBaseService<DefaultAccount> _DefaultAccountService;
        private readonly IBaseService<FinancialPeriod> _FinancialPeriodService;
        private readonly LocalizationService _LocalizationService;






        #endregion

        #region Ctor

        public WebHelper(
            IHostApplicationLifetime hostApplicationLifetime,
            IHttpContextAccessor httpContextAccessor,
            IBaseService<Item> ItemService,
            IBaseService<Store> StoreService,
            IBaseService<Transaction_InvDetails> TransDetailsService,
            IBaseService<Transaction_InvMaster> TransMasterService,
            IidentityService IdentityService,
            IBaseService<Customer> CustomerService,
            IBaseService<CustomerOpenBalance> CustomerOpenBalanceService,
            IBaseService<SupplerOpenBalance> SupplerOpenBalanceService,
            IBaseService<Suppler> SupplerService,
            IBaseService<BankOpenBalance> BankOpenBalanceService,
            IBaseService<TreasuryOpenBalance> TreasuryOpenBalanceService,
            IFinanceService<DailyEntryMaster, DailyEntryDetails> DailyEntryService,
            IBaseService<Bank> BankService,
            IBaseService<Treasury> TreasuryService,
            IBaseService<DefaultAccount> DefaultAccountService,
            IBaseService<FinancialPeriod> FinancialPeriodService,
            LocalizationService localizationService



          )
        {
           
            _hostApplicationLifetime = hostApplicationLifetime;
            _httpContextAccessor = httpContextAccessor;
            _ItemService = ItemService;
            _StoreService = StoreService;
            _TransDetailsService = TransDetailsService;
            _TransMasterService = TransMasterService;
            _IdentityService = IdentityService;
            _CustomerOpenBalanceService = CustomerOpenBalanceService;
            _SupplerOpenBalanceService = SupplerOpenBalanceService;
            _BankOpenBalanceService = BankOpenBalanceService;
            _TreasuryOpenBalanceService = TreasuryOpenBalanceService;
            _DailyEntryService = DailyEntryService;
            _BankService = BankService;
            _TreasuryService = TreasuryService;
            _CustomerService = CustomerService;
            _SupplerService = SupplerService;
            _DefaultAccountService = DefaultAccountService;
            _FinancialPeriodService = FinancialPeriodService;
            _LocalizationService = localizationService;



        }

        #endregion

        #region Utilities

        /// <summary>
        /// Check whether current HTTP request is available
        /// </summary>
        /// <returns>True if available; otherwise false</returns>
        protected virtual bool IsRequestAvailable()
        {
            if (_httpContextAccessor?.HttpContext == null)
                return false;

            try
            {
                if (_httpContextAccessor.HttpContext.Request == null)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Is IP address specified
        /// </summary>
        /// <param name="address">IP address</param>
        /// <returns>Result</returns>
        protected virtual bool IsIpAddressSet(IPAddress address)
        {
            var rez = address != null && address.ToString() != IPAddress.IPv6Loopback.ToString();

            return rez;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get URL referrer if exists
        /// </summary>
        /// <returns>URL referrer</returns>
        public virtual string GetUrlReferrer()
        {
            if (!IsRequestAvailable())
                return string.Empty;

            //URL referrer is null in some case (for example, in IE 8)
            return _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Referer];
        }

        /// <summary>
        /// Get IP address from HTTP context
        /// </summary>
        /// <returns>String of IP address</returns>
        public virtual string GetCurrentIpAddress()
        {
            if (!IsRequestAvailable())
                return string.Empty;

            if (_httpContextAccessor.HttpContext.Connection?.RemoteIpAddress is not IPAddress remoteIp)
                return "";

            if (remoteIp.Equals(IPAddress.IPv6Loopback))
                return IPAddress.Loopback.ToString();

            return remoteIp.MapToIPv4().ToString();
        }

       

        /// <summary>
        /// Gets a value indicating whether current connection is secured
        /// </summary>
        /// <returns>True if it's secured, otherwise false</returns>
        public virtual bool IsCurrentConnectionSecured()
        {
            if (!IsRequestAvailable())
                return false;

            return _httpContextAccessor.HttpContext.Request.IsHttps;
        }

        /// <summary>
        /// Gets store host location
        /// </summary>
        /// <param name="useSsl">Whether to get SSL secured URL</param>
        /// <returns>Store host location</returns>
        public virtual string GetStoreHost(bool useSsl)
        {
            if (!IsRequestAvailable())
                return string.Empty;

            //try to get host from the request HOST header
            var hostHeader = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Host];
            if (StringValues.IsNullOrEmpty(hostHeader))
                return string.Empty;

            //add scheme to the URL
            var storeHost = $"{(useSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp)}{Uri.SchemeDelimiter}{hostHeader.FirstOrDefault()}";

            //ensure that host is ended with slash
            storeHost = $"{storeHost.TrimEnd('/')}/";

            return storeHost;
        }

        
        /// <summary>
        /// Returns true if the requested resource is one of the typical resources that needn't be processed by the cms engine.
        /// </summary>
        /// <returns>True if the request targets a static resource file.</returns>
        public virtual bool IsStaticResource()
        {
            if (!IsRequestAvailable())
                return false;

            string path = _httpContextAccessor.HttpContext.Request.Path;

            //a little workaround. FileExtensionContentTypeProvider contains most of static file extensions. So we can use it
            //source: https://github.com/aspnet/StaticFiles/blob/dev/src/Microsoft.AspNetCore.StaticFiles/FileExtensionContentTypeProvider.cs
            //if it can return content type, then it's a static file
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            return contentTypeProvider.TryGetContentType(path, out var _);
        }    


        /// <summary>
        /// Restart application domain
        /// </summary>
        public virtual void RestartAppDomain()
        {
            _hostApplicationLifetime.StopApplication();
        }

        /// <summary>
        /// Gets a value that indicates whether the client is being redirected to a new location
        /// </summary>
        public virtual bool IsRequestBeingRedirected
        {
            get
            {
                var response = _httpContextAccessor.HttpContext.Response;
                //ASP.NET 4 style - return response.IsRequestBeingRedirected;
                int[] redirectionStatusCodes = { StatusCodes.Status301MovedPermanently, StatusCodes.Status302Found };

                return redirectionStatusCodes.Contains(response.StatusCode);
            }
        }

      
        /// <summary>
        /// Gets current HTTP request protocol
        /// </summary>
        public virtual string GetCurrentRequestProtocol()
        {
            return IsCurrentConnectionSecured() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        }

        /// <summary>
        /// Gets whether the specified HTTP request URI references the local host.
        /// </summary>
        /// <param name="req">HTTP request</param>
        /// <returns>True, if HTTP request URI references to the local host</returns>
        public virtual bool IsLocalRequest(HttpRequest req)
        {
            //source: https://stackoverflow.com/a/41242493/7860424
            var connection = req.HttpContext.Connection;
            if (IsIpAddressSet(connection.RemoteIpAddress))
            {
                //We have a remote address set up
                return IsIpAddressSet(connection.LocalIpAddress)
                    //Is local is same as remote, then we are local
                    ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                    //else we are remote if the remote IP address is not a loopback address
                    : IPAddress.IsLoopback(connection.RemoteIpAddress);
            }

            return true;
        }

        /// <summary>
        /// Get the raw path and full query of request
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>Raw URL</returns>
        public virtual string GetRawUrl(HttpRequest request)
        {
            //first try to get the raw target from request feature
            //note: value has not been UrlDecoded
            var rawUrl = request.HttpContext.Features.Get<IHttpRequestFeature>()?.RawTarget;

            //or compose raw URL manually
            if (string.IsNullOrEmpty(rawUrl))
                rawUrl = $"{request.PathBase}{request.Path}{request.QueryString}";

            return rawUrl;
        }

        /// <summary>
        /// Gets whether the request is made with AJAX 
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>Result</returns>
        public virtual bool IsAjaxRequest(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers == null)
                return false;

            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }




        public string IsAllowedDeleteUnit(int UnitId)
        {
            if (UnitId > 0)
            {
                var UsedWithItem= _ItemService.GetWithCondetion(x=>x.DefaultUnit== UnitId).FirstOrDefault();
                if (UsedWithItem != null )
                {
                    return "NotAllowedDeleteUnit.UsedWithItmes";
                }

                var UsedWithTransaction = _TransDetailsService.GetWithCondetion(x => x.UnitId == UnitId).FirstOrDefault();
                if (UsedWithTransaction != null)
                {
                    return "NotAllowedDeleteUnit.UsedWithTransactions";
                }
            }

            return "";
        }
        public string IsAllowedDeleteItem(int ItemId)
        {
            if (ItemId > 0)
            {
                //var UsedWithItem= _ItemService.GetWithCondetion(x=>x.DefaultUnit== UnitId);
                //if (UsedWithItem != null && UsedWithItem.Count()>0)
                //{
                //    return "NotAllowedDeleteUnit.UsedWithItmes";
                //}

                var UsedWithTransaction = _TransDetailsService.GetWithCondetion(x => x.ItemId == ItemId).FirstOrDefault();
                if (UsedWithTransaction != null)
                {
                    return "NotAllowedDeleteItem.UsedWithTransactions";
                }
            }

            return "";
        } 
        public string IsAllowedDeleteItemGroup(int ItemGroupId)
        {
            if (ItemGroupId > 0)
            {
                var UsedWithItem = _ItemService.GetWithCondetion(x => x.GroupId == ItemGroupId);
                if (UsedWithItem != null && UsedWithItem.Count() > 0)
                {
                    return "NotAllowedDeleteItemGroup.UsedWithItmes";
                }

                var UsedWithTransaction = _TransDetailsService.GetWithCondetion(x => x.GroupId == ItemGroupId).FirstOrDefault();
                if (UsedWithTransaction != null)
                {
                    return "NotAllowedDeleteItemGroup.UsedWithTransactions";
                }
            }

            return "";
        }
        public string IsAllowedDeleteBranch(int BranchId)
        {
            if (BranchId > 0)
            {
                var UsedWithItem = _StoreService.GetWithCondetion(x => x.BranchId == BranchId).FirstOrDefault();
                if (UsedWithItem != null)
                {
                    return "NotAllowedDeleteBranch.UsedWithStores";
                }

                var UsedWithTransaction = _TransMasterService.GetWithCondetion(x => x.BranchId == BranchId).FirstOrDefault();
                if (UsedWithTransaction != null)
                {
                    return "NotAllowedDeleteBranch.UsedWithTransactions";
                }
            }

            return "";
        } 
        public string IsAllowedDeleteStore(int StoreId)
        {
            if (StoreId > 0)
            {                
                var UsedWithTransaction = _TransMasterService.GetWithCondetion(x => x.StoreId == StoreId).FirstOrDefault();
                if (UsedWithTransaction != null)
                {
                    return "NotAllowedDeleteStore.UsedWithTransactions";
                }
            }

            return "";
        }
        public string IsAllowedDeleteCompany(int CompanyId)
        {
            if (CompanyId > 0)
            {
                var UsedWithUser = _IdentityService.GetAllUsers().Where(x => x.CompanyId == CompanyId).FirstOrDefault();
                if (UsedWithUser != null)
                {
                    return "NotAllowedDeleteCompany.UsedWithUsers";
                }
                var UsedWithUsers = _IdentityService.GetAllUsers().Where(x => x.CompanyId == CompanyId).FirstOrDefault();
                if (UsedWithUsers != null)
                {
                    return "NotAllowedDeleteCompany.UsedWithUsers";
                }
                var UsedWithTransaction = _TransMasterService.GetWithCondetion(x => x.CompanyId == CompanyId).FirstOrDefault();
                if (UsedWithTransaction != null)
                {
                    return "NotAllowedDeleteCompany.UsedWithTransactions";
                }
            }

            return "";
        }
        public string IsAllowedDeleteCurruncy(int CurruncyId)
        {
            if (CurruncyId > 0)
            {
                var UsedWithTransaction = _TransMasterService.GetWithCondetion(x => x.CurrencyId == CurruncyId).FirstOrDefault();
                if (UsedWithTransaction != null)
                {
                    return "NotAllowedDeleteCurruncy.UsedWithTransactions";
                }
            }

            return "";
        }
        public string IsAllowedDeleteCustomer(int CustomerId)
        {
            if (CustomerId > 0)
            {
                var UsedWithTransaction = _TransMasterService.GetWithCondetion(x => x.CustomerId == CustomerId).FirstOrDefault();
                if (UsedWithTransaction != null)
                {
                    return "NotAllowedDeleteCustomer.UsedWithTransactions";
                }

                var UsedWithOpenBalance = _CustomerOpenBalanceService.GetWithCondetion(x => x.CustomerId == CustomerId).FirstOrDefault();
                if (UsedWithOpenBalance != null)
                {
                    return "NotAllowedDeleteCustomer.UsedWithCustomerOpenBalance";
                }
            }

            return "";
        } 
        public string IsAllowedDeleteSuppler(int SupplerId)
        {
            if (SupplerId > 0)
            {
                var UsedWithTransaction = _TransMasterService.GetWithCondetion(x => x.SupplierId == SupplerId).FirstOrDefault();
                if (UsedWithTransaction != null)
                {
                    return "NotAllowedDeleteSuppler.UsedWithTransactions";
                }
                var UsedWithOpenBalance = _SupplerOpenBalanceService.GetWithCondetion(x => x.SupplerId == SupplerId).FirstOrDefault();
                if (UsedWithOpenBalance != null)
                {
                    return "NotAllowedDeleteCustomer.UsedWithSupplerOpenBalance";
                }

            }

            return "";
        }
        public string IsAllowedDeleteFinancialPeriod(int FinancialPeriodId)
        {
            if (FinancialPeriodId > 0)
            {
                var UsedWithTransaction = _TransMasterService.GetWithCondetion(x => x.FinancialPeriodId == FinancialPeriodId).FirstOrDefault();
                if (UsedWithTransaction != null)
                {
                    return "NotAllowedDeleteFinancialPeriod.UsedWithTransactions";
                }
            }

            return "";
        }
        public string IsAllowedDeleteDailyAccounts_Def(int DailyAccountId)
        {
            //if (DailyAccountId > 0)
            //{                
            //    var UsedWithTransaction = _TransMasterService.GetWithCondetion(x => x.BranchId == BranchId).FirstOrDefault();
            //    if (UsedWithTransaction != null)
            //    {
            //        return "NotAllowedDeleteBranch.UsedWithTransactions";
            //    }
            //}
            return "";
        }

        public string IsAllowedDeleteBank(int BankId)
        {
            if (BankId > 0)
            {
                //var UsedWithTransaction = _TransMasterService.GetWithCondetion(x => x.SupplierId == SupplerId).FirstOrDefault();
                //if (UsedWithTransaction != null)
                //{
                //    return "NotAllowedDeleteSuppler.UsedWithTransactions";
                //}
                var UsedWithOpenBalance = _BankOpenBalanceService.GetWithCondetion(x => x.BankId == BankId).FirstOrDefault();
                if (UsedWithOpenBalance != null)
                {
                    return "NotAllowedDeleteCustomer.UsedWithBankOpenBalance";
                }

            }

            return "";
        }
        public string IsAllowedDeleteTreasury(int TreasuryId)
        {
            if (TreasuryId > 0)
            {
                //var UsedWithTransaction = _TransMasterService.GetWithCondetion(x => x.SupplierId == SupplerId).FirstOrDefault();
                //if (UsedWithTransaction != null)
                //{
                //    return "NotAllowedDeleteSuppler.UsedWithTransactions";
                //}
                var UsedWithOpenBalance = _TreasuryOpenBalanceService.GetWithCondetion(x => x.TreasuryId == TreasuryId).FirstOrDefault();
                if (UsedWithOpenBalance != null)
                {
                    return "NotAllowedDeleteCustomer.UsedWithTreasuryOpenBalance";
                }

            }

            return "";
        } 
        public string IsAllowedDeleteAccount(List<int> AccountIds)
        {
            if (AccountIds.Count > 0)
            {
                
                var EntryDetails = _DailyEntryService.GetEntryDetails(x => AccountIds.Contains( x.AccountId));
                if (EntryDetails != null && EntryDetails.Count>0)
                {
                    return "NotAllowedDeleteAccount.TheAccountHasSubAccountsUsedWithEntries";
                }

                var Bancks = _BankService.GetAll().Where(x => AccountIds.Contains(x.AccountId)).ToList();

                if (Bancks != null && Bancks.Count > 0)
                {
                    return "NotAllowedDeleteAccount.TheAccountUsedWithBanks";
                }

                var Treasurys = _TreasuryService.GetAll().Where(x => AccountIds.Contains(x.AccountId)).ToList();
                if (Treasurys != null && Treasurys.Count > 0)
                {
                    return "NotAllowedDeleteAccount.TheAccountUsedWithTreasurys";
                }

                var Customers = _CustomerService.GetAll().Where(x => AccountIds.Contains(x.AccountId)).ToList();
                if (Customers != null && Customers.Count > 0)
                {
                    return "NotAllowedDeleteAccount.TheAccountUsedWithCustomers";
                }

                var Supplers = _SupplerService.GetAll().Where(x => AccountIds.Contains(x.AccountId)).ToList();
                if (Supplers != null && Supplers.Count > 0)
                {
                    return "NotAllowedDeleteAccount.TheAccountUsedWithSupplers";
                }

                var DefaultAccounts = _DefaultAccountService.GetAll().Where(x => AccountIds.Contains(x.AccountId)).ToList();
                if (DefaultAccounts != null && DefaultAccounts.Count > 0)
                {
                    return "NotAllowedDeleteAccount.TheAccountUsedWithDefaultAccounts";
                }

            }

            return "";
        }

        public decimal GetItemAveragePurchasPrice(int financePeriodId, int itemId, int branchId, int storeId, DateTime? toDate=null)
        {
            decimal PriceAverage = 0;
            var master = _TransMasterService.GetWithCondetion(x => x.DocTypeId == (int)DocumentTypes.PurchaseInvoice && x.FinancialPeriodId == financePeriodId && x.BranchId == branchId && x.StoreId == storeId);
            if (toDate!=null)
            {
                master = master.Where(x=>x.DocDate<= toDate.Value).ToList();
            }

            var masterIds = master.Select(x => x.Id);

            var details = _TransDetailsService.GetWithCondetion(x => x.ItemId == itemId && masterIds.Contains(x.MasterId));

            var masterDetails = (from m in master join d in details on m.Id equals d.MasterId select new { Quntity = d.Quntity, PurchasePrice = d.PurchasePrice, CurrencyFactor = m.CurrencyFactor }).ToList();

            var totalCoast = masterDetails.Sum(s => s.Quntity * s.PurchasePrice.Value * s.CurrencyFactor);
            var totalQty = masterDetails.Sum(s => s.Quntity);

            if (totalQty > 0)
            {
                PriceAverage = totalCoast / totalQty;
            }

            if (PriceAverage == 0)
            {
                var item = _ItemService.GetById(itemId);
                if (item != null)
                {
                    PriceAverage = item.PurchasePrice;
                }
            }

            return PriceAverage;

        }

        public string ValidateFinancePeriod(int financePeriodId,DateTime? docDate)
        {
            string msg ="";
            var financePeriod = _FinancialPeriodService.GetById(financePeriodId);
            if (financePeriod != null)
            {
                if (financePeriod.isClosed)
                {
                    msg = _LocalizationService.GetLocalizedHtmlString("CuruntPeriodIsClosedFinal");
                }
                else if (docDate.HasValue && (docDate.Value.Date > financePeriod.DateTo.Date || docDate.Value.Date < financePeriod.DateFrom.Date))
                {
                    msg = _LocalizationService.GetLocalizedHtmlString("TransactionDateOutFinanceYear");
                }
            }
            else
            {
                msg= _LocalizationService.GetLocalizedHtmlString("FinancialPeriodNotFound");
            }

            return msg;

        }

        #endregion
    }
}
