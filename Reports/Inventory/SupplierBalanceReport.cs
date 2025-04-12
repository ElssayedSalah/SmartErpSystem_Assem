using PresentationLayer.Helpers;

namespace Reports.Inventory
{
    public partial class SupplierBalanceReport
    {
        private static LocalizationService _LocalizationService;

        public SupplierBalanceReport(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }       
       
        private void SupplierBalanceReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            SupplierNameFilter.Text = _LocalizationService.GetLocalizedHtmlString("SupplerName").Value;
            DocumentName.Text = _LocalizationService.GetLocalizedHtmlString("DocumentName").Value;
            TotalLbl.Text = _LocalizationService.GetLocalizedHtmlString("Total").Value;
            DocCode.Text = _LocalizationService.GetLocalizedHtmlString("DocCode").Value;
            DocDate.Text = _LocalizationService.GetLocalizedHtmlString("DocDate").Value;
            Branch.Text = _LocalizationService.GetLocalizedHtmlString("Branch").Value;
            ToDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("ToDate").Value;
            FromDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("FromDate").Value;
            Debit.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            Credit.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
            BalanceDebit.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            BalanceCredit.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
            BalanceLbl.Text = _LocalizationService.GetLocalizedHtmlString("Balance").Value;
            TransactionsLbl.Text = _LocalizationService.GetLocalizedHtmlString("TransactionsMenuName").Value;
        }
    }
}
