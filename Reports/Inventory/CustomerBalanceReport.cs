using PresentationLayer.Helpers;

namespace Reports.Inventory
{
    public partial class CustomerBalanceReport
    {
        private static LocalizationService _LocalizationService;

        public CustomerBalanceReport(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }       
      
        private void CustomerBalanceReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            CustomerNameFilter.Text = _LocalizationService.GetLocalizedHtmlString("CustomerName").Value;
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
            RowNumber.Text = _LocalizationService.GetLocalizedHtmlString("RowNumber").Value;
        }
    }
}
