using PresentationLayer.Helpers;

namespace Reports.Inventory
{
    public partial class CustomerBalanceTotalReport
    {
        private static LocalizationService _LocalizationService;

        public CustomerBalanceTotalReport(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }  
        private void CustomerBalanceTotalReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            CustomerNameFilter.Text = _LocalizationService.GetLocalizedHtmlString("CustomerName").Value;
            BranchNameFilter.Text = _LocalizationService.GetLocalizedHtmlString("BranchName").Value;
            Serial.Text = _LocalizationService.GetLocalizedHtmlString("Serial").Value;
            TotalLbl.Text = _LocalizationService.GetLocalizedHtmlString("Total").Value;
            CustomerCode.Text = _LocalizationService.GetLocalizedHtmlString("CustomerCode").Value;
            CustomerName.Text = _LocalizationService.GetLocalizedHtmlString("CustomerName").Value;
            ToDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("ToDate").Value;
            FromDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("FromDate").Value;
            Debit.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            Credit.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
            BalanceDebit.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            BalanceCredit.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
            BalanceLbl.Text = _LocalizationService.GetLocalizedHtmlString("Balance").Value;
            TransactionsLbl.Text = _LocalizationService.GetLocalizedHtmlString("TransactionsMenuName").Value;
            OpenBalanceLbl.Text = _LocalizationService.GetLocalizedHtmlString("OpenBalanceMenuName").Value;
            OpenBalanceDebit.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            OpenBalanceCredit.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
        }
    }
}
