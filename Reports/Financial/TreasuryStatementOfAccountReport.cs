using BusinessLayer.Models.Reports.Finance;
using PresentationLayer.Helpers;
using System.Linq;

namespace Reports.Financial
{
    public partial class TreasuryStatementOfAccountReport
    {
        private static LocalizationService _LocalizationService;

        public TreasuryStatementOfAccountReport(LocalizationService localizationService)
        {
            InitializeComponent();
            _LocalizationService = localizationService;

        }         
        private void TreasuryStatementOfAccountReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            TreasuryHedarFilter.Text = _LocalizationService.GetLocalizedHtmlString("TheTreasury").Value;
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
            detailsDocumentName.Text = _LocalizationService.GetLocalizedHtmlString("DocType").Value;
            DocumentNameLbl.Text = _LocalizationService.GetLocalizedHtmlString("DocType").Value;
            detailsNotes.Text = _LocalizationService.GetLocalizedHtmlString("Notes").Value;
           
            var datasource = this.DataSource as System.Collections.Generic.List<TreasuryStatementOfAccountReportDs>;
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
