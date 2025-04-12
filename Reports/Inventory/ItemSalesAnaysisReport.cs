using PresentationLayer.Helpers;

namespace Reports.Inventory
{
    public partial class ItemSalesAnaysisReport
    {
        private static LocalizationService _LocalizationService;

        public ItemSalesAnaysisReport(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }
       
        private void ItemDataReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupFilterName.Text = _LocalizationService.GetLocalizedHtmlString("ItemGroup").Value;
            ItemNameFilter.Text = _LocalizationService.GetLocalizedHtmlString("ItemName").Value;
            CustomerNameFilter.Text = _LocalizationService.GetLocalizedHtmlString("CustomerName").Value;
            BranchNameLbl.Text = _LocalizationService.GetLocalizedHtmlString("BranchName").Value;
            TotalLbl.Text = _LocalizationService.GetLocalizedHtmlString("Total").Value;
            CustomerName.Text = _LocalizationService.GetLocalizedHtmlString("CustomerName").Value;
            ItemName.Text = _LocalizationService.GetLocalizedHtmlString("ItemName").Value;
            QuntityIn.Text = _LocalizationService.GetLocalizedHtmlString("Quntity").Value;
            QuntityReturn.Text = _LocalizationService.GetLocalizedHtmlString("Quntity").Value;
            QuntityNet.Text = _LocalizationService.GetLocalizedHtmlString("Quntity").Value;
            ToDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("ToDate").Value;
            FromDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("FromDate").Value;
            ValueIn.Text = _LocalizationService.GetLocalizedHtmlString("Value").Value;
            ValueReturn.Text = _LocalizationService.GetLocalizedHtmlString("Value").Value;
            ValueNet.Text = _LocalizationService.GetLocalizedHtmlString("Value").Value;
            SalesLbl.Text = _LocalizationService.GetLocalizedHtmlString("SalesMenuName").Value;
            SalesReturnLbl.Text = _LocalizationService.GetLocalizedHtmlString("SalesReturnMenuName").Value;
            SalesNetLbl.Text = _LocalizationService.GetLocalizedHtmlString("SalesNet").Value;
            
        }
    }
}
