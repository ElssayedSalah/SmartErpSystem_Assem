using System.Text.Json.Serialization;

namespace Reports.Model
{
    public class ReportBaseModel:ISubDs
    {
        public string ReportName { get; set; }

        public string CompanyName { get; set; }
        [JsonIgnore]
        public string CompanyLogo { get; set; }

        public string CurrentUserName { get; set; }
        public ReportResultModel reportResultModel { get; set; }

        public ReportBaseModel()
        {

        }
    }
}
