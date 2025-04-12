using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BusinessLayer.Helpers.SharedEnums;
using DataAccessLayer.Entities;
using DataAccessLayer.Reposetories;
using DataAccessLayer.Entities.Identity;
using BusinessLayer.Models.Identity;
using System.ComponentModel;
using BusinessLayer.Models.Financial;
using DataAccessLayer.Entities.Financial;

namespace BusinessLayer.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// set base data to basic entity as create date ,update date,user data
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="Type"></param>
        public static void SetBasicData(this BasicEntity Entity , CRUD_OperationType Type, ApplicationUser currentUser)
        {
            if (Type== CRUD_OperationType.Create)
            {
                Entity.CreationDate = DateTime.Now;
                Entity.CreationUserId = currentUser?.UserName;               


            }
            if (Type == CRUD_OperationType.Update)
            {
                Entity.UpdatedDate = DateTime.Now;
                Entity.UpdatedUserId = currentUser?.UserName;
            }

            Entity.CompanyId = currentUser?.CompanyId;
        }
        /// <summary>
        /// set base data to transaction as create date ,update date,user data
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="Type"></param>
        /// <param name="DocumentType"></param>

        public static void SetBasicData(this TransactionEntity Entity, CRUD_OperationType Type,int DocumentType, ApplicationUser currentUser)
        {
            Entity.DocTypeId = DocumentType;
            if (Type == CRUD_OperationType.Create)
            {
                Entity.CreationDate = DateTime.Now;
                Entity.CreationUserId = currentUser?.UserName;
            }
            if (Type == CRUD_OperationType.Update)
            {
                Entity.UpdatedDate = DateTime.Now;
                Entity.UpdatedUserId = currentUser?.UserName;
            }
            Entity.CompanyId = currentUser?.CompanyId;
            Entity.FinancialPeriodId = currentUser?.FinancialPeriodId;
        }

        public static void SetBasicData(this UserModel Entity, CRUD_OperationType Type, ApplicationUser currentUser)
        {
            if (Type == CRUD_OperationType.Create)
            {
                Entity.CreationDate = DateTime.Now;
                Entity.CreationUserId = currentUser?.UserName;


            }
            if (Type == CRUD_OperationType.Update)
            {
                Entity.UpdatedDate = DateTime.Now;
                Entity.UpdatedUserId = currentUser?.UserName;
            }

            //Entity.CompanyId = currentUser?.CompanyId;
        }

        public static void SetBasicData(this DailyEntryMaster Entity, CRUD_OperationType Type, int DocumentType, ApplicationUser currentUser)
        {
            Entity.DocType = DocumentType;
            if (Type == CRUD_OperationType.Create)
            {
                Entity.CreationDate = DateTime.Now;
                Entity.CreationUserId = currentUser?.UserName;
            }
            if (Type == CRUD_OperationType.Update)
            {
                Entity.UpdatedDate = DateTime.Now;
                Entity.UpdatedUserId = currentUser?.UserName;
            }
            Entity.CompanyId = currentUser?.CompanyId;
            Entity.FinancialPeriodId = currentUser.FinancialPeriodId.HasValue? currentUser.FinancialPeriodId.Value:0;
        }
        public static void SetBasicData(this DailyEntryDetails Entity, ApplicationUser currentUser, DailyEntryMaster master)
        {            
            Entity.CompanyId = currentUser.CompanyId.HasValue? currentUser.CompanyId.Value:0;
            Entity.FinancialPeriodId = currentUser.FinancialPeriodId.HasValue ? currentUser.FinancialPeriodId.Value : 0;
            Entity.MasterEntryNumber = master.EntryNumber;

        }


        public static int GetLastCode(this Entity entity, IBaseReposetory<Entity> repo)
        {
            int LastCode = 0;
            var code = repo.GetAll().OrderBy(o=>o.Code).LastOrDefault();
            if (code != null)
            {
                LastCode = code.Code + 1;
            }
            else
            {
                LastCode = 1;
            }
            return LastCode;
        }
        public static bool IsExistCode(this BasicEntity entity, IBaseReposetory<Entity> repo)
        {
            bool IsExist = false;
            var code = repo.GetById(entity.Id);
            if (code != null)
            {
                IsExist = true;
            }        
            return IsExist;
        }

        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static List<string> GetMonthsNames(string language)
        {
            var months = new List<string>();

            if (language == "ar")
            {
                months = new List<string> { "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو", "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر" };
            }
            else
            {
                months = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            }

            return months;
        }

        //public static IList<Account> GetChild(this Account, IList<Account> items)
        //{
        //    var childs = items
        //        .Where(x => x.ParentId == id || x.Id == id)
        //        .Union(items.Where(x => x.ParentId == id)
        //        .SelectMany(y => GetChild(y.Id, items)));

        //    return childs.ToList();
        //}

        //public static List<SelectListItem> ConvertEnumToSelectListItems(Type t, LocalizationService l)
        //{
        //    var x =new List<SelectListItem>();            
        //    var elements = Enum.GetValues(t);
        //    for (int i = 0; i < elements.Length; i++)
        //    {
        //        x.Add(new SelectListItem() {Text= l.GetLocalizedHtmlString(t.Name+"."+ elements.GetValue(i).ToString()) , Value=i.ToString()} );

        //    }          
        //    return x;
        //}
    }
}
