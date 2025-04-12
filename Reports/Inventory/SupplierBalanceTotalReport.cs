using PresentationLayer.Helpers;

namespace Reports.Inventory
{
    public partial class SupplierBalanceTotalReport
    {
        private static LocalizationService _LocalizationService;

        public SupplierBalanceTotalReport(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }
      
        private void SupplierBalanceTotalReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            SupplierNameFilter.Text = _LocalizationService.GetLocalizedHtmlString("SupplerName").Value;
            BranchNameFilter.Text = _LocalizationService.GetLocalizedHtmlString("BranchName").Value;
            Serial.Text = _LocalizationService.GetLocalizedHtmlString("Serial").Value;
            TotalLbl.Text = _LocalizationService.GetLocalizedHtmlString("Total").Value;
            SupplierCode.Text = _LocalizationService.GetLocalizedHtmlString("SupplierCode").Value;
            SupplierName.Text = _LocalizationService.GetLocalizedHtmlString("SupplierName").Value;
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
