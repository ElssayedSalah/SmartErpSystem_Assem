using System;
using DevExpress.XtraReports.UI;
using PresentationLayer.Helpers;

namespace Reports.Financial
{
    public partial class DailyEntryRptPrint
    {       
        private static LocalizationService _LocalizationService;
        public DailyEntryRptPrint(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }

        private void DailyEntryRptPrint_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            AccountName.Text = _LocalizationService.GetLocalizedHtmlString("AccountName").Value;
            AccountCode.Text = _LocalizationService.GetLocalizedHtmlString("AccountCode").Value;
            CostCenterCode.Text = _LocalizationService.GetLocalizedHtmlString("CostCenterCode").Value;
            CostCenterName.Text = _LocalizationService.GetLocalizedHtmlString("CoastCenter").Value;
            Currency.Text = _LocalizationService.GetLocalizedHtmlString("Currency").Value;
            DocumentDate.Text = _LocalizationService.GetLocalizedHtmlString("DocDate").Value;
            BalanceState.Text = _LocalizationService.GetLocalizedHtmlString("EntryBalanceState").Value;
            EntryCreationMethod.Text = _LocalizationService.GetLocalizedHtmlString("EntryCreationMethod").Value;
            TransferState.Text = _LocalizationService.GetLocalizedHtmlString("TransferState").Value;
            DailyType.Text = _LocalizationService.GetLocalizedHtmlString("DailyAccounts_DefModel").Value;
            DocumentName.Text = _LocalizationService.GetLocalizedHtmlString("DocType").Value;
            EntryNumber.Text = _LocalizationService.GetLocalizedHtmlString("EntryNumber").Value;
            Serial.Text = _LocalizationService.GetLocalizedHtmlString("Serial").Value;
            TotalLbl.Text = _LocalizationService.GetLocalizedHtmlString("Total").Value;
            DocNumber.Text = _LocalizationService.GetLocalizedHtmlString("DocCode").Value;
            Debit.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            Credit.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
            Notes.Text = _LocalizationService.GetLocalizedHtmlString("Notes").Value;
            dNotes.Text = _LocalizationService.GetLocalizedHtmlString("Notes").Value;
        }
    }
}
