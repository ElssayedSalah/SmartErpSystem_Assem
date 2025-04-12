using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Seeds
{
    public static class DefaultAccountSettings
    {        
        public static async Task SeedDefaultAccountsAsync(IBaseService<DefaultAccount> _DefaultAccountService)
        {
            var DefaultAccounts = _DefaultAccountService.GetAll();
            if (DefaultAccounts==null || DefaultAccounts.Count<=0)
            {
                List<DefaultAccount> newDefaultAccounts = new List<DefaultAccount>() 
        {
    #region الحسابات العامة

  //الحسابات العامة/////////////////////////////////////////////////////////////
            new DefaultAccount(){GroupId=1, GroupNameAr= "الحسابات العامة" ,GroupNameEn="General Accounts", AccountNameAr="حساب الخزينة", AccountNameEn="", AccountNameId=1, AccountId=0 },

            new DefaultAccount(){GroupId=1, GroupNameAr= "الحسابات العامة" ,GroupNameEn="General Accounts", AccountNameAr="حساب مخزون البضاعة", AccountNameEn="", AccountNameId=2, AccountId=0 },

            new DefaultAccount(){GroupId=1, GroupNameAr= "الحسابات العامة" ,GroupNameEn="General Accounts", AccountNameAr="قيمة البضاعة المباعة", AccountNameEn="", AccountNameId=3, AccountId=0 },

            new DefaultAccount(){GroupId=1, GroupNameAr= "الحسابات العامة" ,GroupNameEn="General Accounts", AccountNameAr="حساب رأس المال", AccountNameEn="", AccountNameId=4, AccountId=0 },

            new DefaultAccount(){GroupId=1, GroupNameAr= "الحسابات العامة" ,GroupNameEn="General Accounts", AccountNameAr="هالك المخزون", AccountNameEn="", AccountNameId=5, AccountId=0 },

            new DefaultAccount(){GroupId=1, GroupNameAr= "الحسابات العامة" ,GroupNameEn="General Accounts", AccountNameAr="التسويات الجردية", AccountNameEn="", AccountNameId=6, AccountId=0 },

            new DefaultAccount(){GroupId=1, GroupNameAr= "الحسابات العامة" ,GroupNameEn="General Accounts", AccountNameAr="حساب البنوك", AccountNameEn="", AccountNameId=7, AccountId=0 },

            new DefaultAccount(){GroupId=1, GroupNameAr= "الحسابات العامة" ,GroupNameEn="General Accounts", AccountNameAr="حساب أوراق الدفع", AccountNameEn="", AccountNameId=8, AccountId=0 },

            new DefaultAccount(){GroupId=1, GroupNameAr= "الحسابات العامة" ,GroupNameEn="General Accounts", AccountNameAr="حساب أوراق القبض", AccountNameEn="", AccountNameId=9, AccountId=0 },

            new DefaultAccount(){GroupId=1, GroupNameAr= "الحسابات العامة" ,GroupNameEn="General Accounts", AccountNameAr="حساب مجمع الإهلاك", AccountNameEn="", AccountNameId=10, AccountId=0 },

            new DefaultAccount(){GroupId=1, GroupNameAr= "الحسابات العامة" ,GroupNameEn="General Accounts", AccountNameAr="حساب مخصص الإهلاك", AccountNameEn="", AccountNameId=11, AccountId=0 },

            new DefaultAccount(){GroupId=1, GroupNameAr= "الحسابات العامة" ,GroupNameEn="General Accounts", AccountNameAr="حساب المصروفات علي الأصل", AccountNameEn="", AccountNameId=12, AccountId=0 },

            new DefaultAccount(){GroupId=1, GroupNameAr= "الحسابات العامة" ,GroupNameEn="General Accounts", AccountNameAr="حساب رصيد المخزون أول المدة", AccountNameEn="", AccountNameId=13, AccountId=0 },

            new DefaultAccount(){GroupId=1, GroupNameAr= "الحسابات العامة" ,GroupNameEn="General Accounts", AccountNameAr="حساب رصيد المخزون أخر المدة", AccountNameEn="", AccountNameId=14, AccountId=0 },

            new DefaultAccount(){GroupId=1, GroupNameAr= "الحسابات العامة" ,GroupNameEn="General Accounts", AccountNameAr="حساب الصندوق", AccountNameEn="", AccountNameId=15, AccountId=0 },
                    #endregion

    #region العملاء والموردين

               //العملاء والموردين/////////////////////////////////////////////////////////////////

             new DefaultAccount(){GroupId=2, GroupNameAr= "العملاء والموردين" ,GroupNameEn="Sales", AccountNameAr="حساب المبيعات", AccountNameEn="", AccountNameId=67, AccountId=0 },

             new DefaultAccount(){GroupId=2, GroupNameAr= "العملاء والموردين" ,GroupNameEn="Purshaes", AccountNameAr="حساب المشتريات", AccountNameEn="", AccountNameId=68, AccountId=0 },

            new DefaultAccount(){GroupId=2, GroupNameAr= "العملاء والموردين" ,GroupNameEn="Customers And Supplers", AccountNameAr="حساب العملاء", AccountNameEn="", AccountNameId=16, AccountId=0 },

            new DefaultAccount(){GroupId=2, GroupNameAr= "العملاء والموردين" ,GroupNameEn="Customers And Supplers", AccountNameAr="حساب الموردين", AccountNameEn="", AccountNameId=17, AccountId=0 },

            new DefaultAccount(){GroupId=2, GroupNameAr= "العملاء والموردين" ,GroupNameEn="Customers And Supplers", AccountNameAr="الحسابات المختلطة", AccountNameEn="", AccountNameId=18, AccountId=0 },

            new DefaultAccount(){GroupId=2, GroupNameAr= "العملاء والموردين" ,GroupNameEn="Customers And Supplers", AccountNameAr="حساب المصانع", AccountNameEn="", AccountNameId=19, AccountId=0 },

            new DefaultAccount(){GroupId=2, GroupNameAr= "العملاء والموردين" ,GroupNameEn="Customers And Supplers", AccountNameAr="حساب الزمم", AccountNameEn="", AccountNameId=20, AccountId=0 },

            new DefaultAccount(){GroupId=2, GroupNameAr= "العملاء والموردين" ,GroupNameEn="Customers And Supplers", AccountNameAr="حساب الديون المعدومة", AccountNameEn="", AccountNameId=21, AccountId=0 },

            new DefaultAccount(){GroupId=2, GroupNameAr= "العملاء والموردين" ,GroupNameEn="Customers And Supplers", AccountNameAr="حساب التسويات المالية", AccountNameEn="", AccountNameId=22, AccountId=0 },

            new DefaultAccount(){GroupId=2, GroupNameAr= "العملاء والموردين" ,GroupNameEn="Customers And Supplers", AccountNameAr="حساب الإشعارات", AccountNameEn="", AccountNameId=1, AccountId=0 },

            new DefaultAccount(){GroupId=2, GroupNameAr= "العملاء والموردين" ,GroupNameEn="Customers And Supplers", AccountNameAr="حساب المشتريات", AccountNameEn="", AccountNameId=23, AccountId=0 },

            new DefaultAccount(){GroupId=2, GroupNameAr= "العملاء والموردين" ,GroupNameEn="Customers And Supplers", AccountNameAr="حساب مردودات المشتريات", AccountNameEn="", AccountNameId=24, AccountId=0 },

            new DefaultAccount(){GroupId=2, GroupNameAr= "العملاء والموردين" ,GroupNameEn="Customers And Supplers", AccountNameAr="حساب مردودات المبيعات", AccountNameEn="", AccountNameId=25, AccountId=0 },
                    #endregion

    #region الإيرادات والمصروفات

            //الإيرادات والمصروفات////////////////////////////////////////////////////////////////////////
                    new DefaultAccount(){GroupId=3, GroupNameAr= "الإيرادات والمصروفات" ,GroupNameEn="Incoms And Outcoms", AccountNameAr="حساب رواتب الموظفين", AccountNameEn="", AccountNameId=26, AccountId=0 },

            new DefaultAccount(){GroupId=3, GroupNameAr= "الإيرادات والمصروفات" ,GroupNameEn="Incoms And Outcoms", AccountNameAr="حساب سلف الموظفين", AccountNameEn="", AccountNameId=27, AccountId=0 },

            new DefaultAccount(){GroupId=3, GroupNameAr= "الإيرادات والمصروفات" ,GroupNameEn="Incoms And Outcoms", AccountNameAr="حساب مكافأت الموظفين", AccountNameEn="", AccountNameId=28, AccountId=0 },

            new DefaultAccount(){GroupId=3, GroupNameAr= "الإيرادات والمصروفات" ,GroupNameEn="Incoms And Outcoms", AccountNameAr="حساب عهد الموظفين", AccountNameEn="", AccountNameId=29, AccountId=0 },

            new DefaultAccount(){GroupId=3, GroupNameAr= "الإيرادات والمصروفات" ,GroupNameEn="Incoms And Outcoms", AccountNameAr="حساب الصدقة", AccountNameEn="", AccountNameId=30, AccountId=0 },

            new DefaultAccount(){GroupId=3, GroupNameAr= "الإيرادات والمصروفات" ,GroupNameEn="Incoms And Outcoms", AccountNameAr="حساب الهدايا", AccountNameEn="", AccountNameId=31, AccountId=0 },

            new DefaultAccount(){GroupId=3, GroupNameAr= "الإيرادات والمصروفات" ,GroupNameEn="Incoms And Outcoms", AccountNameAr="حساب إيرادات إضافية", AccountNameEn="", AccountNameId=32, AccountId=0 },

            new DefaultAccount(){GroupId=3, GroupNameAr= "الإيرادات والمصروفات" ,GroupNameEn="Incoms And Outcoms", AccountNameAr="حساب إغلاق المصروفات", AccountNameEn="", AccountNameId=33, AccountId=0 },

            new DefaultAccount(){GroupId=3, GroupNameAr= "الإيرادات والمصروفات" ,GroupNameEn="Incoms And Outcoms", AccountNameAr="حساب إغلاق الإيرادات", AccountNameEn="", AccountNameId=34, AccountId=0 },
                    #endregion

    #region الضرايب

             //الضرايب
                    new DefaultAccount(){GroupId=4, GroupNameAr= "الضرائب" ,GroupNameEn="Taxes", AccountNameAr="ضريبة القيمة المضافة", AccountNameEn="", AccountNameId=35, AccountId=0 },

            new DefaultAccount(){GroupId=4, GroupNameAr= "الضرائب" ,GroupNameEn="Taxes", AccountNameAr="ضريبة الخصم و الإضافة", AccountNameEn="", AccountNameId=36, AccountId=0 },

            new DefaultAccount(){GroupId=4, GroupNameAr= "الضرائب" ,GroupNameEn="Taxes", AccountNameAr="ضريبة الأرباح التجارية والصناعية", AccountNameEn="", AccountNameId=37, AccountId=0 },

            new DefaultAccount(){GroupId=4, GroupNameAr= "الضرائب" ,GroupNameEn="Taxes", AccountNameAr="ضريبة إضافية 1 %", AccountNameEn="", AccountNameId=38, AccountId=0 },
                    #endregion

    #region الخصومات

               //الخصومات/////////////////////////////////////////////////////////
                    new DefaultAccount(){GroupId=5, GroupNameAr= "الخصومات" ,GroupNameEn="Discounts", AccountNameAr="خصم الأصناف مكتسب", AccountNameEn="", AccountNameId=39, AccountId=0 },

            new DefaultAccount(){GroupId=5, GroupNameAr= "الخصومات" ,GroupNameEn="Discounts", AccountNameAr="حصم أصناف مسموح به", AccountNameEn="", AccountNameId=40, AccountId=0 },

            new DefaultAccount(){GroupId=5, GroupNameAr= "الخصومات" ,GroupNameEn="Discounts", AccountNameAr="خصم بونص مكتسب", AccountNameEn="", AccountNameId=41, AccountId=0 },

            new DefaultAccount(){GroupId=5, GroupNameAr= "الخصومات" ,GroupNameEn="Discounts", AccountNameAr="خصم بونص مسموح به", AccountNameEn="", AccountNameId=42, AccountId=0 },

            new DefaultAccount(){GroupId=5, GroupNameAr= "الخصومات" ,GroupNameEn="Discounts", AccountNameAr="خصم نقدي عملاء", AccountNameEn="", AccountNameId=43, AccountId=0 },

            new DefaultAccount(){GroupId=5, GroupNameAr= "الخصومات" ,GroupNameEn="Discounts", AccountNameAr="خصم نقدي موردين", AccountNameEn="", AccountNameId=44, AccountId=0 },
                    #endregion

    #region قائمة الدخل

                //قائمة الدخل////////////////////////////////////////////////////////
                    new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="حساب المشتريات", AccountNameEn="", AccountNameId=45, AccountId=0 },

            new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="حساب المبيعات", AccountNameEn="", AccountNameId=46, AccountId=0 },

            new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="مردودات المبيعات", AccountNameEn="", AccountNameId=47, AccountId=0 },

            new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="مسموحات المبيعات", AccountNameEn="", AccountNameId=48, AccountId=0 },

            new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="خصم مسموح به للعملاء", AccountNameEn="", AccountNameId=49, AccountId=0 },

             new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="مردودات المشتريات", AccountNameEn="", AccountNameId=50, AccountId=0 },

            new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="مسموحات المشتريات", AccountNameEn="", AccountNameId=51, AccountId=0 },

            new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="خصم مكتسب", AccountNameEn="", AccountNameId=52, AccountId=0 },

            new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="مصروفات إدارية و عمومية", AccountNameEn="", AccountNameId=53, AccountId=0 },

            new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="مصروفات التشغيل و الإنتاج", AccountNameEn="", AccountNameId=54, AccountId=0 },

             new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="مصروفات بيع و تسويق", AccountNameEn="", AccountNameId=55, AccountId=0 },

            new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="مصروفات بنكية", AccountNameEn="", AccountNameId=56, AccountId=0 },

            new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="مصروفات تحويلية", AccountNameEn="", AccountNameId=57, AccountId=0 },

            new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="إيرادات أخري", AccountNameEn="", AccountNameId=58, AccountId=0 },

            new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="مصروفات أخري", AccountNameEn="", AccountNameId=59, AccountId=0 },

             new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="حساب أرباح و خسائر", AccountNameEn="", AccountNameId=60, AccountId=0 },

            new DefaultAccount(){GroupId=6, GroupNameAr= "قائمة الدخل" ,GroupNameEn="InCome List", AccountNameAr="حساب الأرباح المحتجزة", AccountNameEn="", AccountNameId=61, AccountId=0 },
                    #endregion

    #region الاصول

             //الاصول///////////////////////////////////////////////////////////////
                    new DefaultAccount(){GroupId=7, GroupNameAr= "الأصول" ,GroupNameEn="Assets", AccountNameAr="حساب الخسارة الرأسمالية", AccountNameEn="", AccountNameId=62, AccountId=0 },

            new DefaultAccount(){GroupId=7, GroupNameAr= "الأصول" ,GroupNameEn="Assets", AccountNameAr="حساب أرباح الأصول", AccountNameEn="", AccountNameId=63, AccountId=0 },

            new DefaultAccount(){GroupId=7, GroupNameAr= "الأصول" ,GroupNameEn="Assets", AccountNameAr="حساب بيع الأصول", AccountNameEn="", AccountNameId=64, AccountId=0 },

                    #endregion

    #region نقاط البيع

               //نقاط البيع///////////////////////////////////////////////////////////////
                new DefaultAccount(){GroupId=8, GroupNameAr= "نقاط البيع" ,GroupNameEn="POS", AccountNameAr="حساب النقدية", AccountNameEn="", AccountNameId=65, AccountId=0 },

               new DefaultAccount(){GroupId=8, GroupNameAr= "نقاط البيع" ,GroupNameEn="POS", AccountNameAr="حساب المبيعات", AccountNameEn="", AccountNameId=66, AccountId=0 }, 
	#endregion

          
        };
      
                await _DefaultAccountService.AddRange(newDefaultAccounts);
            }
            

        }

    }
}
