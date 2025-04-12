namespace Reports.Model
{
    public class ReportResultModel
    {
        public string FilePath { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
        public object DataSource { get; set; }
    }
}
