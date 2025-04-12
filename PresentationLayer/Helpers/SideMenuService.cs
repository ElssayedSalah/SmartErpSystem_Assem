using BusinessLayer.Services;
using DataAccessLayer.Entities.Identity;
using System.Collections.Generic;

namespace PresentationLayer.Helpers
{
    public class SideMenuService
    {
        public SideMenuService()
        {

        }

        public List<SideMenuItem> SideMenuItems = new List<SideMenuItem>()
        {
            //Permission ==>control visible of menu according to the permission action list
           
      #region المخازن
      
            new SideMenuItem(){
                ItemTextLocalizationResource="InventoryMenuName", Controler="",Action="",Data_Ds_Target="inventoryMenu",Icon="fa fa-store",ItemType=SideMenuItemType.MainMenu,
                ChaildsMenuItems=new List<SideMenuItem>()
                {
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="BasicDataMenuName", Controler="",Action="",Data_Ds_Target="basicInventoryMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,
                  ChaildsMenuItems=new List<SideMenuItem>()
                  {
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="BranchesMenuName", Controler="Branch",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageBranches
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="StoresMenuName", Controler="Store",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageStores
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="UnitsMenuName", Controler="Unit",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageUnits
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="ItemGroupMenuName", Controler="ItemGroup",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageItemGroups
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="DepartmentMenuName", Controler="Department",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageDepartements
                 },
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="ItemsMenuName", Controler="Item",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageItems
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="CurrencyMenuName", Controler="Currency",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageCurrencys
                 }

                }
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="TransactionsMenuName", Controler="",Action="",Data_Ds_Target="transactionsInventoryMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,
                    ChaildsMenuItems=new List<SideMenuItem>()
                    {
                   new SideMenuItem()
                 {
                   ItemTextLocalizationResource="AddToStore", Controler="AddToStore",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageAddToStore
                 },
                   new SideMenuItem()
                 {
                   ItemTextLocalizationResource="OpenBalanceMenuName", Controler="OpenBalance",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageOpenBalance
                 },
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="AddToStoreMenuName", Controler="",Action="",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="StoreRecieveMenuName", Controler="StoreRecieve",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageStoreRecieve
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="StoreOutMenuName", Controler="StoreOut",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageStoreOut
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="StoreDeprecateMenuName", Controler="StoreDeprecate",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageStoreDeprecate
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="StoreCountMenuName", Controler="StoreCount",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageStoreCount
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="StoreSettlementMenuName", Controler="StoreSettlement",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageStoreSettlement
                 }

                }

                 },
                new SideMenuItem()
                 {
                   ItemTextLocalizationResource="ReportsMenuName", Controler="",Action="",Data_Ds_Target="reportsInventoryMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,ChaildsMenuItems=new List<SideMenuItem>()
                   {
                     new SideMenuItem()
                  {
                   ItemTextLocalizationResource="ItemCartReport", Controler="Reports",Action="ItemCartReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageItemCartReport
                 },
                    new SideMenuItem()
                 {
                   ItemTextLocalizationResource="OpenBalanceReport", Controler="Reports",Action="OpenBalanceReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageOpenBalanceReport
                 },
                    new SideMenuItem()
                 {
                   ItemTextLocalizationResource="ItemDataReport", Controler="Reports",Action="ItemDataReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageItemDataReport
                 },
                    new SideMenuItem()
                 {
                   ItemTextLocalizationResource="ItemBalanceQuantityAndValueReport", Controler="Reports",Action="ItemBalanceQuantityAndValueReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageItemBalanceQuantityAndValueReport
                 }
                   }

                 },
                }

            },
            #endregion
        
      #region المشتريات
    
            new SideMenuItem(){
                ItemTextLocalizationResource="PurchasesMenuName", Controler="",Action="",Data_Ds_Target="purchasesMenu",Icon="fa fa-shopping-basket",ItemType=SideMenuItemType.MainMenu,
                ChaildsMenuItems=new List<SideMenuItem>()
                {
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="BasicDataMenuName", Controler="",Action="",Data_Ds_Target="basicPurchasesMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,
                  ChaildsMenuItems=new List<SideMenuItem>()
                  {
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="Supplers", Controler="Suppler",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageSuppleres
                 },

                }
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="TransactionsMenuName", Controler="",Action="",Data_Ds_Target="transactionsPurchasesMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,
                    ChaildsMenuItems=new List<SideMenuItem>()
                    {
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="PurchaesInvoiceMenuName", Controler="PurchasesInvoice",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManagePurchasesInvoice
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="PurchasesReturnMenuName", Controler="PurchasesReturn",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManagePurchaseReturn
                 },
                }
                 },
                new SideMenuItem()
                 {
                   ItemTextLocalizationResource="ReportsMenuName", Controler="",Action="",Data_Ds_Target="reportsPurchasesMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,ChaildsMenuItems=new List<SideMenuItem>()
                   {
                     new SideMenuItem()
                  {
                   ItemTextLocalizationResource="ItemPurshasAnaysisReport", Controler="Reports",Action="ItemPurshasAnaysisReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageItemPurshasAnaysisReport
                  },new SideMenuItem()
                  {
                   ItemTextLocalizationResource="SupplierBalanceReport", Controler="Reports",Action="SupplierBalanceReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageSupplierBalanceReport
                  }, new SideMenuItem()
                  {
                   ItemTextLocalizationResource="SupplierBalanceTotalReport", Controler="Reports",Action="SupplierBalanceTotalReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageSupplierBalanceTotalReport
                  },
                   }

                 },
                }

            },
            #endregion

      #region المبيعات
      
            new SideMenuItem(){
                ItemTextLocalizationResource="SalesMenuName", Controler="",Action="",Data_Ds_Target="SalesMenu",Icon="fa fa-shopping-cart",ItemType=SideMenuItemType.MainMenu,
                ChaildsMenuItems=new List<SideMenuItem>()
                {
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="BasicDataMenuName", Controler="",Action="",Data_Ds_Target="basicSalesMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,
                  ChaildsMenuItems=new List<SideMenuItem>()
                  {
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="Customers", Controler="Customer",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageCustomeres
                 },

                }
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="TransactionsMenuName", Controler="",Action="",Data_Ds_Target="transactionsSalesMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,
                    ChaildsMenuItems=new List<SideMenuItem>()
                    {
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="SalesInvoiceMenuName", Controler="SalesInvoice",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageSalesInvoice
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="SalesReturnMenuName", Controler="SalesReturn",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageSalesReturn
                 },
                   new SideMenuItem()
                 {
                 ItemTextLocalizationResource="EGInvoiceMenuName", Controler="EGInvoice",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageEGInvoice
                 }

                }

                 },
                new SideMenuItem()
                 {
                   ItemTextLocalizationResource="ReportsMenuName", Controler="",Action="",Data_Ds_Target="reportsSalesMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,ChaildsMenuItems=new List<SideMenuItem>()
                   {
                   new SideMenuItem()
                  {
                   ItemTextLocalizationResource="ItemSalesAnaysisReport", Controler="Reports",Action="ItemSalesAnaysisReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageItemSalesAnaysisReport
                  },
                    new SideMenuItem()
                  {
                   ItemTextLocalizationResource="CustomerBalanceReport", Controler="Reports",Action="CustomerBalanceReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageCustomerBalanceReport
                  },
                       new SideMenuItem()
                  {
                   ItemTextLocalizationResource="CustomerBalanceTotalReport", Controler="Reports",Action="CustomerBalanceTotalReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageCustomerBalanceTotalReport
                  },






                   }

                 },
                }

            },
            #endregion

      #region الحسابات العامة
 
            new SideMenuItem(){
                ItemTextLocalizationResource="FinanceMenuName", Controler="",Action="",Data_Ds_Target="FinanceMenu",Icon="fa fa-coins",ItemType=SideMenuItemType.MainMenu,
                ChaildsMenuItems=new List<SideMenuItem>()
                {
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="BasicDataMenuName", Controler="",Action="",Data_Ds_Target="basicFinanceMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,
                  ChaildsMenuItems=new List<SideMenuItem>()
                  {
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="DailyAccounts_DefModel", Controler="DailyAccounts_Def",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageDailyAccounts_Def
                 },
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="ChartOfAccountSettings", Controler="ChartOfAccountSettings",Action="Create",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageChartOfAccountSettings
                 },
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="ChartOfAccounts", Controler="ChartOfAccounts",Action="ChartOfAccounts",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageChartOfAccounts
                 },
                   new SideMenuItem()
                 {
                   ItemTextLocalizationResource="DefaultAccounts", Controler="DefaultAccount",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageDefaultAccounts
                 },
                  new SideMenuItem()
                 {
                 ItemTextLocalizationResource="TransactionsEntrySetting", Controler="TransactionsEntrySetting",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageTransactionsEntrySettinges
                 }

                }
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="TransactionsMenuName", Controler="",Action="",Data_Ds_Target="transactionsFinanceMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,
                    ChaildsMenuItems=new List<SideMenuItem>()
                    {
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="DailyEntry", Controler="DailyEntry",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageDailyEntry
                 }, new SideMenuItem()
                 {
                   ItemTextLocalizationResource="DebitSettlementTransaction", Controler="DebitSettlementTransaction",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageDebitSettlementTransaction
                 }, new SideMenuItem()
                 {
                   ItemTextLocalizationResource="CreditSettlementTransaction", Controler="CreditSettlementTransaction",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageCreditSettlementTransaction
                 },
                  new SideMenuItem()
                 {
                 ItemTextLocalizationResource="FinancialPeriodClose", Controler="FinancialPeriodClose",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageFinancialPeriods
                 }

                }

                 },
                new SideMenuItem()
                 {
                   ItemTextLocalizationResource="ReportsMenuName", Controler="",Action="",Data_Ds_Target="reportsFinanceMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,ChaildsMenuItems=new List<SideMenuItem>()
                   {
                   new SideMenuItem()
                  {
                   ItemTextLocalizationResource="AlAstazAccountReport", Controler="Reports",Action="AlAstazAccountReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageItemSalesAnaysisReport
                  },
                  new SideMenuItem()
                  {
                   ItemTextLocalizationResource="ReviewBalanceReport", Controler="Reports",Action="ReviewBalanceReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageItemSalesAnaysisReport
                  },
                  new SideMenuItem()
                  {
                   ItemTextLocalizationResource="DailyAccountsDetailsReport", Controler="Reports",Action="DailyAccountsDetailsReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageItemSalesAnaysisReport
                  },
                  new SideMenuItem()
                  {
                   ItemTextLocalizationResource="DailyAccountsTotalReport", Controler="Reports",Action="DailyAccountsTotalReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageItemSalesAnaysisReport
                  },

                   }

                 },
                }

            },
	#endregion
    
      #region المقبوضات والمدفوعات

               new SideMenuItem(){
                ItemTextLocalizationResource="ReceiptsAndPayments", Controler="",Action="",Data_Ds_Target="ReceiptsAndPayments",Icon="fa fa-comments-dollar",ItemType=SideMenuItemType.MainMenu,
                ChaildsMenuItems=new List<SideMenuItem>()
                {
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="BasicDataMenuName", Controler="",Action="",Data_Ds_Target="basicFinanceMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,
                  ChaildsMenuItems=new List<SideMenuItem>()
                  {                 
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="TreasuryesMenu", Controler="Treasury",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageTreasuryes
                 }, 
                }
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="TransactionsMenuName", Controler="",Action="",Data_Ds_Target="transactionsFinanceMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,
                    ChaildsMenuItems=new List<SideMenuItem>()
                    {                 
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="CashRecieveTransaction", Controler="CashRecieveTransaction",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageCashRecieveTransaction
                 },
                   new SideMenuItem()
                 {
                   ItemTextLocalizationResource="CashExchangeTransaction", Controler="CashExchangeTransaction",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageCashExchangeTransaction
                 },
                   new SideMenuItem()
                 {
                   ItemTextLocalizationResource="WriteCheckOutTransaction", Controler="WriteCheckOutTransaction",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageWriteCheckOutTransaction
                 },
                   new SideMenuItem()
                 {
                   ItemTextLocalizationResource="CheckOutExchangeTransaction", Controler="CheckOutExchangeTransaction",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageCheckOutExchangeTransaction
                 },
                   new SideMenuItem()
                 {
                   ItemTextLocalizationResource="RecieveCheckInTransaction", Controler="RecieveCheckInTransaction",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageRecieveCheckInTransaction
                 },
                   new SideMenuItem()
                 {
                   ItemTextLocalizationResource="CheckInExchangeTransaction", Controler="CheckInExchangeTransaction",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageCheckInExchangeTransaction
                 },
                   new SideMenuItem()
                 {
                   ItemTextLocalizationResource="CheckReturnTransaction", Controler="CheckReturnTransaction",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageCheckReturnTransaction
                 }

                }

                 },
                new SideMenuItem()
                 {
                   ItemTextLocalizationResource="ReportsMenuName", Controler="",Action="",Data_Ds_Target="reportsFinanceMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,ChaildsMenuItems=new List<SideMenuItem>()
                   {
                   new SideMenuItem()
                  {
                   ItemTextLocalizationResource="TreasuryStatementOfAccountReport", Controler="Reports",Action="TreasuryStatementOfAccountReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageItemSalesAnaysisReport
                  },

                   }

                 },
                }

            },
            #endregion

      #region البنوك

               new SideMenuItem(){
                ItemTextLocalizationResource="Banks", Controler="",Action="",Data_Ds_Target="Banks",Icon="fa fa-money-bill-wave",ItemType=SideMenuItemType.MainMenu,
                ChaildsMenuItems=new List<SideMenuItem>()
                {
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="BasicDataMenuName", Controler="",Action="",Data_Ds_Target="basicFinanceMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,
                  ChaildsMenuItems=new List<SideMenuItem>()
                  {                  
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="BanksMenu", Controler="Bank",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageBankes
                 },
                }
                 },
                 new SideMenuItem()
                 {
                   ItemTextLocalizationResource="TransactionsMenuName", Controler="",Action="",Data_Ds_Target="transactionsFinanceMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,
                    ChaildsMenuItems=new List<SideMenuItem>()
                    {
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="CashDepositInBankTransaction", Controler="CashDepositInBankTransaction",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageCashDepositInBankTransaction
                 },
                  new SideMenuItem()
                 {
                   ItemTextLocalizationResource="CashWithdrawalFromBankTransaction", Controler="CashWithdrawalFromBankTransaction",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageCashWithdrawalFromBankTransaction
                 },


                }

                 },
                new SideMenuItem()
                 {
                   ItemTextLocalizationResource="ReportsMenuName", Controler="",Action="",Data_Ds_Target="reportsFinanceMenu",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubMenu,ChaildsMenuItems=new List<SideMenuItem>()
                   {
                   new SideMenuItem()
                  {
                   ItemTextLocalizationResource="BankStatementOfAccountReport", Controler="Reports",Action="BankStatementOfAccountReport",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageItemSalesAnaysisReport
                  },

                   }

                 },
                }

            },
            #endregion
 
      #region الاعدادات
           
            new SideMenuItem(){
                ItemTextLocalizationResource="SettingsMenuName", Controler="",Action="",Data_Ds_Target="SettingsMenu",Icon="ti-settings",ItemType=SideMenuItemType.MainMenu,
                ChaildsMenuItems=new List<SideMenuItem>()
                {
                 new SideMenuItem()
                 {
                 ItemTextLocalizationResource="Company", Controler="Company",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageCompanys
                 },
                  new SideMenuItem()
                 {
                 ItemTextLocalizationResource="Users", Controler="User",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageUsers
                 },
                  new SideMenuItem()
                 {
                 ItemTextLocalizationResource="Roles", Controler="Role",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageRoles
                 },                  
                  new SideMenuItem()
                 {
                 ItemTextLocalizationResource="Permissions", Controler="Permissions",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManagePermissions
                 },
                  new SideMenuItem()
                 {
                 ItemTextLocalizationResource="FinancialPeriods", Controler="FinancialPeriod",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageFinancialPeriods
                 },
                   new SideMenuItem()
                 {
                 ItemTextLocalizationResource="SystemSettinges", Controler="SystemSetting",Action="Index",Data_Ds_Target="",Icon="k-icon k-i-collapse",ItemType=SideMenuItemType.SubSubMenu,Permission=StandardPermissionProvider.ManageSystemSettinges
                 }                 
                }

            },
	#endregion
       


          

        };
    }
    public class SideMenuItem
    {
        public string ItemTextLocalizationResource { get; set; }
        public string Controler { get; set; }
        public string Action { get; set; }
        public string Data_Ds_Target { get; set; }
        public string Icon { get; set; }
        public SideMenuItemType ItemType { get; set; }
        public List<SideMenuItem> ChaildsMenuItems { get; set; }
        public AspNetClaims Permission { get; set; }


    }

    public enum SideMenuItemType
    {
        MainMenu,
        SubMenu,
        SubSubMenu
    }

}
