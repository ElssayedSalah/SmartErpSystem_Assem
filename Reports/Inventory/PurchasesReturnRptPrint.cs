using PresentationLayer.Helpers;

namespace Reports.Inventory
{
    public partial class PurchasesReturnRptPrint
    {
        private static LocalizationService _LocalizationService;

        public PurchasesReturnRptPrint(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }
        private void PurchasesReturnRptPrint_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DocumentName.Text = _LocalizationService.GetLocalizedHtmlString("DocumentName").Value;
            DocDate.Text = _LocalizationService.GetLocalizedHtmlString("DocDate").Value;
            DocCode.Text = _LocalizationService.GetLocalizedHtmlString("DocCode").Value;
            StoreName.Text = _LocalizationService.GetLocalizedHtmlString("StoreName").Value;
            BranchName.Text = _LocalizationService.GetLocalizedHtmlString("BranchName").Value;
            SupplerName.Text = _LocalizationService.GetLocalizedHtmlString("SupplerName").Value;
            InvoiceValue.Text = _LocalizationService.GetLocalizedHtmlString("InvoiceValue").Value;
            TotalAdditions.Text = _LocalizationService.GetLocalizedHtmlString("TotalAdditions").Value;
            TotalDisounts.Text = _LocalizationService.GetLocalizedHtmlString("TotalDisounts").Value;
            InvoiceTotal.Text = _LocalizationService.GetLocalizedHtmlString("InvoiceTotal").Value;
            TaxValue.Text = _LocalizationService.GetLocalizedHtmlString("TaxValue").Value;
            InvoiceNet.Text = _LocalizationService.GetLocalizedHtmlString("InvoiceNet").Value;
            ItemCode.Text = _LocalizationService.GetLocalizedHtmlString("ItemCode").Value;
            ItemName.Text = _LocalizationService.GetLocalizedHtmlString("ItemName").Value;
            GroupName.Text = _LocalizationService.GetLocalizedHtmlString("ItemGroup").Value;
            Quntity.Text = _LocalizationService.GetLocalizedHtmlString("Quntity").Value;
            UnitName.Text = _LocalizationService.GetLocalizedHtmlString("DefaultUnit").Value;
            PurshasesPrice.Text = _LocalizationService.GetLocalizedHtmlString("PurchasePrice").Value;
            Total.Text = _LocalizationService.GetLocalizedHtmlString("Total").Value;
        }
    }
}
