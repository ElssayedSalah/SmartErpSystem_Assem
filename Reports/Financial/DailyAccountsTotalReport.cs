using PresentationLayer.Helpers;

namespace Reports.Financial
{
    public partial class DailyAccountsTotalReport
    {
        private static LocalizationService _LocalizationService;

        public DailyAccountsTotalReport(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }
      
        private void DailyAccountsTotalReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            AccountHedarFilter.Text = _LocalizationService.GetLocalizedHtmlString("Account").Value;
            DailyAccountsLbl.Text = _LocalizationService.GetLocalizedHtmlString("DailyEntry").Value;
            CoastCenterLbl.Text = _LocalizationService.GetLocalizedHtmlString("CoastCenter").Value;
            Serial.Text = _LocalizationService.GetLocalizedHtmlString("Serial").Value;
            TotalLbl.Text = _LocalizationService.GetLocalizedHtmlString("Total").Value;
            detailsAccount.Text = _LocalizationService.GetLocalizedHtmlString("Account").Value;
            ToDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("ToDate").Value;
            FromDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("FromDate").Value;
            transDebit.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            BalanceDebit.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            openBalanceDebit.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            BalanceCredit.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
            transCredit.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
            openBalanceCredit.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
            detailsNotes.Text = _LocalizationService.GetLocalizedHtmlString("Notes").Value;
            firstPeriodLbl.Text = _LocalizationService.GetLocalizedHtmlString("FirstPeriod").Value;
            balanceLbl.Text = _LocalizationService.GetLocalizedHtmlString("Balance").Value;
            transactionLbl.Text = _LocalizationService.GetLocalizedHtmlString("TransactionsMenuName").Value;
        }


    }
}
