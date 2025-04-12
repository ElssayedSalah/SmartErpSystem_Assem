using Microsoft.AspNetCore.Identity;
using System;

namespace DataAccessLayer.Entities.Identity
{
    public class ApplicationUser:IdentityUser
    { 
        public int Code { get; set; }
        public int? CompanyId { get; set; }
        public int? EmployeeId { get; set; }       
        public int? FinancialPeriodId { get; set; }
        public int? FinancialPeriod { get; set; } 
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreationUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public bool ActivationState { get; set; }
        public bool IsSystemUser { get; set; }
        public string Notes { get; set; }       
        public int? BranchId { get; set; }   
        public DateTime? FinancialPeriodFromDate { get; set; }
        public DateTime? FinancialPeriodToDate { get; set; }


    }
}
