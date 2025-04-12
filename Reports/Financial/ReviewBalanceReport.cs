using BusinessLayer.Models.Reports.Finance;
using PresentationLayer.Helpers;
using System.Linq;

namespace Reports.Financial
{
    public partial class ReviewBalanceReport
    {
        private static LocalizationService _LocalizationService;

        public ReviewBalanceReport(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }

        private void ReviewBalanceReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            AccountHedarFilter.Text = _LocalizationService.GetLocalizedHtmlString("Account").Value;           
            TotalLbl.Text = _LocalizationService.GetLocalizedHtmlString("Total").Value;
            detailsAccountName.Text = _LocalizationService.GetLocalizedHtmlString("AccountName").Value;
            detailsAccountCode.Text = _LocalizationService.GetLocalizedHtmlString("AccountCode").Value;
            ToDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("ToDate").Value;
            FromDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("FromDate").Value;
            beforeDebitLbl.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            beforeCredit.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
            inDebit.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            outCredit.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
            inPeriodLbl.Text = _LocalizationService.GetLocalizedHtmlString("inPeriod").Value;
            beforeLbl.Text = _LocalizationService.GetLocalizedHtmlString("before").Value;
            firstPeriodCreditLbl.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
            detailsAccountLbl.Text = _LocalizationService.GetLocalizedHtmlString("Account").Value;
            firstPeriodDebitLbl.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            firstPeriodLbl.Text = _LocalizationService.GetLocalizedHtmlString("FirstPeriod").Value;
            balanceLbl.Text = _LocalizationService.GetLocalizedHtmlString("Balance").Value;
            balanceDebitLbl.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            balanceCreditLbl.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
        }

    }
}
