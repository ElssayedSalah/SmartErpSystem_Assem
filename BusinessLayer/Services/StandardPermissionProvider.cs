using BusinessLayer.Models.Identity;
using DataAccessLayer.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessLayer.Helpers.SharedEnums;

namespace BusinessLayer.Services
{
   public class StandardPermissionProvider : IPermissionProvider
    {
        //AllowedActions contains ids of actions that allowed for each permission
        //1= id of Create action
        //2= id of Edite action
        //3= id of Delete action
        //4= id of List action
        //5= id of Report action or print

        //VisibleForSuperOnly detremine if the permission for super user only


        //basics forms
        public static readonly AspNetClaims ManageBranches = new() { Name = "Manage Branches", SystemName = "ManageBranches", Category = "Basic", SubCategory = "Basic", AllowedActions=new List<int>() { 1,2,3,4} };

        public static readonly AspNetClaims ManageStores = new() { Name = "Manage Stores", SystemName = "ManageStores", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4} };

        public static readonly AspNetClaims ManageUnits = new() { Name = "ManagevUnits", SystemName = "ManageUnits", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4} };
        
        public static readonly AspNetClaims ManageCurrencys = new() { Name = "ManageCurrencys", SystemName = "ManageCurrencys", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4} };

        public static readonly AspNetClaims ManageDepartements = new() { Name = "ManageDepartements", SystemName = "ManageDepartements", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4} };

        public static readonly AspNetClaims ManageItems = new() { Name = "ManageItems", SystemName = "ManageItems", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4} };

        public static readonly AspNetClaims ManageItemGroups = new() { Name = "ManageItemGroups", SystemName = "ManageItemGroups", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4} };       

        public static readonly AspNetClaims ManageCustomeres = new() { Name = "ManageCustomeres", SystemName = "ManageCustomeres", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4 } };

        public static readonly AspNetClaims ManageSuppleres = new() { Name = "ManageSuppleres", SystemName = "ManageSuppleres", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4 } };

        public static readonly AspNetClaims ManageUsers = new() { Name = "ManageUsers", SystemName = "ManageUsers", Category = "Permission", SubCategory = "Permission", AllowedActions = new List<int>() { 1, 2, 3, 4} };

        public static readonly AspNetClaims ManageRoles = new() { Name = "ManageRoles", SystemName = "ManageRoles", Category = "Permission", SubCategory = "Permission", AllowedActions = new List<int>() { 1, 2, 3, 4 } };  

        public static readonly AspNetClaims ManagePermissions = new() { Name = "ManagePermissions", SystemName = "ManagePermissions", Category = "Permission", SubCategory = "Permission", AllowedActions = new List<int>() { 1, 2, 3, 4}, VisibleForSuperOnly = false };

        public static readonly AspNetClaims ManageDailyAccounts_Def = new() { Name = "Manage Daily Accounts", SystemName = "ManageDailyAccounts_Def", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4 } };
        
        public static readonly AspNetClaims ManageBankes = new() { Name = "Manage Bankes", SystemName = "ManageBankes", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4 } };
        
        public static readonly AspNetClaims ManageTreasuryes = new() { Name = "Manage Treasuryes", SystemName = "ManageTreasuryes", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4 } }; 
        
        public static readonly AspNetClaims ManageChartOfAccountSettings = new() { Name = "Manage Chart Of Account Settings", SystemName = "ManageChartOfAccountSettings", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4 } };

        public static readonly AspNetClaims ManageDefaultAccounts = new() { Name = "Manage Default Accounts", SystemName = "ManageDefaultAccounts", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4 } };
        
        public static readonly AspNetClaims ManageChartOfAccounts = new() { Name = "Manage Chart Of Accounts", SystemName = "ManageChartOfAccounts", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4 } };       
       
        

        //transactions forms
        public static readonly AspNetClaims ManageOpenBalance = new() { Name = "ManageOpenBalance", SystemName = "ManageOpenBalance", Category = "Transaction",SubCategory="Inventory", AllowedActions = new List<int>() { 1, 2, 3, 4, 5 ,5} };
        public static readonly AspNetClaims ManageAddToStore = new() { Name = "ManageAddToStore", SystemName = "ManageAddToStore", Category = "Transaction",SubCategory="Inventory", AllowedActions = new List<int>() { 1, 2, 3, 4, 5 ,5} };
        public static readonly AspNetClaims ManageStoreRecieve = new() { Name = "ManageStoreRecieve", SystemName = "ManageStoreRecieve", Category = "Transaction",SubCategory="Inventory", AllowedActions = new List<int>() { 1, 2, 3, 4, 5} };
        public static readonly AspNetClaims ManageStoreOut = new() { Name = "ManageStoreOut", SystemName = "ManageStoreOut", Category = "Transaction",SubCategory="Inventory", AllowedActions = new List<int>() { 1, 2, 3, 4, 5} };
        public static readonly AspNetClaims ManageStoreDeprecate = new() { Name = "ManageStoreDeprecate", SystemName = "ManageStoreDeprecate", Category = "Transaction",SubCategory="Inventory", AllowedActions = new List<int>() { 1, 2, 3, 4, 5} };
        public static readonly AspNetClaims ManageStoreCount = new() { Name = "ManageStoreCount", SystemName = "ManageStoreCount", Category = "Transaction", SubCategory = "Inventory", AllowedActions = new List<int>() { 1, 2, 3, 4, 5 } };
        public static readonly AspNetClaims ManageStoreSettlement = new() { Name = "ManageStoreSettlement", SystemName = "ManageStoreSettlement", Category = "Transaction", SubCategory = "Inventory", AllowedActions = new List<int>() { 1, 2, 3, 4, 5 } };



        public static readonly AspNetClaims ManageSalesInvoice = new() { Name = "ManageSalesInvoice", SystemName = "ManageSalesInvoice", Category = "Transaction",SubCategory= "Sales", AllowedActions = new List<int>() { 1, 2, 3, 4 , 5} };
        public static readonly AspNetClaims ManageSalesReturn = new() { Name = "ManageSalesReturn", SystemName = "ManageSalesReturn", Category = "Transaction", SubCategory = "Sales", AllowedActions = new List<int>() { 1, 2, 3, 4 , 5 } };
        public static readonly AspNetClaims ManageEGInvoice = new() { Name = "ManageEGInvoice", SystemName = "ManageEGInvoice", Category = "Transaction", SubCategory = "Sales", AllowedActions = new List<int>() { 1, 2, 3, 4 } };

        public static readonly AspNetClaims ManagePurchasesInvoice = new() { Name = "ManagePurchasesInvoice", SystemName = "ManagePurchasesInvoice", Category = "Transaction",SubCategory= "Purchases", AllowedActions = new List<int>() { 1, 2, 3, 4 , 5} };
        public static readonly AspNetClaims ManagePurchaseReturn = new() { Name = "ManagePurchaseReturn", SystemName = "ManagePurchaseReturn", Category = "Transaction",SubCategory= "Purchases", AllowedActions = new List<int>() { 1, 2, 3, 4 , 5} };

        public static readonly AspNetClaims ManageDailyEntry = new() { Name = "Manage Daily Entry", SystemName = "ManageDailyEntry", Category = "Transaction", SubCategory = "Finance", AllowedActions = new List<int>() { 1, 2, 3, 4 } };
        public static readonly AspNetClaims ManageCashRecieveTransaction = new() { Name = "Manage Cash Recieve Transaction", SystemName = "ManageCashRecieveTransaction", Category = "Transaction", SubCategory = "Finance", AllowedActions = new List<int>() { 1, 2, 3, 4 } };
       public static readonly AspNetClaims ManageCashExchangeTransaction = new() { Name = "Manage Cash Exchange Transaction", SystemName = "ManageCashExchangeTransaction", Category = "Transaction", SubCategory = "Finance", AllowedActions = new List<int>() { 1, 2, 3, 4 } }; 
        public static readonly AspNetClaims ManageDebitSettlementTransaction = new() { Name = "Manage Debit Settlement Transaction", SystemName = "ManageDebitSettlementTransaction", Category = "Transaction", SubCategory = "Finance", AllowedActions = new List<int>() { 1, 2, 3, 4 } };
       public static readonly AspNetClaims ManageCreditSettlementTransaction = new() { Name = "Manage Credit Settlement Transaction", SystemName = "ManageCreditSettlementTransaction", Category = "Transaction", SubCategory = "Finance", AllowedActions = new List<int>() { 1, 2, 3, 4 } };
       public static readonly AspNetClaims ManageWriteCheckOutTransaction = new() { Name = "Manage Write CheckOut Transaction", SystemName = "ManageWriteCheckOutTransaction", Category = "Transaction", SubCategory = "Finance", AllowedActions = new List<int>() { 1, 2, 3, 4 } };
       public static readonly AspNetClaims ManageCheckOutExchangeTransaction = new() { Name = "Manage CheckOut Exchange Transaction", SystemName = "ManageCheckOutExchangeTransaction", Category = "Transaction", SubCategory = "Finance", AllowedActions = new List<int>() { 1, 2, 3, 4 } };
       public static readonly AspNetClaims ManageRecieveCheckInTransaction = new() { Name = "Manage Recieve CheckIn Transaction", SystemName = "ManageRecieveCheckInTransaction", Category = "Transaction", SubCategory = "Finance", AllowedActions = new List<int>() { 1, 2, 3, 4 } };
       public static readonly AspNetClaims ManageCheckInExchangeTransaction = new() { Name = "Manage CheckIn Exchange Transaction", SystemName = "ManageCheckInExchangeTransaction", Category = "Transaction", SubCategory = "Finance", AllowedActions = new List<int>() { 1, 2, 3, 4 } };
       public static readonly AspNetClaims ManageCheckReturnTransaction = new() { Name = "Manage Check Return Transaction", SystemName = "ManageCheckReturnTransaction", Category = "Transaction", SubCategory = "Finance", AllowedActions = new List<int>() { 1, 2, 3, 4 } };
       public static readonly AspNetClaims ManageCashDepositInBankTransaction = new() { Name = "Manage Cash DepositIn Bank Transaction", SystemName = "ManageCashDepositInBankTransaction", Category = "Transaction", SubCategory = "Finance", AllowedActions = new List<int>() { 1, 2, 3, 4 } };  
        public static readonly AspNetClaims ManageCashWithdrawalFromBankTransaction = new() { Name = "Manage Cash Withdrawal From Bank Transaction", SystemName = "ManageCashWithdrawalFromBankTransaction", Category = "Transaction", SubCategory = "Finance", AllowedActions = new List<int>() { 1, 2, 3, 4 } };






        //settings forms
        public static readonly AspNetClaims ManageCompanys = new() { Name = "ManageCompanys", SystemName = "ManageCompanys", Category = "System", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4}, VisibleForSuperOnly = true };

        public static readonly AspNetClaims Dashboard = new() { Name = "Dashboard", SystemName = "Dashboard", Category = "Basic",SubCategory= "Basic", AllowedActions = new List<int>() {4} };

        public static readonly AspNetClaims ManageFinancialPeriods = new() { Name = "ManageFinancialPeriods", SystemName = "ManageFinancialPeriods", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4 } };
        
        public static readonly AspNetClaims ManageSystemSettinges = new() { Name = "ManageSystemSettinges", SystemName = "ManageSystemSettinges", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4 }, VisibleForSuperOnly = true };       
        
        public static readonly AspNetClaims ManageTransactionsEntrySettinges = new() { Name = "Manage Transactions Entry Settinges", SystemName = "ManageTransactionsEntrySettinges", Category = "Basic", SubCategory = "Basic", AllowedActions = new List<int>() { 1, 2, 3, 4 } };



        //reports
        public static readonly AspNetClaims ManageOpenBalanceReport = new() { Name = "ManageOpenBalanceReport", SystemName = "ManageOpenBalanceReport", Category = "Report", SubCategory = "Report", AllowedActions = new List<int>() {5} };
        public static readonly AspNetClaims ManageItemDataReport = new() { Name = "ManageItemDataReport", SystemName = "ManageItemDataReport", Category = "Report", SubCategory = "Report", AllowedActions = new List<int>() {5} };
        public static readonly AspNetClaims ManageItemBalanceQuantityAndValueReport = new() { Name = "ManageItemBalanceQuantityAndValueReport", SystemName = "ManageItemBalanceQuantityAndValueReport", Category = "Report", SubCategory = "Report", AllowedActions = new List<int>() {5} };  
        public static readonly AspNetClaims ManageItemCartReport = new() { Name = "ManageItemCartReport", SystemName = "ManageItemCartReport", Category = "Report", SubCategory = "Report", AllowedActions = new List<int>() {5} };
        public static readonly AspNetClaims ManageItemPurshasAnaysisReport = new() { Name = "ManageItemPurshasAnaysisReport", SystemName = "ManageItemPurshasAnaysisReport", Category = "Report", SubCategory = "Report", AllowedActions = new List<int>() {5} };
        public static readonly AspNetClaims ManageSupplierBalanceReport = new() { Name = "ManageSupplierBalanceReport", SystemName = "ManageSupplierBalanceReport", Category = "Report", SubCategory = "Report", AllowedActions = new List<int>() {5} };
        public static readonly AspNetClaims ManageSupplierBalanceTotalReport = new() { Name = "ManageSupplierBalanceTotalReport", SystemName = "ManageSupplierBalanceTotalReport", Category = "Report", SubCategory = "Report", AllowedActions = new List<int>() {5} }; 
        public static readonly AspNetClaims ManageItemSalesAnaysisReport = new() { Name = "ManageItemSalesAnaysisReport", SystemName = "ManageItemSalesAnaysisReport", Category = "Report", SubCategory = "Report", AllowedActions = new List<int>() {5} };
        public static readonly AspNetClaims ManageCustomerBalanceReport = new() { Name = "ManageCustomerBalanceReport", SystemName = "ManageCustomerBalanceReport", Category = "Report", SubCategory = "Report", AllowedActions = new List<int>() { 5 } };
        public static readonly AspNetClaims ManageCustomerBalanceTotalReport = new() { Name = "ManageCustomerBalanceTotalReport", SystemName = "ManageCustomerBalanceTotalReport", Category = "Report", SubCategory = "Report", AllowedActions = new List<int>() { 5 } };



        public virtual IEnumerable<AspNetClaims> GetPermissions()
        {
            return new[]
            {
               ManageBranches,
               ManageStores,
               ManageUnits,
               Dashboard,
               ManageDepartements,
               ManageItems,
               ManageItemGroups,
               ManageCompanys,
               ManageUsers,
               ManageRoles,
               ManagePermissions,
               ManageOpenBalance,
               ManageStoreRecieve,
               ManageStoreOut,
               ManageStoreDeprecate,
               ManageCustomeres,
               ManageSuppleres,
               ManageCurrencys,
               ManageFinancialPeriods,
               ManageSalesInvoice,
               ManageSalesReturn,
               ManagePurchasesInvoice,
               ManagePurchaseReturn,
               ManageOpenBalanceReport,
               ManageItemDataReport,
               ManageItemBalanceQuantityAndValueReport,
               ManageItemCartReport,
               ManageItemPurshasAnaysisReport,
               ManageSupplierBalanceReport,
               ManageSupplierBalanceTotalReport,
               ManageItemSalesAnaysisReport,
               ManageCustomerBalanceReport,
               ManageCustomerBalanceTotalReport,
               ManageStoreCount,
               ManageStoreSettlement,
               ManageSystemSettinges,
               ManageEGInvoice,
               ManageDailyAccounts_Def,
               ManageBankes,
               ManageTreasuryes,
               ManageChartOfAccountSettings,
               ManageDefaultAccounts,
               ManageChartOfAccounts,
               ManageDailyEntry,
               ManageTransactionsEntrySettinges,
               ManageCashRecieveTransaction,
               ManageCashExchangeTransaction,
               ManageDebitSettlementTransaction,
               ManageCreditSettlementTransaction,
               ManageWriteCheckOutTransaction,
               ManageCheckOutExchangeTransaction,
               ManageRecieveCheckInTransaction,
               ManageCheckInExchangeTransaction,
               ManageCheckReturnTransaction,
               ManageCashDepositInBankTransaction,
               ManageCashWithdrawalFromBankTransaction,
               ManageAddToStore

            };
        }

        public IList<ActionsModel> AvailableActions { get; set; }
        public StandardPermissionProvider()
        {
            AvailableActions = new List<ActionsModel>() {
                new ActionsModel() { Id=(int)PermissionActions.Create, Name = "Create", Selected=false },
                new ActionsModel() { Id=(int)PermissionActions.Edit, Name = "Edit", Selected=false },
                new ActionsModel() { Id=(int)PermissionActions.Delete, Name = "Delete", Selected=false },
                new ActionsModel() { Id=(int)PermissionActions.List, Name = "List", Selected=false },
                new ActionsModel() { Id=(int)PermissionActions.Report, Name = "Report", Selected=false }

            };
        }


    }
}
