using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class Add_AlAstazAccountReportView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE VIEW AlAstazAccountReportView AS SELECT Financial.DailyEntryDetails.AccountId, Financial.Accounts.AccountCode, Financial.Accounts.NameAr as AccountNameAr , ISNULL( Financial.Accounts.NameEn,'') as AccountNameEn, Financial.DailyEntryDetails.Debit, Financial.DailyEntryDetails.Credit,Financial.DailyEntryMaster.Id as EntryId, Financial.DailyEntryMaster.EntryNumber,Financial.DailyEntryMaster.EntryType, Financial.DailyEntryMaster.DailyTypeId, Financial.DailyEntryMaster.EntryCreationMethod,Financial.DailyEntryMaster.CurrencyId, Financial.DailyEntryMaster.CurrencyFactor, Financial.DailyEntryMaster.IsTransfered, Financial.DailyEntryMaster.EntryState, Financial.DailyEntryMaster.DocType,                   Financial.DailyEntryMaster.DocNumber, Financial.DailyEntryMaster.FinancialPeriodId, Financial.DailyEntryMaster.CompanyId,ISNULL(Financial.DailyEntryDetails.CostCenterId,0) as CostCenterId, Financial.DailyEntryMaster.TransactionDate,Financial.DailyEntryMaster.CreationDate, Financial.DailyEntryMaster.UpdatedDate, ISNULL(Financial.DailyEntryMaster.Notes,'') as MasterNotes, ISNULL(Financial.DailyEntryDetails.Notes,'') AS DetailsNotes,convert(decimal,0) as BalanceDebit,convert(decimal,0) as BalanceCredit FROM Financial.DailyEntryDetails INNER JOIN Financial.DailyEntryMaster ON Financial.DailyEntryDetails.MasterId = Financial.DailyEntryMaster.Id INNER JOIN Financial.Accounts ON Financial.DailyEntryDetails.AccountId = Financial.Accounts.Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW AlAstazAccountReportView");

        }
    }
}
