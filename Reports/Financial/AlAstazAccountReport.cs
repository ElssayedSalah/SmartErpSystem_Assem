using BusinessLayer.Models.Reports.Finance;
using PresentationLayer.Helpers;
using System.Linq;

namespace Reports.Financial
{
    public partial class AlAstazAccountReport
    {
        private static LocalizationService _LocalizationService;

        public AlAstazAccountReport(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }         
        private void AlAstazAccountReport_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            AccountHedarFilter.Text = _LocalizationService.GetLocalizedHtmlString("Account").Value;
            detailsEntryNumber.Text = _LocalizationService.GetLocalizedHtmlString("EntryNumber").Value;
            Serial.Text = _LocalizationService.GetLocalizedHtmlString("Serial").Value;
            TotalLbl.Text = _LocalizationService.GetLocalizedHtmlString("Total").Value;
            detailsEntryDate.Text = _LocalizationService.GetLocalizedHtmlString("DocDate").Value;
            detailsDocumentNumber.Text = _LocalizationService.GetLocalizedHtmlString("DocCode").Value;
            ToDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("ToDate").Value;
            FromDateLbl.Text = _LocalizationService.GetLocalizedHtmlString("FromDate").Value;
            Debit.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            Credit.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
            BalanceDebit.Text = _LocalizationService.GetLocalizedHtmlString("Debit").Value;
            BalanceCredit.Text = _LocalizationService.GetLocalizedHtmlString("Credit").Value;
            BalanceLbl.Text = _LocalizationService.GetLocalizedHtmlString("Balance").Value;
            TransactionsLbl.Text = _LocalizationService.GetLocalizedHtmlString("TransactionsMenuName").Value;
            detailsDocumentName.Text = _LocalizationService.GetLocalizedHtmlString("DocumentName").Value;
            detailsNotes.Text = _LocalizationService.GetLocalizedHtmlString("Notes").Value;
           
            var datasource = this.DataSource as System.Collections.Generic.List<AlAstazAccountReportDs>;
            var b = datasource.Sum(x => x.Debit) - datasource.Sum(x => x.Credit);
            if (b > 0)
            {
                tableCell1.Text = b.ToString();
                tableCell14.Text = (0).ToString();
            }
            else if (b < 0)
            {
                tableCell1.Text = (0).ToString();
                tableCell14.Text = b.ToString();

            }
            else
            {
                tableCell1.Text = (0).ToString();
                tableCell14.Text = (0).ToString();
            }
        }
    }
}
