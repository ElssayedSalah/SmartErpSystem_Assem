using PresentationLayer.Helpers;

namespace Reports.Inventory
{
    public partial class ItemBalanceQuantityAndValueReport
    {
        private static LocalizationService _LocalizationService;

        public ItemBalanceQuantityAndValueReport(LocalizationService localizationService)
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
            TotalLbl.Text = _LocalizationService.GetLocalizedHtmlString("Total").Value;
            ItemCode.Text = _LocalizationService.GetLocalizedHtmlString("ItemCode").Value;
            ItemName.Text = _LocalizationService.GetLocalizedHtmlString("ItemName").Value;
            GroupName.Text = _LocalizationService.GetLocalizedHtmlString("ItemGroup").Value;
            CurrentBalance.Text = _LocalizationService.GetLocalizedHtmlString("CurrentBalance").Value;
            UnitName.Text = _LocalizationService.GetLocalizedHtmlString("DefaultUnit").Value;
            StoreName.Text = _LocalizationService.GetLocalizedHtmlString("StoreName").Value;
            BranchName.Text = _LocalizationService.GetLocalizedHtmlString("BranchName").Value;
            ToDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("ToDate").Value;
            CoastLbl.Text = _LocalizationService.GetLocalizedHtmlString("Coast").Value;
            AvgPurchasePriceLbl.Text = _LocalizationService.GetLocalizedHtmlString("AvgPurchasePrice").Value;
        }
    }
}
