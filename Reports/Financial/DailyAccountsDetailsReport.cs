using System;
using DevExpress.XtraReports.UI;
using PresentationLayer.Helpers;

namespace Reports.Financial
{
    public partial class DailyAccountsDetailsReport
    {
        private static LocalizationService _LocalizationService;

        public DailyAccountsDetailsReport(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }

        private void DailyAccountsDetailsReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            AccountHedarFilter.Text = _LocalizationService.GetLocalizedHtmlString("Account").Value;
            DailyAccountsLbl.Text = _LocalizationService.GetLocalizedHtmlString("DailyEntry").Value;
            DocumentTypeLbl.Text = _LocalizationService.GetLocalizedHtmlString("DocType").Value;
            CoastCenterLbl.Text = _LocalizationService.GetLocalizedHtmlString("CoastCenter").Value;
            detailsEntryNumber.Text = _LocalizationService.GetLocalizedHtmlString("EntryNumber").Value;
            Serial.Text = _LocalizationService.GetLocalizedHtmlString("Serial").Value;
            TotalLbl.Text = _LocalizationService.GetLocalizedHtmlString("Total").Value;
            detailsDocumentNumber.Text = _LocalizationService.GetLocalizedHtmlString("DocCode").Value;
            detailsAccount.Text = _LocalizationService.GetLocalizedHtmlString("Account").Value;
            detailsCoastCenter.Text = _LocalizationService.GetLocalizedHtmlString("CoastCenter").Value;
            ToDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("ToDate").Value;
            FromDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("FromDate").Value;
            BalanceDebit.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            BalanceCredit.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
            detailsDocumentName.Text = _LocalizationService.GetLocalizedHtmlString("DocType").Value;
            detailsNotes.Text = _LocalizationService.GetLocalizedHtmlString("Notes").Value;

        }
    }
}
