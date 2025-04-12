using System;
using DevExpress.XtraReports.UI;
using PresentationLayer.Helpers;

namespace Reports.Inventory
{
    public partial class StoreSettlementRptPrint
    {
        private static LocalizationService _LocalizationService;

        public StoreSettlementRptPrint(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }       
      
        private void StoreSettlementRptPrint_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
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
            CountDateFrom.Text = _LocalizationService.GetLocalizedHtmlString("CountDateFrom").Value;
            CountDateTo.Text = _LocalizationService.GetLocalizedHtmlString("CountDateTo").Value;
            StoreCountNumber.Text = _LocalizationService.GetLocalizedHtmlString("StoreCountNumber").Value;
            EntryNumber.Text = _LocalizationService.GetLocalizedHtmlString("EntryNumber").Value;
            StoreSurplusValue.Text = _LocalizationService.GetLocalizedHtmlString("StoreSurplusValue").Value;
            StoreDeficitValue.Text = _LocalizationService.GetLocalizedHtmlString("StoreDeficitValue").Value;
            Total.Text = _LocalizationService.GetLocalizedHtmlString("TotalCoast").Value;
            AvgPurchasePrice.Text = _LocalizationService.GetLocalizedHtmlString("UnitCoast").Value;
            DifferenceQuntity.Text = _LocalizationService.GetLocalizedHtmlString("DifferenceQuntity").Value;
            ActualQuntity.Text = _LocalizationService.GetLocalizedHtmlString("ActualQuntity").Value;



        }
    }
}
