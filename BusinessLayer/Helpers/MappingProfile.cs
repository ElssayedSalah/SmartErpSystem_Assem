
using AutoMapper;
using DataAccessLayer.Entities.Inventory;
using BusinessLayer.Models.Inventory;
using BusinessLayer.Models.Financial;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.System;
using BusinessLayer.Models.System;
using DataAccessLayer.Entities.Identity;
using BusinessLayer.Models.Identity;
using Microsoft.AspNetCore.Identity;
using BusinessLayer.Models.Sales;
using DataAccessLayer.Entities.Sales;
using DataAccessLayer.Entities.Purchases;
using BusinessLayer.Models.Purchases;

namespace BusinessLayer.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Branch, BranchModel>().ReverseMap();
            CreateMap<Store, StoreModel>().ReverseMap();
            CreateMap<Unit, UnitModel>().ReverseMap();
            CreateMap<Department, DepartmentModel>().ReverseMap();
            CreateMap<ItemGroup, ItemGroupModel>().ReverseMap();
            CreateMap<Item, ItemModel>().ReverseMap();
            CreateMap<Transaction_InvMaster, Transaction_InvMasterModel>().ReverseMap();
            CreateMap<Transaction_InvDetails, Transaction_InvDetailsModel>().ReverseMap();
            CreateMap<Document, DocumentModel>().ReverseMap();
            CreateMap<FinancialPeriod, FinancialPeriodModel>().ReverseMap();
            CreateMap<Company, CompanyMobel>().ReverseMap();
            CreateMap<ApplicationUser, UserModel>().ReverseMap();
            CreateMap<ApplicationRole, RoleModel>().ReverseMap();
            CreateMap<CustomerModel, Customer>().ReverseMap();
            CreateMap<CurrencyModel, Currency>().ReverseMap();
            CreateMap<SupplerModel, Suppler>().ReverseMap();
            CreateMap<CountriesModel, Countries>().ReverseMap();
            CreateMap<SystemSettingModel, SystemSetting>().ReverseMap();
            CreateMap<DailyAccounts_DefModel, DailyAccounts_Def>().ReverseMap();
            CreateMap<BankModel, Bank>().ReverseMap();
            CreateMap<TreasuryModel, Treasury>().ReverseMap();
            CreateMap<ChartOfAccountSettingsModel, ChartOfAccountSettings>().ReverseMap();
            CreateMap<AccountSettingModel, AccountSetting>().ReverseMap();
            CreateMap<AccountModel, Account>().ReverseMap();
            CreateMap<DefaultAccountModel, DefaultAccount>().ReverseMap();
            CreateMap<DailyEntryMasterModel, DailyEntryMaster>().ReverseMap();
            CreateMap<DailyEntryDetailsModel, DailyEntryDetails>().ReverseMap();
            CreateMap<TransactionsEntrySettingMasterModel, TransactionsEntrySettingMaster>().ReverseMap();
            CreateMap<TransactionsEntrySettingDetailsModel, TransactionsEntrySettingDetails>().ReverseMap();
            CreateMap<CashTransactionModel, CashTransaction>().ReverseMap();
            CreateMap<CheckTransactionModel, CheckTransaction>().ReverseMap();

        }

    }
}
