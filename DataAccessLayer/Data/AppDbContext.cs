using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Identity;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Entities.Sales;
using DataAccessLayer.Entities.Purchases;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim, IdentityUserToken<string>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        //dont forget to add tables in Schema
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<ItemGroup> ItemGroups { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Transaction_InvMaster> Transaction_InvMaster { get; set; }
        public DbSet<Transaction_InvDetails> Transaction_InvDetails { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<FinancialPeriod> FinancialPeriods { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<AspNetRoleClaimActions> AspNetRoleClaimActions { get; set; }
        public DbSet<AspNetClaims> AspNetClaims { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Currency> Currencys { get; set; }
        public DbSet<Suppler> Supplers { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<CustomerOpenBalance> CustomerOpenBalance { get; set; }
        public DbSet<SupplerOpenBalance> SupplerOpenBalance { get; set; }
        public DbSet<Taxes> Taxs { get; set; }
        public DbSet<DailyAccounts_Def> DailyAccounts_Def { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankOpenBalance> BankOpenBalance { get; set; }
        public DbSet<Treasury> Treasurys { get; set; }
        public DbSet<TreasuryOpenBalance> TreasuryOpenBalance { get; set; }
        public DbSet<ChartOfAccountSettings> ChartOfAccountSettings { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountOpenBalance> AccountOpenBalance { get; set; }
        public DbSet<AccountSetting> AccountSetting { get; set; }
        public DbSet<DefaultAccount> DefaultAccounts { get; set; }
        public DbSet<DailyEntryMaster> DailyEntryMaster { get; set; }
        public DbSet<DailyEntryDetails> DailyEntryDetails { get; set; }
        public DbSet<TransactionsEntrySettingMaster> TransactionsEntrySettingMaster { get; set; }
        public DbSet<TransactionsEntrySettingDetails> TransactionsEntrySettingDetails { get; set; }
        public DbSet<CashTransaction> CashTransaction { get; set; }
        public DbSet<CheckTransaction> CheckTransaction { get; set; }
        public DbSet<ItemOpenBalance> ItemOpenBalance { get; set; }







        //views
        public DbSet<OpenBalanceReportView> OpenBalanceReportView { get; set; }
        public DbSet<ItemDataReportView> ItemDataReportView { get; set; }
        public DbSet<ItemBalanceQuantityAndValueReportView> ItemBalanceQuantityAndValueReportView { get; set; }
        public DbSet<ItemCartReportView> ItemCartReportView { get; set; }
        public DbSet<ItemPurshasAnaysisReportView> ItemPurshasAnaysisReportView { get; set; }
        public DbSet<ItemSalesAnaysisReportView> ItemSalesAnaysisReportView { get; set; }
        public DbSet<AlAstazAccountReportView> AlAstazAccountReportView { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //ignore views mapping
            //modelBuilder.Ignore<OpenBalanceReportView>();

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Transaction_InvDetails>()
                .HasOne(d => d.Transaction_InvMaster)
                .WithMany(m => m.Transaction_InvDetails)
                .HasForeignKey(d => d.MasterId)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");



            modelBuilder.Entity<OpenBalanceReportView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("OpenBalanceReportView");
            }); 
            modelBuilder.Entity<ItemDataReportView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("ItemDataReportView");
            });
            modelBuilder.Entity<ItemBalanceQuantityAndValueReportView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("ItemBalanceQuantityAndValueReportView");
            });
            modelBuilder.Entity<ItemCartReportView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("ItemCartReportView");
            });
           modelBuilder.Entity<ItemPurshasAnaysisReportView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("ItemPurshasAnaysisReportView");
            });
            modelBuilder.Entity<ItemSalesAnaysisReportView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("ItemSalesAnaysisReportView");
            });
            modelBuilder.Entity<AlAstazAccountReportView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("AlAstazAccountReportView");
            });

        }






    }
}
