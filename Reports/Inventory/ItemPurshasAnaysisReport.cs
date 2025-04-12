using PresentationLayer.Helpers;

namespace Reports.Inventory
{
    public partial class ItemPurshasAnaysisReport
    {
        private static LocalizationService _LocalizationService;

        public ItemPurshasAnaysisReport(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }
       
        private void ItemDataReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GroupFilterName.Text = _LocalizationService.GetLocalizedHtmlString("ItemGroup").Value;
            ItemNameFilter.Text = _LocalizationService.GetLocalizedHtmlString("ItemName").Value;
            SupplierNameFilter.Text = _LocalizationService.GetLocalizedHtmlString("SupplerName").Value;
            BranchNameLbl.Text = _LocalizationService.GetLocalizedHtmlString("BranchName").Value;
            TotalLbl.Text = _LocalizationService.GetLocalizedHtmlString("Total").Value;
            SupplierName.Text = _LocalizationService.GetLocalizedHtmlString("SupplerName").Value;
            ItemName.Text = _LocalizationService.GetLocalizedHtmlString("ItemName").Value;
            QuntityIn.Text = _LocalizationService.GetLocalizedHtmlString("Quntity").Value;
            QuntityReturn.Text = _LocalizationService.GetLocalizedHtmlString("Quntity").Value;
            QuntityNet.Text = _LocalizationService.GetLocalizedHtmlString("Quntity").Value;
            ToDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("ToDate").Value;
            FromDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("FromDate").Value;
            ValueIn.Text = _LocalizationService.GetLocalizedHtmlString("Value").Value;
            ValueReturn.Text = _LocalizationService.GetLocalizedHtmlString("Value").Value;
            ValueNet.Text = _LocalizationService.GetLocalizedHtmlString("Value").Value;
            PurshasLbl.Text = _LocalizationService.GetLocalizedHtmlString("PurchasesMenuName").Value;
            PurchasesReturnLbl.Text = _LocalizationService.GetLocalizedHtmlString("PurchasesReturnMenuName").Value;
            PurchasesNetLbl.Text = _LocalizationService.GetLocalizedHtmlString("PurchasesNet").Value;
            
        }
    }
}
