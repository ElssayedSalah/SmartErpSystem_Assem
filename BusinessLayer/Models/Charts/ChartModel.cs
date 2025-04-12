using System.Collections.Generic;

namespace BusinessLayer.Models.Charts
{
    public class ChartModel
    {
        public List<string> labels { get; set; } //monthes x axis
        public List<decimal> data { get; set; } //vlaue for each month y axis
        public List<string> dataAsText { get; set; } //vlaue for each month y axis
        public string label { get; set; } //title of chart

        public ChartModel()
        {
            labels = new List<string>() { };
            data = new List<decimal>() {0,0,0,0,0,0,0,0,0,0,0,0 };
            dataAsText = new List<string>();
            label = "";
        }
    }
    public class LineChartModel
    {
        public List<string> labels { get; set; } 
        public List<LineChartDataset> datasets { get; set; }
        public LineChartModel()
        {
            labels = new List<string>() { };
            datasets = new List<LineChartDataset>();
        }

    }
    public class LineChartDataset
    {      
        public List<decimal> data { get; set; } 
        public string label { get; set; } 
        public bool fill { get; set; } 
        public string borderColor { get; set; } 
        public double tension { get; set; }   
        public LineChartDataset()
        {           
            data = new List<decimal>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            label = "";
            fill = false;
            borderColor = "blue";
            tension = 0.1;
        }
    }
}
