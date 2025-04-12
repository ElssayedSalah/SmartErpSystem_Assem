using System;
using System.Threading;
using DevExpress.XtraReports.UI;

namespace Reports.Headers
{
    public partial class ReportHeaderAr_Portal
    {
        public ReportHeaderAr_Portal()
        {
            InitializeComponent();
        }

        private void ReportHeaderAr_Landscape_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Thread.CurrentThread.CurrentCulture.Name != "ar")
            {
                label1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;

            }
        }
    }
}
