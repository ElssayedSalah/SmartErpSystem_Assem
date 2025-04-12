using System;
using DevExpress.XtraReports.UI;
using PresentationLayer.Helpers;

namespace Reports.Inventory
{
    public partial class SalesReturnRptPrint
    {
        private static LocalizationService _LocalizationService;

        public SalesReturnRptPrint(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }

        private void SalesReturnRptPrint_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DocumentName.Text = _LocalizationService.GetLocalizedHtmlString("DocumentName").Value;
            DocDate.Text = _LocalizationService.GetLocalizedHtmlString("DocDate").Value;
            DocCode.Text = _LocalizationService.GetLocalizedHtmlString("DocCode").Value;
            StoreName.Text = _LocalizationService.GetLocalizedHtmlString("StoreName").Value;
            BranchName.Text = _LocalizationService.GetLocalizedHtmlString("BranchName").Value;
            CustomerName.Text = _LocalizationService.GetLocalizedHtmlString("CustomerName").Value;
            InvoiceValue.Text = _LocalizationService.GetLocalizedHtmlString("Total").Value;            
            TaxValue.Text = _LocalizationService.GetLocalizedHtmlString("TaxValue").Value;
            InvoiceNet.Text = _LocalizationService.GetLocalizedHtmlString("Net").Value;
            ItemCode.Text = _LocalizationService.GetLocalizedHtmlString("ItemCode").Value;
            ItemName.Text = _LocalizationService.GetLocalizedHtmlString("ItemName").Value;
            GroupName.Text = _LocalizationService.GetLocalizedHtmlString("ItemGroup").Value;
            Quntity.Text = _LocalizationService.GetLocalizedHtmlString("Quntity").Value;
            UnitName.Text = _LocalizationService.GetLocalizedHtmlString("DefaultUnit").Value;
            SalesPrice.Text = _LocalizationService.GetLocalizedHtmlString("SalesPrice").Value;
        }
    }
}
