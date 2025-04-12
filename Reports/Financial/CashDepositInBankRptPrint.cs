using System;
using DevExpress.XtraReports.UI;
using PresentationLayer.Helpers;

namespace Reports.Financial
{
    public partial class CashDepositInBankRptPrint
    {       
        private static LocalizationService _LocalizationService;
        public CashDepositInBankRptPrint(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }
       
        private void CashDepositInBankRptPrint_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            AccountName1.Text = _LocalizationService.GetLocalizedHtmlString("Account.Name").Value;
            AccountName2.Text = _LocalizationService.GetLocalizedHtmlString("Account.Name").Value;
            CostCenterName.Text = _LocalizationService.GetLocalizedHtmlString("CoastCenter").Value;
            CurrencyFactor.Text = _LocalizationService.GetLocalizedHtmlString("CurrencyChangrRate").Value;
            Currency.Text = _LocalizationService.GetLocalizedHtmlString("Currency").Value;
            DocumentDate.Text = _LocalizationService.GetLocalizedHtmlString("DocDate").Value;
            DiscountAmount.Text = _LocalizationService.GetLocalizedHtmlString("DiscountAmount").Value;
            DiscountAccount.Text = _LocalizationService.GetLocalizedHtmlString("DiscountAccount").Value;
            Amount.Text = _LocalizationService.GetLocalizedHtmlString("Amount").Value;
            Branch.Text = _LocalizationService.GetLocalizedHtmlString("Branch").Value;
            Name1.Text = _LocalizationService.GetLocalizedHtmlString("Name").Value;
            Name2.Text = _LocalizationService.GetLocalizedHtmlString("Name").Value;
            FirstType.Text = _LocalizationService.GetLocalizedHtmlString("FirstSide").Value;
            SecondType.Text = _LocalizationService.GetLocalizedHtmlString("SecondSide").Value;
            EntryNumber.Text = _LocalizationService.GetLocalizedHtmlString("EntryNumber").Value;
            DocNumber.Text = _LocalizationService.GetLocalizedHtmlString("DocCode").Value;
            Type1.Text = _LocalizationService.GetLocalizedHtmlString("Type").Value;
            Type2.Text = _LocalizationService.GetLocalizedHtmlString("Type").Value;
            Notes.Text = _LocalizationService.GetLocalizedHtmlString("Notes").Value;
        }
    }
}
