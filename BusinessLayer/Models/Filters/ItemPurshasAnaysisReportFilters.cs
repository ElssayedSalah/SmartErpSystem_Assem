using Reports.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Filters
{
    public class ItemPurshasAnaysisReportFilters : BaseFiltersModel
    {
        [Display(Name = "SupplerName")]
        public int? SupplierId { get; set; }       

        [Display(Name = "ItemModel")]
        public int? ItemId { get; set; }

        [Display(Name = "ItemGroup")]
        public int? GroupId { get; set; }
        [Display(Name = "FromDate")]
        public DateTime FromDate { get; set; }
        [Display(Name = "ToDate")]
        public DateTime ToDate { get; set; }
        public ItemPurshasAnaysisReportFilters()
        {
            FromDate = new DateTime(DateTime.Now.Year, 1, 1);
            ToDate = new DateTime(DateTime.Now.Year, 12, 30);
        }

        
    }
}
