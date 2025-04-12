using System;
using DevExpress.XtraReports.UI;
using PresentationLayer.Helpers;

namespace Reports.Inventory
{
    public partial class StoreReciveRptPrint
    {
        private static LocalizationService _LocalizationService;

        public StoreReciveRptPrint(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }

        private void StoreReciveRptPrint_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DocumentName.Text = _LocalizationService.GetLocalizedHtmlString("DocumentName").Value;
            DocDate.Text = _LocalizationService.GetLocalizedHtmlString("DocDate").Value;
            DocCode.Text = _LocalizationService.GetLocalizedHtmlString("DocCode").Value;
            StoreName.Text = _LocalizationService.GetLocalizedHtmlString("StoreName").Value;
            BranchName.Text = _LocalizationService.GetLocalizedHtmlString("BranchName").Value;
            TotalLbl.Text = _LocalizationService.GetLocalizedHtmlString("Total").Value;
            ItemCode.Text = _LocalizationService.GetLocalizedHtmlString("ItemCode").Value;
            ItemName.Text = _LocalizationService.GetLocalizedHtmlString("ItemName").Value;
            GroupName.Text = _LocalizationService.GetLocalizedHtmlString("ItemGroup").Value;
            Quntity.Text = _LocalizationService.GetLocalizedHtmlString("Quntity").Value;
            UnitName.Text = _LocalizationService.GetLocalizedHtmlString("DefaultUnit").Value;
    
        }
    }
}
