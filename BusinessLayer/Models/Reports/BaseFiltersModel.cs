namespace Reports.Model
{
    public class BaseFiltersModel
    {
        public string ReportName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string CurrentUserName { get; set; }
        public string ExportType { get; set; }
        public int? FinancialPeriodId { get; set; }
        public int? CompanyId { get; set; }
        public ReportResultModel reportResultModel { get; set; }

        public BaseFiltersModel()
        {
            ExportType = "pdf";          
         
        }
    }
}
