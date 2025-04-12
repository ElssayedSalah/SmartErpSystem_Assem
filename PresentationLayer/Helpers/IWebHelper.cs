using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace PresentationLayer.Helpers
{
    public partial interface IWebHelper
    {
        /// <summary>
        /// Get URL referrer if exists
        /// </summary>
        /// <returns>URL referrer</returns>
        string GetUrlReferrer();

        /// <summary>
        /// Get IP address from HTTP context
        /// </summary>
        /// <returns>String of IP address</returns>
        string GetCurrentIpAddress();      

        /// <summary>
        /// Gets a value indicating whether current connection is secured
        /// </summary>
        /// <returns>True if it's secured, otherwise false</returns>
        bool IsCurrentConnectionSecured();

        /// <summary>
        /// Gets store host location
        /// </summary>
        /// <param name="useSsl">Whether to get SSL secured URL</param>
        /// <returns>Store host location</returns>
        string GetStoreHost(bool useSsl);

       
        /// <summary>
        /// Returns true if the requested resource is one of the typical resources that needn't be processed by the CMS engine.
        /// </summary>
        /// <returns>True if the request targets a static resource file.</returns>
        bool IsStaticResource();


        /// <summary>
        /// Restart application domain
        /// </summary>
        void RestartAppDomain();

        /// <summary>
        /// Gets a value that indicates whether the client is being redirected to a new location
        /// </summary>
        bool IsRequestBeingRedirected { get; }       

        /// <summary>
        /// Gets current HTTP request protocol
        /// </summary>
        string GetCurrentRequestProtocol();

        /// <summary>
        /// Gets whether the specified HTTP request URI references the local host.
        /// </summary>
        /// <param name="req">HTTP request</param>
        /// <returns>True, if HTTP request URI references to the local host</returns>
        bool IsLocalRequest(HttpRequest req);

        /// <summary>
        /// Get the raw path and full query of request
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>Raw URL</returns>
        string GetRawUrl(HttpRequest request);

        /// <summary>
        /// Gets whether the request is made with AJAX 
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>Result</returns>
        bool IsAjaxRequest(HttpRequest request);





        string IsAllowedDeleteUnit(int UnitId);
        string IsAllowedDeleteItem(int ItemId);
        string IsAllowedDeleteItemGroup(int ItemGroupId);
        string IsAllowedDeleteBranch(int BranchId);
        string IsAllowedDeleteStore(int StoreId);
        string IsAllowedDeleteCompany(int CompanyId);
        string IsAllowedDeleteCurruncy(int CurruncyId);
        string IsAllowedDeleteCustomer(int CustomerId);
        string IsAllowedDeleteSuppler(int SupplerId);
        string IsAllowedDeleteFinancialPeriod(int FinancialPeriodId);
        string IsAllowedDeleteDailyAccounts_Def(int FinancialPeriodId);
        string IsAllowedDeleteBank(int BankId);
        string IsAllowedDeleteTreasury(int TreasuryId);
        string IsAllowedDeleteAccount(List<int> AccountIds);
        public decimal GetItemAveragePurchasPrice(int financePeriodId, int itemId, int branchId, int storeId, DateTime? toDate);
        public string ValidateFinancePeriod(int financePeriodId, DateTime? docDate);



    }
}
