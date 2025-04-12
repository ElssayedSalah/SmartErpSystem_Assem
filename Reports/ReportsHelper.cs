using DevExpress.XtraReports.UI;
using Reports.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Reports
{
    public static class ReportsHelper
    {
        //static IHostingEnvironment _HostingEnvironment;
       
        static string ExportPath = "/TRS/";
        static string LocalExportPath;
        static Dictionary<ReportsList, XtraReport> ReportsDictionary { get; set; }
        public static XtraReport GetReport(ReportsList report)
        {
            return ReportsDictionary[report];
        }
        public static XtraReport GetReport(int reportId)
        {
            return GetReport((ReportsList)reportId);
        }

        public static void InitializeHostingEnvironment(/*IHostingEnvironment hostingEnvironment*/)
        {
            //_HostingEnvironment = hostingEnvironment;
            //ExportPath = _HostingEnvironment.ContentRootPath;
        }
        static ReportsHelper()
        {          
            LocalExportPath = Directory.GetCurrentDirectory() + "/wwwroot";
            if (!Directory.Exists(LocalExportPath + ExportPath))
                Directory.CreateDirectory(LocalExportPath + ExportPath);
        }
        static string GenerateReportName(XtraReport report)
        {
            return report.Name + "_" + Guid.NewGuid().ToString().Substring(0, 8);

        }
        public static string GenerateXls(this XtraReport report)
        {
            ExportPath = ExportPath + Guid.NewGuid();
            if (!Directory.Exists(ExportPath))
                Directory.CreateDirectory(LocalExportPath + ExportPath);
            string path = ExportPath + GenerateReportName(report) + ".xls";

            var localPath = path;
            report.ExportToXls(localPath);
            return path;
        }
        public static string GenerateXlsx(this XtraReport report)
        {
            string path = LocalExportPath + GenerateReportName(report) + ".xlsx";
            var localPath = LocalExportPath + path;
            report.ExportToXlsx(localPath);
            return path;
        }
        public static MemoryStream GeneratePdf(this XtraReport report)
        {
            MemoryStream stream = new MemoryStream();
            report.ExportToPdf(stream);
            return stream;
        }

        //used to show reports pdf
        public static string GeneratePdfByPath(this XtraReport report, BaseFiltersModel filters)
        {
            if (report == null)
                return null;

            DeleteOldReports();

            report.DataSource.GetType();
            var language = Thread.CurrentThread.CurrentCulture.Name;
            if (language == "ar")
            {
                report.RightToLeft = RightToLeft.Yes;
                report.RightToLeftLayout = RightToLeftLayout.Yes;
            }
            else
            {
                report.RightToLeft = RightToLeft.No;
                report.RightToLeftLayout = RightToLeftLayout.No;
            }

            report.BeforePrint += (s, e) =>
            {

                XRSubreport headerReport = report.FindControl("RptHeader", true) as XRSubreport;
                if (headerReport != null && headerReport.ReportSource != null)
                {
                    headerReport.ReportSource.DataSource = new List<ReportBaseModel>() { new ReportBaseModel() { ReportName = filters.ReportName, CompanyLogo = filters.CompanyLogo, CompanyName = filters.CompanyName } ?? new ReportBaseModel() };
                }
            };
          
            string path = ExportPath + GenerateReportName(report) + ".Pdf";
            var localPath = LocalExportPath + path;
            report.ExportToPdf(localPath);
            return path;
        }

        private static void DeleteOldReports()
        {
            Task.Run(() =>
            {
                var reportsFolder = LocalExportPath + ExportPath;
                var checkDate = DateTime.Now.AddDays(-1);
                foreach (var oldReportDir in Directory.GetDirectories(reportsFolder))
                {
                    if (Directory.GetCreationTime(oldReportDir).Date >= (checkDate).Date)
                    {
                        Directory.Delete(oldReportDir, true); // Set second parameter to true for recursive deletion
                    }
                }
                foreach (var oldReport in Directory.GetFiles(reportsFolder))
                {
                    if (File.GetCreationTime(oldReport).Date >= (checkDate).Date)
                    {
                        File.Delete(oldReport);
                    }
                }
            });
        }
        //public static void SetRptParameter(XtraReport rpt, object filter, string ParamterName, object Value = null, bool IgnoreNullFilter = false)
        //{
        //    if (rpt.Parameters[ParamterName] != null)
        //        if (filter != null || IgnoreNullFilter)
        //            rpt.Parameters[ParamterName].Value = Value ?? filter;
        //}


        //used to show transactions print pdf
        public static MemoryStream GenerateReport(this XtraReport report, string type)
        {
            if (report == null)
                return null;

            report.DataSource.GetType();
            var language = Thread.CurrentThread.CurrentCulture.Name;
            if (language == "ar")
            {
                report.RightToLeft = RightToLeft.Yes;
                report.RightToLeftLayout = RightToLeftLayout.Yes;
            }
            else
            {
                report.RightToLeft = RightToLeft.No;
                report.RightToLeftLayout = RightToLeftLayout.No;
            }

            report.BeforePrint += (s, e) =>
            {
                var CurrentRow = report.GetCurrentRow() as ReportBaseModel;

                XRSubreport headerReport = report.FindControl("RptHeader", true) as XRSubreport;
                if (headerReport != null && headerReport.ReportSource != null)
                {                   
                    headerReport.ReportSource.DataSource = new List<ReportBaseModel>() { new ReportBaseModel() { ReportName = CurrentRow.ReportName, CompanyLogo = CurrentRow.CompanyLogo, CompanyName = CurrentRow.CompanyName } ?? new ReportBaseModel() };
                }
            };
           
            report.ExportOptions.PrintPreview.DefaultFileName = type.ToLower();
            if (type.ToLower() == "pdf")
                return report.GeneratePdf();
            //else if (type.ToLower() == "xls")
            //    return report.GenerateXls();
            //else if (type.ToLower() == "xlsx")
            //    return report.GenerateXlsx();
            return null;
        }
    }

    public enum ReportsList
    {
        SalesInvoiceRptPrint
    }
}
