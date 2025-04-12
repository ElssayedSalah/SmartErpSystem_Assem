using PresentationLayer.Helpers;

namespace Reports.Inventory
{
    public partial class SalesInvoiceRptPrint
    {
        private static LocalizationService _LocalizationService;

        public SalesInvoiceRptPrint(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }

        private void SalesInvoiceRptPrint_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DocumentName.Text = _LocalizationService.GetLocalizedHtmlString("DocumentName").Value;
            DocDate.Text = _LocalizationService.GetLocalizedHtmlString("DocDate").Value;
            DocCode.Text = _LocalizationService.GetLocalizedHtmlString("DocCode").Value;
            StoreName.Text = _LocalizationService.GetLocalizedHtmlString("StoreName").Value;
            BranchName.Text = _LocalizationService.GetLocalizedHtmlString("BranchName").Value;
            CustomerName.Text = _LocalizationService.GetLocalizedHtmlString("CustomerName").Value;
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
            SalesPrice.Text = _LocalizationService.GetLocalizedHtmlString("SalesPrice").Value;
            DiscountType.Text = _LocalizationService.GetLocalizedHtmlString("DiscountType").Value;
            DiscountValue.Text = _LocalizationService.GetLocalizedHtmlString("DiscountValue").Value;
            PriceAfterDiscount.Text = _LocalizationService.GetLocalizedHtmlString("PriceAfterDiscount").Value;
            
        }
    }
}
