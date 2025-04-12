using PresentationLayer.Helpers;

namespace Reports.Inventory
{
    public partial class ItemCartReport
    {
        private static LocalizationService _LocalizationService;

        public ItemCartReport(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }
       
        private void ItemDataReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            HeaderGroupName.Text = _LocalizationService.GetLocalizedHtmlString("ItemGroup").Value;
            HeaderItemName.Text = _LocalizationService.GetLocalizedHtmlString("ItemName").Value;
            HeaderStoreName.Text = _LocalizationService.GetLocalizedHtmlString("StoreName").Value;
            HeaderBranchName.Text = _LocalizationService.GetLocalizedHtmlString("BranchName").Value;
            DocumentName.Text = _LocalizationService.GetLocalizedHtmlString("DocumentName").Value;
            TotalLbl.Text = _LocalizationService.GetLocalizedHtmlString("Total").Value;
            DocCode.Text = _LocalizationService.GetLocalizedHtmlString("DocCode").Value;
            DocDate.Text = _LocalizationService.GetLocalizedHtmlString("DocDate").Value;
            QuntityIn.Text = _LocalizationService.GetLocalizedHtmlString("Quntity").Value;
            QuntityOut.Text = _LocalizationService.GetLocalizedHtmlString("Quntity").Value;           
            QuntityCurrunt.Text = _LocalizationService.GetLocalizedHtmlString("Quntity").Value;
            ToDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("ToDate").Value;
            FromDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("FromDate").Value;
            UnitIn.Text = _LocalizationService.GetLocalizedHtmlString("DefaultUnit").Value;
            UnitOut.Text = _LocalizationService.GetLocalizedHtmlString("DefaultUnit").Value;
            UnitCurrunt.Text = _LocalizationService.GetLocalizedHtmlString("DefaultUnit").Value;
            PurchasePrice.Text = _LocalizationService.GetLocalizedHtmlString("PurchasePrice").Value;
            AvgPurchasePrice.Text = _LocalizationService.GetLocalizedHtmlString("AvgPurchasePrice").Value;
            DocumentValue.Text = _LocalizationService.GetLocalizedHtmlString("DocumentValue").Value;
            BalanceValue.Text = _LocalizationService.GetLocalizedHtmlString("BalanceValue").Value;
            InQuntity.Text = _LocalizationService.GetLocalizedHtmlString("InQuntity").Value;
            OutQuntity.Text = _LocalizationService.GetLocalizedHtmlString("OutQuntity").Value;
            CurruntQuntity.Text = _LocalizationService.GetLocalizedHtmlString("CurruntQuntity").Value;
        }
    }
}
